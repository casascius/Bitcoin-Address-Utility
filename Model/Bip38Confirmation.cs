using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math;
using CryptSharp.Utility;

namespace Casascius.Bitcoin {
    public class Bip38Confirmation : Bip38Base {

        /// <summary>
        /// The public key of the confirmed address, once properly decrypted with a passphrase.
        /// Without proper decryption, this is null.
        /// </summary>
        public PublicKey PublicKey { get; private set; }

        public bool PublicKeyIsPresent {
            get {
                return PublicKey != null;
            }
        }

        /// <summary>
        /// Returns true if the confirmation code indicates that the Bitcoin address must be formed by
        /// hashing a compressed point.
        /// </summary>
        public bool IsCompressedPoint {
            get {
                return (flagbyte & 0x20) == 0x20;
            }
        }

        public override bool LotSequencePresent {
            get {
                return (flagbyte & 0x04) == 0x04;
            }
        }

        
        public byte flagbyte { get; private set; }

        private byte[] _addressHash;

        /// <summary>
        /// The encrypted payload representing the point.
        /// </summary>
        private byte[] _encryptedpointb;


        /// <summary>
        /// Parses a Base58Check-encoded string as a Bip38Confirmation, throwing an ArgumentException if
        /// parsing was not possible.  Parsing does not validate whether decryption is possible.
        /// When parsing is successful, use DecryptWithPassphrase to perform the validation and generate PublicKey.
        /// </summary>
        public Bip38Confirmation(string base58CheckString) {
            byte[] bytes = Util.Base58CheckToByteArray(base58CheckString);
            if (bytes == null) {
                throw new ArgumentException("This is not a valid confirmation code. (Not a valid Base58Check string)");
            }

            Exception failureReason = ValidateBase58(bytes);
            if (failureReason != null) throw failureReason;

            // Get the flag byte.
            // This gives access to IsCompressedPoint and LotSequencePresent
            flagbyte = bytes[5];

            // Get the address hash.
            _addressHash = new byte[4];
            Array.Copy(bytes, 6, _addressHash, 0, 4);

            // Get the owner entropy.  (This gives access to LotNumber and SequenceNumber when applicable)
            _ownerentropy = new byte[8];
            Array.Copy(bytes, 10, _ownerentropy, 0, 8);

            // Get encryptedpointb
            _encryptedpointb = new byte[33];
            Array.Copy(bytes, 18, _encryptedpointb, 0, 33);
        }

        public static Exception ValidateBase58(byte[] bytes) {
            if (bytes==null || 
                bytes.Length != 51 || 
                // check the magic
                bytes[0] != 0x64 || bytes[1] != 0x3B || bytes[2] != 0xF6 ||
                bytes[3] != 0xA8 || bytes[4] != 0x9A || 
                // check valid values for first byte of pointb
                bytes[18] < 0x02 || bytes[18] > 0x03) {
                return new ArgumentException("This is not a valid confirmation code.");
            }
            return null;
        }

        public Exception DecryptWithPassphrase(string passphrase) {
            // check for null entry
            if (passphrase == null || passphrase == "") {
                return new ArgumentException("Passphrase is required");
			}

            Bip38Intermediate intermediate = new Bip38Intermediate(passphrase, _ownerentropy, LotSequencePresent);

            // derive the 64 bytes we need
            // get ECPoint from passpoint            
            PublicKey pk = new PublicKey(intermediate.passpoint);

            byte[] addresshashplusownerentropy = Util.ConcatenateByteArrays(_addressHash, intermediate.ownerentropy);

            // derive encryption key material
            byte[] derived = new byte[64];
            SCrypt.ComputeKey(intermediate.passpoint, addresshashplusownerentropy, 1024, 1, 1, 1, derived);

            byte[] derivedhalf2 = new byte[32];
            Array.Copy(derived, 32, derivedhalf2, 0, 32);

            byte[] unencryptedpubkey = new byte[33];
            // recover the 0x02 or 0x03 prefix
            unencryptedpubkey[0] = (byte)(_encryptedpointb[0] ^ (derived[63] & 0x01));

            // decrypt
            var aes = Aes.Create();
            aes.KeySize = 256;
            aes.Mode = CipherMode.ECB;
            aes.Key = derivedhalf2;
            ICryptoTransform decryptor = aes.CreateDecryptor();

            decryptor.TransformBlock(_encryptedpointb, 1, 16, unencryptedpubkey, 1);
            decryptor.TransformBlock(_encryptedpointb, 1, 16, unencryptedpubkey, 1);
            decryptor.TransformBlock(_encryptedpointb, 1 + 16, 16, unencryptedpubkey, 17);
            decryptor.TransformBlock(_encryptedpointb, 1 + 16, 16, unencryptedpubkey, 17);

            // xor out the padding
            for (int i = 0; i < 32; i++) unencryptedpubkey[i + 1] ^= derived[i];

            // reconstitute the ECPoint
            var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");
            ECPoint point;
            try {
                point = ps.Curve.DecodePoint(unencryptedpubkey);

                // multiply passfactor.  Result is going to be compressed.
                ECPoint pubpoint = point.Multiply(new BigInteger(1, intermediate.passfactor));

                // Do we want it uncompressed?  then we will have to uncompress it.
                if (IsCompressedPoint==false) {
                    pubpoint = ps.Curve.CreatePoint(pubpoint.X.ToBigInteger(), pubpoint.Y.ToBigInteger(), false);
                }

                // Convert to bitcoin address and check address hash.
                PublicKey generatedaddress = new PublicKey(pubpoint);

                // get addresshash
                UTF8Encoding utf8 = new UTF8Encoding(false);
                Sha256Digest sha256 = new Sha256Digest();
                byte[] generatedaddressbytes = utf8.GetBytes(generatedaddress.AddressBase58);
                sha256.BlockUpdate(generatedaddressbytes, 0, generatedaddressbytes.Length);
                byte[] addresshashfull = new byte[32];
                sha256.DoFinal(addresshashfull, 0);
                sha256.BlockUpdate(addresshashfull, 0, 32);
                sha256.DoFinal(addresshashfull, 0);

                for (int i = 0; i < 4; i++) {
                    if (addresshashfull[i] != _addressHash[i]) {
                        return new ArgumentException("This passphrase is wrong or does not belong to this confirmation code.");
                    }
                }

                this.PublicKey = generatedaddress;
            } catch {
                return new ArgumentException("This passphrase is wrong or does not belong to this confirmation code.");
            }
            return null;
        }    
    }

}

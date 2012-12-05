using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;
using System.IO;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math;
using CryptSharp.Utility;

namespace BtcAddress {
    /// <summary>
    /// Represents an encrypted keypair using the methodology described in BIP 38.
    /// </summary>
    public class Bip38KeyPair : PassphraseKeyPair {

        /// <summary>
        /// Load constructor (in preparation for decryption)
        /// </summary>
        public Bip38KeyPair(string encryptedkey)
            : base() {
            this._encryptedKey = encryptedkey;

            byte[] hex = Bitcoin.Base58CheckToByteArray(encryptedkey);

            if (hex == null) {
                throw new ArgumentException("Not a valid key");
            } else if (hex.Length == 39 && hex[0] == 1 && hex[1] == 0x42) {
                // Casascius BIP passphrase-encrypted key using scrypt
                if (hex[2] == 0xe0) {
                    this.IsCompressedPoint = true;
                } else if (hex[2] != 0xc0) {
                    throw new ArgumentException("Private key is not valid or is a newer format unsupported by this version of the software.");
                }
            } else if (hex.Length == 39 && hex[0] == 1 && hex[1] == 0x43) {
                if (hex[2] == 0x20) {
                    IsCompressedPoint = true;
                } else if (hex[2] != 0x00) {
                    throw new ArgumentException("Private key is not valid or is a newer format unsupported by this version of the software.");
                }
            } else {
                throw new ArgumentException("Not a BIP38-encoded key");
            }
        }

        public override bool DecryptWithPassphrase(string passphrase) {
            if (passphrase == null) {
                return false;
            }

            byte[] hex = Bitcoin.Base58CheckToByteArray(_encryptedKey);
            KeyPair tempkey = null;

            if (hex.Length == 39 && hex[0] == 1 && hex[1] == 0x42) {
                UTF8Encoding utf8 = new UTF8Encoding(false);
                byte[] addresshash = new byte[] { hex[3], hex[4], hex[5], hex[6] };

                byte[] derivedBytes = new byte[64];
                SCrypt.ComputeKey(utf8.GetBytes(passphrase), addresshash, 16384, 8, 8, 8, derivedBytes);

                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                aes.KeySize = 256;
                aes.Mode = CipherMode.ECB;
                byte[] aeskey = new byte[32];
                Array.Copy(derivedBytes, 32, aeskey, 0, 32);
                aes.Key = aeskey;
                ICryptoTransform decryptor = aes.CreateDecryptor();

                byte[] decrypted = new byte[32];
                decryptor.TransformBlock(hex, 7, 16, decrypted, 0);
                decryptor.TransformBlock(hex, 7, 16, decrypted, 0);
                decryptor.TransformBlock(hex, 23, 16, decrypted, 16);
                decryptor.TransformBlock(hex, 23, 16, decrypted, 16);
                for (int x = 0; x < 32; x++) decrypted[x] ^= derivedBytes[x];

                tempkey = new KeyPair(decrypted, compressed: IsCompressedPoint);

                Sha256Digest sha256 = new Sha256Digest();
                byte[] addrhash = new byte[32];
                byte[] addrtext = utf8.GetBytes(tempkey.AddressBase58);
                sha256.BlockUpdate(addrtext, 0, addrtext.Length);
                sha256.DoFinal(addrhash, 0);
                sha256.BlockUpdate(addrhash, 0, 32);
                sha256.DoFinal(addrhash, 0);
                if (addrhash[0] != hex[3] || addrhash[1] != hex[4] || addrhash[2] != hex[5] || addrhash[3] != hex[6]) {
                    return false;
                }
                _privKey = tempkey.PrivateKeyBytes;
                _pubKey = tempkey.PublicKeyBytes;
                _hash160 = tempkey.Hash160;
                return true;
            } else if (hex.Length == 39 && hex[0] == 1 && hex[1] == 0x43) {

                // produce the intermediate from the passphrase


                // get ownersalt and encryptedpart2 since they are in the record
                byte[] ownersalt = new byte[8];
                Array.Copy(hex, 7, ownersalt, 0, 8);
                Bip38Intermediate intermediate = new Bip38Intermediate(passphrase, ownersalt);


                tempkey = decryptUsingIntermediate(intermediate, hex);
                if (verifyAddressHash(tempkey.AddressBase58, hex) == false) return false;
            }
            _privKey = tempkey.PrivateKeyBytes;
            _pubKey = tempkey.PublicKeyBytes;
            _hash160 = tempkey.Hash160;
            return true;
        }

        private bool verifyAddressHash(string addressBase58, byte[] hex) {
            // check address hash
            UTF8Encoding utf8 = new UTF8Encoding(false);
            Sha256Digest sha256 = new Sha256Digest();
            byte[] addrhash = new byte[32];
            byte[] addrtext = utf8.GetBytes(addressBase58);
            sha256.BlockUpdate(addrtext, 0, addrtext.Length);
            sha256.DoFinal(addrhash, 0);
            sha256.BlockUpdate(addrhash, 0, 32);
            sha256.DoFinal(addrhash, 0);
            if (addrhash[0] != hex[3] || addrhash[1] != hex[4] || addrhash[2] != hex[5] || addrhash[3] != hex[6]) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Decrypts a key pair given an intermediate generated from a passphrase.
        /// Using a pre-generated intermediate allows for fast decryption of batches of keys that were
        /// generated from the same intermediate.
        /// </summary>
        public bool DecryptWithIntermediate(Bip38Intermediate intermediate) {
            byte[] hex = Bitcoin.Base58CheckToByteArray(_encryptedKey);
            // verify that the intermediate has the same ownersalt
            byte[] ownersalt = intermediate.ownersalt;
            for (int i = 0; i < 8; i++) {
                if (hex[i + 7] != ownersalt[i]) {
                    throw new ArgumentException("Intermediate does not have same salt");
                }
            }

            KeyPair tempkey = decryptUsingIntermediate(intermediate, hex);
            if (verifyAddressHash(tempkey.AddressBase58, hex) == false) return false;
            
            _privKey = tempkey.PrivateKeyBytes;
            _pubKey = tempkey.PublicKeyBytes;
            _hash160 = tempkey.Hash160;
            return true;

        }

        private KeyPair decryptUsingIntermediate(Bip38Intermediate intermediate, byte[] hex) {
            if (intermediate.passfactor == null) {
                throw new ArgumentException("Intermediate must have been created from passphrase to be used for decryption");
            }

            byte[] encryptedpart2 = new byte[16];
            Array.Copy(hex, 23, encryptedpart2, 0, 16);

            // get the first part of encryptedpart1 (the rest is encrypted within encryptedpart2)
            byte[] encryptedpart1 = new byte[16];
            Array.Copy(hex, 15, encryptedpart1, 0, 8);

            // derive decryption key
            byte[] addresshashplusownersalt = new byte[12];
            Array.Copy(hex, 3, addresshashplusownersalt, 0, 4);
            Array.Copy(intermediate.ownersalt, 0, addresshashplusownersalt, 4, 8);
            byte[] derived = new byte[64];
            SCrypt.ComputeKey(intermediate.passpoint, addresshashplusownersalt, 1024, 1, 1, 1, derived);
            byte[] derivedhalf2 = new byte[32];
            Array.Copy(derived, 32, derivedhalf2, 0, 32);

            // decrypt encrypted payload
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.KeySize = 256;
            aes.Mode = CipherMode.ECB;
            aes.Key = derivedhalf2;
            ICryptoTransform decryptor = aes.CreateDecryptor();

            byte[] unencryptedpart2 = new byte[16];
            decryptor.TransformBlock(encryptedpart2, 0, 16, unencryptedpart2, 0);
            decryptor.TransformBlock(encryptedpart2, 0, 16, unencryptedpart2, 0);
            for (int i = 0; i < 16; i++) {
                unencryptedpart2[i] ^= derived[i + 16];
            }

            // take the decrypted part and recover encrypted part 1
            Array.Copy(unencryptedpart2, 0, encryptedpart1, 8, 8);

            // decrypt part 1
            byte[] unencryptedpart1 = new byte[16];
            decryptor.TransformBlock(encryptedpart1, 0, 16, unencryptedpart1, 0);
            decryptor.TransformBlock(encryptedpart1, 0, 16, unencryptedpart1, 0);
            for (int i = 0; i < 16; i++) {
                unencryptedpart1[i] ^= derived[i];
            }

            // recover seedb
            byte[] seedb = new byte[24];
            Array.Copy(unencryptedpart1, 0, seedb, 0, 16);
            Array.Copy(unencryptedpart2, 8, seedb, 16, 8);

            // turn seedb into factorb
            Sha256Digest sha256 = new Sha256Digest();
            sha256.BlockUpdate(seedb, 0, 24);
            byte[] factorb = new byte[32];
            sha256.DoFinal(factorb, 0);
            sha256.BlockUpdate(factorb, 0, 32);
            sha256.DoFinal(factorb, 0);

            // get private key
            var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");
            BigInteger privatekey = new BigInteger(1, intermediate.passfactor).Multiply(new BigInteger(1, factorb)).Mod(ps.N);

            // use private key
            return new KeyPair(privatekey, this.IsCompressedPoint, this._addressType);
        }
        /// <summary>
        /// Encryption constructor (non-EC-multiply)
        /// </summary>
        public Bip38KeyPair(KeyPair key, string passphrase) {
            if (passphrase == null && passphrase == "") {
                throw new ArgumentException("Passphrase is required");
            }

            if (key == null) throw new ArgumentException("Passphrase is required");

            this.IsCompressedPoint = key.IsCompressedPoint;
            this._addressType = key.AddressType;

            UTF8Encoding utf8 = new UTF8Encoding(false);
            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
            byte[] addrhashfull = sha256.ComputeHash(sha256.ComputeHash(utf8.GetBytes(key.AddressBase58)));
            byte[] addresshash = new byte[] { addrhashfull[0], addrhashfull[1], addrhashfull[2], addrhashfull[3] };

            byte[] derivedBytes = new byte[64];
            SCrypt.ComputeKey(utf8.GetBytes(passphrase), addresshash, 16384, 8, 8, 8, derivedBytes);

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.KeySize = 256;
            aes.Mode = CipherMode.ECB;
            byte[] aeskey = new byte[32];
            Array.Copy(derivedBytes, 32, aeskey, 0, 32);
            aes.Key = aeskey;
            ICryptoTransform encryptor = aes.CreateEncryptor();

            byte[] unencrypted = new byte[32];
            byte[] rv = new byte[39];
            Array.Copy(key.PrivateKeyBytes, unencrypted, 32);
            for (int x = 0; x < 32; x++) unencrypted[x] ^= derivedBytes[x];

            encryptor.TransformBlock(unencrypted, 0, 16, rv, 7);
            encryptor.TransformBlock(unencrypted, 0, 16, rv, 7);
            encryptor.TransformBlock(unencrypted, 16, 16, rv, 23);
            encryptor.TransformBlock(unencrypted, 16, 16, rv, 23);

            // put header
            rv[0] = 0x01;
            rv[1] = 0x42;
            rv[2] = IsCompressedPoint ? (byte)0xe0 : (byte)0xc0;

            byte[] checksum = sha256.ComputeHash(sha256.ComputeHash(utf8.GetBytes(key.AddressBase58)));
            rv[3] = checksum[0];
            rv[4] = checksum[1];
            rv[5] = checksum[2];
            rv[6] = checksum[3];

            _encryptedKey = Bitcoin.ByteArrayToBase58Check(rv);
            _pubKey = key.PublicKeyBytes;
            _hash160 = key.Hash160;

        }

        /// <summary>
        /// Encryption constructor to create a new random key from an intermediate
        /// </summary>
        public Bip38KeyPair(Bip38Intermediate intermediate, bool retainPrivateKeyWhenPossible=false) {

            // generate seedb
            byte[] seedb = new byte[24];
            SecureRandom sr = new SecureRandom();
            sr.NextBytes(seedb);

            // get factorb as sha256(sha256(seedb))
            Sha256Digest sha256 = new Sha256Digest();
            sha256.BlockUpdate(seedb, 0, 24);
            byte[] factorb = new byte[32];
            sha256.DoFinal(factorb, 0);
            sha256.BlockUpdate(factorb, 0, 32);
            sha256.DoFinal(factorb, 0);

            // get ECPoint from passpoint            
            PublicKey pk = new PublicKey(intermediate.passpoint);

            ECPoint generatedpoint = pk.GetECPoint().Multiply(new BigInteger(1, factorb));
            byte[] generatedpointbytes = generatedpoint.GetEncoded();
            PublicKey generatedaddress = new PublicKey(generatedpointbytes);

            // get addresshash
            UTF8Encoding utf8 = new UTF8Encoding(false);
            byte[] generatedaddressbytes = utf8.GetBytes(generatedaddress.AddressBase58);
            sha256.BlockUpdate(generatedaddressbytes, 0, generatedaddressbytes.Length);
            byte[] addresshashfull = new byte[32];
            sha256.DoFinal(addresshashfull, 0);
            sha256.BlockUpdate(addresshashfull, 0, 32);
            sha256.DoFinal(addresshashfull, 0);

            byte[] addresshashplusownersalt = new byte[12];
            Array.Copy(addresshashfull, 0, addresshashplusownersalt, 0, 4);
            Array.Copy(intermediate.ownersalt, 0, addresshashplusownersalt, 4, 8);

            // derive encryption key material
            byte[] derived = new byte[64];
            SCrypt.ComputeKey(intermediate.passpoint, addresshashplusownersalt, 1024, 1, 1, 1, derived);

            byte[] derivedhalf2 = new byte[32];
            Array.Copy(derived, 32, derivedhalf2, 0, 32);

            byte[] unencryptedpart1 = new byte[16];
            for (int i = 0; i < 16; i++) {
                unencryptedpart1[i] = (byte)(seedb[i] ^ derived[i]);
            }
            byte[] encryptedpart1 = new byte[16];

            // encrypt it
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.KeySize = 256;
            aes.Mode = CipherMode.ECB;
            aes.Key = derivedhalf2;
            ICryptoTransform encryptor = aes.CreateEncryptor();

            encryptor.TransformBlock(unencryptedpart1, 0, 16, encryptedpart1, 0);
            encryptor.TransformBlock(unencryptedpart1, 0, 16, encryptedpart1, 0);

            byte[] unencryptedpart2 = new byte[16];
            for (int i = 0; i < 8; i++) {
                unencryptedpart2[i] = (byte)(encryptedpart1[i + 8] ^ derived[i + 16]);
            }
            for (int i = 0; i < 8; i++) {
                unencryptedpart2[i + 8] = (byte)(seedb[i + 16] ^ derived[i + 24]);
            }

            byte[] encryptedpart2 = new byte[16];
            encryptor.TransformBlock(unencryptedpart2, 0, 16, encryptedpart2, 0);
            encryptor.TransformBlock(unencryptedpart2, 0, 16, encryptedpart2, 0);

            byte[] result = new byte[39];
            result[0] = 0x01;
            result[1] = 0x43;
            result[2] = generatedaddress.IsCompressedPoint ? (byte)0x20 : (byte)0x00;
            Array.Copy(addresshashfull, 0, result, 3, 4);
            Array.Copy(intermediate.ownersalt, 0, result, 7, 8);
            Array.Copy(encryptedpart1, 0, result, 15, 8);
            Array.Copy(encryptedpart2, 0, result, 23, 16);

            _encryptedKey = Bitcoin.ByteArrayToBase58Check(result);
            _pubKey = generatedaddress.PublicKeyBytes;
            _hash160 = generatedaddress.Hash160;

            if (retainPrivateKeyWhenPossible && intermediate.passfactor != null) {
                var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");
                BigInteger privatekey = new BigInteger(1, intermediate.passfactor).Multiply(new BigInteger(1, factorb)).Mod(ps.N);
                _privKey = new KeyPair(privatekey).PrivateKeyBytes;

            }

        }

    }
}

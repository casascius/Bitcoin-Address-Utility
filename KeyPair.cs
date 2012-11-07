using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math;

namespace BtcAddress {

    public class KeyPair : PublicKey {
        protected KeyPair() { }

        private static Int64 nonce;

        /// <summary>
        /// Creates a new key pair using the SHA256 hash of a given string as the private key.
        /// </summary>
        public static KeyPair CreateFromString(string tohash) {
            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
            UTF8Encoding utf8 = new UTF8Encoding(false);
            byte[] forsha = utf8.GetBytes(tohash);
            return new KeyPair(forsha);
        }

        /// <summary>
        /// Creates a new random key pair, using a user-provided string to add entropy to the
        /// SecureRandom generator provided by the .NET Framework.
        /// </summary>
        public static KeyPair Create(string usersalt, bool compressed=false) {
            if (usersalt == null) usersalt = "ok, whatever";
            usersalt += DateTime.UtcNow.Ticks.ToString();

            SecureRandom sr = new SecureRandom();
            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();

            byte[] poop = sha256.ComputeHash(Encoding.ASCII.GetBytes(usersalt + nonce.ToString()));
            nonce++;

            byte[] newkey = new byte[32];

            for (int i = 0; i < 32; i++) {
                long x = sr.NextLong() & long.MaxValue;
                x += poop[i];
                newkey[i] = (byte)(x & 0xff);
            }
            return new KeyPair(newkey,compressed: compressed);
        }

        public KeyPair(BigInteger bi) {
            
            var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");
            if (bi.CompareTo(ps.N) >= 0 || bi.SignValue <= 0) {
                throw new ApplicationException("BigInteger is out of range of valid private keys");
            }
            byte[] bb = Bitcoin.Force32Bytes(bi.ToByteArrayUnsigned());
            PrivateKeyBytes = bb;
        }


        /// <summary>
        /// Create a Bitcoin address from a 32-byte private key
        /// </summary>
        public KeyPair(byte[] bytes, string passphrase=null, bool compressed=false) {
            if (bytes.Length == 32) {
                this.Passphrase = passphrase;
                PrivateKeyBytes = bytes;
                _compressedPoint = compressed;
            } else {
                throw new ArgumentException("Byte array provided to KeyPair constructor must be 32 bytes long");
            }
        }

        public KeyPair(string key, string passphrase=null, bool compressed=false) {
            string result = constructWithKey(key, passphrase, compressed);
            if (result != null) throw new ArgumentException(result);

        }

        /// <summary>
        /// Constructs the object with string key, returning any intended exception as a string.
        /// </summary>
        private string constructWithKey(string key, string passphrase, bool compressed) {
            this.Passphrase = passphrase;
            byte[] hex = Bitcoin.Base58CheckToByteArray(key);
            if (hex == null) {
                hex = Bitcoin.HexStringToBytes(key, true);
                if (hex == null) {
                    // tolerate a minikey
                    if (MiniKeyPair.IsValidMiniKey(key) > 0) {
                        PrivateKeyBytes = new MiniKeyPair(key).PrivateKeyBytes;
                        return null;
                    } else {
                        return "Invalid private key";
                    }
                }
            }
            if (hex.Length == 32) {
                _privKey = new byte[32];
                Array.Copy(hex, 0, _privKey, 0, 32);
                _compressedPoint = compressed;
            } else if (hex.Length == 33 && hex[0] == 0x80) {
                // normal private key
                _privKey = new byte[32];
                Array.Copy(hex, 1, _privKey, 0, 32);
                _compressedPoint = false;
            } else if (hex.Length == 34 && hex[0] == 0x80 && hex[33] == 0x01) {
                // compressed private key
                _privKey = new byte[32];
                Array.Copy(hex, 1, _privKey, 0, 32);
                _compressedPoint = true;
            } else if (hex.Length == 36 && hex[0] == 0x02 && hex[1] == 0x05) {
                // Encrypted private key.
                _compressedPoint = false;
                if (_passphrase == null || _passphrase == "") {
                    return "This is an encrypted private key and no passphrase has been provided";
                }

                SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                UTF8Encoding utf8 = new UTF8Encoding(false);
                byte[] checksum = sha256.ComputeHash(utf8.GetBytes(_passphrase + "?"));

                if (hex[2] > 0x80) return "Private key is not valid.";
                if (hex[2] != 0x80) {
                    if ((checksum[0] & 0x7f) != hex[2] || (checksum[1] & 0x7e) != (hex[3] & 0x7e)) {
                        return "The passphrase is not correct.";
                    }
                    if ((hex[3] & 0x01) == 0x01) {
                        _compressedPoint = true;
                    }
                }

                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                aes.KeySize = 256;
                aes.Mode = CipherMode.ECB;

                //aes.BlockSize = 256;

                byte[] encryptionKey = sha256.ComputeHash(utf8.GetBytes(_passphrase));
                aes.Key = encryptionKey;
                ICryptoTransform encryptor = aes.CreateDecryptor();

                byte[] decrypted = new byte[33];
                decrypted[0] = 0x80;

                encryptor.TransformBlock(hex, 4, 16, decrypted, 1);
                encryptor.TransformBlock(hex, 4, 16, decrypted, 1);
                encryptor.TransformBlock(hex, 20, 16, decrypted, 17);
                encryptor.TransformBlock(hex, 20, 16, decrypted, 17);
                for (int x = 0; x < 16; x++) decrypted[17 + x] ^= hex[4 + x];

                _privKey = new byte[32];
                Array.Copy(decrypted, 1, _privKey, 0, 32);

            }  else {
                return "Not a recognized private key format";
            }
            return validateRange();
            
        }

        /// <summary>
        /// Returns error message in a string if private key is not within the valid range (2 ... N-1)
        /// </summary>
        private string validateRange() {
            Org.BouncyCastle.Math.BigInteger Db = new Org.BouncyCastle.Math.BigInteger(1, _privKey);
            BigInteger N = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1").N;
            if (Db == BigInteger.Zero || Db.CompareTo(N) >= 0) {
                return "Not a valid private key";
            }
            return null;
        }

        /// <summary>
        /// Returns true if a given string can be turned into a private key.
        /// </summary>
        public static bool IsValidPrivateKey(string key) {
            KeyPair kp = new KeyPair();
            string result = kp.constructWithKey(key, null, false);
            return (result == null);
        }

        /// <summary>
        /// Sets passphrase.  Set this before reading PrivWIF to get an encrypted private key.
        /// Set it before setting PrivWIF to decrypt an encrypted private key.
        /// </summary>
        protected string Passphrase {
            set {
                _passphrase = value;
                if (value == "") _passphrase = null;
            }
        }

        private string _passphrase = null;


        /// <summary>
        /// Provides access to the private key.
        /// </summary>
        public byte[] PrivateKeyBytes {
            get {
                byte[] rv = new byte[_privKey.Length];
                _privKey.CopyTo(rv, 0);
                return rv;
            }
            protected set {
                if (value == null || value.Length < 32 || value.Length > 33) throw new ArgumentException("Must be 32 bytes");

                _privKey = new byte[32];
                Array.Copy(value, value.Length - 32, _privKey, 0, 32);

            }
        }

        private byte[] _privKey;

        /// <summary>
        /// Computes the public key from the private key.
        /// </summary>
        protected override byte[] ComputePublicKey() {
            var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");
            ECPoint point = ps.G;
            
            Org.BouncyCastle.Math.BigInteger Db = new Org.BouncyCastle.Math.BigInteger(1, _privKey);
            ECPoint dd = point.Multiply(Db);
            

            if (_compressedPoint) {
                dd = ps.Curve.CreatePoint(dd.X.ToBigInteger(), dd.Y.ToBigInteger(), true);                
                return dd.GetEncoded();
            } else {
                byte[] pubaddr = new byte[65];
                byte[] Y = dd.Y.ToBigInteger().ToByteArray();
                Array.Copy(Y, 0, pubaddr, 64 - Y.Length + 1, Y.Length);
                byte[] X = dd.X.ToBigInteger().ToByteArray();
                Array.Copy(X, 0, pubaddr, 32 - X.Length + 1, X.Length);
                pubaddr[0] = 4;
                return pubaddr;
            }
        }

        /// <summary>
        /// Returns the private key as a string of hexadecimal digits.
        /// </summary>
        public string PrivateKeyHex {
            get {
                return Bitcoin.ByteArrayToString(PrivateKeyBytes);
            }
            protected set {
                byte[] hex =  Bitcoin.ValidateAndGetHexPrivateKey(0x80, value, 32);
                if (hex == null) throw new ApplicationException("Invalid private hex key");
                _privKey = hex;
            }
        }

        /// <summary>
        /// Returns the private key in the most preferred display format for the type.
        /// </summary>
        public virtual string PrivateKey {
            get {
                return PrivateKeyBase58;
            }
        }

        /// <summary>
        /// Getter: Returns the private key, either unencrypted if no password was set, or encrypted
        /// with the password.
        /// Setter: Accepts a private key in wallet import format.  If the private key is encrypted, the
        /// correct Passphrase must have been set, or else an ApplicationException will be thrown.
        /// </summary>
        public string PrivateKeyBase58 {
            get {

                if (_privKey.Length != 32) throw new ApplicationException("Not a valid private key");

                if (_passphrase != null && _passphrase != "") {
                    AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                    aes.KeySize = 256;
                    aes.Mode = CipherMode.ECB;

                    SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                    UTF8Encoding utf8 = new UTF8Encoding(false);
                    byte[] encryptionKey = sha256.ComputeHash(utf8.GetBytes(_passphrase));
                    aes.Key = encryptionKey;
                    ICryptoTransform encryptor = aes.CreateEncryptor();

                    byte[] rv = new byte[36];

                    encryptor.TransformBlock(_privKey, 0, 16, rv, 4);
                    encryptor.TransformBlock(_privKey, 0, 16, rv, 4);
                    byte[] interblock = new byte[16];
                    Array.Copy(rv, 4, interblock, 0, 16);
                    for (int x = 0; x < 16; x++) interblock[x] ^= _privKey[16 + x];
                    encryptor.TransformBlock(interblock, 0, 16, rv, 20);
                    encryptor.TransformBlock(interblock, 0, 16, rv, 20);

                    // put header
                    rv[0] = 0x02;
                    rv[1] = 0x05;

                    byte[] checksum = sha256.ComputeHash(utf8.GetBytes(_passphrase + "?"));

                    rv[2] = (byte)(checksum[0] & 0x7F);
                    rv[3] = (byte)(checksum[1] & 0xFE);
                    if (_compressedPoint) rv[3]++;

                    return Bitcoin.ByteArrayToBase58Check(rv);
                } else if (_compressedPoint) {
                    byte[] rv = new byte[34];
                    Array.Copy(_privKey, 0, rv, 1, 32);
                    rv[0] = 0x80;
                    rv[33] = 1;
                    return Bitcoin.ByteArrayToBase58Check(rv);
                } else {
                    byte[] rv = new byte[33];
                    Array.Copy(_privKey, 0, rv, 1, 32);
                    rv[0] = 0x80;
                    return Bitcoin.ByteArrayToBase58Check(rv);
                }
            }
            protected set {

                byte[] hex = Bitcoin.Base58CheckToByteArray(value);

                if (hex == null) {
                    throw new ApplicationException("WIF private key is not valid.");
                }


                // pywallet seems to accept keys like this... they are private keys starting with 00 or 0000 and
                // they pass the base58 check but are missing leading byte(s) that were never put into the original payload.
                // We will simply fill in the missing bytes with 00's.
                if (hex.Length == 31 || hex.Length == 32) {
                    byte[] hex2 = new byte[33];
                    hex2[0] = hex[0];
                    Array.Copy(hex, 1, hex2, 34 - hex.Length, hex.Length - 1);
                    hex = hex2;
                }

                if (hex.Length != 33) {
                    throw new ApplicationException("WIF private key is not valid (wrong byte count, should be 33, was " + hex.Length + ")");
                }

                if (hex[0] != 0x80) {
                    throw new ApplicationException("This is a valid base58 string but it has no Wallet Import Format identifier.");
                }

                _privKey = new byte[32];
                Array.Copy(hex, 1, _privKey, 0, 32);

                _address = null;

            }
        }
    }
}
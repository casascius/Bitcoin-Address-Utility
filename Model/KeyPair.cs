// Copyright 2012 Mike Caldwell (Casascius)
// This file is part of Bitcoin Address Utility.

// Bitcoin Address Utility is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// Bitcoin Address Utility is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with Bitcoin Address Utility.  If not, see http://www.gnu.org/licenses/.


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

namespace Casascius.Bitcoin {

    /// <summary>
    /// A KeyPair represents a Bitcoin address and its known private key.
    /// </summary>
    public class KeyPair : PublicKey {
        protected KeyPair() { }

        private static Int64 nonce;

        /// <summary>
        /// Creates a new key pair using the SHA256 hash of a given string as the private key.
        /// </summary>
        public static KeyPair CreateFromString(string tohash) {            
            UTF8Encoding utf8 = new UTF8Encoding(false);
            byte[] forsha = utf8.GetBytes(tohash);
            Sha256Digest sha256 = new Sha256Digest();
            sha256.BlockUpdate(forsha, 0, forsha.Length);
            byte[] thehash = new byte[32];
            sha256.DoFinal(thehash, 0);
            return new KeyPair(thehash);
        }

        /// <summary>
        /// Creates a new random key pair, using a user-provided string to add entropy to the
        /// SecureRandom generator provided by the .NET Framework.
        /// </summary>
        public static KeyPair Create(string usersalt, bool compressed=false, byte addressType = 0) {
            if (usersalt == null) usersalt = "ok, whatever";
            usersalt += DateTime.UtcNow.Ticks.ToString();

            SecureRandom sr = new SecureRandom();

            byte[] poop = Util.ComputeSha256(usersalt + nonce.ToString());
            nonce++;

            byte[] newkey = new byte[32];

            for (int i = 0; i < 32; i++) {
                long x = sr.NextLong() & long.MaxValue;
                x += poop[i];
                newkey[i] = (byte)(x & 0xff);
            }
            return new KeyPair(newkey, compressed: compressed, addressType: addressType);
        }

        /// <summary>
        /// Generates a KeyPair using a BigInteger as a private key.
        /// BigInteger is checked for appropriate range.
        /// </summary>
        public KeyPair(BigInteger bi, bool compressed = false, byte addressType = 0) {
            this.IsCompressedPoint = compressed;
            this._addressType = addressType;
            
            var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");
            if (bi.CompareTo(ps.N) >= 0 || bi.SignValue <= 0) {
                throw new ArgumentException("BigInteger is out of range of valid private keys");
            }
            byte[] bb = Util.Force32Bytes(bi.ToByteArrayUnsigned());
            PrivateKeyBytes = bb;
        }


        /// <summary>
        /// Create a Bitcoin address from a 32-byte private key
        /// </summary>
        public KeyPair(byte[] bytes, bool compressed=false, byte addressType=0) {
            if (bytes.Length == 32) {
                PrivateKeyBytes = bytes;
                this.IsCompressedPoint = compressed;
                this._addressType = addressType;
            } else {
                throw new ArgumentException("Byte array provided to KeyPair constructor must be 32 bytes long");
            }
        }

        /// <summary>
        /// Create a Bitcoin address from a key represented in a string.
        /// </summary>
        public KeyPair(string key, bool compressed=false, byte addressType=0) {
            this._addressType = addressType;
            string result = constructWithKey(key, compressed);
            if (result != null) throw new ArgumentException(result);

        }

        /// <summary>
        /// Constructs the object with string key, returning any intended exception as a string.
        /// </summary>
        private string constructWithKey(string key, bool compressed) {
            byte[] hex = Util.Base58CheckToByteArray(key);
            if (hex == null) {
                hex = Util.HexStringToBytes(key, true);
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
                IsCompressedPoint = compressed;
            } else if (hex.Length == 33 && hex[0] == 0x80) {
                // normal private key
                _privKey = new byte[32];
                Array.Copy(hex, 1, _privKey, 0, 32);
                IsCompressedPoint = false;
            } else if (hex.Length == 34 && hex[0] == 0x80 && hex[33] == 0x01) {
                // compressed private key
                _privKey = new byte[32];
                Array.Copy(hex, 1, _privKey, 0, 32);
                IsCompressedPoint = true;
            } else if (key.StartsWith("6")) {
                return "Key is encrypted, decrypt first.";
            } else {
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
            string result = kp.constructWithKey(key, false);
            return (result == null);
        }


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


            if (IsCompressedPoint) {
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
                return Util.ByteArrayToString(PrivateKeyBytes);
            }
            protected set {
                byte[] hex = Util.ValidateAndGetHexPrivateKey(0x80, value, 32);
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
        /// if a password is set or if we do not have it unencrypted.
        /// Setter: Accepts a private key in wallet import format.  If the private key is encrypted, the
        /// correct Passphrase must have been set, or else an ApplicationException will be thrown.
        /// </summary>
        public string PrivateKeyBase58 {
            get {
                if (_privKey.Length != 32) throw new ApplicationException("Not a valid private key");

                if (IsCompressedPoint) {
                    byte[] rv = new byte[34];
                    Array.Copy(_privKey, 0, rv, 1, 32);
                    rv[0] = 0x80;
                    rv[33] = 1;
                    return Util.ByteArrayToBase58Check(rv);
                } else {
                    byte[] rv = new byte[33];
                    Array.Copy(_privKey, 0, rv, 1, 32);
                    rv[0] = 0x80;
                    return Util.ByteArrayToBase58Check(rv);
                }
            }
            protected set {

                byte[] hex = Util.Base58CheckToByteArray(value);

                if (hex == null) {
                    throw new ApplicationException("WIF private key is not valid.");
                }


                // pywallet seems to produce and accept keys like this... they are private keys starting with 00 or 0000 and
                // they pass the base58 check but are missing leading byte(s) that were never put into the original payload.
                // We will simply fill in the missing bytes with 00's.
                if (hex.Length == 29 || hex.Length == 30 || hex.Length == 31 || hex.Length == 32) {
                    byte[] hex2 = new byte[33];
                    hex2[0] = hex[0];
                    Array.Copy(hex, 1, hex2, 34 - hex.Length, hex.Length - 1);
                    hex = hex2;
                }

                if (hex.Length != 33) {
                    throw new ApplicationException("WIF private key is not valid (wrong byte count, should be 33, was " + hex.Length + ")");
                }

                if (hex[0] == 0x82) {
                    this.IsCompressedPoint = true;
                } else if (hex[0] != 0x80) {
                    throw new ApplicationException("This is a valid base58 string but it has no Wallet Import Format identifier.");
                }

                _privKey = new byte[32];
                Array.Copy(hex, 1, _privKey, 0, 32);

                _address = null;

            }
        }
    }
}
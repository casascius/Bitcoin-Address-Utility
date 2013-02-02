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
using System.Drawing;
using System.Security.Cryptography;
using System.IO;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Math.EC;

namespace Casascius.Bitcoin {
    public class Util {
        public static string PassphraseToPrivHex(string passphrase) {
            return ByteArrayToString(ComputeSha256(passphrase));
        }

        public static string ByteArrayToBase58Check(byte[] ba) {

            byte[] bb = new byte[ba.Length + 4];
            Array.Copy(ba, bb, ba.Length);
            Sha256Digest bcsha256a = new Sha256Digest();
            bcsha256a.BlockUpdate(ba, 0, ba.Length);
            byte[] thehash = new byte[32];
            bcsha256a.DoFinal(thehash, 0);
            bcsha256a.BlockUpdate(thehash, 0, 32);
            bcsha256a.DoFinal(thehash, 0);
            for (int i = 0; i < 4; i++) bb[ba.Length + i] = thehash[i];
            return Base58.FromByteArray(bb);
        }


        public static byte[] ValidateAndGetHexPublicKey(string PubHex) {
            byte[] hex = GetHexBytes(PubHex, 64);

            if (hex == null || hex.Length < 64 || hex.Length > 65) {
                throw new ApplicationException("Hex is not 64 or 65 bytes.");
            }

            // if leading 00, change it to 0x80
            if (hex.Length == 65) {
                if (hex[0] == 0 || hex[0] == 4) {
                    hex[0] = 4;
                } else {
                    throw new ApplicationException("Not a valid public key");
                }
            }

            // add 0x80 byte if not present
            if (hex.Length == 64) {
                byte[] hex2 = new byte[65];
                Array.Copy(hex, 0, hex2, 1, 64);
                hex2[0] = 4;
                hex = hex2;
            }
            return hex;
        }

        public static byte[] ValidateAndGetHexPublicHash(string PubHash) {
            byte[] hex = GetHexBytes(PubHash, 20);

            if (hex == null || hex.Length != 20) {
                throw new ApplicationException("Hex is not 20 bytes.");
            }
            return hex;
        }


        public static byte[] ValidateAndGetHexPrivateKey(byte leadingbyte, string PrivHex, int desiredByteCount) {
            if (desiredByteCount != 32 && desiredByteCount != 33) throw new ApplicationException("desiredByteCount must be 32 or 33");

            byte[] hex = GetHexBytes(PrivHex, 32);

            if (hex == null || hex.Length < 32 || hex.Length > 33) {
                throw new ApplicationException("Hex is not 32 or 33 bytes.");
            }

            // if leading 00, change it to 0x80
            if (hex.Length == 33) {
                if (hex[0] == 0 || hex[0] == 0x80) {
                    hex[0] = 0x80;
                } else {
                    throw new ApplicationException("Not a valid private key");
                }
            }

            // add 0x80 byte if not present
            if (hex.Length == 32 && desiredByteCount==33) {
                byte[] hex2 = new byte[33];
                Array.Copy(hex, 0, hex2, 1, 32);
                hex2[0] = 0x80;
                hex = hex2;
            }

            if (desiredByteCount==33) hex[0] = leadingbyte;

            if (desiredByteCount == 32 && hex.Length == 33) {
                byte[] hex2 = new byte[33];
                Array.Copy(hex, 1, hex2, 0, 32);
                hex = hex2;
            }

            return hex;

        }

        /// <summary>
        /// Trims whitespace from within and outside string.
        /// Whitespace is anything non-alphanumeric that may have been inserted into a string.
        /// </summary>
        public static string Base58Trim(string base58) {
            char[] strin = base58.ToCharArray();
            char[] cc = new char[base58.Length];
            int pos = 0;
            for (int i = 0; i < base58.Length; i++) {
                char c = strin[i];
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')) {
                    cc[pos++] = c;
                }
            }
            return new String(cc, 0, pos);
        }

        /// <summary>
        /// Converts a base-58 string to a byte array, checking the checksum, and
        /// returning null if it wasn't valid.  Appending "?" to the end of the string skips
        /// the checksum calculation, but still strips the four checksum bytes from the
        /// result.
        /// </summary>
        public static byte[] Base58CheckToByteArray(string base58) {

            bool IgnoreChecksum = false;
            if (base58.EndsWith("?")) {
                IgnoreChecksum = true;
                base58 = base58.Substring(0, base58.Length - 1);
            }

            byte[] bb = Base58.ToByteArray(base58);
            if (bb == null || bb.Length < 4) return null;

            if (IgnoreChecksum == false) {
                Sha256Digest bcsha256a = new Sha256Digest();
                bcsha256a.BlockUpdate(bb, 0, bb.Length - 4);

                byte[] checksum = new byte[32];  //sha256.ComputeHash(bb, 0, bb.Length - 4);
                bcsha256a.DoFinal(checksum, 0);
                bcsha256a.BlockUpdate(checksum, 0, 32);
                bcsha256a.DoFinal(checksum, 0);

                for (int i = 0; i < 4; i++) {
                    if (checksum[i] != bb[bb.Length - 4 + i]) return null;
                }
            }

            byte[] rv = new byte[bb.Length - 4];
            Array.Copy(bb, 0, rv, 0, bb.Length - 4);
            return rv;
        }

        public static string ByteArrayToString(byte[] ba) {
            return ByteArrayToString(ba, 0, ba.Length);
        }

        public static string ByteArrayToString(byte[] ba, int offset, int count) {
            string rv = "";
            int usedcount = 0;
            for (int i = offset; usedcount < count; i++, usedcount++) {
                rv += String.Format("{0:X2}", ba[i]) + " ";
            }
            return rv;
        }




        public static byte[] GetHexBytes(string source, int minimum) {
            byte[] hex = HexStringToBytes(source);
            if (hex == null) return null;
            // assume leading zeroes if we're short a few bytes
            if (hex.Length > (minimum - 6) && hex.Length < minimum) {
                byte[] hex2 = new byte[minimum];
                Array.Copy(hex, 0, hex2, minimum - hex.Length, hex.Length);
                hex = hex2;
            }
            // clip off one overhanging leading zero if present
            if (hex.Length == minimum + 1 && hex[0] == 0) {
                byte[] hex2 = new byte[minimum];
                Array.Copy(hex, 1, hex2, 0, minimum);
                hex = hex2;

            }

            return hex;
        }


        /// <summary>
        /// Converts a hex string to bytes.  Hex chars can optionally be space-delimited, otherwise,
        /// any two contiguous hex chars are considered to be a byte.  If testingForValidHex==true,
        /// then if any invalid characters are found, the function returns null instead of bytes.
        /// </summary>
        public static byte[] HexStringToBytes(string source, bool testingForValidHex=false) {
            List<byte> bytes = new List<byte>();
            bool gotFirstChar = false;
            byte accum = 0;

            foreach (char c in source.ToCharArray()) {                
                if (c == ' ' || c == '-' || c == ':') {
                    // if we got a space, then accept it as the end if we have 1 character.
                    if (gotFirstChar) {
                        bytes.Add(accum);
                        accum = 0;
                        gotFirstChar = false;
                    }
                } else if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f')) {
                    // get the character's value
                    byte v = (byte)(c - 0x30);
                    if (c >= 'A' && c <= 'F') v = (byte)(c + 0x0a - 'A');
                    if (c >= 'a' && c <= 'f') v = (byte)(c + 0x0a - 'a');

                    if (gotFirstChar == false) {
                        gotFirstChar = true;
                        accum = v;
                    } else {
                        accum <<= 4;
                        accum += v;
                        bytes.Add(accum);
                        accum = 0;
                        gotFirstChar = false;
                    }
                } else {
                    if (testingForValidHex) return null;
                }
            }
            if (gotFirstChar) bytes.Add(accum);
            return bytes.ToArray();    
        }



        public static string PrivHexToPubHex(string PrivHex) {
            var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");
            return PrivHexToPubHex(PrivHex, ps.G);
        }

        public static string PrivHexToPubHex(string PrivHex, ECPoint point) {

            byte[] hex = ValidateAndGetHexPrivateKey(0x00, PrivHex, 33);
            if (hex == null) throw new ApplicationException("Invalid private hex key");
            Org.BouncyCastle.Math.BigInteger Db = new Org.BouncyCastle.Math.BigInteger(hex);
            ECPoint dd = point.Multiply(Db);

            byte[] pubaddr = PubKeyToByteArray(dd);
                
            return ByteArrayToString(pubaddr);

        }

        public static ECPoint PrivHexToPubKey(string PrivHex) {
            byte[] hex = ValidateAndGetHexPrivateKey(0x00, PrivHex, 33);
            if (hex == null) throw new ApplicationException("Invalid private hex key");
            Org.BouncyCastle.Math.BigInteger Db = new Org.BouncyCastle.Math.BigInteger(1, hex);
            var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");
            return ps.G.Multiply(Db);
        }

        public static ECPoint PrivKeyToPubKey(byte[] PrivKey) {
            if (PrivKey == null || PrivKey.Length > 32) throw new ApplicationException("Invalid private hex key");            
            Org.BouncyCastle.Math.BigInteger Db = new Org.BouncyCastle.Math.BigInteger(1, PrivKey);
            var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");
            return ps.G.Multiply(Db);
        }


        public static byte[] PubKeyToByteArray(ECPoint point) {
            byte[] pubaddr = new byte[65];
            byte[] Y = point.Y.ToBigInteger().ToByteArray();
            Array.Copy(Y, 0, pubaddr, 64 - Y.Length + 1, Y.Length);
            byte[] X = point.X.ToBigInteger().ToByteArray();
            Array.Copy(X, 0, pubaddr, 32 - X.Length + 1, X.Length);
            pubaddr[0] = 4;
            return pubaddr;
        }

        public static string PubHexToPubHash(string PubHex) {
            byte[] hex = ValidateAndGetHexPublicKey(PubHex);
            if (hex == null) throw new ApplicationException("Invalid public hex key");
            return PubHexToPubHash(hex);
        }

        public static string PubHexToPubHash(byte[] PubHex) {

            byte[] shaofpubkey = ComputeSha256(PubHex);

            RIPEMD160 rip = System.Security.Cryptography.RIPEMD160.Create();
            byte[] ripofpubkey = rip.ComputeHash(shaofpubkey);

            return ByteArrayToString(ripofpubkey);

        }

        public static string PubHashToAddress(string PubHash, string AddressType) {
            byte[] hex = ValidateAndGetHexPublicHash(PubHash);
            if (hex == null) throw new ApplicationException("Invalid public hex key");

            byte[] hex2 = new byte[21];
            Array.Copy(hex, 0, hex2, 1, 20);

            int cointype = 0;
            if (Int32.TryParse(AddressType, out cointype) == false) cointype = 0;

            if (AddressType == "Testnet") cointype = 111;
            if (AddressType == "Namecoin") cointype = 52;
            if (AddressType == "Litecoin") cointype = 48;
            hex2[0] = (byte)(cointype & 0xff);
            return ByteArrayToBase58Check(hex2);


        }

        public static bool PassphraseTooSimple(string passphrase) {

            int Lowercase = 0, Uppercase = 0, Numbers = 0, Symbols = 0, Spaces = 0;
            foreach (char c in passphrase.ToCharArray()) {
                if (c >= 'a' && c <= 'z') {
                    Lowercase++;
                } else if (c >= 'A' && c <= 'Z') {
                    Uppercase++;
                } else if (c >= '0' && c <= '9') {
                    Numbers++;
                } else if (c == ' ') {
                    Spaces++;
                } else {
                    Symbols++;
                }
            }

            // let mini private keys through - they won't contain words, they are nonsense characters, so their entropy is a bit better per character
            if (MiniKeyPair.IsValidMiniKey(passphrase) != 1) return false;

            if (passphrase.Length < 30 && (Lowercase < 10 || Uppercase < 3 || Numbers < 2 || Symbols < 2)) {
                return true;
            }

            return false;

        }

        public static byte[] ComputeSha256(string ofwhat) {
            UTF8Encoding utf8 = new UTF8Encoding(false);
            return ComputeSha256(utf8.GetBytes(ofwhat));
        }


        public static byte[] ComputeSha256(byte[] ofwhat) {
            Sha256Digest sha256 = new Sha256Digest();
            sha256.BlockUpdate(ofwhat, 0, ofwhat.Length);
            byte[] rv = new byte[32];
            sha256.DoFinal(rv, 0);
            return rv;
        }

        public static byte[] ComputeDoubleSha256(string ofwhat) {
            UTF8Encoding utf8 = new UTF8Encoding(false);
            return ComputeDoubleSha256(utf8.GetBytes(ofwhat));
        }

        public static byte[] ComputeDoubleSha256(byte[] ofwhat) {
            Sha256Digest sha256 = new Sha256Digest();
            sha256.BlockUpdate(ofwhat, 0, ofwhat.Length);
            byte[] rv = new byte[32];
            sha256.DoFinal(rv, 0);
            sha256.BlockUpdate(rv, 0, rv.Length);           
            sha256.DoFinal(rv, 0);
            return rv;
        }

        public static Int64 nonce = 0;

        public static byte[] Force32Bytes(byte[] inbytes) {
            if (inbytes.Length == 32) return inbytes;
            byte[] rv = new byte[32];
            if (inbytes.Length > 32) {
                Array.Copy(inbytes, inbytes.Length - 32, rv, 0, 32);
            } else {
                Array.Copy(inbytes, 0, rv, 32 - inbytes.Length, inbytes.Length);
            }
            return rv;
        }

        /// <summary>
        /// Extension for cloning a byte array
        /// </summary>
        public static byte[] CloneByteArray(byte[] ba) {
            if (ba == null) return null;
            byte[] copy = new byte[ba.Length];
            Array.Copy(ba, copy, ba.Length);
            return copy;
        }

        /// <summary>
        /// Extension for cloning a portion of a byte array
        /// </summary>
        public static byte[] CloneByteArray(byte[] ba, int offset, int length) {
            if (ba == null) return null;
            byte[] copy = new byte[length];
            Array.Copy(ba, offset, copy, 0, length);
            return copy;
        }

        public static byte[] ConcatenateByteArrays(params byte[][] bytearrays) {
            int totalLength = 0;
            for (int i = 0; i < bytearrays.Length; i++) totalLength += bytearrays[i].Length;
            byte[] rv = new byte[totalLength];
            int idx = 0;
            for (int i = 0; i < bytearrays.Length; i++) {
                Array.Copy(bytearrays[i], 0, rv, idx, bytearrays[i].Length);
                idx += bytearrays[i].Length;
            }
            return rv;
        }


    }



}

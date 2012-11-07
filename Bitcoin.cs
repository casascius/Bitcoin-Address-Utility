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
using ThoughtWorks.QRCode.Codec;
using System.Text.RegularExpressions;

namespace BtcAddress {
    public class Bitcoin {



        public static string PassphraseToPrivHex(string passphrase) {

            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();            
            UTF8Encoding utf8 = new UTF8Encoding(false);
            byte[] forsha = utf8.GetBytes(passphrase);
            byte[] shahash = sha256.ComputeHash(forsha);
            return ByteArrayToString(shahash);
        }








        public static string ByteArrayToBase58Check(byte[] ba) {

            byte[] bb = new byte[ba.Length + 4];
            Array.Copy(ba, bb, ba.Length);
            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
            byte[] thehash = sha256.ComputeHash(ba);
            thehash = sha256.ComputeHash(thehash);
            for (int i = 0; i < 4; i++) bb[ba.Length + i] = thehash[i];
            return ByteArrayToBase58(bb);
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
        /// Converts a base-58 string to a byte array, returning null if it wasn't valid.
        /// </summary>
        public static byte[] Base58CheckToByteArray(string base58) {

            Org.BouncyCastle.Math.BigInteger bi2 = new Org.BouncyCastle.Math.BigInteger("0");
            string b58 = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

            bool IgnoreChecksum = false;

            foreach (char c in base58) {
                if (b58.IndexOf(c) != -1) {
                    bi2 = bi2.Multiply(new Org.BouncyCastle.Math.BigInteger("58"));
                    bi2 = bi2.Add(new Org.BouncyCastle.Math.BigInteger(b58.IndexOf(c).ToString()));
                } else if (c == '?') {
                    IgnoreChecksum = true;
                } else {
                    return null;
                }
            }

            byte[] bb = bi2.ToByteArrayUnsigned();

            // interpret leading '1's as leading zero bytes
            foreach (char c in base58) {
                if (c != '1') break;
                byte[] bbb = new byte[bb.Length + 1];
                Array.Copy(bb, 0, bbb, 1, bb.Length);
                bb = bbb;
            }

            if (bb.Length < 4) return null;

            if (IgnoreChecksum == false) {
                SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                byte[] checksum = sha256.ComputeHash(bb, 0, bb.Length - 4);
                checksum = sha256.ComputeHash(checksum);
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

        public static string ByteArrayToBase58(byte[] ba) {
            Org.BouncyCastle.Math.BigInteger addrremain = new Org.BouncyCastle.Math.BigInteger(1, ba);

            Org.BouncyCastle.Math.BigInteger big0 = new Org.BouncyCastle.Math.BigInteger("0");
            Org.BouncyCastle.Math.BigInteger big58 = new Org.BouncyCastle.Math.BigInteger("58");

            string b58 = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

            string rv = "";

            while (addrremain.CompareTo(big0) > 0) {
                int d = Convert.ToInt32(addrremain.Mod(big58).ToString());
                addrremain = addrremain.Divide(big58);
                rv = b58.Substring(d, 1) + rv;
            }

            // handle leading zeroes
            foreach (byte b in ba) {
                if (b != 0) break;
                rv = "1" + rv;

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
                if (c == ' ') {
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

            byte[] hex = Bitcoin.ValidateAndGetHexPrivateKey(0x00, PrivHex, 33);
            if (hex == null) throw new ApplicationException("Invalid private hex key");
            Org.BouncyCastle.Math.BigInteger Db = new Org.BouncyCastle.Math.BigInteger(hex);
            ECPoint dd = point.Multiply(Db);

            byte[] pubaddr = PubKeyToByteArray(dd);
                
            return Bitcoin.ByteArrayToString(pubaddr);

        }

        public static ECPoint PrivHexToPubKey(string PrivHex) {
            byte[] hex = Bitcoin.ValidateAndGetHexPrivateKey(0x00, PrivHex, 33);
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
            byte[] hex = Bitcoin.ValidateAndGetHexPublicKey(PubHex);
            if (hex == null) throw new ApplicationException("Invalid public hex key");
            return PubHexToPubHash(hex);
        }

        public static string PubHexToPubHash(byte[] PubHex) {

            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
            byte[] shaofpubkey = sha256.ComputeHash(PubHex);

            RIPEMD160 rip = System.Security.Cryptography.RIPEMD160.Create();
            byte[] ripofpubkey = rip.ComputeHash(shaofpubkey);

            return Bitcoin.ByteArrayToString(ripofpubkey);

        }

        public static string PubHashToAddress(string PubHash, string AddressType) {
            byte[] hex = Bitcoin.ValidateAndGetHexPublicHash(PubHash);
            if (hex == null) throw new ApplicationException("Invalid public hex key");

            byte[] hex2 = new byte[21];
            Array.Copy(hex, 0, hex2, 1, 20);

            int cointype = 0;
            if (Int32.TryParse(AddressType, out cointype) == false) cointype = 0;

            if (AddressType == "Testnet") cointype = 111;
            if (AddressType == "Namecoin") cointype = 52;
            if (AddressType == "Litecoin") cointype = 48;
            hex2[0] = (byte)(cointype & 0xff);
            return Bitcoin.ByteArrayToBase58Check(hex2);


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
        /// Encodes a QR code, making the best choice based on string length
        /// (apparently not provided by QR lib?)
        /// </summary>
        public static Bitmap EncodeQRCode(string what) {
            if (what==null || what=="") return null;

            // Determine if we can use alphanumeric encoding (e.g. public key hex)
            Regex r = new Regex("^[0-9A-F]{63,154}$");
            bool IsAlphanumeric  = r.IsMatch(what);

            QRCodeEncoder qr = new QRCodeEncoder();
            if (IsAlphanumeric) {
                qr.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
                if (what.Length > 154) {
                    return null;
                } else if (what.Length > 67) {
                    // 5L is good to 154 alphanumeric characters
                    qr.QRCodeVersion = 5;
                    qr.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                } else {
                    // 4Q is good to 67 alphanumeric characters
                    qr.QRCodeVersion = 4;
                    qr.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                }
            } else {
                if (what.Length > 62) {
                    // We don't intend to encode any alphanumeric strings longer than private keys
                    return null;
                } else if (what.Length > 34) {
                    // 4M is good to 62 characters
                    qr.QRCodeVersion = 4;
                    qr.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                } else if (what.Length > 32) {
                    // 4H is good to 34 characters
                    qr.QRCodeVersion = 4;
                    qr.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                } else {
                    // 3Q is good to 32 characters
                    qr.QRCodeVersion = 3;
                    qr.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                }
            }

            return qr.Encode(what);
        }

    }



}

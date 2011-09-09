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

namespace BtcAddress {
    public class Bitcoin {



        public static string PassphraseToPrivHex(string passphrase) {

            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();            
            UTF8Encoding utf8 = new UTF8Encoding(false);
            byte[] forsha = utf8.GetBytes(passphrase);
            byte[] shahash = sha256.ComputeHash(forsha);
            return ByteArrayToString(shahash);
        }

        /// <summary>
        /// Returns 1 if candidate is a valid Mini Private Key per rules described in
        /// Bitcoin Wiki article "Mini private key format".
        /// Zero or negative indicates not a valid Mini Private Key.
        /// -1 means well formed but fails typo check.
        /// </summary>
        public static int IsValidMiniKey(string candidate) {
            if (candidate.Length != 22) return 0;
            if (candidate.StartsWith("S")==false) return 0;
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("^S[1-9A-HJ-NP-Za-km-z]{21}$");
            if (reg.IsMatch(candidate) == false) return 0;
            ASCIIEncoding ae = new ASCIIEncoding();
            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
            byte[] ahash = sha256.ComputeHash(ae.GetBytes(candidate + "?")); // first round
            if (ahash[0] == 0) return 1;
            for (int ct = 0; ct < 716; ct++) ahash = sha256.ComputeHash(ahash); // second thru 717th
            if (ahash[0] == 0) return 1;
            return -1;
        }



        public static string PrivWIFtoPrivHex(string PrivWIF) {
            byte[] hex = Base58ToByteArray(PrivWIF);
            /*
            if (hex == null) {
                
                int L = PrivWIF.Length;
                if (L >= 50 && L <= 52) {
                    if (MessageBox.Show("Private key is not valid.  Attempt to correct?", "Invalid address", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                        CorrectWIF();
                        return;
                    }
                } else {
                    throw new ApplicationException("WIF private key is not valid.");
                }
                return;
            }
            */
            if (hex==null) {
                throw new ApplicationException("WIF private key is not valid.");            
            }
            
            if (hex.Length != 33) {
                throw new ApplicationException("WIF private key is not valid (wrong byte count, should be 33, was " + hex.Length + ")");            
            }

            return ByteArrayToString(hex, 1, 32);

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
                return null;
            }

            // if leading 00, change it to 0x80
            if (hex.Length == 65) {
                if (hex[0] == 0 || hex[0] == 4) {
                    hex[0] = 4;
                } else {
                    throw new ApplicationException("Not a valid public key");
                    return null;
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
                return null;
            }
            return hex;
        }


        public static byte[] ValidateAndGetHexPrivateKey(byte leadingbyte, string PrivHex) {
            byte[] hex = GetHexBytes(PrivHex, 32);

            if (hex == null || hex.Length < 32 || hex.Length > 33) {
                throw new ApplicationException("Hex is not 32 or 33 bytes.");
                return null;
            }

            // if leading 00, change it to 0x80
            if (hex.Length == 33) {
                if (hex[0] == 0 || hex[0] == 0x80) {
                    hex[0] = 0x80;
                } else {
                    throw new ApplicationException("Not a valid private key");
                    return null;
                }
            }

            // add 0x80 byte if not present
            if (hex.Length == 32) {
                byte[] hex2 = new byte[33];
                Array.Copy(hex, 0, hex2, 1, 32);
                hex2[0] = 0x80;
                hex = hex2;
            }

            hex[0] = leadingbyte;
            return hex;

        }



        public static byte[] Base58ToByteArray(string base58) {

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
            byte[] hex = GetHexBytes(source);
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


        public static byte[] GetHexBytes(string source) {


            List<byte> bytes = new List<byte>();
            // copy s into ss, adding spaces between each byte
            string s = source;
            string ss = "";
            int currentbytelength = 0;
            foreach (char c in s.ToCharArray()) {
                if (c == ' ') {
                    currentbytelength = 0;
                } else {
                    currentbytelength++;
                    if (currentbytelength == 3) {
                        currentbytelength = 1;
                        ss += ' ';
                    }
                }
                ss += c;
            }

            foreach (string b in ss.Split(' ')) {
                int v = 0;
                if (b.Trim() == "") continue;
                foreach (char c in b.ToCharArray()) {
                    if (c >= '0' && c <= '9') {
                        v *= 16;
                        v += (c - '0');

                    } else if (c >= 'a' && c <= 'f') {
                        v *= 16;
                        v += (c - 'a' + 10);
                    } else if (c >= 'A' && c <= 'F') {
                        v *= 16;
                        v += (c - 'A' + 10);
                    }

                }
                v &= 0xff;
                bytes.Add((byte)v);
            }
            return bytes.ToArray();
        }


        public static string PrivHexToWIF(string PrivHex) {

            byte[] hex = Bitcoin.ValidateAndGetHexPrivateKey(0x80,PrivHex);
            if (hex == null) throw new ApplicationException("Invalid private hex key");
            return Bitcoin.ByteArrayToBase58Check(hex);

        }

        public static string PrivHexToPubHex(string PrivHex) {
            byte[] hex = Bitcoin.ValidateAndGetHexPrivateKey(0x00, PrivHex);
            if (hex == null) throw new ApplicationException("Invalid private hex key");
            var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");
            Org.BouncyCastle.Math.BigInteger Db = new Org.BouncyCastle.Math.BigInteger(hex);
            ECPoint dd = ps.G.Multiply(Db);

            byte[] pubaddr = new byte[65];
            byte[] Y = dd.Y.ToBigInteger().ToByteArray();
            Array.Copy(Y, 0, pubaddr, 64 - Y.Length + 1, Y.Length);
            byte[] X = dd.X.ToBigInteger().ToByteArray();
            Array.Copy(X, 0, pubaddr, 32 - X.Length + 1, X.Length);
            pubaddr[0] = 4;

            return Bitcoin.ByteArrayToString(pubaddr);



        }


        public static string PubHexToPubHash(string PubHex) {
            byte[] hex = Bitcoin.ValidateAndGetHexPublicKey(PubHex);
            if (hex == null) throw new ApplicationException("Invalid public hex key");

            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
            byte[] shaofpubkey = sha256.ComputeHash(hex);

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
            if (IsValidMiniKey(passphrase) != 1) return false;

            if (passphrase.Length < 30 && (Lowercase < 10 || Uppercase < 3 || Numbers < 2 || Symbols < 2)) {
                return true;
            }

            return false;

        }



    }
}

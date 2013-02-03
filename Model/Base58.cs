using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Casascius.Bitcoin {
    public class Base58 {
        /// <summary>
        /// Converts a base-58 string to a byte array, returning null if it wasn't valid.
        /// </summary>
        public static byte[] ToByteArray(string base58) {
            Org.BouncyCastle.Math.BigInteger bi2 = new Org.BouncyCastle.Math.BigInteger("0");
            string b58 = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

            foreach (char c in base58) {
                if (b58.IndexOf(c) != -1) {
                    bi2 = bi2.Multiply(new Org.BouncyCastle.Math.BigInteger("58"));
                    bi2 = bi2.Add(new Org.BouncyCastle.Math.BigInteger(b58.IndexOf(c).ToString()));
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

            return bb;
        }

        public static string FromByteArray(byte[] ba) {
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

    }
}

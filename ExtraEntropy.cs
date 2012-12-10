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

    /// <summary>
    /// Manages getting entropy from timer, keyboard, and other unpredictable events.
    /// This is probably unnecessary, but the more the better.
    /// </summary>
    public class ExtraEntropy {

        private static volatile string entropystring = DateTime.Now.Ticks.ToString();

        private static object LockObject = new object();

        public static void AddExtraEntropy(string what) {
            lock (LockObject) {
                entropystring += what;
                if (entropystring.Length > 300) {
                    SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                    UTF8Encoding utf8 = new UTF8Encoding(false);
                    entropystring = BitConverter.ToString(sha256.ComputeHash(utf8.GetBytes(entropystring)));
                }
            }
        }

        public static string GetEntropy() {
            lock (LockObject) {
                string rv = entropystring;
                AddExtraEntropy(DateTime.Now.Ticks.ToString());
                return rv;
            }
        }

    }
}

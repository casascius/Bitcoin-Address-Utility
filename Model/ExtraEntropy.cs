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
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Math.EC;


namespace Casascius.Bitcoin {

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
                    entropystring = BitConverter.ToString(Util.ComputeSha256(entropystring));
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

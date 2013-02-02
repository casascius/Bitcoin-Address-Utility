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
using System.IO;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Math.EC;

namespace Casascius.Bitcoin {

    public class MiniKeyPair : KeyPair {
		
        public static MiniKeyPair CreateDeterministic(string seed) {

            // flow:
            // 1. take SHA256 of seed to yield 32 bytes
            // 2. base58-encode those 32 bytes as though it were a regular private key. now we have 51 characters.
            // 3. remove all instances of the digit 1. (likely source of typos)
            // 4. take 29 characters starting with position 4
            //    (this is to skip those first characters of a base58check-encoded private key with low entropy)
            // 5. test to see if it matches the typo check.  while it does not, increment and try again.
            UTF8Encoding utf8 = new UTF8Encoding(false);
            byte[] sha256ofseed = Util.ComputeSha256(seed);

            string asbase58 = new KeyPair(sha256ofseed).PrivateKeyBase58.Replace("1","");

            string keytotry = "S" + asbase58.Substring(4, 29);
            char[] chars = keytotry.ToCharArray();
            char[] charstest = (keytotry + "?").ToCharArray();
            
            while (Util.ComputeSha256(utf8.GetBytes(charstest))[0] != 0) {
                // As long as key doesn't pass typo check, increment it.
                for (int i = chars.Length - 1; i >= 0; i--) {
                    char c = chars[i];
                    if (c == '9') {
                        charstest[i] = chars[i] = 'A';                        
                        break;
                    } else if (c == 'H') {
                        charstest[i] = chars[i] = 'J';
                        break;
                    } else if (c == 'N') {
                        charstest[i] = chars[i] = 'P';
                        break;
                    } else if (c == 'Z') {
                        charstest[i] = chars[i] = 'a';
                        break;
                    } else if (c == 'k') {
                        charstest[i] = chars[i] = 'm';
                        break;
                    } else if (c == 'z') {
                        charstest[i] = chars[i] = '2';
                        // No break - let loop increment prior character.
                    } else {
                        charstest[i] = chars[i] = ++c;
                        break;
                    }
                }
            }
            return new MiniKeyPair(new String(chars));
        }

        /// <summary>
        /// Create a new random MiniKey.
        /// Entropy is taken from .NET's SecureRandom, the system clock,
        /// and any optionally provided salt.
        /// </summary>
        public static MiniKeyPair CreateRandom(string usersalt) {
            if (usersalt == null) usersalt = "ok, whatever";
            usersalt += DateTime.UtcNow.Ticks.ToString();
            SecureRandom sr = new SecureRandom();
            char[] chars = new char[64];
            for (int i = 0; i < 64; i++) {
                chars[i] = (char)(32 + (sr.NextInt() % 64));
            }
            return CreateDeterministic(usersalt + new String(chars));
        }


        public MiniKeyPair(string key) {
            MiniKey = key;
        }

        /// <summary>
        /// Returns the private key in the most preferred display format for the type.
        /// </summary>
        public override string PrivateKey {
            get {
                return MiniKey;
            }
        }

        public string MiniKey {
            get {
                return _minikey;
            }
            protected set {
                _minikey = value;
                if (value == null) {
                    PrivateKeyBytes = null;
                } else {
                    if (IsValidMiniKey(value) <= 0) {
                        throw new ApplicationException("Not a valid minikey");
                    }
                    _minikey = value;
                    // Setting PrivateKeyBytes sets up delegates so the public key, hash160, and
                    // bitcoin address can be computed upon demand.
                    PrivateKeyBytes = Util.ComputeSha256(value);
                }
            }
        }

        private string _minikey;

        /// <summary>
        /// Returns 1 if candidate is a valid Mini Private Key per rules described in
        /// Bitcoin Wiki article "Mini private key format".
        /// Zero or negative indicates not a valid Mini Private Key.
        /// -1 means well formed but fails typo check.
        /// </summary>
        public static int IsValidMiniKey(string candidate) {
            if (candidate.Length != 22 && candidate.Length != 26 && candidate.Length != 30) return 0;
            if (candidate.StartsWith("S") == false) return 0;
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("^S[1-9A-HJ-NP-Za-km-z]{21,29}$");
            if (reg.IsMatch(candidate) == false) return 0;
            byte[] ahash = Util.ComputeSha256(candidate + "?"); // first round
            if (ahash[0] == 0) return 1;
            // for (int ct = 0; ct < 716; ct++) ahash = sha256.ComputeHash(ahash); // second thru 717th
            // if (ahash[0] == 0) return 1;
            return -1;
        }

    }
}
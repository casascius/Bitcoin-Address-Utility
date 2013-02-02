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
using System.Text.RegularExpressions;

namespace Casascius.Bitcoin {

    public class StringInterpreter {

        /// <summary>
        /// Scans a string for any valid Base58Check-encoded substrings, and returns them as objects.
        /// </summary>
        public static List<object> InterpretBatch(string what, bool compressed = false, byte addressType = 0) {
            int biggest_anticipated_string = 100;
            char[] curstring = new char[biggest_anticipated_string];
            int curstringidx = 0;
            char[] inchars = what.ToCharArray();
            int incharcount = inchars.Length;

            List<object> returnList = new List<object>();
            HashSet<string> seenStrings = new HashSet<string>();

            // 123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz
            for (int i = 0; i < incharcount; i++) {
                char c = inchars[i];
                bool isbase58 = false;
                if (c >= '1' && c <= '9') {
                    isbase58 = true;
                } else if (c >= 'A' && c <= 'H') {
                    isbase58 = true;
                } else if (c >= 'J' && c <= 'N') {
                    isbase58 = true;
                } else if (c >= 'P' && c <= 'Z') {
                    isbase58 = true;
                } else if (c >= 'a' && c <= 'k') {
                    isbase58 = true;
                } else if (c >= 'm' && c <= 'z') {
                    isbase58 = true;
                } else {
                    // Not a base58 character
                }

                if (isbase58) {
                    curstring[curstringidx++] = c;
                    if (curstringidx >= biggest_anticipated_string) curstringidx--;
                }

                // Should we interpret curstring?
                if (curstringidx > 0) {
                    if (isbase58 == false || i == (incharcount - 1)) {
                        string trystring = new string(curstring, 0, curstringidx);
                        // Use the hashset to avoid interpreting the same string more than once.
                        if (seenStrings.Add(trystring)) {
                            // add returns true to indicate added to the HashSet, false indicates it was already there
                            object interpretation = Interpret(trystring, compressed, addressType);
                            if (interpretation != null) returnList.Add(interpretation);
                        }
                        curstringidx = 0;
                    }
                }
            }
            return returnList;
        }

        /// <summary>
        /// Interprets a string to automatically detect a type of Bitcoin-related object.
        /// compressed and addresstype are only considered when the object doesn't define these itself.
        /// </summary>
        public static object Interpret(string what, bool compressed = false, byte addressType = 0) {
            if (what == null) return null;
            
            what = what.Trim();

            // Is the string interpretable as base58?
            byte[] hex = Util.Base58CheckToByteArray(what);

            if (hex != null) {
                try {
                    switch (hex.Length) {
                        case 21:
                            // It's an address of some sort.
                            return new AddressBase(what);
                        case 31:
                        case 32:
                        case 33:
                        case 34:
                            // Unencrypted private key
                            return new KeyPair(what);
                        case 36:
                            // these pairs aren't decided by length alone,
                            // but the constructors will throw an exception if they
                            // don't contain valid key material.
                            return new ShaPassphraseKeyPair(what);                        
                        case 39:
                            return new Bip38KeyPair(what);
                        case 49:
                            return new Bip38Intermediate(what, Bip38Intermediate.Interpretation.IntermediateCode);
                        case 51:
                            return new Bip38Confirmation(what);
                    }
                } catch {}
                // If a constructor didn't like something, then don't return anything.                
            }


            hex = Util.HexStringToBytes(what, true);
            if (hex != null) {
                try {
                    switch (hex.Length) {
                        case 33:
                        case 65:
                            return new PublicKey(hex);
                        case 20:
                            return new AddressBase(hex, addressType);
                        case 21: // hash160
                            return new AddressBase(hex);
                        case 30:
                        case 31:
                        case 32:
                            return new KeyPair(hex, compressed: compressed, addressType: addressType);
                    }
                } catch { }
            }

            if (MiniKeyPair.IsValidMiniKey(what) == 1) return new MiniKeyPair(what);

            return null;

        }
    }
}

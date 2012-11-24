using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BtcAddress {

    public class StringInterpreter {

        /// <summary>
        /// Interprets a string to automatically detect a type of Bitcoin-related object.
        /// compressed and addresstype are only considered when the object doesn't define these itself.
        /// </summary>
        public static object Interpret(string what, bool compressed=false, byte addressType=0) {
            if (what == null || what == "") return null;

            // Is the string interpretable as base58?
            byte[] hex = Bitcoin.Base58CheckToByteArray(what);

            if (hex != null) {
                try {
                    switch (hex.Length) {
                        case 21:
                            // It's an address of some sort.
                            return new Address(what);
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
                    }
                } catch {}
                // If a constructor didn't like something, then don't return anything.                
            }


            hex = Bitcoin.HexStringToBytes(what, true);
            if (hex != null) {
                try {
                    switch (hex.Length) {
                        case 33:
                        case 65:
                            return new PublicKey(hex);
                        case 20:
                            return new Address(hex, addressType);
                        case 21: // hash160
                            return new Address(hex);
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

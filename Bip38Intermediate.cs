using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Math.EC;
using CryptSharp.Utility;
using Org.BouncyCastle.Math;


namespace BtcAddress {
    
    /// <summary>
    /// Represents a passphrase converted to an EC-point for use with Bip38KeyPair.
    /// </summary>
    public class Bip38Intermediate {


        private byte[] _ownersalt;
        public byte[] ownersalt {
            get {
                return bacopy(_ownersalt);
            }
        }

        private byte[] _passpoint;
        public byte[] passpoint {
            get {
                return bacopy(_passpoint);
            }
        }


        private byte[] _passfactor;
        public byte[] passfactor {
            get {
                return bacopy(_passfactor);
            }
        }


        private byte[] bacopy(byte[] ba) {
            if (ba == null) return null;
            byte[] copy = new byte[ba.Length];
            Array.Copy(ba, copy, ba.Length);
            return copy;
        }


        public string Code { get; private set; }

        private static byte[] magic = new byte[] { 0x2C, 0xE9, 0xB3, 0xE1, 0xFF, 0x39, 0xE2, 0x53 };

        public enum Interpretation {
            Passphrase,
            IntermediateCode
        }

        /// <summary>
        /// Create an intermediate from a passphrase or intermediate code
        /// </summary>
        public Bip38Intermediate(string fromstring, Interpretation interpretation) {
            if (interpretation == Interpretation.IntermediateCode) {
                createFromCode(fromstring);
            } else {
                _ownersalt = new byte[8];

                // Get 8 random bytes to use as salt
                SecureRandom sr = new SecureRandom();
                sr.NextBytes(_ownersalt);
                createFromPassphrase(fromstring, _ownersalt);
            }
        }

        /// <summary>
        /// Creates a Bip38Intermediate with a specific ownersalt, useful for decryption
        /// </summary>
        public Bip38Intermediate(string passphrase, byte[] ownersalt) {
            if (ownersalt == null || ownersalt.Length != 8) {
                throw new ArgumentException("ownersalt is not valid");
            }

            if (passphrase == null) {
                throw new ArgumentException("Passphrase is required");
            }

            createFromPassphrase(passphrase, ownersalt);

        }

        private void createFromCode(string code) {
            if (code == null || code == "") {
                throw new ArgumentException("Intermediate passphrase code is required");
            }

            // get passphrase code
            byte[] ppcode = Bitcoin.Base58CheckToByteArray(code);
            if (ppcode == null) {
                throw new ArgumentException("Intermediate passphrase code is not valid.");
            }

            // check length
            if (ppcode.Length != 49) {
                throw new ArgumentException("This is not an intermediate passphrase code.");
            }

            // check magic
            for (int i = 0; i < 8; i++) {
                if (magic[i] != ppcode[i]) {
                    throw new ArgumentException("This is not an intermediate passphrase code.");
                }
            }

            // get ownersalt and passpoint
            _ownersalt = new byte[8];
            _passpoint = new byte[33];
            Array.Copy(ppcode, 8, _ownersalt, 0, 8);
            Array.Copy(ppcode, 16, _passpoint, 0, 33);
            this.Code = code;

            // ensure that passpoint can be turned into a valid ECPoint
            PublicKey pk = new PublicKey(_passpoint);
        }

        /// <summary>
        /// Initialize the intermediate from a passphrase
        /// </summary>
        private void createFromPassphrase(string passphrase, byte[] ownersalt) {
            if (passphrase == null || passphrase == "") {
                throw new ArgumentException("Passphrase is required");
            }

            _ownersalt = ownersalt;

            UTF8Encoding utf8 = new UTF8Encoding(false);
            _passfactor = new byte[32];
            SCrypt.ComputeKey(utf8.GetBytes(passphrase), _ownersalt, 16384, 8, 8, 8, _passfactor);

            // make a compressed key out of it just by using the existing bitcoin address classes
            KeyPair kp = new KeyPair(_passfactor, compressed: true);

            _passpoint = kp.PublicKeyBytes;

            byte[] result = new byte[49];

            // 8 bytes are a constant, responsible for making the result start with the characters "passphrase"
            Array.Copy(magic, 0, result, 0, 8);
            Array.Copy(_ownersalt, 0, result, 8, 8);
            Array.Copy(_passpoint, 0, result, 16, 33);
            Code = Bitcoin.ByteArrayToBase58Check(result);

        }


    }
}

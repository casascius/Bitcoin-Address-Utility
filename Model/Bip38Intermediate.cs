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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
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
using CryptSharp.Utility;
using Org.BouncyCastle.Math;


namespace Casascius.Bitcoin {
    
    /// <summary>
    /// Represents a passphrase converted to an EC-point for use with Bip38KeyPair.
    /// </summary>
    public class Bip38Intermediate : Bip38Base {



        private byte[] _passpoint;
        public byte[] passpoint {
            get {
                return Util.CloneByteArray(_passpoint);
            }
        }


        private byte[] _passfactor;
        public byte[] passfactor {
            get {
                return Util.CloneByteArray(_passfactor);
            }
        }



        private byte[] derivedBytes = null;

        public string Code { get; private set; }


        private static byte[] magic = new byte[] { 0x2C, 0xE9, 0xB3, 0xE1, 0xFF, 0x39, 0xE2, 0x51};

        private bool _lotSequencePresent;

        public override bool LotSequencePresent {
            get {
                return _lotSequencePresent;
            }
        }

        public enum Interpretation {
            Passphrase,
            IntermediateCode
        }

        /// <summary>
        /// Create an intermediate from a passphrase or intermediate code
        /// </summary>
        public Bip38Intermediate(string fromstring, Interpretation interpretation, int startingSequenceNumber = 0) {
            if (interpretation == Interpretation.IntermediateCode) {
                createFromCode(fromstring);
            } else {
                _ownerentropy = new byte[8];

                // Get 8 random bytes to use as salt
                SecureRandom sr = new SecureRandom();
                sr.NextBytes(_ownerentropy);
				// set lot number between 100000 and 999999, and sequence number to 1
				long x = (sr.NextLong () % 900000L + 100000L) * 4096L + (long)startingSequenceNumber;
				for (int i=7; i>=4; i--) {
					_ownerentropy[i] = (byte)(x & 0xFF);
					x >>= 8;
				}
                createFromPassphrase(fromstring, _ownerentropy, true);
            }
        }

        /// <summary>
        /// Creates a Bip38Intermediate with specific ownerentropy, useful for decryption
        /// </summary>
        public Bip38Intermediate(string passphrase, byte[] ownerentropy, bool includeHashStep) {
            if (ownerentropy == null || ownerentropy.Length != 8) {
                throw new ArgumentException("ownersalt is not valid");
            }

            if (passphrase == null) {
                throw new ArgumentException("Passphrase is required");
            }

            createFromPassphrase(passphrase, ownerentropy, includeHashStep);

        }

        /// <summary>
        /// Creates another intermediate code with the same passphrase but differing only by ownersaltB.
        /// This takes less time.
        /// </summary>
        public Bip38Intermediate(Bip38Intermediate prevchain) {
            if (prevchain.LotSequencePresent == false) {
                throw new ArgumentException("Can only create from an intermediate code that includes the hash step.");
            }
            _ownerentropy = Util.CloneByteArray(prevchain._ownerentropy);
	
            this._lotSequencePresent = true;

            // increment ownersaltB
            _ownerentropy[7]++;
            if (_ownerentropy[7] == 0) {
                _ownerentropy[6]++;
                if (_ownerentropy[6] == 0) {
                    _ownerentropy[5]++;
                    if (_ownerentropy[5] == 0) {
                        _ownerentropy[4]++;
                    }
                }
            }

			derivedBytes = prevchain.derivedBytes;
			byte[] prefactorB = new byte[32 + _ownerentropy.Length];
			Array.Copy(derivedBytes, 0, prefactorB, 0, 32);
			Array.Copy(_ownerentropy, 0, prefactorB, 32, _ownerentropy.Length);
			_passfactor = Util.ComputeDoubleSha256(prefactorB);

            computeCode();        
        }

        private void createFromCode(string code) {
            if (code == null || code == "") {
                throw new ArgumentException("Intermediate passphrase code is required");
            }

            // get passphrase code
            byte[] ppcode = Util.Base58CheckToByteArray(code);
            if (ppcode == null) {
                throw new ArgumentException("Intermediate passphrase code is not valid.");
            }

            // check length
            if (ppcode.Length != 49) {
                throw new ArgumentException("This is not an intermediate passphrase code.");
            }

            // check magic
            for (int i = 0; i < 7; i++) {
                // skip magic check for i[8] which is allowed to be 0x53 for deprecated intermediate code
                if (magic[i] != ppcode[i]) {
                    throw new ArgumentException("This is not an intermediate passphrase code.");
                }
            }

            if (ppcode[7] == 0x51) {
                this._lotSequencePresent = true;
            } else if (ppcode[7] != 0x53) {
                throw new ArgumentException("This is not an intermediate passphrase code.");
            }

            // get ownersalt and passpoint
            _ownerentropy = new byte[8];
            _passpoint = new byte[33];
            Array.Copy(ppcode, 8, _ownerentropy, 0, 8);
            Array.Copy(ppcode, 16, _passpoint, 0, 33);
            this.Code = code;

            // ensure that passpoint can be turned into a valid ECPoint
            new PublicKey(_passpoint);
        }

        /// <summary>
        /// Initialize the intermediate from a passphrase
        /// </summary>
        private void createFromPassphrase(string passphrase, byte[] existingownerentropy, bool entropyContainsLotSequence) {
            if (passphrase == null || passphrase == "") {
                throw new ArgumentException("Passphrase is required");
            }

            if (existingownerentropy.Length != 8) {
                throw new ArgumentException("existingownerentropy must be 8 bytes");
            }

            _ownerentropy = existingownerentropy;
            this._lotSequencePresent = entropyContainsLotSequence;

            UTF8Encoding utf8 = new UTF8Encoding(false);
            byte[] prefactorA = new byte[32];
            SCrypt.ComputeKey(utf8.GetBytes(passphrase), ownersalt, 16384, 8, 8, 8, prefactorA);

            if (LotSequencePresent) {
                derivedBytes = prefactorA;
                byte[] prefactorB = new byte[32 + _ownerentropy.Length];
                Array.Copy(prefactorA, 0, prefactorB, 0, 32);
                Array.Copy(_ownerentropy, 0, prefactorB, 32, _ownerentropy.Length);
                _passfactor = Util.ComputeDoubleSha256(prefactorB);
            } else {
                _passfactor = prefactorA;
            }

            computeCode();

        }

        private void computeCode() {
            // make a compressed key out of it just by using the existing bitcoin address classes
            KeyPair kp = new KeyPair(_passfactor, compressed: true);

            _passpoint = kp.PublicKeyBytes;

            byte[] result = new byte[49];

            // 8 bytes are a constant, responsible for making the result start with the characters "passphrase"
            Array.Copy(magic, 0, result, 0, 8);
            result[7] = (byte)(LotSequencePresent ? 0x51 : 0x53);
            Array.Copy(_ownerentropy, 0, result, 8, 8);
            Array.Copy(_passpoint, 0, result, 16, 33);
            Code = Util.ByteArrayToBase58Check(result);

        }
    }
}

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
using System.Diagnostics;
using System.Security.Cryptography;

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math;

namespace Casascius.Bitcoin {






    public class MofN {




        public List<string> GetKeyParts() {
            return new List<string>(KeyParts);
        }



        private List<string> KeyParts = new List<string>();
        private List<byte[]> decodedKeyParts = new List<byte[]>();
        private int expectedChecksum;


        /// <summary>
        /// Returns the number of parts needed to solve the MofN, based on information gathered from
        /// the parts already added.  Returns -1 if no parts are added.
        /// </summary>
        public int PartsNeeded {
            get {
                if (decodedKeyParts.Count == 0) return -1;
                return (decodedKeyParts[0][1] - 0x93);
            }
        }

        /// <summary>
        /// Returns the number of parts that were accepted.  If this is equal to or greater than
        /// PartsNeeded, then decoding is possible.
        /// </summary>
        public int PartsAccepted {
            get {
                return decodedKeyParts.Count;
            }
        }

        public string BitcoinPrivateKey {
            get {
                if (KeyPair == null) return null;
                return KeyPair.PrivateKeyBase58;
            }
        }

        public string BitcoinAddress {
            get {
                if (KeyPair == null) return null;
                return KeyPair.AddressBase58;
            }
        }

        KeyPair KeyPair = null;

        public bool Decoded { get; private set; }

        public bool ChecksumMatched { get; private set; }


        /// <summary>
        /// Generate a set of M-of-N parts for a new random private key.
        /// </summary>
        public void Generate(int PartsNeededToDecode, int PartsToGenerate) {
            Generate(PartsNeededToDecode, PartsToGenerate, null);
        }



        /// <summary>
        /// Generate a set of M-of-N parts for a specific private key.
        /// If desiredPrivKey is null, then a random key will be selected.
        /// </summary>
        public void Generate(int PartsNeededToDecode, int PartsToGenerate, byte[] desiredPrivKey) {
            if (PartsNeededToDecode > PartsToGenerate) {
                throw new ApplicationException("Number of parts needed exceeds number of parts to generate.");                
            }

            if (PartsNeededToDecode > 8 || PartsToGenerate > 8) {
                throw new ApplicationException("Maximum number of parts is 8");
            }

            if (PartsNeededToDecode < 1 || PartsToGenerate < 1) {
                throw new ApplicationException("Minimum number of parts is 1");
            }

            if (desiredPrivKey != null && desiredPrivKey.Length != 32) {
                throw new ApplicationException("Desired private key must be 32 bytes");
            }

            KeyParts.Clear();
            decodedKeyParts.Clear();

            SecureRandom sr = new SecureRandom();

            // Get 8 random big integers into v[i].
            byte[][] vvv = new byte[8][];
            BigInteger[] v = new BigInteger[8];

            for (int i = 0; i < 8; i++) {
                byte[] b = new byte[32];
                sr.NextBytes(b, 0, 32);
                // For larger values of i, chop off some most-significant-bits to prevent overflows as they are
                // multiplied with increasingly larger factors.
                if (i >= 7) {
                    b[0] &= 0x7f;
                }
                v[i] = new BigInteger(1, b);
                Debug.WriteLine(String.Format("v({0})={1}", i, v[i].ToString()));

            }

            // if a certain private key is desired, then specify it.
            if (desiredPrivKey != null) {
                // replace v[0] with xor(v[1...7]) xor desiredPrivKey
                BigInteger newv0 = BigInteger.Zero;
                for (int i=1; i<PartsNeededToDecode; i++) {
                    newv0 = newv0.Xor(v[i]);
                }
                v[0] = newv0.Xor(new BigInteger(1,desiredPrivKey));

            }


            // Generate the expected private key from all the parts
            BigInteger privkey = new BigInteger("0");
            for (int i = 0; i < PartsNeededToDecode; i++) {
                privkey = privkey.Xor(v[i]);
            }

            // Get the bitcoin address
            byte[] keybytes = privkey.ToByteArrayUnsigned();
            // make sure we have 32 bytes, we'll need it
            if (keybytes.Length < 32) {
                byte[] array32 = new byte[32];
                Array.Copy(keybytes, 0, array32, 32 - keybytes.Length, keybytes.Length);
                keybytes = array32;
            }
            KeyPair = new KeyPair(keybytes);

            byte[] checksum = Util.ComputeSha256(BitcoinAddress);



            // Generate the parts
            for (int i = 0; i < PartsToGenerate; i++) {
                BigInteger total = new BigInteger("0");
                for (int j = 0; j < PartsNeededToDecode; j++) {

                    int factor = 1;
                    for (int ii = 0; ii <= i; ii++) factor = factor * (j + 1);

                    BigInteger bfactor = new BigInteger(factor.ToString());

                    total = total.Add(v[j].Multiply(bfactor));
                }

                Debug.WriteLine(String.Format(" pc{0}={1}", i, total.ToString()));
                byte[] parts = new byte[39];
                parts[0] = 0x4f;
                parts[1] = (byte)(0x93 + PartsNeededToDecode);
                int parts23 = (((checksum[0] << 8) + checksum[1]) & 0x1ff);
                Debug.WriteLine("checksum " + parts23.ToString());
                parts23 += 0x6000;
                parts23 += (i << 9);
                byte[] btotal = total.ToByteArrayUnsigned();
                for (int jj = 0; jj < btotal.Length; jj++) {
                    parts[jj + 4 + (35 - btotal.Length)] = btotal[jj];
                }

                parts[2] = (byte)((parts23 & 0xFF00) >> 8);
                parts[3] = (byte)(parts23 & 0xFF);

                KeyParts.Add(Util.ByteArrayToBase58Check(parts));
                decodedKeyParts.Add(parts);
            }



        }




        public void Decode() {

            ChecksumMatched=false;
            Decoded=false;
            KeyPair = null;

            if (PartsAccepted < PartsNeeded) return;


            BigInteger[] pc = new BigInteger[8];
            for (int i = 0; i < decodedKeyParts.Count; i++) {
                byte[] g = decodedKeyParts[i];
                pc[i] = new BigInteger(1, g, 4, 35);
                // If there is an overflow, then add it in.
                if ((g[2] & 0x80) == 0x80) {
                    pc[i] = pc[i].Add(new BigInteger(((int)((g[2] & 0x60) >> 5)).ToString()).ShiftLeft(280));
                    Debug.WriteLine("overflow added");

                }
            }


            List<equation> equations = new List<equation>();

            // create equations for all the parts we need.
            for (int i = 0; i < PartsNeeded; i++) {
                byte[] got = decodedKeyParts[i];
                // extract out part number
                int partnumber0 = (byte)((got[2] & 0x0e) >> 1);
                equations.Add(new equation(i, PartsNeeded, partnumber0));
            }

            List<List<equation>> steps = new List<List<equation>>();
            steps.Add(equations);

            // goal: get our equation set down such that there's only one coefficient on the left side.
            while (equations.Count > 1) {
                equations = solvesome(equations);
                steps.Add(equations);
                Debug.WriteLine("-----");
                foreach (equation eq in equations) {
                    Debug.WriteLine(eq.ToString());
                }
            }

            BigInteger[] v = new BigInteger[8];

            while (steps.Count > 0) {
                // pop off the last step
                List<equation> laststeps = steps[steps.Count - 1];
                steps.RemoveAt(steps.Count - 1);
                equation laststep = laststeps[0];
                Debug.WriteLine("-----");
                Debug.WriteLine(laststep.ToString());

                // solve for v
                long divisor = laststep.leftside[0].multiplier;
                laststep.divisor = laststep.leftside[0].multiplier;
                laststep.leftside[0].multiplier = 1;
                //foreach (coefficient c in laststep.rightside) c.divisor = divisor;
                //laststep.subtractor = laststep.subtractor.Divide(new BigInteger(divisor.ToString()));



                long idx = laststep.leftside[0].vindex;
                v[idx] = laststep.SolveRight(pc);
                Debug.WriteLine(String.Format("v({0})={1}", laststep.leftside[0].vindex, v[idx].ToString()));
                // go through all other steps and see that our solved value is incorporated into the equation.
                foreach (List<equation> eqbl in steps) {
                    foreach (equation eqb in eqbl) eqb.SolveLeft(v);
                }

            }

            // xor the ones we need

            BigInteger xoraccum = BigInteger.Zero;
            for (int i = 0; i < PartsNeeded; i++) {
                xoraccum = xoraccum.Xor(v[i]);
            }

            ChecksumMatched = false;
            Decoded = true;

            byte[] keybytes = xoraccum.ToByteArrayUnsigned();
            if (keybytes.Length > 32) {
                // if more than 32 bytes, decoding probably went wrong! truncate to 32 bytes, but force a checksum failure
                byte[] newkey = new byte[32];
                for (int jj = 0; jj < 32; jj++) newkey[jj] = keybytes[jj];
                keybytes = newkey;
                return;
            } else if (keybytes.Length < 32) {
                byte[] array32 = new byte[32];
                Array.Copy(keybytes, 0, array32, 32 - keybytes.Length, keybytes.Length);
                keybytes = array32;
            }
            KeyPair = new KeyPair(keybytes);

            // Get the bitcoin address            


            byte[] checksum = Util.ComputeSha256(BitcoinAddress);

            int mychecksum = ((checksum[0] & 1) << 8) + checksum[1];
            if (mychecksum == expectedChecksum) ChecksumMatched=true;


        }


        /// <summary>
        /// Add a key part, for the purpose of decoding.
        /// Returns null if the part was accepted, or a string explaining why it was not.
        /// </summary>
        public string AddKeyPart(string KeyPart) {

            KeyPart = KeyPart.Trim();

            byte[] ins = Util.Base58CheckToByteArray(KeyPart);

            if (ins == null) {
                return "Not a valid M-of-N code, this is not a valid Base58Check string or its checksum is incorrect";
            }

            // Validate the key part.
            if (ins.Length != 39) ins = null;
            if (ins != null && ins[0] != 0x4f && (ins[1] < 0x94 || ins[1] > 0x9b)) ins = null;
            if (ins == null) {
                return "Not a valid M-of-N code, they usually start with '6s'.";
            }

            if (KeyParts.Count > 0) {


                // go through the list, make sure they all belong to the first on the list
                byte[] first = null;
                int needed = 0;
                for (int i = 0; i < decodedKeyParts.Count; i++) {
                    byte[] b = decodedKeyParts[i];
                    if (first == null) {
                        first = b;
                        needed = b[1] - 0x93;
                        expectedChecksum = ((b[2] & 1) << 8) + b[3];
                    }

                    // check that bytes 2 and 3 are a match
                    if ((ins[2] & 1) != (b[2] & 1) || ins[3] != b[3]) {
                        return "M-of-N code is valid, but does not belong to the earlier code(s) provided.";
                    }

                    bool same = true;
                    for (int jj = 0; jj < b.Length; jj++) {
                        if (ins[jj] != b[jj]) same = false;
                    }
                    if (same) {
                        return "Code is identical to an earlier one provided.";
                    }

                    if ((ins[2] & 0x0e) == (b[2] & 0x0e)) {
                        return "This code has an identifier that conflicts with one of the earlier codes provided and cannot be used.  It may not belong to the set.";
                    }
                }
            }

            KeyParts.Add(KeyPart);
            decodedKeyParts.Add(ins);
            return null;

        }


        private static List<equation> solvesome(List<equation> ineq) {
            if (ineq.Count == 1) return ineq;

            List<equation> outeq = new List<equation>();

            for (int i = 1; i < ineq.Count; i++) {
                outeq.Add(ineq[i].CombineAndReduce(ineq[0]));
            }
            return outeq;
        }

    }


        public class coefficient {
        public long multiplier;
        public long vindex;
       
        public coefficient(long m, long v) {
            multiplier = m;
            vindex = v;
        }
    }

        public class equation {
            public List<coefficient> leftside = new List<coefficient>();
            public List<coefficient> rightside = new List<coefficient>();
            public BigInteger subtractor = BigInteger.Zero;
            public long divisor = 1;

            public equation() { }

            public equation CombineAndReduce(equation othereq) {
                long topmultiplier = 0, bottommultiplier = 0;

                // This function considers "this" the top, and othereq the bottom.
                // It multiplies both equations by the first coefficient of the opposite equation,
                // and then subtracts the bottom from the top, eliminating the first coefficient from
                // the left side, and adding coefficients to the right.

                equation neweq = new equation();
                othereq.KillFactors();

                for (int i = 0; i < leftside.Count; i++) {
                    coefficient topco = leftside[i];
                    coefficient bottomco = othereq.leftside[i];

                    if (i == 0) {
                        topmultiplier = bottomco.multiplier;
                        bottommultiplier = topco.multiplier;
                        // avoid saving a coefficient because the multiplier and subtraction guarantee they cancel
                    } else {
                        neweq.leftside.Add(new coefficient(topco.multiplier * topmultiplier - bottomco.multiplier * bottommultiplier, topco.vindex));
                    }
                }

                for (int i = 0; i < 8; i++) {
                    long multiplier = 0;
                    foreach (coefficient r in rightside) {
                        if (r.vindex == i) multiplier += (r.multiplier * topmultiplier);
                    }

                    foreach (coefficient r in othereq.rightside) {
                        if (r.vindex == i) multiplier -= (r.multiplier * bottommultiplier);
                    }

                    if (multiplier != 0) {
                        neweq.rightside.Add(new coefficient(multiplier, i));
                    }
                }

                neweq.KillFactors();
                return neweq;

            }


            public equation(int partnumber0, int partcount, int partsequence0) {
                // create one leftside coefficient for each partcount
                for (int i = 0; i < partcount; i++) {
                    //(i+1) ^ (partsequence0+1)
                    long factor = 1;
                    for (int ii = 0; ii <= partsequence0; ii++) factor = factor * (long)(i + 1);
                    leftside.Add(new coefficient(factor, i));
                }

                // create a rightside for the part
                rightside.Add(new coefficient(1, partnumber0));

            }

            public BigInteger SolveRight(BigInteger[] pcs) {
                BigInteger accum = new BigInteger("0");
                foreach (coefficient c in rightside) {
                    BigInteger scratch = new BigInteger(pcs[c.vindex].ToString());
                    scratch = scratch.Multiply(new BigInteger(c.multiplier.ToString()));
                    accum = accum.Add(scratch);
                }
                return accum.Subtract(subtractor).Divide(new BigInteger(divisor.ToString())); ;
            }

            public void SolveLeft(BigInteger[] v) {
                for (int i = 0; i < 8; i++) {
                    if (v[i] != null && v[i].Equals(BigInteger.Zero) == false) {
                        // find anything on the left that uses this value and eliminate it.
                        for (int j = 0; j < leftside.Count; j++) {
                            coefficient c = leftside[j];
                            if (c.vindex == i) {
                                // found something - eliminate it
                                BigInteger accum = v[i];
                                accum = accum.Multiply(new BigInteger(c.multiplier.ToString()));
                                // now subtract the whole thing from both sides
                                subtractor = subtractor.Add(accum);
                                leftside.RemoveAt(j);
                                j--;
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Kills small factors out of the multipliers wherever possible to keep them from overflowing.
            /// </summary>
            public void KillFactors() {
                long[] primes = new long[] { 2, 3, 5, 7, 11, 13, 17 };

                for (int i = 0; i < primes.Length; i++) {
                    long myprime = primes[i];

                    bool brk = false;
                    // are all left and right side items a factor?
                    foreach (coefficient c in leftside) {
                        if (c.multiplier >= 0) {
                            if ((c.multiplier % myprime) != 0) brk = true;
                        } else {
                            if (((-c.multiplier) % myprime) != 0) brk = true; ;
                        }
                    }
                    if (brk) continue;
                    foreach (coefficient c in rightside) {
                        if (c.multiplier >= 0) {
                            if ((c.multiplier % myprime) != 0) brk = true; ;
                        } else {
                            if (((-c.multiplier) % myprime) != 0) brk = true; ;
                        }
                    }
                    if (brk) continue;

                    // reduce the factor and check for primes over again.

                    foreach (coefficient c in leftside) c.multiplier /= myprime;
                    foreach (coefficient c in rightside) c.multiplier /= myprime;
                    i = -1; // start loop over
                }
            }


            public override string ToString() {
                StringBuilder sb = new StringBuilder();
                List<string> coeffstrings = new List<string>();
                for (int i = 0; i < leftside.Count; i++) {
                    if (i != 0 && leftside[i].multiplier >= 0) sb.Append("+");
                    //  if (leftside[i].divisor != 1) {
                    //      sb.AppendFormat("({0}/{2})v({1})", leftside[i].multiplier, leftside[i].vindex, leftside[i].divisor);
                    //  } else {
                    sb.AppendFormat("{0}v({1})", leftside[i].multiplier, leftside[i].vindex);

                    //  }
                }
                sb.Append("=");
                for (int i = 0; i < rightside.Count; i++) {
                    if (i != 0 && rightside[i].multiplier >= 0) sb.Append("+");
                    //   if (rightside[i].divisor != 1) {
                    //       sb.AppendFormat("({0}/{2})pc({1})", rightside[i].multiplier, rightside[i].vindex, rightside[i].divisor);
                    //   } else {
                    sb.AppendFormat("{0}pc({1})", rightside[i].multiplier, rightside[i].vindex);
                    //   }
                }
                if (subtractor != BigInteger.Zero) {
                    sb.AppendFormat("-" + subtractor.ToString());
                }

                if (divisor != 1) {
                    sb.AppendFormat("  ... all / " + divisor.ToString());
                }
                return sb.ToString();
            }

        }


}

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
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;

namespace Casascius.Bitcoin {
    public class EscrowCodeSet {

        private const long headbaseA = 0x140bebc0a12ca9c6L;
        private const long headbaseB = 0x140bebc16ae0563bL;
        private const long headbaseP = 0x140bebcba900182fL;
        private const long headconfP = 0x12f4eff38f7d74f9L;

        /// <summary>
        /// When applicable, this provides the Escrow Invitation Code that starts with "einva".
        /// </summary>
        public string EscrowInvitationCodeA { get; private set; }


        /// <summary>
        /// When applicable, this provides the Escrow Invitation Code that starts with "einvb".
        /// </summary>
        public string EscrowInvitationCodeB { get; private set; }

        /// <summary>
        /// When applicable, this provides the Payment Invitation Code that starts with "einvp".
        /// </summary>
        public string PaymentInvitationCode { get; private set; }

        /// <summary>
        /// Address Confirmation Code can be used to prove to Escrow Agent that a bitcoin address has been
        /// created for which he has escrow control.  It contains the same thing as PaymentInvitationCode
        /// except it does not contain the private element that allows the payment to be claimed.
        /// Verifying it requires both escrow invitation codes A and B.  It starts with "cfrmp".
        /// </summary>
        public string AddressConfirmationCode { get; private set; }

        /// <summary>
        /// BitcoinAddress is calculated any time PaymentInvitationCode is generated or provided with any single
        /// EscrowInvitationCode.
        /// </summary>
        public string BitcoinAddress { get; private set; }

        /// <summary>
        /// This gets set if a Payment Invitation Code is validated and appears to have been generated
        /// from the same Escrow Invitation Code being used to verify it, rather than its mate.  It causes
        /// a warning to be shown to the user.
        /// </summary>
        public bool SamePartyWarningApplies { get; private set; }

        public string PrivateKey { get; private set; }

        /// <summary>
        /// Default constructor.  Creates a new matched pair of escrow invitation codes.
        /// </summary>
        public EscrowCodeSet() {
            SecureRandom sr = new SecureRandom();

            byte[] x = new byte[32];
            byte[] y = new byte[32];
            sr.NextBytes(x);
            sr.NextBytes(y);

            // Force x to be even
            // Force y to be odd
            x[31] &= 0xFE;
            y[31] |= 0x01;

            KeyPair kx = new KeyPair(x, true);
            KeyPair ky = new KeyPair(y, true);

            BigInteger xi = new BigInteger(1, x);
            BigInteger yi = new BigInteger(1, y);

            ECPoint Gx = kx.GetECPoint();
            byte[] bytesGx = Gx.GetEncoded();
            ECPoint Gy = ky.GetECPoint();
            byte[] bytesGy = Gy.GetEncoded();
            ECPoint Gxy = Gx.Multiply(yi);

            byte[] bytesGxy = Gxy.GetEncoded();
            Sha256Digest sha256 = new Sha256Digest();
            byte[] hashGxy = new byte[32];
            sha256.BlockUpdate(bytesGxy, 0, bytesGxy.Length);
            sha256.DoFinal(hashGxy, 0);
            sha256.BlockUpdate(hashGxy, 0, 32);
            sha256.DoFinal(hashGxy, 0);

            int identifier30 = ((hashGxy[0] & 0x3f) << 24) + (hashGxy[1] << 16) + (hashGxy[2] << 8) + hashGxy[3];

            byte[] invitationA = new byte[74];
            byte[] invitationB = new byte[74];

            long headA = headbaseA + (long)identifier30;
            long headB = headbaseB + (long)identifier30;

            // turn headA and headB into bytes
            for (int i = 7; i >= 0; i--) {
                invitationA[i] = (byte)(headA & 0xFF);
                invitationB[i] = (byte)(headB & 0xFF);
                headA >>= 8;
                headB >>= 8;
            }

            Array.Copy(x, 0, invitationA, 8 + 1, 32);
            Array.Copy(y, 0, invitationB, 8 + 1, 32);
            Array.Copy(bytesGy, 0, invitationA, 8 + 1 + 32, 33);
            Array.Copy(bytesGx, 0, invitationB, 8 + 1 + 32, 33);

            EscrowInvitationCodeA = Util.ByteArrayToBase58Check(invitationA);
            EscrowInvitationCodeB = Util.ByteArrayToBase58Check(invitationB);
        }

        /// <summary>
        /// Constructor that takes a single Escrow Invitation Code and produces a Payment Invitation Code.
        /// </summary>
        public EscrowCodeSet(string escrowInvitationCode, bool doCompressed=false, byte networkByte = 0) {
            byte[] pubpart, privpart;
            int identifier30;
            string failreason = parseEscrowCode(escrowInvitationCode, out pubpart, out privpart, out identifier30);
            if (failreason != null) throw new ArgumentException(failreason);

            // Look for mismatched parity.
            // (we expect LSB of einva's private part to be 0 and LSB of einvb's private part to be 1)
            // (this is to guarantee that einva's and einvb's private parts aren't equal)
            if ((escrowInvitationCode.StartsWith("einva") && (privpart[31] & 0x01) == 1) ||
                (escrowInvitationCode.StartsWith("einvb") && (privpart[31] & 0x01) == 0)) {
                throw new ArgumentException("This escrow invitation has mismatched parity.  Ask your escrow agent to " +
                    "generate a new pair using the latest version of the software.");
            }

            // Look for 48 0's or 48 1's
            if (privpart[0] == privpart[1] && privpart[1] == privpart[2] && privpart[2] == privpart[3] &&
               privpart[3] == privpart[4] && privpart[4] == privpart[5] && privpart[5] == privpart[6] &&
               privpart[6] == privpart[7] && privpart[7] == privpart[8]) {
                if (privpart[0] == 0x00 || privpart[0] == 0xFF) {
                    throw new ArgumentException("This escrow invitation is invalid and cannot be used (bad private key).");
                }
            }


            // produce a new factor
            byte[] z = new byte[32];
            SecureRandom sr = new SecureRandom();
            sr.NextBytes(z);

            // calculate Gxy then Gxyz
            PublicKey pk = new PublicKey(pubpart);
            ECPoint Gxyz = pk.GetECPoint().Multiply(new BigInteger(1, privpart)).Multiply(new BigInteger(1, z));

       
            // Uncompress it
            Gxyz = PublicKey.GetUncompressed(Gxyz);

            // We can get the Bitcoin address now, so do so
            PublicKey pkxyz = new PublicKey(Gxyz);           
            byte[] hash160 = pkxyz.Hash160;
            BitcoinAddress = new AddressBase(hash160, networkByte).AddressBase58;
            
            // make the payment invitation record
            byte[] invp = new byte[74];
            long headP = headbaseP + (long)identifier30;
            for (int i = 7; i >= 0; i--) {
                invp[i] = (byte)(headP & 0xff);
                headP >>= 8;
            }
            invp[8] = networkByte;
            Array.Copy(z, 0, invp, 8 + 1 + 1, 32);
            
            // set flag to indicate if einvb was used to generate this, and make it available in the object
            if (escrowInvitationCode.StartsWith("einvb")) {
                invp[8 + 1 + 1 + 32 + 20] = 0x2;
            }

            // copy hash160
            Array.Copy(hash160, 0, invp, 8 + 1 + 1 + 32, 20);

            PaymentInvitationCode = Util.ByteArrayToBase58Check(invp);
            setAddressConfirmationCode(identifier30, networkByte, invp[8 + 1 + 1 + 32 + 20], z, hash160);
        }

        /// <summary>
        /// Constructor that calculates the address given one escrow invitation code and one matching payment invitation code.
        /// They can be provided in any order.
        /// </summary>
        public EscrowCodeSet(string code1, string code2) {
            if (code1 == null || code2 == null || code1 == "" || code2 == "") {
                throw new ArgumentException("Two codes are required to use this function.");
            }

            string escrowInvitationCode=null, paymentInvitationCode=null;

            if (code1.StartsWith("einva") || code1.StartsWith("einvb")) escrowInvitationCode = code1;
            if (code2.StartsWith("einva") || code2.StartsWith("einvb")) escrowInvitationCode = code2;
            if (code1.StartsWith("einvp")) paymentInvitationCode = code1;
            if (code2.StartsWith("einvp")) paymentInvitationCode = code2;


            if (escrowInvitationCode == null || paymentInvitationCode == null) {
                throw new ArgumentException("In order to use this function, one code MUST be an Escrow Invitation (starting " +
                    "with \"einva\" or \"einvb\") and the other code MUST be a Payment Invitation (starting with \"einvp\").");
            }

            byte[] pubpart, privpart;
            int identifier30;
            string failreason = parseEscrowCode(escrowInvitationCode, out pubpart, out privpart, out identifier30);
            if (failreason != null) throw new ArgumentException(failreason);


            // Look for first 40 bits being all 0's or all 1's


            string notvalid = "Not a valid Payment Invitation Code";
            string notvalid2 = "Code is not a valid Payment Invitation Code or may have a typo or other error.";
            string notvalid3 = "The Payment Invitation does not belong to the provided Escrow Invitation.";

            long head;
            byte[] invbytes;
            string failReason = parseEitherCode(paymentInvitationCode, notvalid, notvalid2, out invbytes, out head);

            if (head < headbaseP) throw new ArgumentException(notvalid);
            long identifier30L = head - headbaseP;
            if (identifier30L < 0 || identifier30L > 0x3FFFFFFFL) throw new ArgumentException(notvalid);

            if ((long)identifier30 != identifier30L) {
                throw new ArgumentException(notvalid3);
            }

            byte[] privpartz = new byte[32];
            Array.Copy(invbytes, 8 + 1 + 1, privpartz, 0, 32);
            byte networkByte = invbytes[8];
            bool compressedFlag = (invbytes[8+1+1+32+20] & 0x1) == 1;


            // get bitcoin address
            PublicKey pk = new PublicKey(pubpart);
            ECPoint Gxyz = pk.GetECPoint().Multiply(new BigInteger(1, privpart)).Multiply(new BigInteger(1, privpartz));
            // uncompress if compress is not indicated
            if (compressedFlag == false) Gxyz = PublicKey.GetUncompressed(Gxyz);

            // We can get the Bitcoin address now, so do so
            PublicKey pkxyz = new PublicKey(Gxyz);
            byte[] addrhash160 = pkxyz.Hash160;
            BitcoinAddress = new AddressBase(addrhash160, networkByte).AddressBase58;

            // Does the hash160 match?
            for (int i = 0; i < 20; i++) {
                if (addrhash160[i] != invbytes[8+1+1+32+i]) {
                    throw new ArgumentException(notvalid3);
                }
            }

            this.PaymentInvitationCode = paymentInvitationCode;

            byte expectedabflag = (byte)(escrowInvitationCode.StartsWith("einva") ? 2 : 0);
            if ((invbytes[8+1+1+32+20] & 0x2) != expectedabflag) {
                SamePartyWarningApplies = true;
            }
        }


        /// <summary>
        /// Constructor which attempts to redeem a completed set of three codes, and calculate the private key.
        /// </summary>
        public EscrowCodeSet(string code1, string code2, string code3) {
            if (code1 == null || code2 == null || code3 == null || code1 == "" || code2 == "" || code3 == "") {
                throw new ArgumentException("Three codes are required to use this function.");
            }

            string codea = null, codeb=null, codep = null;

            if (code1.StartsWith("einva")) codea = code1;
            if (code2.StartsWith("einva")) codea = code2;
            if (code3.StartsWith("einva")) codea = code3;
            if (code1.StartsWith("einvb")) codeb = code1;
            if (code2.StartsWith("einvb")) codeb = code2;
            if (code3.StartsWith("einvb")) codeb = code3;
            if (code1.StartsWith("einvp")) codep = code1;
            if (code2.StartsWith("einvp")) codep = code2;
            if (code3.StartsWith("einvp")) codep = code3;


            if (codea==null || codeb == null || codep == null) {
                throw new ArgumentException("In order to use this function, one code MUST be an Escrow Invitation A (starting " +
                    "with \"einva\"), one must be an Escrow Invitation B (starting with \"einvb\") and the last " +
                    "code MUST be a Payment Invitation (starting with \"einvp\").");
            }


            byte[] pubparta, privparta;
            int identifier30a;
            string failreason = parseEscrowCode(codea, out pubparta, out privparta, out identifier30a);
            if (failreason != null) throw new ArgumentException("Escrow Invitation Code A: " + failreason);

            byte[] pubpartb, privpartb;
            int identifier30b;
            failreason = parseEscrowCode(codeb, out pubpartb, out privpartb, out identifier30b);
            if (failreason != null) throw new ArgumentException("Escrow Invitation Code B: " + failreason);

            if (identifier30a != identifier30b) {
                throw new ArgumentException("The two Escrow Invitations are not mates and cannot unlock the private key.");
            }


            
            string notvalid = "Not a valid Payment Invitation Code";
            string notvalid2 = "Code is not a valid Payment Invitation Code or may have a typo or other error.";
            string notvalid3 = "The Payment Invitation does not belong to the provided Escrow Invitation.";

            long headp;
            byte[] invbytesp;
            string failReason = parseEitherCode(codep, notvalid, notvalid2, out invbytesp, out headp);

            if (headp < headbaseP) throw new ArgumentException(notvalid);
            long identifier30L = headp - headbaseP;
            if (identifier30L < 0 || identifier30L > 0x3FFFFFFFL) throw new ArgumentException(notvalid);

            if (identifier30L != (long)identifier30a) {
                throw new ArgumentException("The Payment Invitation was not generated from either of the provided Escrow Invitation codes and cannot be unlocked by them.");
            }

            byte[] privpartz = new byte[32];
            Array.Copy(invbytesp, 8 + 1 + 1, privpartz, 0, 32);
            byte networkByte = invbytesp[8];
            bool compressedFlag = (invbytesp[8 + 1 + 1 + 32 + 20] & 0x1) == 1;


            // get private key
            BigInteger xyz = new BigInteger(1, privparta).Multiply(new BigInteger(1, privpartb)).Multiply(new BigInteger(1, privpartz));
            var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");
            xyz = xyz.Mod(ps.N);

            KeyPair kp = new KeyPair(xyz, compressedFlag, networkByte);

            // provide everything
            this.EscrowInvitationCodeA = codea;
            this.EscrowInvitationCodeB = codeb;
            this.PaymentInvitationCode = codep;
            this.BitcoinAddress = kp.AddressBase58;
            this.PrivateKey = kp.PrivateKey;           

        }



        private void setAddressConfirmationCode(int identifier30, byte networkbyte, byte flagbyte, byte[] z, byte[] hash160) {
            byte[] accbytes = new byte[74];
            long head = headconfP + (int)identifier30;
            for (int i=7; i>=0; i--) {
                accbytes[i] = (byte)(head & 0x7F);
                head >>= 8;
            }
            accbytes[8]=networkbyte;

            byte[] Gzbytes = new KeyPair(z, true).GetECPoint().GetEncoded();
            Array.Copy(Gzbytes, 0, accbytes, 8+1, 33);
            Array.Copy(hash160, 0, accbytes, 8+1+33, 20);
            accbytes[8+1+33+20] = flagbyte;
            this.AddressConfirmationCode = Util.ByteArrayToBase58Check(accbytes);

        }


        /// <summary>
        /// Parses an escrow invitation code, returning null (plus the out parts) if valid, and returns a string reason if not.
        /// </summary>
        private string parseEscrowCode(string thecode, out byte[] pubpart, out byte[] privpart, out int identifier30) {
            pubpart = null;
            privpart = null;
            identifier30 = 0;

            string notvalid = "Not a valid Escrow Invitation Code";
            string notvalid2 = "Code is not a valid Escrow Invitation Code or may have a typo or other error.";

            if (thecode.StartsWith("einva") == false && thecode.StartsWith("einvb") == false) {
                return notvalid;
            }


            long identifier30L, head;
            byte[] invbytes;
            string failReason = parseEitherCode(thecode, notvalid, notvalid2, out invbytes, out head);

            if (head < headbaseA) return notvalid;
            if (head < headbaseB) {
                identifier30L = head - headbaseA;
            } else {
                identifier30L = head - headbaseB;
            }
            if (identifier30L < 0 || identifier30L > 0x3FFFFFFFL) return notvalid;
            identifier30 = (int)identifier30L;

            privpart = new byte[32];
            pubpart = new byte[33];
            Array.Copy(invbytes, 8 + 1, privpart, 0, 32);
            Array.Copy(invbytes, 8 + 1 + 32, pubpart, 0, 33);
            if (thecode.StartsWith("einvb")) {
                EscrowInvitationCodeB = thecode;
            } else {
                EscrowInvitationCodeA = thecode;
            }

            return null;
        }

        private string parseEitherCode(string thecode, string notvalid, string notvalid2, out byte[] invbytes, out Int64 head) {
            invbytes = null;
            head = 0L;
            if (thecode == null) return notvalid;
            invbytes = Util.Base58CheckToByteArray(thecode);
            if (invbytes == null) {
                return notvalid2;
            }

            if (invbytes.Length != 74) return notvalid;
            
            for (int i = 0; i < 8; i++) {
                head <<= 8;
                head += invbytes[i];
            }
            return null;
        }
    }



}

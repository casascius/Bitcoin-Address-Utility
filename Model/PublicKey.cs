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
using Org.BouncyCastle.Math;

namespace Casascius.Bitcoin {

    /// <summary>
    /// Bitcoin address extended to include knowledge of public key.
    /// </summary>
    public class PublicKey : AddressBase {

        protected PublicKey() { }

        public PublicKey(ECPoint point) {
            this.IsCompressedPoint = point.IsCompressed;
            this.point = point;
            this.PublicKeyBytes = point.GetEncoded();
            if (validatePoint() == false) throw new ArgumentException("Not a valid public key");
        }

        public PublicKey(string hex) {
            byte[] pubKeyBytes = Util.HexStringToBytes(hex);
            string result = constructFromBytes(pubKeyBytes);
            if (result != null) throw new ArgumentException(result);
        }

        /// <summary>
        /// Constructor that takes a byte array of 33 or 65 bytes representing a public key.
        /// </summary>
        public PublicKey(byte[] pubKeyBytes) {
            string result = constructFromBytes(pubKeyBytes);
            if (result != null) throw new ArgumentException(result);

        }

        public static bool IsValidPublicKey(string hex) {
            byte[] pubKeyBytes = Util.HexStringToBytes(hex);
            PublicKey pk = new PublicKey();
            string result = pk.constructFromBytes(pubKeyBytes);
            return (result == null);
        }

        private string constructFromBytes(byte[] pubKeyBytes) {
            if (pubKeyBytes == null) {
                return "PublicKey constructor requires a byte array with 65 bytes.";
            }

            if (pubKeyBytes.Length == 65) {
                if (pubKeyBytes[0] != 4) {
                    return "Invalid public key, for 65-byte keys the first byte must be 0x04";
                }

            } else if (pubKeyBytes.Length == 33) {
                if (pubKeyBytes[0] != 2 && pubKeyBytes[0] != 3) {
                    return "Invalid public key, for 3-byte keys the first byte must be 0x02 or 0x03";
                }
                IsCompressedPoint = true;
            } else {
                return "Invalid public key, must be 33 or 65 bytes";
            }
            try {
                var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");
                point = ps.Curve.DecodePoint(pubKeyBytes);
                if (validatePoint()==false) return "Not a valid public key";

                // todo: ensure X and Y are on the curve
                PublicKeyBytes = pubKeyBytes;
            } catch (Exception e) {
                // catches errors like "invalid point compression"
                return "Not a valid public key: " + e.Message;
            }
            return null;
        }

        /// <summary>
        /// Returns true if the point coordinates satisfy the elliptic curve equation.
        /// </summary>
        private bool validatePoint() {
            var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");
            var y2 = point.Y.Multiply(point.Y);
            var x3 = point.X.Multiply(point.X).Multiply(point.X);
            var ax = point.X.Multiply(ps.Curve.A);
            var x3axb = x3.Add(ax).Add(ps.Curve.B);
            return y2.Equals(x3axb);
        }

        protected ECPoint point = null;

        private byte[] _publicKey = null;

        public bool IsCompressedPoint { get; protected set; }


        /// <summary>
        /// Virtual method to compute public key on demand when doing so is expensive.
        /// Not used if we are handed a public key through the constructor, but this is used
        /// if a descendant class (e.g. KeyPair) has a private key and the only way to know
        /// the public key is to compute it.
        /// </summary>
        protected virtual byte[] ComputePublicKey() { return null;  }

        /// <summary>
        /// Returns the public key bytes.  This will return 65 bytes for an uncompressed public key
        /// or 33 bytes for a compressed public key.
        /// </summary>
        public byte[] PublicKeyBytes {
            get {
                if (_publicKey == null) _publicKey = ComputePublicKey();

                byte[] rv = new byte[_publicKey.Length];
                _publicKey.CopyTo(rv, 0);
                return rv;
            }
            protected set {
                _publicKey = new byte[value.Length];
                value.CopyTo(_publicKey, 0);
            }
        }

        public byte[] GetCompressed() {
            return getReencoded(true);
        }

        public byte[] GetUncompressed() {
            return getReencoded(false);
        }

        public ECPoint GetECPoint() {
            byte[] pubKeyBytes = PublicKeyBytes;
            var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");
            return ps.Curve.DecodePoint(pubKeyBytes);
        }
        
        private byte[] getReencoded(bool compressed) {
            byte[] pubKeyBytes = PublicKeyBytes;
            var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");
            point = ps.Curve.DecodePoint(pubKeyBytes);
            var point2 = ps.Curve.CreatePoint(point.X.ToBigInteger(), point.Y.ToBigInteger(), compressed);
            return point2.GetEncoded();
        }

        public static ECPoint GetUncompressed(ECPoint point) {
            var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");
            return ps.Curve.CreatePoint(point.X.ToBigInteger(), point.Y.ToBigInteger(), false);
        }

        public static ECPoint GetCompressed(ECPoint point) {
            var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");
            return ps.Curve.CreatePoint(point.X.ToBigInteger(), point.Y.ToBigInteger(), true);
        }


        /// <summary>
        /// Computes the Hash160 of the public key upon demand.
        /// </summary>
        protected override byte[] ComputeHash160() {
            byte[] shaofpubkey = Util.ComputeSha256(PublicKeyBytes);
            RIPEMD160 rip = System.Security.Cryptography.RIPEMD160.Create();
            return rip.ComputeHash(shaofpubkey);
        }

        /// <summary>
        /// Hexadecimal representation of public key.  Each byte is 2 hex digits, uppercase,
        /// delimited with spaces.
        /// </summary>
        public string PublicKeyHex {

            get {
                return Util.ByteArrayToString(PublicKeyBytes);
            }
        }
        


    }
}
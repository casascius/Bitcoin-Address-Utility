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



    /// <summary>
    /// Represents a single Bitcoin address, assumes knowledge only of a Hash160.
    /// </summary>
    public class AddressBase {

        protected AddressBase() { }

        /// <summary>
        /// Constructs a Bitcoin address from a 20 byte array representing a Hash160.
        /// If 21 bytes are provided, the extra byte denotes address type.
        /// </summary>
        public AddressBase(byte[] addressBytes) {
            // Hash160 setter validates length and throws exception if needed
            Hash160 = addressBytes;
        }

        /// <summary>
        /// Constructs a Bitcoin address from a 20 byte array representing a Hash160,
        /// and also denoting a specific address type.
        /// </summary>
        public AddressBase(byte[] addressBytes, byte addressType) {
            // Hash160 setter validates length and throws exception if needed
            Hash160 = addressBytes;
            this.AddressType = addressType;
        }

        /// <summary>
        /// Allows calculation of address with a different AddressType
        /// </summary>
        public AddressBase(AddressBase otheraddress, byte addressType) {
            // Hash160 setter validates length and throws exception if needed
            Hash160 = otheraddress.Hash160;
            this.AddressType = addressType;
        }

        /// <summary>
        /// Constructs an Address from an address string
        /// </summary>
        public AddressBase(string address) {
            byte[] hex = Util.Base58CheckToByteArray(address);            
            if (hex.Length != 21) throw new ArgumentException("Not a valid or recognized address");
            // Hash160 setter validates length and throws exception if needed
            Hash160 = hex;
        }



        /// <summary>
        /// Returns the address type.  For example, 0=Bitcoin
        /// </summary>
        public byte AddressType {
            get {
                return _addressType;
            }
            protected set {
                _addressType = value;
                _address = null;
            }
        }


        protected byte _addressType = 0;

        private byte[] _hash160 = null;

        /// <summary>
        /// Overridden in descendant classes allowing Hash160 to be computed on an as-needed
        /// basis (since it's CPU-costly if it comes from a private key)
        /// </summary>
        protected virtual byte[] ComputeHash160() { return null; }

        /// <summary>
        /// Returns a copy of the 20-byte Hash160 of the Bitcoin address
        /// </summary>
        public byte[] Hash160 {
            get {
                if (_hash160 == null) _hash160 = ComputeHash160();

                // make a copy for the caller
                byte[] rv = new byte[20];
                Array.Copy(_hash160, rv, 20);
                return rv;
            }
            protected set {
                if (value.Length == 20) {
                    _hash160 = new byte[20];
                    value.CopyTo(_hash160, 0);
                } else if (value.Length == 21) {
                    _hash160 = new byte[20];
                    Array.Copy(value, 1, _hash160, 0, 20);
                    AddressType = value[0];
                } else {
                    throw new ArgumentException("Address constructor with byte array requires 20 or 21 bytes");
                }            
            }
        }

        public string Hash160Hex {
            get {
                return Util.ByteArrayToString(Hash160);
            }
        }
                

        /// <summary>
        /// Get the Bitcoin address in Base58 format as it would be seen by the user.
        /// </summary>
        public string AddressBase58 {           
            get {
                if (_address == null) {
                    // compute the base58 but cache it for subsequent references.
                    byte[] hex2 = new byte[21];
                    Array.Copy(Hash160, 0, hex2, 1, 20);
                    hex2[0] = AddressType;
                    _address = Util.ByteArrayToBase58Check(hex2);
                    return _address;
                }
                return _address;
            }
        }

        protected string _address = null;
    }
}
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

namespace Casascius.Bitcoin {

    /// <summary>
    /// Represents an encrypted private key.
    /// The only thing we are certain to have is a representation of a private key in a known format.
    /// We might know the decryption key, but we might not.
    /// We might know the unencrypted private key, but we might not.
    /// We might know the public key, but we might not.
    /// We might know the Bitcoin address, but we might not.
    /// </summary>
    public abstract class EncryptedKeyPair {
        protected EncryptedKeyPair() { }


        /// <summary>
        /// The encrypted representation of the private key, validated to be in a known format.
        /// Which known format depends on the derived implementation.
        /// </summary>
        protected string _encryptedKey;

        public string EncryptedPrivateKey {
            get {
                return _encryptedKey;
            }
        }

        /// <summary>
        /// Contains the private key if we know it.
        /// If we don't know it, this contains null.
        /// </summary>
        protected byte[] _privKey;

        /// <summary>
        /// Calculates private key and returns true if calculating private key was possible.
        /// </summary>
        protected virtual bool calculatePrivKey() {
            return false;
        }

        /// <summary>
        /// Returns true if the unencrypted private key is available.
        /// Calling this may cause a delay of up to a few seconds while the private key is decrypted if decryption
        /// is possible and necessary to determine that we have the private key.
        /// Successful decryption is cached and there is no delay for subsequent checks.
        /// </summary>
        public virtual bool IsUnencryptedPrivateKeyAvailable() {
            if (_privKey != null) return true;
            if (calculatePrivKey()) return true;
            return false;
        }

        public KeyPair GetUnencryptedPrivateKey() {
            if (IsUnencryptedPrivateKeyAvailable() == false) {
                throw new InvalidOperationException("Unencrypted private key is not available");
            }
            return new KeyPair(_privKey, compressed: IsCompressedPoint, addressType: _addressType);
        }

        /// <summary>
        /// Contains the public key if we know it.
        /// If we don't know it, this contains null.
        /// </summary>
        protected byte[] _pubKey;

        public bool IsCompressedPoint { get; protected set; }

        /// <summary>
        /// Calculates public key and returns true if calculating public key was possible.
        /// This might cause a delay of milliseconds if it must be computed from the public
        /// key, possibly more if it leads to decrypting a private key.
        /// </summary>
        protected virtual bool calculatePubKey() {
            if (IsUnencryptedPrivateKeyAvailable()) {
                KeyPair kp = new KeyPair(_privKey, compressed: IsCompressedPoint, addressType: _addressType);
                _pubKey = kp.PublicKeyBytes;
                return true;
            }
            return false;
        }


        /// <summary>
        /// Returns true if the public key is available.
        /// Calling this may cause a delay of up to a few seconds while the private key is decrypted if decryption
        /// is possible and necessary to determine that we have the public key.
        /// Successful decryption is cached and there is no delay for subsequent checks.
        /// </summary>
        public virtual bool IsPublicKeyAvailable() {
            if (_pubKey != null) return true;
            if (_privKey != null) return true;
            return false;
        }

        public PublicKey GetPublicKey() {
            if (_pubKey == null) {
                calculatePubKey();
                if (IsPublicKeyAvailable() == false) {
                    throw new InvalidOperationException("Public key is not available");
                }
            }
            return new PublicKey(_pubKey);
        }


        /// <summary>
        /// Contains the hash160 if we know it.
        /// If we don't know it, this contains null.
        /// </summary>
        protected byte[] _hash160;

        protected virtual bool calculateHash160() {
            if (IsPublicKeyAvailable()) {
                PublicKey pub = new PublicKey(_pubKey);
                _hash160 = pub.Hash160;
                return true;
            }
            return false;
        }

        protected byte _addressType = 0;

        /// <summary>
        /// Returns true if it is possible to return or calculate the Address.
        /// </summary>
        public virtual bool IsAddressAvailable() {
            // Return true on having the address if we have hash160, pubkey, or privkey (since all can be used
            // to calculate the address).  Avoid actually calculating it because it's expensive.
            if (_hash160 != null) return true;
            if (_pubKey != null) return true;
            if (_privKey != null) return true;
            return false;
        }

        /// <summary>
        /// Gets address if known or can be calculated.
        /// Throws an InvalidOperationException if not.
        /// </summary>
        public AddressBase GetAddress() {
            if (_hash160 == null) {
                calculateHash160();
                if (IsAddressAvailable() == false) {
                    throw new InvalidOperationException("Address is not available");
                }
            }
            return new AddressBase(_hash160, _addressType);
        }
    }
}

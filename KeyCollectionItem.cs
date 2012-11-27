using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtcAddress {
    public class KeyCollectionItem {
        public KeyCollectionItem() { }

        public KeyCollectionItem(Address address) {
            this.Address = address;
        }

        public KeyCollectionItem(EncryptedKeyPair ekp) {
            this.EncryptedKeyPair = ekp;
        }
        /// <summary>
        /// The encrypted item, if this item is encrypted.
        /// </summary>
        public EncryptedKeyPair EncryptedKeyPair;

        /// <summary>
        /// The plain item, if it is not encrypted.  This could be a PublicKey, KeyPair, etc.
        /// </summary>
        public Address Address;

        /// <summary>
        /// Gets the address in Base58, calculating it if necessary.
        /// </summary>
        public string GetAddressBase58() {
            if (Address != null) return Address.AddressBase58;
            return EncryptedKeyPair.GetAddress().AddressBase58;
        }

        /// <summary>
        /// Gets the private key in the best known printable form.
        /// </summary>
        public string PrivateKey {
            get {
                if (Address != null && Address is KeyPair) {
                    return ((KeyPair)Address).PrivateKey;
                } else if (EncryptedKeyPair != null) {
                    return EncryptedKeyPair.EncryptedPrivateKey;
                }
                return "Unknown";
            }
        }

        /// <summary>
        /// Returns "MiniKey", "Known", "Encrypted", or "Unknown" depending on what private key we have.
        /// </summary>
        public string PrivateKeyKind {
            get {
                if (Address != null && Address is KeyPair) {
                    if (Address is MiniKeyPair) return "MiniKey";
                    return "Known";
                } else if (EncryptedKeyPair != null) {
                    return "Encrypted";
                }
                return "Unknown";
            }
        }

        public override string ToString() {
            if (Address != null) return Address.AddressBase58;
            if (EncryptedKeyPair == null) return "<null>";
            if (EncryptedKeyPair.IsAddressAvailable() == false) {
                return EncryptedKeyPair.EncryptedPrivateKey;
            }
            return EncryptedKeyPair.GetAddress().AddressBase58;
        }
    }
}

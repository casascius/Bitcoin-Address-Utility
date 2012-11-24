using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtcAddress {
    /// <summary>
    /// Represents an encrypted KeyPair that can be decrypted with a passphrase.
    /// </summary>
    public abstract class PassphraseKeyPair : EncryptedKeyPair {
        protected PassphraseKeyPair() : base() { }

        /// <summary>
        /// Provides the passphrase and decrypts the private.
        /// Returns true if the passphrase resulted in decryption.
        /// Returns false if the passphrase did not decrypt the key (even if the key is
        /// already unencrypted and no passphrase was needed).
        /// </summary>
        public virtual bool DecryptWithPassphrase(string passphrase) {
            return false;
        }
    }
}

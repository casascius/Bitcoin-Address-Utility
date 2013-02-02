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

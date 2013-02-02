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
using Org.BouncyCastle.Math;
using Casascius.Bitcoin;

namespace BtcAddress.Forms {
    public partial class DecryptKey : Form {
        public DecryptKey() {
            InitializeComponent();
        }

        private void btnDecrypt_Click(object sender, EventArgs e) {
            // Remove any spaces or dashes from the encrypted key (in case they were typed)
            txtEncrypted.Text = txtEncrypted.Text.Replace("-", "").Replace(" ", "");

            if (txtEncrypted.Text == "" || txtPassphrase.Text == "") {
                MessageBox.Show("Enter an encrypted key and its passphrase.", "Entries Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // What were we given as encrypted text?

            object encrypted = StringInterpreter.Interpret(txtEncrypted.Text);

            if (encrypted == null) {
                if (txtEncrypted.Text.StartsWith("cfrm38")) {
                    var r = MessageBox.Show("This is not a private key.  This looks like a confirmation code.  " +
                        "Do you want to open the Confirmation Code Validator?", "Invalid private key", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (r == DialogResult.Yes) {
                        Program.ShowConfValidator();
                    }
                    return;
                }


                string containsL = "";
                if (txtEncrypted.Text.Contains("l")) {
                    containsL = " Your entry contains the lowercase letter l.  Private keys are far " +
                        "more likely to contain the digit 1, and not the lowercase letter l.";
                }

                MessageBox.Show("The private key entry (top box) was invalid.  " +
                    "Please verify the private key was properly typed." + containsL, "Invalid private key", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            } if (encrypted is PassphraseKeyPair) {
                PassphraseKeyPair pkp = encrypted as PassphraseKeyPair;
                if (pkp.DecryptWithPassphrase(txtPassphrase.Text) == false) {
                    MessageBox.Show("The passphrase is incorrect.", "Could not decrypt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                MessageBox.Show("Decryption successful.", "Decryption", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Program.ShowAddressUtility();
                Program.AddressUtility.DisplayKeyCollectionItem(new KeyCollectionItem(pkp.GetUnencryptedPrivateKey()));
                return;                        
            } else if (encrypted is KeyPair) {
                // it's unencrypted - perhaps we're doing an EC multiply and the passphrase is a private key.

                object encrypted2 = StringInterpreter.Interpret(txtPassphrase.Text);
                if (encrypted2 == null) {
                    var r = MessageBox.Show("Does the key you entered belong to the following address?: " + (encrypted as KeyPair).AddressBase58,
                        "Key appears unencrypted",
                        MessageBoxButtons.YesNo);

                    if (r == DialogResult.Yes) {
                        r = MessageBox.Show("Then this key is already unencrypted and you don't need to decrypt it.  " +
                            "Would you like to open it in the Address Utility screen to see its various forms?", "Key is not encrypted", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (r == DialogResult.Yes) {
                            Program.ShowAddressUtility();
                            Program.AddressUtility.DisplayKeyCollectionItem(new KeyCollectionItem(encrypted as KeyPair));
                        }
                    } else {
                        MessageBox.Show("The passphrase or secondary key is incorrect.  Please verify it was properly typed.", "Second entry is not a valid private key", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    return;
                }


                BigInteger n1 = new BigInteger(1, (encrypted as KeyPair).PrivateKeyBytes);
                BigInteger n2 = new BigInteger(1, (encrypted2 as KeyPair).PrivateKeyBytes);
                var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");
                BigInteger privatekey = n1.Multiply(n2).Mod(ps.N);
                MessageBox.Show("Keys successfully combined using EC multiplication.", "EC multiplication successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (n1.Equals(n2)) {
                    MessageBox.Show("The two key entries have the same public hash.  The results you see might be wrong.", "Duplicate key hash", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // use private key
                KeyPair kp = new KeyPair(privatekey);
                Program.ShowAddressUtility();
                Program.AddressUtility.DisplayKeyCollectionItem(new KeyCollectionItem(kp));



            } else if (encrypted is AddressBase) {
                MessageBox.Show("This is not a private key.  It looks like an address or a public key.  Private keys usually start with 5, 6, or S.", "Not a private key", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            } else {
                MessageBox.Show("This is not a private key that this program can decrypt.", "Not a recognized private key", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }




        }
    }
}

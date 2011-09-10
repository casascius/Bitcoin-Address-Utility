using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Math.EC;
using System.IO;

namespace BtcAddress {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

		// a different test

        private void btnPassphrase_Click(object sender, EventArgs e) {

            ChangeFlag++;
            try {
                SetText(txtPrivHex, Bitcoin.PassphraseToPrivHex(txtPassphrase.Text));
                int isminikey = Bitcoin.IsValidMiniKey(txtPassphrase.Text);
                if (isminikey==1) {
                    lblWhyNot.Visible = false;
                    lblNotSafe.Visible = true;
                    lblNotSafe.Text = "Valid mini key";
                    lblNotSafe.ForeColor = Color.DarkGreen;
                } else if (isminikey==-1) {
                    lblWhyNot.Visible = false;
                    lblNotSafe.Visible = true;
                    lblNotSafe.Text = "Invalid mini key";
                    lblNotSafe.ForeColor = Color.Red;
                } else if (txtPassphrase.Text.Length < 20 || Bitcoin.PassphraseTooSimple(txtPassphrase.Text)) {
                    lblWhyNot.Visible = true;
                    lblNotSafe.Visible = true;
                    lblNotSafe.Text = "Warning - Not Safe";
                    lblNotSafe.ForeColor = Color.Red;
                } else {
                    lblWhyNot.Visible = false;
                    lblNotSafe.Visible = false;
                    lblNotSafe.Text = "Warning - Not Safe";
                    lblNotSafe.ForeColor = Color.Red;
                }

                btnPrivHexToWIF_Click(null, null);
                btnPrivToPub_Click(null, null);
                btnPubHexToHash_Click(null, null);
                btnPubHashToAddress_Click(null, null);
            } catch (ApplicationException ae) {
                MessageBox.Show(ae.Message);
            } finally {
                ChangeFlag--;
            }
        }


        private void btnPrivHexToWIF_Click(object sender, EventArgs e) {
            ChangeFlag++;
            try {
                SetText(txtPrivWIF, Bitcoin.PrivHexToWIF(txtPrivHex.Text));
                btnPrivToPub_Click(null, null);
            } catch (ApplicationException ae) {
                MessageBox.Show(ae.Message);
            } finally {
                ChangeFlag--;
            }


        }

        private void btnPrivWIFToHex_Click(object sender, EventArgs e) {
            ChangeFlag++;
            try {
                SetText(txtPrivHex, Bitcoin.PrivWIFtoPrivHex(txtPrivWIF.Text));

                btnPrivHexToWIF_Click(null, null);

            } catch (ApplicationException ae) {
                MessageBox.Show(ae.Message);
            } finally {
                ChangeFlag--;
            }

        }

        private void btnPrivToPub_Click(object sender, EventArgs e) {

            ChangeFlag++;
            try {
                SetText(txtPubHex, Bitcoin.PrivHexToPubHex(txtPrivHex.Text));
                btnPubHexToHash_Click(null, null);
            } catch (ApplicationException ae) {
                MessageBox.Show(ae.Message);
            } finally {
                ChangeFlag--;
            }

        }

        private void btnPubHexToHash_Click(object sender, EventArgs e) {
            ChangeFlag++;
            try {
                SetText(txtPubHash, Bitcoin.PubHexToPubHash(txtPubHex.Text));
                btnPubHashToAddress_Click(null, null);
            } catch (ApplicationException ae) {
                MessageBox.Show(ae.Message);
            } finally {
                ChangeFlag--;
            }
        }

        private void btnPubHashToAddress_Click(object sender, EventArgs e) {
            ChangeFlag++;
            try {
                SetText(txtBtcAddr, Bitcoin.PubHashToAddress(txtPubHash.Text, cboCoinType.Text));
            } catch (ApplicationException ae) {
                MessageBox.Show(ae.Message);
            } finally {
                ChangeFlag--;
            }
        }

        private void btnAddressToPubHash_Click(object sender, EventArgs e) {
            ChangeFlag++;
            try {
                byte[] hex = Bitcoin.Base58ToByteArray(txtBtcAddr.Text);
                if (hex == null || hex.Length != 21) {
                    int L = txtBtcAddr.Text.Length;
                    if (L >= 33 && L <= 34) {
                        if (MessageBox.Show("Address is not valid.  Attempt to correct?", "Invalid address", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                            CorrectBitcoinAddress();
                            return;
                        }
                    } else {
                        MessageBox.Show("Address is not valid.");
                    }
                    return;
                }
                SetText(txtPubHash, Bitcoin.ByteArrayToString(hex, 1, 20));
            } finally {
                ChangeFlag--;
            }

        }

        private void btnGenerate_Click(object sender, EventArgs e) {
            ChangeFlag++;
            try {
                lblNotSafe.Visible = false;
                lblWhyNot.Visible = false;
                SetText(txtPassphrase, "");

                ECKeyPairGenerator gen = new ECKeyPairGenerator();
                var secureRandom = new SecureRandom();
                var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");
                var ecParams = new ECDomainParameters(ps.Curve, ps.G, ps.N, ps.H);
                var keyGenParam = new ECKeyGenerationParameters(ecParams, secureRandom);
                gen.Init(keyGenParam);

                AsymmetricCipherKeyPair kp = gen.GenerateKeyPair();

                ECPrivateKeyParameters priv = (ECPrivateKeyParameters)kp.Private;

                byte[] hexpriv = priv.D.ToByteArrayUnsigned();
                SetText(txtPrivHex, Bitcoin.ByteArrayToString(hexpriv));

                btnPrivHexToWIF_Click(null, null);
            } finally {
                ChangeFlag--;
            }

        }

        private void btnBlockExplorer_Click(object sender, EventArgs e) {
            try {
                if (cboCoinType.Text == "Testnet") {
                    Process.Start("http://www.blockexplorer.com/testnet/address/" + txtBtcAddr.Text);
                } else if (cboCoinType.Text == "Namecoin") {
                    Process.Start("http://explorer.dot-bit.org/a/" + txtBtcAddr.Text);
                } else {
                    Process.Start("http://www.blockexplorer.com/address/" + txtBtcAddr.Text);
                }
            } catch { }
            
        }

        private void CorrectBitcoinAddress() {
            ChangeFlag++;
            try {
                txtBtcAddr.Text = Correction(txtBtcAddr.Text);
            } finally {
                ChangeFlag--;
            }
        }

        private string Correction(string btcaddr) {
            
            int btcaddrlen = btcaddr.Length;
            string b58 = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

            for (int i = 0; i < btcaddrlen; i++) {
                for (int j = 0; j < 58; j++) {
                    string attempt = btcaddr.Substring(0, i) + b58.Substring(j, 1) + btcaddr.Substring(i + 1);
                    byte[] bytes = Bitcoin.Base58ToByteArray(attempt);
                    if (bytes != null) {
                        MessageBox.Show("Correction was successful.  Try your request again.");
                        return attempt;
                    }
                }
            }
            return btcaddr;
        }

        private void CorrectWIF() {
            ChangeFlag++;
            try {
                txtPrivWIF.Text = Correction(txtPrivWIF.Text);
            } finally {
                ChangeFlag--;
            }
        }

        private void btnShacode_Click(object sender, EventArgs e) {
            ChangeFlag++;
            try {
                // SHAcode is a 22-character string that starts with S, whose SHA256 hash can be used as private key.
                // There is a simple 8-bit check: first byte of SHA256(string + '?') must be 00.

                string b58 = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
                SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                Sha256Digest bcsha256a = new Sha256Digest();
                Sha256Digest bcsha256b = new Sha256Digest();

                string shacode = "";
                SecureRandom sr = new SecureRandom();          

                do {

                    // Get 'S' + 21 random base58 characters, where sha256(result + '?') starts with the byte 00 (1 in 256 possibilities)
                    shacode = "S";
                    for (int i = 0; i < 21; i++) {
                        long x = sr.NextLong() & long.MaxValue;
                        long x58 = x % 58;
                        shacode += b58.Substring((int)x58, 1);
                    }
                    string shacodeq = shacode + "?";
                    byte[] ahash = sha256.ComputeHash(Encoding.ASCII.GetBytes(shacodeq));
                    // for more secure codes (but poorer performance - or perhaps put in an hourglass) uncomment the following.
                    // Note that 2 rounds of sha256 are done within each loop, and 358 * 2 = 716.
                    // The check code looks for a leading 0 byte either on the first or the 717th round of sha256
                    // per Mini private key format (Wiki).
                    /*
                    byte[] bhash = new byte[32];
                    for (int ct = 0; ct < 358; ct++) {
                        bcsha256a.BlockUpdate(ahash, 0, 32);
                        bcsha256a.DoFinal(bhash, 0);
                        bcsha256a.Reset();
                        bcsha256a.BlockUpdate(bhash, 0, 32);
                        bcsha256a.DoFinal(ahash, 0);
                        bcsha256a.Reset();
                    }
                     */ 
                    if (ahash[0] == 0) break;
                } while (true);

                txtPassphrase.Text = shacode;

                btnPassphrase_Click(null, null);
                btnPrivWIFToHex_Click(null, null);
                btnPrivToPub_Click(null, null);
                btnPubHexToHash_Click(null, null);
                btnPubHashToAddress_Click(null, null);

            } finally {
                ChangeFlag--;
            }
        }

        private void walletGeneratorToolStripMenuItem_Click(object sender, EventArgs e) {
            Walletgen w = new Walletgen();
            w.Show();
        }

        private void whatIsASHAcodeToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("A Mini Private Key is a Bitcoin address generated from a short 22-character string of text.  The advantage is that the text representation of the private key is shorter, and easier to use in " +
            "QR codes and in other places where space is at a premium. " +
                "The private key is simply the SHA256 hash of the string.  SHAcodes start with the letter 'S' and include a 7-bit typo check.");
        }

        private void lblWhyNot_Click(object sender, EventArgs e) {
            MessageBox.Show("Bitcoins are vulnerable to theft from hackers when sent to addresses generated from short or non-complex passphrases.  A longer one, or one that uses a good " +
              "mix of uppercase, lowercase, numbers, and symbols is recommended.", "Security Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Counter used for tracking the number of functions in the call stack that might change the text in textboxes.
        /// If this is 0, we assume the user made the changes, and grey out anything that depends on what they changed.
        /// If nonzero, the program is making the changes, which should ungrey what was changed.
        /// </summary>
        private int ChangeFlag = 0;

        /// <summary>
        /// Greys the text in all textboxes except for the one that has just been changed.
        /// </summary>
        private void TextBox_TextChanged(object sender, EventArgs e) {
            if (ChangeFlag > 0) return;
            TextBox txtSender = (TextBox)sender;
            TextBox[] textboxes = new TextBox[] { txtPassphrase, txtPrivWIF, txtPrivHex, txtPubHex, txtPubHash, txtBtcAddr };
            foreach (TextBox t in textboxes) {
                t.ForeColor = (t == txtSender) ? SystemColors.WindowText : SystemColors.GrayText;
            }
            // if passphrase changed, remove notation
            if (txtSender == txtPassphrase && lblNotSafe.Visible) {
                lblNotSafe.Visible = false;
                lblWhyNot.Visible = false;
            }

        }
        /// <summary>
        /// Changes the text in a textbox ensuring that it will be ungreyed in the process.
        /// (Needed to do it this way, because changing the Text property won't fire TextChanged if it is getting set to what happens
        /// to already be in the box.        
        /// </summary>
        private void SetText(TextBox thebox, string TheText) {
            thebox.ForeColor = SystemColors.WindowText;
            thebox.Text = TheText;
        }

        /// <summary>
        /// if coin type is changed, grey out the address until it is rerendered
        /// </summary>
        private void cboCoinType_SelectionChangeCommitted(object sender, EventArgs e) {
            txtBtcAddr.ForeColor = SystemColors.GrayText;
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == 13) {
                e.Handled = true;
                TextBox txtSender = (TextBox)sender;
                if (txtSender == txtPassphrase) btnPassphrase_Click(null, null);
                if (txtSender == txtPrivWIF) btnPrivWIFToHex_Click(null, null);
            }
        }

    }
}

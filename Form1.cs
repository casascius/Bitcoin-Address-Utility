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



        private void btnPassphrase_Click(object sender, EventArgs e) {

            try {
                txtPrivHex.Text = Bitcoin.PassphraseToPrivHex(txtPassphrase.Text);
                if (txtPassphrase.Text.Length < 20 || Bitcoin.PassphraseTooSimple(txtPassphrase.Text)) {
                    lblWhyNot.Visible = true;
                    lblNotSafe.Visible = true;
                } else {
                    lblWhyNot.Visible = false;
                    lblNotSafe.Visible = false;
                }

                btnPrivHexToWIF_Click(null, null);
                btnPrivToPub_Click(null, null);
                btnPubHexToHash_Click(null, null);
                btnPubHashToAddress_Click(null, null);
              


            } catch (ApplicationException ae) {
                MessageBox.Show(ae.Message);
            }




        }


        private void btnPrivHexToWIF_Click(object sender, EventArgs e) {

            try {
                txtPrivWIF.Text = Bitcoin.PrivHexToWIF(txtPrivHex.Text);
                btnPrivToPub_Click(null, null);
            } catch (ApplicationException ae) {
                MessageBox.Show(ae.Message);
            }


        }

        private void btnPrivWIFToHex_Click(object sender, EventArgs e) {

            try {
                txtPrivHex.Text = Bitcoin.PrivWIFtoPrivHex(txtPrivWIF.Text);

                btnPrivHexToWIF_Click(null, null);
                
              
                
            } catch (ApplicationException ae) {
                MessageBox.Show(ae.Message);
            }

        }

        private void btnPrivToPub_Click(object sender, EventArgs e) {

            try {
                txtPubHex.Text = Bitcoin.PrivHexToPubHex(txtPrivHex.Text);
                btnPubHexToHash_Click(null, null);
            } catch (ApplicationException ae) {
                MessageBox.Show(ae.Message);
            }

        }

        private void btnPubHexToHash_Click(object sender, EventArgs e) {

            try {
                txtPubHash.Text = Bitcoin.PubHexToPubHash(txtPubHex.Text);
                btnPubHashToAddress_Click(null, null);
            } catch (ApplicationException ae) {
                MessageBox.Show(ae.Message);
            }
        }

        private void btnPubHashToAddress_Click(object sender, EventArgs e) {
            try {
                txtBtcAddr.Text = Bitcoin.PubHashToAddress(txtPubHash.Text, cboCoinType.SelectedText);
            } catch (ApplicationException ae) {
                MessageBox.Show(ae.Message);
            }            
        }

        private void btnAddressToPubHash_Click(object sender, EventArgs e) {
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
            txtPubHash.Text = Bitcoin.ByteArrayToString(hex, 1, 20);

        }

        private void btnGenerate_Click(object sender, EventArgs e) {
            lblNotSafe.Visible = false;
            lblWhyNot.Visible = false;
            txtPassphrase.Text = "";

            ECKeyPairGenerator gen = new ECKeyPairGenerator();
            var secureRandom = new SecureRandom();
            var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");
            var ecParams = new ECDomainParameters(ps.Curve, ps.G, ps.N, ps.H);
            var keyGenParam = new ECKeyGenerationParameters(ecParams, secureRandom);
            gen.Init(keyGenParam);

            AsymmetricCipherKeyPair kp = gen.GenerateKeyPair();

            ECPrivateKeyParameters priv = (ECPrivateKeyParameters)kp.Private;

            byte[] hexpriv = priv.D.ToByteArrayUnsigned();
            txtPrivHex.Text = Bitcoin.ByteArrayToString(hexpriv);

            btnPrivHexToWIF_Click(null, null);

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
            txtBtcAddr.Text = Correction(txtBtcAddr.Text);
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
            txtPrivWIF.Text = Correction(txtPrivWIF.Text);
        }

        private void btnShacode_Click(object sender, EventArgs e) {

            // SHAcode is a 22-character string that starts with S, whose SHA256 hash can be used as private key.
            // There is a simple 8-bit check: first byte of SHA256(string + '?') must be 00.
            
            string b58 = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
            string shacode = "";
            do {

                // Get 'S' + 21 random base58 characters, where sha256(result + '?') starts with the byte 00 (1 in 256 possibilities)
                shacode = "S";
                SecureRandom sr = new SecureRandom();
                for (int i = 0; i < 21; i++) {
                    long x = sr.NextLong() & long.MaxValue;                    
                    long x58 = x % 58;
                    shacode += b58.Substring((int)x58, 1);                    
                }                
                string shacodeq = shacode + "?";
                if (sha256.ComputeHash(Encoding.ASCII.GetBytes(shacodeq))[0] == 0) break;
            } while (true);

            txtPassphrase.Text = shacode;

            btnPassphrase_Click(null, null);
            btnPrivWIFToHex_Click(null, null);
            btnPrivToPub_Click(null, null);
            btnPubHexToHash_Click(null, null);
            btnPubHashToAddress_Click(null, null);







        }

        private void walletGeneratorToolStripMenuItem_Click(object sender, EventArgs e) {
            Walletgen w = new Walletgen();
            w.Show();
        }

        private void whatIsASHAcodeToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("A SHAcode is a Bitcoin address generated from a short 22-character string of text.  The advantage is that the text representation of the private key is shorter, and easier to use in " +
            "QR codes and in other places where space is at a premium. " +
                "The private key is simply the SHA256 hash of the string.  SHAcodes start with the letter 'S' and include an 8-bit typo check.");
        }

        private void lblWhyNot_Click(object sender, EventArgs e) {
            MessageBox.Show("Bitcoins are vulnerable to theft from hackers when sent to addresses generated from short or non-complex passphrases.  A longer one, or one that uses a good " +
              "mix of uppercase, lowercase, numbers, and symbols is recommended.", "Security Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}

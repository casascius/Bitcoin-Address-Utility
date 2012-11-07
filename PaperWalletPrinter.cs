using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using System.Drawing.Printing;
using System.IO;

namespace BtcAddress {
    public partial class PaperWalletPrinter : Form {

        protected int CurrentSequence;
        protected string CurrentPassphrase;
        protected bool CurrentlyGenerating = false;
        protected int TotalToGenerate = 0;
        protected List<KeyPair> Addresses = new List<KeyPair>();


        protected bool CurrentSelectionPrinted = false;
        protected bool CurrentSelectionSaved = false;

        public PaperWalletPrinter() {
            InitializeComponent();
        }

        private void rdoDeterministicWallet_CheckedChanged(object sender, EventArgs e) {
            if (rdoDeterministicWallet.Checked) {
                lblInputDescription.Text = "Passphrase";
                txtPassphrase.Text = GetUglyRandomString();
            }
        }

        private void rdoRandomWallet_CheckedChanged(object sender, EventArgs e) {
            if (rdoRandomWallet.Checked) {
                lblInputDescription.Text = "Enter some random text with your keyboard to add entropy.";
            }
        }

        private string GetUglyRandomString() {
            StringBuilder sb = new StringBuilder(128);
            for (int i = 0; i < 64; i++) {
                SecureRandom sr = new SecureRandom();
                int idx = sr.Next(0, 61);
                sb.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".Substring(idx, 1));
            }
            return sb.ToString();

        }

        private void LockButtons(bool locked) {
            btnSortKeys.Enabled = !locked;
            btnPrintWallet.Enabled = !locked;
            btnSaveAddresses.Enabled = !locked;
            txtEncryptionPassword.Enabled = !locked;
            chkEncrypt.Enabled = !locked;
        }


        private void btnGenerateAddresses_Click(object sender, EventArgs e) {
            if (CurrentlyGenerating == false) {
                if (rdoRandomWallet.Checked) {
                    if (txtPassphrase.Text.Length < 30) {
                        MessageBox.Show("Please provide some random characters.  Just hit different keys on the keyboard until the box is full. " +
                            "This adds security to your paper wallet.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                if (numGenCount.Value < 1) {
                    MessageBox.Show("Enter the number of addresses to create.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (chkEncrypt.Checked && txtEncryptionPassword.Text == "") {
                    MessageBox.Show("You must enter an encryption passphrase if you are generating encrypted private keys.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                if (Addresses.Count > 0 && CurrentSelectionSaved==false && rdoDeterministicWallet.Checked==false) {

                    string msg = "You have generated " + Addresses.Count + " addresses, which will be discarded if you continue.  Continue?";
                        if (Addresses.Count == 1) msg = msg.Replace("addresses", "address");

                    if (MessageBox.Show(msg, "Continue with generation?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

                }

                if (rdoDeterministicWallet.Checked && txtPassphrase.Text == CurrentPassphrase) {
                    string msg = "You have not changed the passphrase since the last time you generated addresses, so you will be generating the same addresses as last time.  Continue?";
                    if (MessageBox.Show(msg, "Continue with generation?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

                } else {
                    CurrentSelectionSaved = false;
                    CurrentSelectionPrinted = false;
                }

                if (txtEncryptionPassword.Text != txtEncryptionPassword.Text.Trim()) {
                    MessageBox.Show("The encryption passphrase you entered starts or ends with whitespace.  The whitespace will be part of the passphrase needed to decrypt.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                Addresses = new List<KeyPair>();
                

                lblGenCount.Text = Addresses.Count.ToString() + (txtEncryptionPassword.Text != "" ? " encrypted" : "") + " addresses have been generated.";

                TotalToGenerate = (int)numGenCount.Value;
                CurrentSequence = 1;

                if (rdoRandomWallet.Checked) {
                    CurrentPassphrase = txtPassphrase.Text + GetUglyRandomString();
                } else {
                    if (txtPassphrase.Text.Length < 30) {
                        if (MessageBox.Show("Passphrases must be highly unique and very long to be secure against hackers, who try trillions of random passwords in hopes of " +
                            "finding coins to steal.  Use a Random Wallet if you are not 100% sure about what you're doing.  Continue?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)
                         == DialogResult.Cancel) return;
                    }
                    CurrentPassphrase = txtPassphrase.Text;
                }

                CurrentlyGenerating = true;
                LockButtons(true);
                btnGenerateAddresses.Text = "Stop generating";
                timer1.Enabled = true;

            } else {
                CurrentlyGenerating = false;
                LockButtons(false);
                timer1.Enabled = false;
                btnGenerateAddresses.Text = "Generate addresses";
            }
        }

        private void timer1_Tick(object sender, EventArgs e) {
            if (CurrentlyGenerating == false) return;
            if (CurrentSequence >= TotalToGenerate) {
                CurrentlyGenerating = false;
                LockButtons( false);
                timer1.Enabled = false;
                btnGenerateAddresses.Text = "Generate addresses";
            }

            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();

            string myhash = CurrentPassphrase + ((int)CurrentSequence).ToString();

            KeyPair k;
            if (chkMiniKeys.Checked) {
                k = MiniKeyPair.CreateDeterministic(myhash);
            } else {
                UTF8Encoding utf8 = new UTF8Encoding(false);
                byte[] mykey = sha256.ComputeHash(utf8.GetBytes(myhash));
                k = new KeyPair(mykey,txtPassphrase.Text);
            }
            Addresses.Add(k);
            lblGenCount.Text = Addresses.Count.ToString() + (txtEncryptionPassword.Text != "" ? " encrypted" : "") + " addresses have been generated.";
            CurrentSequence++;

        }

        private void btnPrintWallet_Click(object sender, EventArgs e) {
            if (Addresses.Count == 0) {
                MessageBox.Show("Please generate some addresses before trying to print.");
                return;
            }

            if (CurrentSelectionPrinted) {
                string msg = "You have already printed these addresses before.  Print again?";
                if (MessageBox.Show(msg, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            }


            PrintDialog pd = new PrintDialog();
            PrinterSettings  ps = new PrinterSettings();
            pd.PrinterSettings = ps;
            DialogResult dr = pd.ShowDialog();

            if (dr == DialogResult.OK) {
                QRPrint printer = new QRPrint();
                if (this.rdoWalletPrivQR.Checked) printer.PrintMode = QRPrint.PrintModes.PrivQR;
                if (this.rdoWalletPubPrivQR.Checked) printer.PrintMode = QRPrint.PrintModes.PubPrivQR;
                if (this.rdoBitcoinBanknote.Checked) printer.PrintMode = QRPrint.PrintModes.PsyBanknote;
                printer.NotesPerPage = Convert.ToInt32(cboNumPerPage.SelectedItem.ToString());
                printer.ImageFilename = "note-" + cboColor.SelectedItem.ToString() + ".png";
                printer.Denomination = txtDenomination.Text;
                printer.keys = new List<Address>(Addresses.Count);
                foreach (Address a in Addresses) printer.keys.Add(a);
                printer.PrinterSettings = pd.PrinterSettings;
                CurrentSelectionPrinted = true;
                printer.Print();

            }



        }

        private void btnSaveAddresses_Click(object sender, EventArgs e) {
            if (Addresses.Count == 0) {
                MessageBox.Show("First generate some addresses.");
                return;
            }

            if (rdoSavePrivKeys.Checked) {
                if (MessageBox.Show("Saving the private keys is a security risk.  Only do this if you know what " +
                    "you're doing.  Continue?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel) return;
            }

            if (rdoSendmany.Checked && txtDenomination.Text == "") {
                MessageBox.Show("Sendmany requires a denomination.  Enter the denomination in BTC in the box above, and try again.");
                lblDenomination.Visible = true;
                txtDenomination.Visible = true;
                return;
            }


            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "Text file|*.txt";
            fd.Title = "Save Text File";
            fd.ShowDialog();

            bool wroteFirst=false;
            if (fd.FileName != "") {
                try {
                    using (StreamWriter sw = new StreamWriter(fd.FileName)) {
                        foreach (KeyPair k in Addresses) {
                            if (rdoSaveAddressesOnly.Checked) {
                                sw.WriteLine(k.AddressBase58);
                            } else if (rdoSavePrivKeys.Checked) {

                                sw.WriteLine(k.AddressBase58 + "," + k.PrivateKeyBase58);
                            } else if (rdoSendmany.Checked) {
                                if (wroteFirst) {
                                    sw.Write(", ");
                                } else {
                                    sw.Write("bitcoind sendmany \"\" \"{");
                                    wroteFirst = true;
                                }
                                sw.Write(string.Format("\\\"{0}\\\": {1}", k.AddressBase58, txtDenomination.Text));
                            }
                        
                        }
                        if (rdoSendmany.Checked) {
                            sw.Write("}\""); // no CR on purpose, so paste doesn't execute command
                        }
                        CurrentSelectionSaved = true;
                        sw.Close();
                    }
                } catch (Exception ex) {
                    MessageBox.Show("Could not save file. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void btnSortKeys_Click(object sender, EventArgs e) {
            if (Addresses.Count == 0) {
                MessageBox.Show("Generate some addresses first.");
                return;
            }

            Addresses.Sort(new KeyComparer());

            MessageBox.Show("The addresses in memory have been sorted into alphabetical (case-insensitive) order by Bitcoin address, and will save and print in this order.");

        }

        private void chkEncrypt_CheckedChanged(object sender, EventArgs e) {
            chkMiniKeys.Enabled = !chkEncrypt.Checked;
            if (chkEncrypt.Checked) {
                lblPassword.Visible = true;
                txtEncryptionPassword.Visible = true;
            } else {
                txtEncryptionPassword.Text = "";
                txtEncryptionPassword.Visible = false;
                lblPassword.Visible = false;
            }
        }

        private void PaperWalletPrinter_Load(object sender, EventArgs e) {
            cboColor.SelectedIndex = 0;
            cboNumPerPage.SelectedIndex = 0;
        }

        private void rdoBitcoinBanknote_CheckedChanged(object sender, EventArgs e) {

        }

        private void chkMiniKeys_CheckedChanged(object sender, EventArgs e) {
            chkEncrypt.Enabled = !chkMiniKeys.Checked;
        }


    }
}

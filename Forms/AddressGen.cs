using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace BtcAddress.Forms {
    public partial class AddressGen : Form {
        public AddressGen() {
            InitializeComponent();
        }

        private enum GenChoices {
            Minikey, WIF, Encrypted, Deterministic
        }

        private GenChoices GenChoice;

        private bool Generating = false;
        private bool GeneratingEnded = false;

        private bool StopRequested = false;

        private bool PermissionToCloseWindow = false;

        private bool RetainPrivateKeys = false;

        private string UserText;

        private int RemainingToGenerate = 0;

        private Thread GenerationThread = null;

        public List<KeyCollectionItem> GeneratedItems = new List<KeyCollectionItem>();

        private void rdoWalletType_CheckedChanged(object sender, EventArgs e) {
            txtTextInput.Text = "";
            txtTextInput.Visible = (rdoDeterministicWallet.Checked || rdoEncrypted.Checked);
            if (rdoDeterministicWallet.Checked) {
                lblTextInput.Text = "Seed for deterministic generation";
            } else if (rdoEncrypted.Checked) {
                lblTextInput.Text = "Encryption passphrase or Intermediate Code";                
            }
            lblTextInput.Visible = txtTextInput.Visible;
            chkRetainPrivKey.Visible = (rdoEncrypted.Checked);
        }

        private void AddressGen_FormClosing(object sender, FormClosingEventArgs e) {
            if (PermissionToCloseWindow) return;
            if (Generating) {
                if (MessageBox.Show("Cancel and abandon generation in progress?", "Abort generation", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No) {
                    e.Cancel = true;
                } else {
                    StopRequested = true;
                    if (GenerationThread.ThreadState == System.Threading.ThreadState.Running) {
                        GenerationThread.Join();
                        GeneratedItems.Clear();
                    }
                }
            }
        }

        private void btnGenerateAddresses_Click(object sender, EventArgs e) {

            if (Generating) {
                StopRequested = true;
                btnGenerateAddresses.Text = "Stopping...";
                return;
            }

            if (rdoEncrypted.Checked && txtTextInput.Text == "") {
                MessageBox.Show("An encryption passphrase is required. Choose a different option if you don't want encrypted keys.",
                    "Passphrase missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (rdoDeterministicWallet.Checked && txtTextInput.Text == "") {
                MessageBox.Show("A deterministic seed is required.  If you do not intend to create a deterministic " +
                    "wallet or know what one is used for, it is recommended you choose one of the other options.  An inappropriate seed can result " +
                    "in the unexpected theft of funds.",
                    "Seed missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }



            GenerationThread = new Thread(new ThreadStart(GenerationThreadProcess));
            RemainingToGenerate = (int)numGenCount.Value;
            UserText = txtTextInput.Text;
            RetainPrivateKeys = chkRetainPrivKey.Checked;

            if (rdoDeterministicWallet.Checked) GenChoice = GenChoices.Deterministic;
            if (rdoEncrypted.Checked) {
                GenChoice = GenChoices.Encrypted;
                // intermediate codes start with "passphrasek" thru "passphrases"
                string ti = txtTextInput.Text.Trim();
                if (txtTextInput.Text.Length > 40 && ti.CompareTo("passphrasek") > 0 && ti.CompareTo("passphraset") < 0) {
                    Bip38Intermediate inter = null;
                    // try using it as an intermediate
                    try {
                        inter = new Bip38Intermediate(txtTextInput.Text.Trim(), Bip38Intermediate.Interpretation.IntermediateCode);
                        // if this is an actual intermediate code, ensure surrounding whitespace isn't preserved.
                        txtTextInput.Text = txtTextInput.Text.Trim();
                    } catch {
                        var r = MessageBox.Show("The passphrase resembles an Intermediate Code, but isn't one. " +
                        "If this is supposed to be an intermediate code, it is invalid, malformed, or has an error. " +
                            "If you're attempting to generate from an intermediate code, the resulting keys will not work as expected. " +
                            "Do you want to continue?",
                            "Invalid intermediate code", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                        if (r == System.Windows.Forms.DialogResult.Cancel) {
                            return;

                        }
                    }
                    if (inter != null) {
                        MessageBox.Show("Intermediate Code accepted.  " +
                            "Intermediate Codes can be used for generating encrypted keys, but not for decrypting them. " +
                            "You will only be able to decrypt generated keys with " +
                            "the original passphrase that was used to create the Intermediate Code.", "Intermediate Code accepted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                txtTextInput.UseSystemPasswordChar = true;
            }
            if (rdoMiniKeys.Checked) GenChoice = GenChoices.Minikey;
            if (rdoRandomWallet.Checked) GenChoice = GenChoices.WIF;

            timer1.Interval = 250;
            timer1.Enabled = true;
            Generating = true;
            GeneratingEnded = false;
            StopRequested = false;
            btnGenerateAddresses.Text = "Cancel";
            SetControlsEnabled(false);
            toolStripProgressBar1.Visible = true;
            GenerationThread.Start();

        }

        private void SetControlsEnabled(bool enabled) {
            foreach (Control c in this.Controls) {
                if (c is TextBox) {
                    ((TextBox)c).Enabled = enabled;
                } else if (c is NumericUpDown) {
                    ((NumericUpDown)c).Enabled = enabled;
                }
            }
            foreach (Control c in groupBox1.Controls) {
                if (c is RadioButton) {
                    ((RadioButton)c).Enabled = enabled;
                }
            }

        }

        /// <summary>
        /// Code which is actually run on the generation thread.
        /// </summary>
        private void GenerationThreadProcess() {

            Bip38Intermediate intermediate = null;
            if (GenChoice == GenChoices.Encrypted) {
                try {
                    intermediate = new Bip38Intermediate(txtTextInput.Text, Bip38Intermediate.Interpretation.IntermediateCode);
                } catch { } // sink exceptions - just means it's not an intermediate code
                if (intermediate == null) {
                    intermediate = new Bip38Intermediate(UserText, Bip38Intermediate.Interpretation.Passphrase);
                }
            }

            int detcount = 1;

            while (RemainingToGenerate > 0 && StopRequested == false) {
                KeyCollectionItem newitem = null;
                switch (GenChoice) {
                    case GenChoices.Minikey:
                        MiniKeyPair mkp = MiniKeyPair.CreateRandom(ExtraEntropy.GetEntropy());
                        string s = mkp.AddressBase58; // read the property to entice it to compute everything
                        newitem = new KeyCollectionItem(mkp);
                        break;
                    case GenChoices.WIF:
                        KeyPair kp = KeyPair.Create(ExtraEntropy.GetEntropy());
                        s = kp.AddressBase58;
                        newitem = new KeyCollectionItem(kp);
                        break;
                    case GenChoices.Deterministic:
                        kp = KeyPair.CreateFromString(UserText + detcount);
                        detcount++;
                        s = kp.AddressBase58;
                        newitem = new KeyCollectionItem(kp);
                        break;
                    case GenChoices.Encrypted:
                        Bip38KeyPair ekp = new Bip38KeyPair(intermediate);
                        newitem = new KeyCollectionItem(ekp);
                        break;
                }

                lock (GeneratedItems) {
                    GeneratedItems.Add(newitem);
                    RemainingToGenerate--;
                }
            }
            GeneratingEnded = true;
        }

        private void timer1_Tick(object sender, EventArgs e) {
            if (GeneratingEnded) {
                Generating = false;
                GeneratingEnded = false;
                toolStripProgressBar1.Value = 0;
                toolStripProgressBar1.Visible = false;
                toolStripStatusLabel1.Text = "";

                btnGenerateAddresses.Text = "Generate Addresses";
                timer1.Enabled = false;
                SetControlsEnabled(true);
                if (StopRequested == false) {
                    PermissionToCloseWindow = true;
                    this.Close();
                } else if (GeneratedItems.Count > 0) {
                    toolStripStatusLabel1.Text = "Keys generated: " + GeneratedItems.Count;
                    if (PermissionToCloseWindow) {
                        this.Close();
                        return;
                    } else if (MessageBox.Show("Keep the " + GeneratedItems.Count + " generated keys?", "Cancel generation", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No) {
                        GeneratedItems.Clear();
                    }
                    PermissionToCloseWindow = true;
                    this.Close();
                }
                return;
            }

            if (Generating) {
                int generated = 0;
                int totaltogenerate = 0;
                lock (GeneratedItems) {
                    generated = GeneratedItems.Count;
                    totaltogenerate = generated + RemainingToGenerate;
                }
                if (generated == 0 && rdoEncrypted.Checked) {
                    toolStripStatusLabel1.Text = "Hashing the passphrase...";
                } else {
                    toolStripStatusLabel1.Text = "Keys generated: " + generated;
                    toolStripProgressBar1.Maximum = totaltogenerate;
                    toolStripProgressBar1.Value = generated;
                }
            }
        }
    }
}

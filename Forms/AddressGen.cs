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
using System.Threading;
using System.Diagnostics;
using Casascius.Bitcoin;

namespace BtcAddress.Forms {
    public partial class AddressGen : Form {
        public AddressGen() {
            InitializeComponent();
        }

        private enum GenChoices {
            Minikey, WIF, Encrypted, Deterministic, TwoFactor
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

        private Bip38Intermediate[] intermediatesForGeneration;

        private int intermediateIdx;

        private void rdoWalletType_CheckedChanged(object sender, EventArgs e) {
            txtTextInput.Text = "";
            txtTextInput.Visible = (rdoDeterministicWallet.Checked || rdoEncrypted.Checked);
            lblTextInput.Visible = (rdoDeterministicWallet.Checked || rdoEncrypted.Checked || rdoTwoFactor.Checked);
            if (rdoDeterministicWallet.Checked) {
                lblTextInput.Text = "Seed for deterministic generation";
            } else if (rdoEncrypted.Checked) {
                lblTextInput.Text = "Encryption passphrase or Intermediate Code";
            } else if (rdoTwoFactor.Checked) {
                int icodect = ScanClipboardForIntermediateCodes().Count;
                if (icodect == 0) {
                    lblTextInput.Text = "Copy one or more intermediate codes to the clipboard.";
                } else {
                    lblTextInput.Text = icodect + " intermediate codes found on clipboard.";
                }
            }
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

            if (rdoTwoFactor.Checked) {
                // Read the clipboard for intermediate codes
                List<Bip38Intermediate> intermediates = ScanClipboardForIntermediateCodes();
                if (intermediates.Count == 0) {
                    MessageBox.Show("No valid intermediate codes were found on the clipboard.  Intermediate codes are typically " +
                        "sent to you from someone else desiring paper wallets, or from your mobile phone.  Copy the received intermediate " +
                        "codes to the clipboard, and try again.  Address Generator automatically detects valid intermediate codes and ignores " +
                        "everything else on the clipboard", "No intermediate codes found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                intermediatesForGeneration = intermediates.ToArray();
                intermediateIdx = 0;

            } else {
                intermediatesForGeneration = null;
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
                txtTextInput.UseSystemPasswordChar = true;
            }
            if (rdoMiniKeys.Checked) GenChoice = GenChoices.Minikey;
            if (rdoRandomWallet.Checked) GenChoice = GenChoices.WIF;
            if (rdoTwoFactor.Checked) {
                GenChoice = GenChoices.TwoFactor;
            }

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
                intermediate = new Bip38Intermediate(UserText, Bip38Intermediate.Interpretation.Passphrase);                
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
                    case GenChoices.TwoFactor:
                        ekp = new Bip38KeyPair(intermediatesForGeneration[intermediateIdx++]);
                        if (intermediateIdx >= intermediatesForGeneration.Length) intermediateIdx = 0;
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

        private List<Bip38Intermediate> ScanClipboardForIntermediateCodes() {
            string cliptext = Clipboard.GetText(TextDataFormat.UnicodeText);
            List<object> objects = StringInterpreter.InterpretBatch(cliptext);
            List<Bip38Intermediate> intermediates = new List<Bip38Intermediate>(from c in objects where c is Bip38Intermediate select c as Bip38Intermediate);
            return intermediates;
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

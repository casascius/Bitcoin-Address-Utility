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
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using System.Drawing.Printing;
using System.IO;
using Casascius.Bitcoin;

namespace BtcAddress {
    public partial class PaperWalletPrinter : Form {

        protected int CurrentSequence;
        protected string CurrentPassphrase;
        protected bool CurrentlyGenerating = false;
        protected int TotalToGenerate = 0;
        protected List<KeyCollectionItem> Addresses = new List<KeyCollectionItem>();


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

                Addresses = new List<KeyCollectionItem>();
                

                lblGenCount.Text = Addresses.Count.ToString() + " addresses have been generated.";

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

            string myhash = CurrentPassphrase + ((int)CurrentSequence).ToString();

            KeyPair k;
            if (chkMiniKeys.Checked) {
                k = MiniKeyPair.CreateDeterministic(myhash);
            } else {
                byte[] mykey = Util.ComputeSha256(myhash);
            }
            lblGenCount.Text = Addresses.Count.ToString() + " addresses have been generated.";
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
                printer.keys = new List<KeyCollectionItem>(Addresses.Count);
                foreach (KeyCollectionItem a in Addresses) printer.keys.Add(a);
                printer.PrinterSettings = pd.PrinterSettings;
                CurrentSelectionPrinted = true;
                printer.Print();

            }



        }



    }
}

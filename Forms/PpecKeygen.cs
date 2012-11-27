using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using CryptSharp.Utility;
using Org.BouncyCastle.Math;


namespace BtcAddress {
    public partial class PpecKeygen : Form {
        public PpecKeygen() {
            InitializeComponent();
        }

        private void btnEncode_Click(object sender, EventArgs e) {
            if ((txtPassphrase.Text ?? "") == "") {
                MessageBox.Show("Enter a passphrase first.");
                return;
            }

            try {
                Bip38Intermediate intermediate = new Bip38Intermediate(txtPassphrase.Text, Bip38Intermediate.Interpretation.Passphrase);
                txtPassphraseCode.Text = intermediate.Code;
            } catch (Exception ae) {
                MessageBox.Show(ae.Message);
            }
        }

        private void btnGenerateKey_Click(object sender, EventArgs e) {
            try {
                Bip38Intermediate intermediate = new Bip38Intermediate(txtPassphraseCode.Text, Bip38Intermediate.Interpretation.IntermediateCode);
                Bip38KeyPair kp = new Bip38KeyPair(intermediate);
                txtEncryptedKey.Text = kp.EncryptedPrivateKey;
                txtBitcoinAddress.Text = kp.GetAddress().AddressBase58;
            } catch (Exception ae) {
                MessageBox.Show(ae.Message);
                return;
            }
       }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("https://en.bitcoin.it/wiki/BIP_0038");
        }



    }
}

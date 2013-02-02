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
using Casascius.Bitcoin;


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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("https://en.bitcoin.it/wiki/BIP_0038");
        }


    }
}

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
using System.Security.Cryptography;
using System.Diagnostics;
using System.Windows.Forms;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math;
using Casascius.Bitcoin;


namespace BtcAddress {
    public partial class MofNcalc : Form {
        public MofNcalc() {
            InitializeComponent();
        }

        private TextBox GetPartBox(int i) {
            TextBox[] parts = new TextBox[] {txtPart1,txtPart2,txtPart3,txtPart4,txtPart5,txtPart6,txtPart7,txtPart8};
            return parts[i];
        }


        private void textBox9_TextChanged(object sender, EventArgs e) {

        }

        private byte[] targetPrivKey = null;

        private void btnGenerate_Click(object sender, EventArgs e) {

            if (numPartsNeeded.Value > numPartsToGenerate.Value) {
                MessageBox.Show("Number of parts needed exceeds number of parts to generate.");
                return;
            }


            for (int i = 0; i < 8; i++) {
                TextBox t = GetPartBox(i);
                t.Text = "";
                t.BackColor = System.Drawing.Color.White;                
            }

            MofN mn = new MofN();

            if (targetPrivKey == null) {
                mn.Generate((int)numPartsNeeded.Value, (int)numPartsToGenerate.Value);
            } else {
                mn.Generate((int)numPartsNeeded.Value, (int)numPartsToGenerate.Value, targetPrivKey);
            }

            int j = 0;
            foreach (string kp in mn.GetKeyParts()) {
                GetPartBox(j++).Text = kp;
            }

            txtPrivKey.Text = mn.BitcoinPrivateKey ?? "?";
            txtAddress.Text = mn.BitcoinAddress ?? "?";

        }

        public static List<equation> solvesome(List<equation> ineq) {
            if (ineq.Count == 1) return ineq;

            List<equation> outeq = new List<equation>();

            for (int i = 1; i < ineq.Count; i++) {
                outeq.Add(ineq[i].CombineAndReduce(ineq[0]));
            }
            return outeq;
        }

        private void btnDecode_Click(object sender, EventArgs e) {
            MofN mn = new MofN();

            for (int i = 0; i < 8; i++) {
                TextBox t = GetPartBox(i);
                string p = t.Text.Trim();

                if (p == "" || (mn.PartsAccepted >= mn.PartsNeeded && mn.PartsNeeded > 0)) {
                    t.BackColor = System.Drawing.Color.White;
                } else {
                    string result = mn.AddKeyPart(p);
                    if (result == null) {
                        t.BackColor = System.Drawing.Color.LightGreen;
                    } else {
                        t.BackColor = System.Drawing.Color.Pink;
                    }
                }
            }

            if (mn.PartsAccepted >= mn.PartsNeeded && mn.PartsNeeded > 0) {
                mn.Decode();
                txtPrivKey.Text = mn.BitcoinPrivateKey;
                txtAddress.Text = mn.BitcoinAddress;                
            } else {
                MessageBox.Show("Not enough valid parts were present to decode an address.");
            }


            

        }

        private void btnGenerateSpecific_Click(object sender, EventArgs e) {

            KeyPair k = null;

            try {
                k = new KeyPair(txtPrivKey.Text);
                targetPrivKey = k.PrivateKeyBytes;

            } catch (Exception) {
                MessageBox.Show("Not a valid private key.");
            }

            btnGenerate_Click(sender, e);
            targetPrivKey = null;
            
        }

        private void MofNcalc_Load(object sender, EventArgs e) {
            MessageBox.Show("This feature is experimental, a proof of concept, and the key format will probably be revised heavily before this ever makes it into production.  Don't rely on it to secure large numbers of Bitcoins.  If you use it, " +
                "make sure you keep a copy of this version of the utility in case the m-of-n format is changed before being accepted as any kind of standard.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

    }




    
}

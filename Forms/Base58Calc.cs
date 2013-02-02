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
using Casascius.Bitcoin;

namespace BtcAddress {
    public partial class Base58Calc : Form {
        public Base58Calc() {
            InitializeComponent();
        }

        private void txtHex_TextChanged(object sender, EventArgs e) {
            if (txtHex.ContainsFocus == false) return;
            byte[] bytes = Util.HexStringToBytes(txtHex.Text);
            if (useChecksumToolStripMenuItem.Checked) {
                txtBase58.Text = Util.ByteArrayToBase58Check(bytes);
            } else {
                txtBase58.Text = Base58.FromByteArray(bytes);
            }

            UpdateByteCounts();
        }

        private void txtBase58_TextChanged(object sender, EventArgs e) {
            if (txtBase58.ContainsFocus == false) return;
            byte[] bytes;
            if (useChecksumToolStripMenuItem.Checked) {
                bytes = Util.Base58CheckToByteArray(txtBase58.Text);
            } else {
                bytes = Base58.ToByteArray(txtBase58.Text);
            }
            string hex = "invalid";
            if (bytes != null) {
                hex = Util.ByteArrayToString(bytes);
            }
            txtHex.Text = hex;
            UpdateByteCounts();
        }

        private void UpdateByteCounts() {
            lblByteCounts.Text = "Bytes: " + Util.HexStringToBytes(txtHex.Text).Length + "  Base58 length: " + txtBase58.Text.Length;

        }

        private void useChecksumToolStripMenuItem_Click(object sender, EventArgs e) {
            useChecksumToolStripMenuItem.Checked = !useChecksumToolStripMenuItem.Checked;
            // pretend that whatever had the focus was just changed
            if (txtBase58.Focused) {
                txtBase58_TextChanged(txtBase58, null);
            } else if (txtHex.Focused) {
                txtHex_TextChanged(txtHex, null);
            }

        }

    }
}

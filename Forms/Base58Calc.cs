using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BtcAddress {
    public partial class Base58Calc : Form {
        public Base58Calc() {
            InitializeComponent();
        }

        private void txtHex_TextChanged(object sender, EventArgs e) {
            if (txtHex.ContainsFocus == false) return;
            byte[] bytes = Bitcoin.HexStringToBytes(txtHex.Text);
            txtBase58.Text = Bitcoin.ByteArrayToBase58Check(bytes);

            UpdateByteCounts();
        }

        private void txtBase58_TextChanged(object sender, EventArgs e) {
            if (txtBase58.ContainsFocus == false) return;
            byte[] bytes = Bitcoin.Base58CheckToByteArray(txtBase58.Text);
            string hex = "invalid";
            if (bytes != null) {
                hex = Bitcoin.ByteArrayToString(bytes);
            }
            txtHex.Text = hex;
            UpdateByteCounts();


        }

        private void UpdateByteCounts() {
            lblByteCounts.Text = "Bytes: " + Bitcoin.HexStringToBytes(txtHex.Text).Length + "  Base58 length: " + txtBase58.Text.Length;

        }

    }
}

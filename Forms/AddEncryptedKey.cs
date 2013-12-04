


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Casascius.Bitcoin;

namespace BtcAddress.Forms {
    public partial class AddEncryptedKey : Form {
        public AddEncryptedKey() {
            InitializeComponent();
        }

        public object Result;

        private void btnOK_Click(object sender, EventArgs e) {

            if (txtBoxPlainPrivKey.Text == "") {
                MessageBox.Show("Enter a key first.");
                return;
            }

            Result = StringInterpreter.Interpret(txtBoxPlainPrivKey.Text, this.chkBoxCompress.Checked);
            if (Result == null) {
                MessageBox.Show("Unrecognized or invalid string");
            } else {

                if (Result is KeyPair) {
                    Result = new Bip38KeyPair(Result as KeyPair, txtBoxPassphrase.Text);
                }

                this.Close();
            }

        }

        private void AddEncryptedKey_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

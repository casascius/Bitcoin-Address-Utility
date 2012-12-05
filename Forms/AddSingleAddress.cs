using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BtcAddress.Forms {
    public partial class AddSingleAddress : Form {
        public AddSingleAddress() {
            InitializeComponent();
        }

        public object Result;

        private void button1_Click(object sender, EventArgs e) {
            if (textBox1.Text == "") {
                MessageBox.Show("Enter a key first.");
                return;
            }

            Result = StringInterpreter.Interpret(textBox1.Text);
            if (Result == null) {
                MessageBox.Show("Unrecognized or invalid string");
            } else {
                this.Close();

            }

        }

        private void btnGoMulti_Click(object sender, EventArgs e) {
            textBox1.Focus();
            textBox1.Multiline = true;
            btnGoMulti.Visible = false;
            lblEnterWhat.Text = "Enter or paste text. Addresses and keys will be picked out.";
            this.Text = "Add Multiple Addresses";
            if (this.Height < 500) this.Height = 500;
            this.AcceptButton = null;
        }


    }
}

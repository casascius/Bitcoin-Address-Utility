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

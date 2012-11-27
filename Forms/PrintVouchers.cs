using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;


namespace BtcAddress.Forms {
    public partial class PrintVouchers : Form {


        public List<KeyCollectionItem> Items = new List<KeyCollectionItem>();

        public bool PrintAttempted = false;

        public PrintVouchers() {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            txtDenomination.Text += ((LinkLabel)sender).Text;
        }

        private void button1_Click(object sender, EventArgs e) {
            PrintDialog pd = new PrintDialog();
            PrinterSettings ps = new PrinterSettings();
            pd.PrinterSettings = ps;
            DialogResult dr = pd.ShowDialog();

            if (dr == DialogResult.OK) {
                QRPrint printer = new QRPrint();
                printer.PrintMode = QRPrint.PrintModes.PsyBanknote;
                printer.NotesPerPage = (int)numVouchersPerPage.Value;
                switch (cboArtworkStyle.Text.ToLower()) {
                    case "yellow":
                    case "green":
                    case "blue":
                    case "purple":
                    case "greyscale":
                        printer.ImageFilename = "note-" + cboArtworkStyle.SelectedItem.ToString() + ".png";
                        break;
                }
                printer.Denomination = txtDenomination.Text;
                printer.keys = new List<KeyCollectionItem>(Items.Count);
                foreach (KeyCollectionItem a in Items) printer.keys.Add(a);
                printer.PrinterSettings = pd.PrinterSettings;
                printer.Print();
                PrintAttempted = true;
            }




        }

        private void PrintVouchers_Load(object sender, EventArgs e) {
            this.Text = "Print " + Items.Count + " Vouchers";
            cboArtworkStyle.Text = "Yellow";
        }

    }
}

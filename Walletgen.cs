using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using PC;
using System.Drawing.Printing;


namespace BtcAddress {
    public partial class Walletgen : Form {
        public Walletgen() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            if (button1.Enabled)
            {
                int n = 0;

                if (Int32.TryParse(textBox2.Text, out n) == false) n = 0;
                if (n < 1 || n > 999) {                
                    MessageBox.Show("Please enter a number of addresses between 1 and 999", "Invalid entry");
                    return;
                }


                if (txtPassphrase.Text.Length < 20)
                {

                    if (MessageBox.Show("Your passphrase is too short (< 20 characters). If you generate this wallet it may be easily compromised. Are you sure you'd like to use this passphrase?", "Passphrase too short", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }

                }



                if (Bitcoin.PassphraseTooSimple(txtPassphrase.Text))
                {

                    if (MessageBox.Show("Your passphrase is too simple. If you generate this wallet it may be easily compromised. Are you sure you'd like to use this passphrase?", "Passphrase too simple", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }

                }

                StringBuilder wallet = new StringBuilder();

                
                bool CSVmode = cboOutputType.Text.Contains("CSV");
                bool ScriptMode = cboOutputType.Text.Contains("Import script");
                bool ShowHelpText = cboOutputType.Text.Contains("Normal");

                if (ShowHelpText) {
                    wallet.AppendLine("Paper Bitcoin Wallet.  Keep private, do not lose, do not allow anyone to make a copy.  Anyone with the passphrase or private keys can steal your funds.\r\n");

                    wallet.AppendLine("Passphrase was:");
                    wallet.AppendLine(txtPassphrase.Text);
                    wallet.AppendLine("Freely give out the Bitcoin address.  The private key after each address is the key needed to unlock funds sent to the Bitcoin address.\r\n");

                }
                progressBar1.Maximum = n;
                progressBar1.Minimum = 0;
                progressBar1.Visible = true;
                label3.Text = "Progress:";
                button1.Enabled = false;

                for (int i = 1; i <= n; i++)
                {
                    Application.DoEvents();

                    string privatestring;
                    switch (GenerationFormula) {
                        case 1:
                            privatestring = txtPassphrase.Text + i.ToString();
                            break;
                        default:
                            privatestring = i.ToString() + "/" + txtPassphrase.Text + "/" + i.ToString() + "/BITCOIN";
                            break;
                    }
                    SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                    UTF8Encoding encoding = new UTF8Encoding(false);
                    byte[] privatekey = sha256.ComputeHash(encoding.GetBytes(privatestring));

                    string bytestring = Bitcoin.ByteArrayToString(privatekey);
                    string PrivWIF = Bitcoin.PrivHexToWIF(bytestring);
                    string PubHex = Bitcoin.PrivHexToPubHex(bytestring);
                    string Address = Bitcoin.PubHashToAddress(Bitcoin.PubHexToPubHash(PubHex), "Bitcoin");

                    if (CSVmode) {
                        wallet.AppendFormat("{0},\"{1}\",\"{2}\"\r\n", i, Address, PrivWIF);
                    } else if (ScriptMode) {
                        wallet.AppendFormat("# {0}: {1}\"\r\n./bitcoind importprivkey {2}\r\n", i, Address, PrivWIF);                                            
                    } else {
                        wallet.AppendFormat("Bitcoin Address #{0}: {1}\r\n", i, Address);
                        wallet.AppendFormat("Private Key: {0}\r\n\r\n", PrivWIF);
                    }

                    progressBar1.Value = i;
                }

                txtWallet.Text = wallet.ToString();

                progressBar1.Value = 0;
                progressBar1.Visible = false;
                label3.Text = "Passphrase:";
                button1.Enabled = true;
            }
        }

        private void Walletgen_Shown(object sender, EventArgs e) {
            const int phraselength = 80;

            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            Byte[] byte8 = new byte[phraselength];
            rng.GetBytes(byte8);
            string randomphrase = "";
            string junk64 = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz*#_%+!";
            for (int i = 0; i < phraselength; i++) {
                randomphrase += junk64.Substring(byte8[i] & 63, 1);
            }
            txtPassphrase.Text = randomphrase;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            PrintDialog pd = new PrintDialog();
            PrinterSettings ps = new PrinterSettings();
            pd.PrinterSettings = ps;
            DialogResult dr = pd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                PCPrint printer = new PCPrint();
                printer.PrinterSettings.PrinterName = pd.PrinterSettings.PrinterName;
                printer.PrinterFont = new Font("Verdana", 10);
                printer.TextToPrint = txtWallet.Text;
                printer.Print();

            }

        }

        private int GenerationFormula = 0;


        private void lblFormula_DoubleClick(object sender, EventArgs e) {
            // Change formula upon double click
            GenerationFormula = 1; // straight passphrase + number
            lblFormula.Text = "Generation formula: PrivKey = SHA256(passphrase + n)";
        }




    }
}

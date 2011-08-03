using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;



namespace BtcAddress {
    public partial class Walletgen : Form {
        public Walletgen() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {

            if (txtPassphrase.Text.Length < 20) {
                MessageBox.Show("Your passphrase is too short.  It MUST be at least 20 characters.  If you generate a wallet with a passphrase that is too short, your coins are likely to get stolen.","Passphrase too short");
                return;
            }

            int Lowercase=0, Uppercase=0, Numbers=0, Symbols=0;
            foreach (char c in txtPassphrase.Text.ToCharArray()) {
                if (c >= 'a' && c <= 'z') {
                    Lowercase++;
                } else if (c >= 'A' && c <= 'Z') {
                    Uppercase++;
                } else if (c >= '0' && c >= '9') {
                    Numbers++;
                } else if (c == ' ') {
                    // not counting spaces
                } else {
                    Symbols++;
                }
            }

            if (txtPassphrase.Text.Length < 30 && (Lowercase < 10 || Uppercase < 3 || Numbers < 2 || Symbols < 2)) {
                MessageBox.Show("Your passphrase is too simple.  Make it longer, or add more lowercase, uppercase, numbers, and/or symbols.  This is for your protection.  If you generate a wallet with a passphrase that is too simple, your coins are likely to get stolen.","Passphrase too short");
                return;
            }

            StringBuilder wallet = new StringBuilder();
            wallet.AppendLine("Paper Bitcoin Wallet.  Keep private, do not lose, do not allow anyone to make a copy.  Anyone with the passphrase or private keys can steal your funds.\r\n");

            wallet.AppendLine("Passphrase was:");
            wallet.AppendLine(txtPassphrase.Text);
            wallet.AppendLine("Freely give out the Bitcoin address.  The private key after each address is the key needed to unlock funds sent to the Bitcoin address.\r\n");

            for (int i = 1; i <= 10; i++) {

                string privatestring = i.ToString() + "/" + txtPassphrase.Text + "/" + i.ToString() + "/BITCOIN";
                SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                UTF8Encoding encoding = new UTF8Encoding(false);
                byte[] privatekey = sha256.ComputeHash(encoding.GetBytes(privatestring));

                string bytestring = Bitcoin.ByteArrayToString(privatekey);
                string PrivWIF = Bitcoin.PrivHexToWIF(bytestring);
                string PubHex = Bitcoin.PrivHexToPubHex(bytestring);
                string Address = Bitcoin.PubHashToAddress(Bitcoin.PubHexToPubHash(PubHex),"Bitcoin");

                wallet.AppendFormat("Bitcoin Address #{0}: {1}\r\n", i, Address);
                wallet.AppendFormat("Private Key: {0}\r\n\r\n", PrivWIF);

            }

            txtWallet.Text = wallet.ToString();

        }

        private void Walletgen_Shown(object sender, EventArgs e) {
            const int phraselength = 80;

            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            Byte[] byte8 = new byte[phraselength];
            rng.GetBytes(byte8);
            string randomphrase = "";
            string junk64 = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz*-_.+!";
            for (int i = 0; i < phraselength; i++) {
                randomphrase += junk64.Substring(byte8[i] & 63, 1);
            }
            txtPassphrase.Text = randomphrase;
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {

        }
    }
}

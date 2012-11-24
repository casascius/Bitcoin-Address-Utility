using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Math.EC;
using ThoughtWorks.QRCode.Codec;

using System.IO;
using System.Drawing.Printing;

namespace BtcAddress {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void btnPassphrase_Click(object sender, EventArgs e) {

            ChangeFlag++;
            try {
                SetText(txtPrivHex, RemoveSpacesIf(Bitcoin.PassphraseToPrivHex(txtMinikey.Text)));
                int isminikey = MiniKeyPair.IsValidMiniKey(txtMinikey.Text);
                if (isminikey==1) {
                    lblWhyNot.Visible = false;
                    lblNotSafe.Visible = true;
                    lblNotSafe.Text = "Valid mini key";
                    lblNotSafe.ForeColor = Color.DarkGreen;
                } else if (isminikey==-1) {
                    lblWhyNot.Visible = false;
                    lblNotSafe.Visible = true;
                    lblNotSafe.Text = "Invalid mini key";
                    lblNotSafe.ForeColor = Color.Red;
                } else if (txtMinikey.Text.Length < 20 || Bitcoin.PassphraseTooSimple(txtMinikey.Text)) {
                    lblWhyNot.Visible = true;
                    lblNotSafe.Visible = true;
                    lblNotSafe.Text = "Warning - Not Safe";
                    lblNotSafe.ForeColor = Color.Red;
                } else {
                    lblWhyNot.Visible = false;
                    lblNotSafe.Visible = false;
                    lblNotSafe.Text = "Warning - Not Safe";
                    lblNotSafe.ForeColor = Color.Red;
                }

                btnPrivHexToWIF_Click(null, null);
                btnPrivToPub_Click(null, null);
                btnPubHexToHash_Click(null, null);
                btnPubHashToAddress_Click(null, null);
                SetText(txtMinikey, txtMinikey.Text); // ungreys box
            } catch (Exception ae) {
                MessageBox.Show(ae.Message);
            } finally {
                ChangeFlag--;
            }
        }


        private void btnPrivHexToWIF_Click(object sender, EventArgs e) {
            ChangeFlag++;
            try {
                if (txtPrivHex.Text.StartsWith("\"") && txtPrivHex.Text.EndsWith("\"") && txtPrivHex.Text.Length > 2) {
                    UTF8Encoding utf8 = new UTF8Encoding(false);
                    byte[] str = Bitcoin.Force32Bytes(utf8.GetBytes(txtPrivHex.Text.Substring(1, txtPrivHex.Text.Length - 2)));
                    txtPrivHex.Text = RemoveSpacesIf(Bitcoin.ByteArrayToString(str));
                }
                KeyPair ba = new KeyPair(txtPrivHex.Text, compressed: compressToolStripMenuItem.Checked);

                if (txtPassphrase.Text != "") {
                    SetText(txtPrivWIF, new Bip38KeyPair(ba, txtPassphrase.Text).EncryptedPrivateKey);
                } else {
                    SetText(txtPrivWIF, ba.PrivateKeyBase58);

                }
                SetText(txtPrivHex, ba.PrivateKeyHex);
                SetText(txtPubHex, ba.PublicKeyHex);
                SetText(txtPubHash, ba.Hash160Hex);
                SetText(txtBtcAddr, new Address(ba, AddressTypeByte).AddressBase58);              
            } catch (Exception ae) {
                MessageBox.Show(ae.Message);
            } finally {
                ChangeFlag--;
            }


        }

        private void btnPrivWIFToHex_Click(object sender, EventArgs e) {
            ChangeFlag++;
            try {
                object interpretation = StringInterpreter.Interpret(txtPrivWIF.Text, compressed: compressToolStripMenuItem.Checked, addressType: this.AddressTypeByte);
                KeyPair kp = null;
                if (interpretation is PassphraseKeyPair) {
                    if (txtPassphrase.Text == "") {
                        MessageBox.Show("This is an encrypted key. A passphrase is required.");
                        return;
                    }
                    PassphraseKeyPair ppkp = (PassphraseKeyPair) interpretation;
                    if (ppkp.DecryptWithPassphrase(txtPassphrase.Text)==false) {
                        MessageBox.Show("The passphrase is incorrect.");
                        return;
                    }
                    kp = ppkp.GetUnencryptedPrivateKey();
                } else if (interpretation is KeyPair) {
                    kp = (KeyPair)interpretation;
                }


                if (kp == null) {
                    MessageBox.Show("Not a valid private key.");
                    return;
                }

                SetText(txtPrivHex, kp.PrivateKeyHex);
                SetText(txtPubHex, kp.PublicKeyHex);
                SetText(txtPubHash, kp.Hash160Hex);
                SetText(txtBtcAddr, new Address(kp, AddressTypeByte).AddressBase58);
            } catch (Exception ae) {
                MessageBox.Show(ae.Message);
            } finally {
                ChangeFlag--;
            }

        }

        private void btnPrivToPub_Click(object sender, EventArgs e) {

            ChangeFlag++;
            try {
                KeyPair kp = new KeyPair(txtPrivHex.Text, compressed: compressToolStripMenuItem.Checked);
                SetText(txtPubHex, kp.PublicKeyHex);
                SetText(txtPubHash, kp.Hash160Hex);
                SetText(txtBtcAddr, new Address(kp, AddressTypeByte).AddressBase58);
            } catch (ArgumentException ae) {
                MessageBox.Show(ae.Message);
            } finally {
                ChangeFlag--;
            }

        }

        private void btnPubHexToHash_Click(object sender, EventArgs e) {
            ChangeFlag++;
            try {
                PublicKey pub = new PublicKey(txtPubHex.Text);
                SetText(txtPubHash, pub.Hash160Hex);
                SetText(txtBtcAddr, new Address(pub, AddressTypeByte).AddressBase58);
            } catch (Exception ae) {
                MessageBox.Show(ae.Message);
            } finally {
                ChangeFlag--;
            }
        }

        private void btnPubHashToAddress_Click(object sender, EventArgs e) {
            ChangeFlag++;
            try {                
                SetText(txtBtcAddr, Bitcoin.PubHashToAddress(txtPubHash.Text, cboCoinType.Text));
            } catch (Exception ae) {
                MessageBox.Show(ae.Message);
            } finally {
                ChangeFlag--;
            }
        }

        private void btnAddressToPubHash_Click(object sender, EventArgs e) {
            ChangeFlag++;
            try {
                byte[] hex = Bitcoin.Base58CheckToByteArray(txtBtcAddr.Text);
                if (hex == null || hex.Length != 21) {
                    int L = txtBtcAddr.Text.Length;
                    if (L >= 33 && L <= 34) {
                        if (MessageBox.Show("Address is not valid.  Attempt to correct?", "Invalid address", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                            CorrectBitcoinAddress();
                            return;
                        }
                    } else {
                        MessageBox.Show("Address is not valid.");
                    }
                    return;
                }
                SetText(txtPubHash, RemoveSpacesIf(Bitcoin.ByteArrayToString(hex, 1, 20)));
            } finally {
                ChangeFlag--;
            }

        }

        private void btnGenerate_Click(object sender, EventArgs e) {
            ChangeFlag++;
            try {
                lblNotSafe.Visible = false;
                lblWhyNot.Visible = false;
                SetText(txtMinikey, "");

                KeyPair kp = KeyPair.Create(ExtraEntropy, compressToolStripMenuItem.Checked);

                if (txtPassphrase.Text != "") {
                    SetText(txtPrivWIF, new Bip38KeyPair(kp, txtPassphrase.Text).EncryptedPrivateKey);
                } else {
                    SetText(txtPrivWIF, kp.PrivateKeyBase58);
                }
                SetText(txtPrivHex, kp.PrivateKeyHex);
                SetText(txtPubHex, kp.PublicKeyHex);
                SetText(txtPubHash, kp.Hash160Hex);
                SetText(txtBtcAddr, new Address(kp, AddressTypeByte).AddressBase58);              

            } finally {
                ChangeFlag--;
            }

        }

        private void btnBlockExplorer_Click(object sender, EventArgs e) {
            try {
                if (cboCoinType.Text == "Testnet") {
                    Process.Start("http://www.blockexplorer.com/testnet/address/" + txtBtcAddr.Text);
                } else if (cboCoinType.Text == "Namecoin") {
                    Process.Start("http://explorer.dot-bit.org/a/" + txtBtcAddr.Text);
                } else if (cboCoinType.Text == "Litecoin") {
                    Process.Start("http://explorer.litecoin.net/address/" + txtBtcAddr.Text);
                } else {
                    Process.Start("http://www.blockchain.info/address/" + txtBtcAddr.Text);
                }
            } catch { }
            
        }

        private void CorrectBitcoinAddress() {
            ChangeFlag++;
            try {
                txtBtcAddr.Text = Correction(txtBtcAddr.Text);
            } finally {
                ChangeFlag--;
            }
        }

        private string Correction(string btcaddr) {
            
            int btcaddrlen = btcaddr.Length;
            string b58 = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

            for (int i = 0; i < btcaddrlen; i++) {
                for (int j = 0; j < 58; j++) {
                    string attempt = btcaddr.Substring(0, i) + b58.Substring(j, 1) + btcaddr.Substring(i + 1);
                    byte[] bytes = Bitcoin.Base58CheckToByteArray(attempt);
                    if (bytes != null) {
                        MessageBox.Show("Correction was successful.  Try your request again.");
                        return attempt;
                    }
                }
            }
            return btcaddr;
        }

        private void CorrectWIF() {
            ChangeFlag++;
            try {
                txtPrivWIF.Text = Correction(txtPrivWIF.Text);
            } finally {
                ChangeFlag--;
            }
        }

        private void btnShacode_Click(object sender, EventArgs e) {
            ChangeFlag++;
            try {
                MiniKeyPair mkp = MiniKeyPair.CreateRandom(ExtraEntropy);

                SetText(txtMinikey, mkp.MiniKey);
                if (txtPassphrase.Text != "") {
                    SetText(txtPrivWIF, new Bip38KeyPair(new KeyPair(mkp.PrivateKeyBytes), txtPassphrase.Text).EncryptedPrivateKey);
                } else {
                    SetText(txtPrivWIF, new KeyPair(mkp.PrivateKeyBytes).PrivateKeyBase58);
                }
                SetText(txtPrivHex, mkp.PrivateKeyHex);
                SetText(txtPubHex, mkp.PublicKeyHex);
                SetText(txtPubHash, mkp.Hash160Hex);
                SetText(txtBtcAddr, new Address(mkp, AddressTypeByte).AddressBase58);
                
            } finally {
                ChangeFlag--;
            }
        }

        private byte AddressTypeByte {
            get {
                string cointype = cboCoinType.SelectedText.ToLowerInvariant();
                switch (cointype) {
                    case "bitcoin": return 0;
                    case "namecoin": return 52;
                    case "testnet": return 111;
                    case "litecoin": return 48;
                }
                byte b = 0;
                if (Byte.TryParse(cointype, out b)) return b;
                return 0;
            }
        }

        private void walletGeneratorToolStripMenuItem_Click(object sender, EventArgs e) {
            Walletgen w = new Walletgen();
            w.Show();
        }

        private void whatIsASHAcodeToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("A Mini Private Key is a Bitcoin address generated from a short 30-character string of text.  The advantage is that the text representation of the private key is shorter, and easier to use in " +
            "QR codes and in other places where space is at a premium. " +
                "The private key is simply the SHA256 hash of the string.  SHAcodes start with the letter 'S' and include a 7-bit typo check.");
        }

        private void lblWhyNot_Click(object sender, EventArgs e) {
            MessageBox.Show("Bitcoins are vulnerable to theft from hackers when sent to addresses generated from short or non-complex passphrases.  A longer one, or one that uses a good " +
              "mix of uppercase, lowercase, numbers, and symbols is recommended.", "Security Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Counter used for tracking the number of functions in the call stack that might change the text in textboxes.
        /// If this is 0, we assume the user made the changes, and grey out anything that depends on what they changed.
        /// If nonzero, the program is making the changes, which should ungrey what was changed.
        /// </summary>
        private int ChangeFlag = 0;

        /// <summary>
        /// Greys the text in all textboxes except for the one that has just been changed.
        /// </summary>
        private void TextBox_TextChanged(object sender, EventArgs e) {
            if (ChangeFlag > 0) return;
            TextBox txtSender = (TextBox)sender;
            TextBox[] textboxes = new TextBox[] { txtMinikey, txtPrivWIF, txtPrivHex, txtPubHex, txtPubHash, txtBtcAddr };
            foreach (TextBox t in textboxes) {
                t.ForeColor = (t == txtSender) ? SystemColors.WindowText : SystemColors.GrayText;
            }
            // if passphrase changed, remove notation
            if (txtSender == txtMinikey && lblNotSafe.Visible) {
                lblNotSafe.Visible = false;
                lblWhyNot.Visible = false;
            }

        }
        /// <summary>
        /// Changes the text in a textbox ensuring that it will be ungreyed in the process.
        /// (Needed to do it this way, because changing the Text property won't fire TextChanged if it is getting set to what happens
        /// to already be in the box.        
        /// </summary>
        private void SetText(TextBox thebox, string TheText) {
            thebox.ForeColor = SystemColors.WindowText;
            if (Object.ReferenceEquals(thebox, txtPrivHex) ||
                Object.ReferenceEquals(thebox, txtPubHex) ||
                object.ReferenceEquals(thebox, txtPubHash)) {
                    thebox.Text = RemoveSpacesIf(TheText);
            } else {
                thebox.Text = TheText;
            }
        }

        /// <summary>
        /// if coin type is changed, grey out the address until it is rerendered
        /// </summary>
        private void cboCoinType_SelectionChangeCommitted(object sender, EventArgs e) {            
            txtBtcAddr.ForeColor = SystemColors.GrayText;
            // convert address when possible
            ChangeFlag++;
            try {
                Address addr = new Address(new Address(txtBtcAddr.Text), AddressTypeByte);
                txtBtcAddr.Text = addr.AddressBase58;
            } catch (Exception) {
                // ignore
            } finally {
                ChangeFlag--;
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == 13) {
                e.Handled = true;
                TextBox txtSender = (TextBox)sender;
                if (txtSender == txtMinikey) btnPassphrase_Click(null, null);
                if (txtSender == txtPrivWIF) btnPrivWIFToHex_Click(null, null);
            }
        }


        private object LockObject = new object();

        private List<Thread> Threads = new List<Thread>();
        
        
        private void label5_Click(object sender, EventArgs e) {
        }



        private string salt = "wefdhwfkhjwefopiwjdfldkdsfjndkljf"; // initial salt replaced at runtime

        private void GenerateAddresses() {

            string b58 = "23456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
            int b58len = b58.Length;
            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
            Sha256Digest bcsha256a = new Sha256Digest();
            Sha256Digest bcsha256b = new Sha256Digest();

            string shacode = "";


            SecureRandom sr = new SecureRandom();
            int dec = 0;

            List<string> myaddresses = new List<string>();
            Dictionary<string, string> myprivkeys = new Dictionary<string, string>();

            for (int ji = 0; ji < 100000000; ji++) {
                byte[] poop = sha256.ComputeHash(Encoding.ASCII.GetBytes(salt + ji.ToString()));

                byte[] ahash = null;
                do {

                    // Get 'S' + 21 random base58 characters, where sha256(result + '?') starts with the byte 00 (1 in 256 possibilities)
                    shacode = "S";
                    for (int i = 0; i < 29; i++) {
                        long x = sr.NextLong() & long.MaxValue;
                        x += poop[i];
                        long x58 = x % b58len;
                        shacode += b58.Substring((int)x58, 1);
                    }
                    string shacodeq = shacode + "?";
                    ahash = sha256.ComputeHash(Encoding.ASCII.GetBytes(shacodeq));

                    if (ahash[0] == 0) break;

                    Application.DoEvents();
                } while (true);


                string pubhex = Bitcoin.PrivHexToPubHex(Bitcoin.ByteArrayToString(sha256.ComputeHash(Encoding.ASCII.GetBytes(shacode))));
                string pubhash = Bitcoin.PubHexToPubHash(pubhex);
                string address = Bitcoin.PubHashToAddress(pubhash, "Bitcoin");


                pubhex = pubhex.Replace(" ", "");

                lock (LockObject) {

                    using (StreamWriter sw1 = new StreamWriter("privkeys3.txt", true)) {
                        sw1.WriteLine("\"" + address + "\",\"" + shacode + "\",\"" + pubhex + "\"");
                        sw1.Close();
                    }


                    using (StreamWriter sw1 = new StreamWriter("addresses3.txt", true)) {
                        sw1.WriteLine("\"" + address + "\",\"" + pubhex + "\"");
                        sw1.Close();
                    }
                }
                
                Debug.WriteLine(shacode + "=" + address);
                /*
                myaddresses.Add(address);
                myprivkeys.Add(address, shacode);
                dec++;
                if (dec == 1000) {
                    dec = 0;
                    Application.DoEvents();
                }
                 * */
            }


        }

        private void label4_Click(object sender, EventArgs e) {
            return;
            int Records=0;
            int LineNumber = 1;
            using (StreamReader sr1 = new StreamReader("privkeys3.txt")) {
                while (sr1.EndOfStream == false) {
                    string line = sr1.ReadLine();
                    LineNumber++;
                    line = line.Replace("\"", "");
                    string[] fields = line.Split(',');
                    if (fields.Length == 3) {
                        SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                        byte[] privkey = sha256.ComputeHash(Encoding.ASCII.GetBytes(fields[1]));

                        string pubhex = Bitcoin.PrivHexToPubHex(Bitcoin.ByteArrayToString(privkey)).Replace(" ", "");
                        string pubhash = Bitcoin.PubHexToPubHash(pubhex);
                        string address = Bitcoin.PubHashToAddress(pubhash, "Bitcoin");

                        if (address != fields[0] || pubhex != fields[2]) {
                            MessageBox.Show("Validation failure on line " + LineNumber.ToString());
                        }
                        Records++;
                        if (Records % 100 == 0) Debug.WriteLine("Records: " + Records);
                    }
                }
            }
            MessageBox.Show("Successfully validated " + Records + " records.");
        }

        private void label3_Click(object sender, EventArgs e) {
            return;
            


            
            // * CODE TO BREAK PRIVATE KEYS OUT OF WALLET.DAT
            using (FileStream sr = new FileStream("wallet.dat",FileMode.Open)) {
                byte[] b = new byte[4000000];
                int len = sr.Read(b, 0, b.Length);
                sr.Close();

                for (int i = 0; i < len - 34; i++) {
                    if (b[i] == 4 && b[i + 1] == 0x20) {
                        byte[] privkey = new byte[33];
                        privkey[0] = 0x80;
                        for (int j = 0; j < 32; j++) {
                            privkey[j+1] = b[i + j + 2];
                        }
                        Debug.WriteLine("./bitcoind importprivkey " + Bitcoin.ByteArrayToBase58Check(privkey));
                    }
                }
            }
             
        }



        private void base58CalcToolStripMenuItem_Click(object sender, EventArgs e) {
            new Base58Calc().Show();
        }

        private void mofNCalcToolStripMenuItem_Click(object sender, EventArgs e) {
            new MofNcalc().Show();
        }

        private void paperWalletPrinterToolStripMenuItem_Click(object sender, EventArgs e) {
            new PaperWalletPrinter().Show();
        }

        /// <summary>
        /// User toggled whether spaces should be shown between bytes of hex.
        /// Update any existing display to match new preference.
        /// </summary>
        private void spaceBetweenHexBytesToolStripMenuItem_Click(object sender, EventArgs e) {
            ChangeFlag++;
            spaceBetweenHexBytesToolStripMenuItem.Checked = !spaceBetweenHexBytesToolStripMenuItem.Checked;
            txtPrivHex.Text = RemoveSpacesIf(Bitcoin.ByteArrayToString(Bitcoin.HexStringToBytes(txtPrivHex.Text)));
            txtPubHex.Text = RemoveSpacesIf(Bitcoin.ByteArrayToString(Bitcoin.HexStringToBytes(txtPubHex.Text)));
            txtPubHash.Text = RemoveSpacesIf(Bitcoin.ByteArrayToString(Bitcoin.HexStringToBytes(txtPubHash.Text)));
            ChangeFlag--;

        }

        /// <summary>
        /// Removes spaces from a string if user has selected no spaces to appear in hex strings.
        /// </summary>
        private string RemoveSpacesIf(string what) {
            if (spaceBetweenHexBytesToolStripMenuItem.Checked) return what;
            return what.Replace(" ", "");
        }

        private void compressPublicKeyToolStripMenuItem_Click(object sender, EventArgs e) {
            ChangeFlag++;
            try {
                PublicKey pub = new PublicKey(txtPubHex.Text);
                pub = new PublicKey(pub.GetCompressed());
                SetText(txtPubHex, pub.PublicKeyHex);
                SetText(txtPubHash, pub.Hash160Hex);
                SetText(txtBtcAddr, new Address(pub, AddressTypeByte).AddressBase58);
            } catch (Exception ae) {
                MessageBox.Show(ae.Message);
            } finally {
                ChangeFlag--;
            }

        }

        private void uncompressPublicKeyToolStripMenuItem_Click(object sender, EventArgs e) {
            ChangeFlag++;
            try {
                PublicKey pub = new PublicKey(txtPubHex.Text);
                pub = new PublicKey(pub.GetUncompressed());
                SetText(txtPubHex, pub.PublicKeyHex);
                SetText(txtPubHash, pub.Hash160Hex);
                SetText(txtBtcAddr, new Address(pub, AddressTypeByte).AddressBase58);
            } catch (Exception ae) {
                MessageBox.Show(ae.Message);
            } finally {
                ChangeFlag--;
            }
        }

        private void compressToolStripMenuItem_Click(object sender, EventArgs e) {
            compressToolStripMenuItem.Checked = !compressToolStripMenuItem.Checked;
            
        }

        private void showFieldsToolStripMenuItem_Click(object sender, EventArgs e) {
            txtPubHex.Focus();
            if (txtPubHex.Text.Length == 130 || txtPubHex.Text.Length == 66) {
                // 33 or 65 bytes, no spaces = 66 or 130 characters
                txtPubHex.Select(2, 64);
            } else if (txtPubHex.Text.Length == 194 || txtPubHex.Text.Length == 98) {
                // 65 bytes + 64 spaces, or 33 bytes = 32 spaces
                txtPubHex.Select(2, 95);
            } else {
                MessageBox.Show("Enter a public key first.");
            }
        }

        private void keyCombinerToolStripMenuItem_Click(object sender, EventArgs e) {
            KeyCombiner k = new KeyCombiner();
            k.Show();

        }

        private void copyPrivateKeyQRMenuItem_Click(object sender, EventArgs e) {
            string toencode = txtPrivWIF.Text;
            Bitmap b = Bitcoin.EncodeQRCode(toencode);
            if (b == null) {
                MessageBox.Show("Enter or create a valid private key first.");
                return;
            }
            Clipboard.SetText(toencode);
            Clipboard.SetImage(b);

        }

        private void copyMinikeyQRMenuItem_Click(object sender, EventArgs e) {
            string toencode = txtMinikey.Text;
            Bitmap b = Bitcoin.EncodeQRCode(toencode);
            if (b == null) {
                MessageBox.Show("Enter or create a valid minikey first.");
                return;
            }
            Clipboard.SetText(toencode);
            Clipboard.SetImage(b);

        }

        private void copyAddressQRMenuItem_Click(object sender, EventArgs e) {
            string toencode = txtBtcAddr.Text;
            Bitmap b = Bitcoin.EncodeQRCode(toencode);
            if (b == null) {
                MessageBox.Show("Enter or create a valid address first.");
                return;
            }
            Clipboard.SetText(toencode);
            Clipboard.SetImage(b);
        }

        private void copyPublicHexQRMenuItem_Click(object sender, EventArgs e) {
            string toencode = txtPubHex.Text.Replace(" ","");
            Bitmap b = Bitcoin.EncodeQRCode(toencode);
            if (b == null) {
                MessageBox.Show("Enter or create a valid public key first.");
                return;
            }
            Clipboard.SetText(toencode);
            Clipboard.SetImage(b);

        }


        private string ExtraEntropy = DateTime.Now.Ticks.ToString();

        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            AddExtraEntropy(e.KeyCode.ToString() + e.KeyData + DateTime.Now.Ticks.ToString());
        }

        private void AddExtraEntropy(string what) {
            ExtraEntropy += what;
            if (ExtraEntropy.Length > 300) {
                SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                UTF8Encoding utf8 = new UTF8Encoding(false);
                ExtraEntropy = BitConverter.ToString(sha256.ComputeHash(utf8.GetBytes(ExtraEntropy)));
            }
        }

        private void timer1_Tick(object sender, EventArgs e) {
            AddExtraEntropy(DateTime.Now.Ticks.ToString());
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e) {
            AddExtraEntropy(DateTime.Now.Ticks.ToString() + e.X + "," + e.Y);
        }

        private void pPECKeygenToolStripMenuItem_Click(object sender, EventArgs e) {
            new PpecKeygen().Show();
        }
    }
    public class KeyComparer : IComparer<Address> {
        public int Compare(Address x, Address y) {
            return string.Compare(x.AddressBase58, y.AddressBase58, true);
        }

    }
}

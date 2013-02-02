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
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing.Printing;
using Casascius.Bitcoin;


namespace BtcAddress.Forms {
    public partial class KeyCollectionView : Form {

        public KeyCollection KeyCollection = new KeyCollection();

        public KeyCollectionView() {
            InitializeComponent();
        }

        private void newAddressToolStripMenuItem_Click(object sender, EventArgs e) {
            KeyPair kp = KeyPair.Create(ExtraEntropy.GetEntropy());
            KeyCollectionItem item = new KeyCollectionItem(kp);
            KeyCollection.AddItem(item);
        }

        private void KeyCollectionView_Load(object sender, EventArgs e) {
            listView1.Columns[0].Width = 400;
            listView1.Columns[1].Width = 100;
            toolStripStatusLabel1.Text = "Click Address to generate some addresses.";

            // subscribe to keycollection events
            KeyCollection.ItemAdded += new Action<KeyCollectionItem>(KeyCollection_ItemAdded);
            KeyCollection.ItemsAdded += new Action<IEnumerable<KeyCollectionItem>>(KeyCollection_ItemsAdded);
            KeyCollection.ItemsDeleted += new Action<IEnumerable<KeyCollectionItem>>(KeyCollection_ItemsDeleted);
        }

        void KeyCollection_ItemsDeleted(IEnumerable<KeyCollectionItem> items) {
            Dictionary<KeyCollectionItem, ListViewItem> index = new Dictionary<KeyCollectionItem, ListViewItem>();
            foreach (ListViewItem lvi in listView1.Items) index.Add((KeyCollectionItem)lvi.Tag, lvi);
            foreach (KeyCollectionItem kci in items) {
                if (index.ContainsKey(kci)) listView1.Items.Remove(index[kci]);
            }
            UpdateStatusLabel();


        }

        void UpdateStatusLabel() {
            if (listView1.Items.Count == 1) {
                toolStripStatusLabel1.Text = "1 address";
            } else {
                toolStripStatusLabel1.Text = listView1.Items.Count + " addresses";
            }

        }

        void KeyCollection_ItemsAdded(IEnumerable<KeyCollectionItem> obj) {
            foreach (var item in obj) {
                ListViewItem lvi = new ListViewItem(new string[] { item.ToString(), item.PrivateKeyKind, "0.00" });
                lvi.Tag = item;
                lvi.Checked = true;
                listView1.Items.Add(lvi);                
            }
            UpdateStatusLabel();
        }

        void KeyCollection_ItemAdded(KeyCollectionItem item) {
            ListViewItem lvi = new ListViewItem(new string[] { item.ToString(), item.PrivateKeyKind, "0.00" });
            lvi.Tag = item;
            lvi.Checked = true;
            listView1.Items.Add(lvi);
            UpdateStatusLabel();
        }

        private void generateKeysToolStripMenuItem_Click(object sender, EventArgs e) {
            var genform = new AddressGen();
            genform.ShowDialog();
            if (genform.GeneratedItems != null && genform.GeneratedItems.Count > 0) {
                KeyCollection.AddItemRange(genform.GeneratedItems);
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (ListViewItem i in listView1.Items) i.Checked = true;
        }
        private void deselectAllToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (ListViewItem i in listView1.Items) i.Checked = false;

        }

        private List<KeyCollectionItem> getEncryptedItemsToPrint() {
            List<KeyCollectionItem> itemsToPrint = new List<KeyCollectionItem>();
            int Unprintables = 0;
            //foreach (KeyCollectionItem i in KeyCollection.Items) {
            foreach (ListViewItem lvi in listView1.Items) {
                if (lvi.Checked) {
                    KeyCollectionItem i = lvi.Tag as KeyCollectionItem;
                    if (i.EncryptedKeyPair != null || (i.Address != null && i.Address is KeyPair)) {
                        itemsToPrint.Add(i);
                    } else {
                        Unprintables++;
                    }
                }
            }
            if (itemsToPrint.Count == 0) {
                MessageBox.Show("No items with printable private keys are selected.",
                    "Can't print encrypted keys",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            if (Unprintables != 0) {
                MessageBox.Show(Unprintables.ToString() + " of the selected items cannot be " +
                    "printed because the private key is not known.  These items will be skipped.",
                    "Can't print some items",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return itemsToPrint;
        }


        private void printBanknoteVouchersToolStripMenuItem_Click(object sender, EventArgs e) {
            List<KeyCollectionItem> itemsToPrint = getEncryptedItemsToPrint();
            if (itemsToPrint == null) return;
            var printform = new PrintVouchers();
            printform.Items = itemsToPrint;
            printform.ShowDialog();
            if (printform.PrintAttempted) {
                foreach (ListViewItem lvi in listView1.Items) {
                    if (lvi.Checked) lvi.Checked = false;

                }
            }
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e) {

            if (listView1.Sorting == SortOrder.Ascending) {
                listView1.Sorting = SortOrder.Descending;
            } else {
                listView1.Sorting = SortOrder.Ascending;
            }


        }

        private void addressUtilityToolStripMenuItem_Click(object sender, EventArgs e) {
            Program.ShowAddressUtility();
        }


        private void listView1_ItemActivate(object sender, EventArgs e) {
            if (listView1.SelectedItems.Count > 0) {
                ListViewItem item = listView1.SelectedItems[0];
                item.Checked = true;
                Program.ShowAddressUtility();
                Program.AddressUtility.DisplayKeyCollectionItem((KeyCollectionItem)item.Tag);
            }
            
        }

        private void detailsToolStripMenuItem_Click(object sender, EventArgs e) {
            listView1_ItemActivate(sender, e);
        }


        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e) {
            DialogResult result = MessageBox.Show(
                "Do you want to clear (delete) these keys?  This cannot be undone.",
                "Clear keys?",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

            if (result != DialogResult.OK) return;

            KeyCollection.DeleteItemRange(new List<KeyCollectionItem>(KeyCollection.Items));

        }

        private void base58CalculatorToolStripMenuItem_Click(object sender, EventArgs e) {
            Program.ShowBase58Calc();
        }

        private void keyCombinerToolStripMenuItem_Click(object sender, EventArgs e) {
            Program.ShowKeyCombiner();
        }

        private void mofNCalculatorToolStripMenuItem_Click(object sender, EventArgs e) {
            Program.ShowMofNcalc();
        }

        private void intermediateGeneratorToolStripMenuItem_Click(object sender, EventArgs e) {
            Program.ShowIntermediateGen();
        }

        private void saveAddressListToolStripMenuItem_Click(object sender, EventArgs e) {
            List<KeyCollectionItem> selected = new List<KeyCollectionItem>();
            foreach (ListViewItem lvi in listView1.Items) {
                if (lvi.Checked) selected.Add(lvi.Tag as KeyCollectionItem);
            }
            if (selected.Count == 0) {
                MessageBox.Show("No items are selected","Empty selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try {
                saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (DialogResult.OK == saveFileDialog1.ShowDialog()) {
                    // If the file name is not an empty string open it for saving.
                    if (saveFileDialog1.FileName != "") {
                        // Saves the Image via a FileStream created by the OpenFile method.
                        using (StreamWriter w = File.CreateText(saveFileDialog1.FileName)) {
                            foreach (var k in selected) {
                                w.WriteLine(k.GetAddressBase58());
                            }
                            w.Close();
                        }
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Failed to save file", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void enterAnAddressToolStripMenuItem_Click(object sender, EventArgs e) {
            var asa = new BtcAddress.Forms.AddSingleAddress();
            asa.ShowDialog();
            if (asa.Result != null) {
                if (asa.Result is EncryptedKeyPair) {
                    this.KeyCollection.AddItem(new KeyCollectionItem(asa.Result as EncryptedKeyPair));
                } else {
                    this.KeyCollection.AddItem(new KeyCollectionItem(asa.Result as AddressBase));
                }
            }
        }

        private void deleteSelectedItemsToolStripMenuItem_Click(object sender, EventArgs e) {

            DialogResult result = MessageBox.Show(
                "Do you want to clear (delete) the selected keys?  This cannot be undone.",
                "Clear keys?",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

            if (result != DialogResult.OK) return;


            List<KeyCollectionItem> itemsToDelete = new List<KeyCollectionItem>();
            foreach (ListViewItem lvi in listView1.Items) {
                if (lvi.Checked) itemsToDelete.Add((KeyCollectionItem)lvi.Tag);
            }
            if (itemsToDelete.Count == 0) {
                MessageBox.Show("No items selected.",
                    "Nothing to delete",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            KeyCollection.DeleteItemRange(itemsToDelete);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void confirmationCodeValidatorToolStripMenuItem_Click(object sender, EventArgs e) {
            Program.ShowConfValidator();
        }

        private void printPaperWalletsToolStripMenuItem_Click(object sender, EventArgs e) {
        }

        private void printTwoFactorCoinInsertsToolStripMenuItem_Click(object sender, EventArgs e) {
            List<KeyCollectionItem> itemsToPrint = getEncryptedItemsToPrint();
            if (itemsToPrint == null) return;

            PrintDialog pd = new PrintDialog();
            PrinterSettings ps = new PrinterSettings();
            pd.PrinterSettings = ps;
            DialogResult dr = pd.ShowDialog();

            if (dr == DialogResult.OK) {
                CoinInsert printer;
                if (sender.Equals(printPhysicalBitcoinInsertsDenseToolStripMenuItem)) {
                    printer = new CoinInsertDense();
                } else {
                    printer = new CoinInsert();
                }                
                
                printer.keys = itemsToPrint;
                printer.PrinterSettings = pd.PrinterSettings;
                printer.DenseMode = true;
                printer.Print();
                foreach (ListViewItem lvi in listView1.Items) {
                    if (lvi.Checked) lvi.Checked = false;
                }
            }


        }

        private void menuStrip1_MouseMove(object sender, MouseEventArgs e) {
            ExtraEntropy.AddExtraEntropy(DateTime.Now.Ticks.ToString() + e.X + "," + e.Y);
        }

        private void keyDecrypterToolStripMenuItem_Click(object sender, EventArgs e) {
            Program.ShowKeyDecrypter();
        }

        private void escrowToolsToolStripMenuItem_Click(object sender, EventArgs e) {
            Program.ShowEscrowTools();
        }








        /*
        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {

            try {
                saveFileDialog1.Filter = "XML files (*.xml)|*.xml|Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (DialogResult.OK == saveFileDialog1.ShowDialog()) {
                    // If the file name is not an empty string open it for saving.
                    if (saveFileDialog1.FileName != "") {
                        // Saves the Image via a FileStream created by the OpenFile method.
                        using (StreamWriter w = File.CreateText(saveFileDialog1.FileName)) {
                            w.Write(ObjectToXML(KeyCollection));
                            w.Close();
                        }
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Failed to save file", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        public static string ObjectToXML(object SerializableObject) {
            Type serializableObjectType = SerializableObject.GetType();
            XmlSerializer ser = new XmlSerializer(serializableObjectType);
            using (MemoryStream memStream = new MemoryStream()) {
                XmlWriter xmlWriter = XmlWriter.Create(memStream);
                //xmlWriter.Namespaces = true;
                ser.Serialize(xmlWriter, SerializableObject);
                xmlWriter.Close();
                memStream.Close();
                string xml;
                // clips out the <?xml> tag that describes encoding, as well as any unicode byte order mark.
                // these are undesirable if embedding in another object.                
                xml = Encoding.UTF8.GetString(memStream.ToArray());
                if (xml.Contains("<?")) {
                    xml = xml.Substring(xml.IndexOf("?>") + 2);
                }
                return xml;
            }
        }
         * */

    }
}

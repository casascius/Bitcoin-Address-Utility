namespace BtcAddress.Forms {
    partial class KeyCollectionView {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeyCollectionView));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addressUtilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.base58CalculatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keyCombinerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mofNCalculatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.intermediateGeneratorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateKeysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deselectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.printBanknoteVouchersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printPaperWalletsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.saveAddressListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.listView1 = new System.Windows.Forms.ListView();
            this.chAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chPrivateKey = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chBalance = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.detailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem,
            this.fileToolStripMenuItem,
            this.keyToolStripMenuItem,
            this.selectionToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(629, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addressUtilityToolStripMenuItem,
            this.base58CalculatorToolStripMenuItem,
            this.keyCombinerToolStripMenuItem,
            this.mofNCalculatorToolStripMenuItem,
            this.intermediateGeneratorToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // addressUtilityToolStripMenuItem
            // 
            this.addressUtilityToolStripMenuItem.Name = "addressUtilityToolStripMenuItem";
            this.addressUtilityToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.addressUtilityToolStripMenuItem.Text = "Address Utility";
            this.addressUtilityToolStripMenuItem.Click += new System.EventHandler(this.addressUtilityToolStripMenuItem_Click);
            // 
            // base58CalculatorToolStripMenuItem
            // 
            this.base58CalculatorToolStripMenuItem.Name = "base58CalculatorToolStripMenuItem";
            this.base58CalculatorToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.base58CalculatorToolStripMenuItem.Text = "Base58 Calculator";
            this.base58CalculatorToolStripMenuItem.Click += new System.EventHandler(this.base58CalculatorToolStripMenuItem_Click);
            // 
            // keyCombinerToolStripMenuItem
            // 
            this.keyCombinerToolStripMenuItem.Name = "keyCombinerToolStripMenuItem";
            this.keyCombinerToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.keyCombinerToolStripMenuItem.Text = "Key Combiner";
            this.keyCombinerToolStripMenuItem.Click += new System.EventHandler(this.keyCombinerToolStripMenuItem_Click);
            // 
            // mofNCalculatorToolStripMenuItem
            // 
            this.mofNCalculatorToolStripMenuItem.Name = "mofNCalculatorToolStripMenuItem";
            this.mofNCalculatorToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.mofNCalculatorToolStripMenuItem.Text = "M-of-N Calculator";
            this.mofNCalculatorToolStripMenuItem.Click += new System.EventHandler(this.mofNCalculatorToolStripMenuItem_Click);
            // 
            // intermediateGeneratorToolStripMenuItem
            // 
            this.intermediateGeneratorToolStripMenuItem.Name = "intermediateGeneratorToolStripMenuItem";
            this.intermediateGeneratorToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.intermediateGeneratorToolStripMenuItem.Text = "Intermediate Generator";
            this.intermediateGeneratorToolStripMenuItem.Click += new System.EventHandler(this.intermediateGeneratorToolStripMenuItem_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearAllToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.clearAllToolStripMenuItem.Text = "Clear All";
            this.clearAllToolStripMenuItem.Click += new System.EventHandler(this.clearAllToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Visible = false;
            // 
            // keyToolStripMenuItem
            // 
            this.keyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newAddressToolStripMenuItem,
            this.generateKeysToolStripMenuItem});
            this.keyToolStripMenuItem.Name = "keyToolStripMenuItem";
            this.keyToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.keyToolStripMenuItem.Text = "&Address";
            // 
            // newAddressToolStripMenuItem
            // 
            this.newAddressToolStripMenuItem.Name = "newAddressToolStripMenuItem";
            this.newAddressToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.newAddressToolStripMenuItem.Text = "New Address";
            this.newAddressToolStripMenuItem.Click += new System.EventHandler(this.newAddressToolStripMenuItem_Click);
            // 
            // generateKeysToolStripMenuItem
            // 
            this.generateKeysToolStripMenuItem.Name = "generateKeysToolStripMenuItem";
            this.generateKeysToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.generateKeysToolStripMenuItem.Text = "Generate Addresses";
            this.generateKeysToolStripMenuItem.Click += new System.EventHandler(this.generateKeysToolStripMenuItem_Click);
            // 
            // selectionToolStripMenuItem
            // 
            this.selectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.deselectAllToolStripMenuItem,
            this.toolStripSeparator1,
            this.printBanknoteVouchersToolStripMenuItem,
            this.printPaperWalletsToolStripMenuItem,
            this.toolStripSeparator2,
            this.saveAddressListToolStripMenuItem});
            this.selectionToolStripMenuItem.Name = "selectionToolStripMenuItem";
            this.selectionToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.selectionToolStripMenuItem.Text = "&Selection";
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // deselectAllToolStripMenuItem
            // 
            this.deselectAllToolStripMenuItem.Name = "deselectAllToolStripMenuItem";
            this.deselectAllToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.deselectAllToolStripMenuItem.Text = "Deselect All";
            this.deselectAllToolStripMenuItem.Click += new System.EventHandler(this.deselectAllToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(201, 6);
            // 
            // printBanknoteVouchersToolStripMenuItem
            // 
            this.printBanknoteVouchersToolStripMenuItem.Name = "printBanknoteVouchersToolStripMenuItem";
            this.printBanknoteVouchersToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.printBanknoteVouchersToolStripMenuItem.Text = "Print Banknote Vouchers";
            this.printBanknoteVouchersToolStripMenuItem.Click += new System.EventHandler(this.printBanknoteVouchersToolStripMenuItem_Click);
            // 
            // printPaperWalletsToolStripMenuItem
            // 
            this.printPaperWalletsToolStripMenuItem.Name = "printPaperWalletsToolStripMenuItem";
            this.printPaperWalletsToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.printPaperWalletsToolStripMenuItem.Text = "Print Paper Wallets";
            this.printPaperWalletsToolStripMenuItem.Visible = false;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(201, 6);
            // 
            // saveAddressListToolStripMenuItem
            // 
            this.saveAddressListToolStripMenuItem.Name = "saveAddressListToolStripMenuItem";
            this.saveAddressListToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.saveAddressListToolStripMenuItem.Text = "Save Address List";
            this.saveAddressListToolStripMenuItem.Click += new System.EventHandler(this.saveAddressListToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 412);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(629, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chAddress,
            this.chPrivateKey,
            this.chBalance});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Location = new System.Drawing.Point(0, 27);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(629, 382);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            this.listView1.ItemActivate += new System.EventHandler(this.listView1_ItemActivate);
            // 
            // chAddress
            // 
            this.chAddress.Text = "Address";
            this.chAddress.Width = 480;
            // 
            // chPrivateKey
            // 
            this.chPrivateKey.Text = "Private Key";
            this.chPrivateKey.Width = 100;
            // 
            // chBalance
            // 
            this.chBalance.Text = "Balance";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.detailsToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(110, 26);
            // 
            // detailsToolStripMenuItem
            // 
            this.detailsToolStripMenuItem.Name = "detailsToolStripMenuItem";
            this.detailsToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.detailsToolStripMenuItem.Text = "&Details";
            this.detailsToolStripMenuItem.Click += new System.EventHandler(this.detailsToolStripMenuItem_Click);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // KeyCollectionView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 434);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "KeyCollectionView";
            this.Text = "Bitcoin Key Collection";
            this.Load += new System.EventHandler(this.KeyCollectionView_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader chAddress;
        private System.Windows.Forms.ColumnHeader chPrivateKey;
        private System.Windows.Forms.ColumnHeader chBalance;
        private System.Windows.Forms.ToolStripMenuItem keyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newAddressToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateKeysToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deselectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem printPaperWalletsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printBanknoteVouchersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addressUtilityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem base58CalculatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keyCombinerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mofNCalculatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem intermediateGeneratorToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem saveAddressListToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem detailsToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}
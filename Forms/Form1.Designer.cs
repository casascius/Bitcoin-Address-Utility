namespace BtcAddress {
    partial class Form1 {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtPrivWIF = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPrivHex = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPubHex = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPubHash = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBtcAddr = new System.Windows.Forms.TextBox();
            this.btnAddressToPubHash = new System.Windows.Forms.Button();
            this.btnPubHashToAddress = new System.Windows.Forms.Button();
            this.btnPubHexToHash = new System.Windows.Forms.Button();
            this.btnPrivToPub = new System.Windows.Forms.Button();
            this.btnPrivWIFToHex = new System.Windows.Forms.Button();
            this.btnPrivHexToWIF = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnBlockExplorer = new System.Windows.Forms.Button();
            this.cboCoinType = new System.Windows.Forms.ComboBox();
            this.btnShacode = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.base58CalcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keyCombinerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mofNCalcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pPECKeygenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyMinikeyQRMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyPrivateKeyQRMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyPublicHexQRMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyAddressQRMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spaceBetweenHexBytesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.publicKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compressPublicKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uncompressPublicKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showFieldsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSha256ToPrivate = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMinikey = new System.Windows.Forms.TextBox();
            this.lblNotSafe = new System.Windows.Forms.Label();
            this.lblWhyNot = new System.Windows.Forms.Label();
            this.txtPassphrase = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPrivWIF
            // 
            this.txtPrivWIF.Location = new System.Drawing.Point(101, 93);
            this.txtPrivWIF.Name = "txtPrivWIF";
            this.txtPrivWIF.Size = new System.Drawing.Size(557, 20);
            this.txtPrivWIF.TabIndex = 7;
            this.txtPrivWIF.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Private Key (WIF)";
            // 
            // txtPrivHex
            // 
            this.txtPrivHex.Location = new System.Drawing.Point(101, 151);
            this.txtPrivHex.Name = "txtPrivHex";
            this.txtPrivHex.Size = new System.Drawing.Size(557, 20);
            this.txtPrivHex.TabIndex = 12;
            this.txtPrivHex.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 154);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Private Key (Hex)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 212);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Public Key (Hex)";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // txtPubHex
            // 
            this.txtPubHex.Location = new System.Drawing.Point(101, 209);
            this.txtPubHex.Multiline = true;
            this.txtPubHex.Name = "txtPubHex";
            this.txtPubHex.Size = new System.Drawing.Size(557, 43);
            this.txtPubHex.TabIndex = 15;
            this.txtPubHex.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 297);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Public Key (Hash)";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // txtPubHash
            // 
            this.txtPubHash.Location = new System.Drawing.Point(101, 294);
            this.txtPubHash.Name = "txtPubHash";
            this.txtPubHash.Size = new System.Drawing.Size(557, 20);
            this.txtPubHash.TabIndex = 19;
            this.txtPubHash.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 355);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Address";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // txtBtcAddr
            // 
            this.txtBtcAddr.Location = new System.Drawing.Point(101, 352);
            this.txtBtcAddr.Name = "txtBtcAddr";
            this.txtBtcAddr.Size = new System.Drawing.Size(557, 20);
            this.txtBtcAddr.TabIndex = 24;
            this.txtBtcAddr.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // btnAddressToPubHash
            // 
            this.btnAddressToPubHash.Location = new System.Drawing.Point(443, 316);
            this.btnAddressToPubHash.Name = "btnAddressToPubHash";
            this.btnAddressToPubHash.Size = new System.Drawing.Size(46, 30);
            this.btnAddressToPubHash.TabIndex = 20;
            this.btnAddressToPubHash.Text = "▲";
            this.btnAddressToPubHash.UseVisualStyleBackColor = true;
            this.btnAddressToPubHash.Click += new System.EventHandler(this.btnAddressToPubHash_Click);
            // 
            // btnPubHashToAddress
            // 
            this.btnPubHashToAddress.Location = new System.Drawing.Point(491, 316);
            this.btnPubHashToAddress.Name = "btnPubHashToAddress";
            this.btnPubHashToAddress.Size = new System.Drawing.Size(46, 30);
            this.btnPubHashToAddress.TabIndex = 21;
            this.btnPubHashToAddress.Text = "▼";
            this.btnPubHashToAddress.UseVisualStyleBackColor = true;
            this.btnPubHashToAddress.Click += new System.EventHandler(this.btnPubHashToAddress_Click);
            // 
            // btnPubHexToHash
            // 
            this.btnPubHexToHash.Location = new System.Drawing.Point(491, 258);
            this.btnPubHexToHash.Name = "btnPubHexToHash";
            this.btnPubHexToHash.Size = new System.Drawing.Size(46, 30);
            this.btnPubHexToHash.TabIndex = 16;
            this.btnPubHexToHash.Text = "▼▼";
            this.btnPubHexToHash.UseVisualStyleBackColor = true;
            this.btnPubHexToHash.Click += new System.EventHandler(this.btnPubHexToHash_Click);
            // 
            // btnPrivToPub
            // 
            this.btnPrivToPub.Location = new System.Drawing.Point(491, 173);
            this.btnPrivToPub.Name = "btnPrivToPub";
            this.btnPrivToPub.Size = new System.Drawing.Size(46, 30);
            this.btnPrivToPub.TabIndex = 13;
            this.btnPrivToPub.Text = "▼▼";
            this.btnPrivToPub.UseVisualStyleBackColor = true;
            this.btnPrivToPub.Click += new System.EventHandler(this.btnPrivToPub_Click);
            // 
            // btnPrivWIFToHex
            // 
            this.btnPrivWIFToHex.Location = new System.Drawing.Point(491, 115);
            this.btnPrivWIFToHex.Name = "btnPrivWIFToHex";
            this.btnPrivWIFToHex.Size = new System.Drawing.Size(46, 30);
            this.btnPrivWIFToHex.TabIndex = 9;
            this.btnPrivWIFToHex.Text = "▼▼";
            this.btnPrivWIFToHex.UseVisualStyleBackColor = true;
            this.btnPrivWIFToHex.Click += new System.EventHandler(this.btnPrivWIFToHex_Click);
            // 
            // btnPrivHexToWIF
            // 
            this.btnPrivHexToWIF.Location = new System.Drawing.Point(443, 115);
            this.btnPrivHexToWIF.Name = "btnPrivHexToWIF";
            this.btnPrivHexToWIF.Size = new System.Drawing.Size(46, 30);
            this.btnPrivHexToWIF.TabIndex = 8;
            this.btnPrivHexToWIF.Text = "▲";
            this.btnPrivHexToWIF.UseVisualStyleBackColor = true;
            this.btnPrivHexToWIF.Click += new System.EventHandler(this.btnPrivHexToWIF_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(543, 115);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(114, 30);
            this.btnGenerate.TabIndex = 10;
            this.btnGenerate.Text = "Generate Address";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnBlockExplorer
            // 
            this.btnBlockExplorer.Location = new System.Drawing.Point(543, 316);
            this.btnBlockExplorer.Name = "btnBlockExplorer";
            this.btnBlockExplorer.Size = new System.Drawing.Size(114, 29);
            this.btnBlockExplorer.TabIndex = 23;
            this.btnBlockExplorer.Text = "Block Explorer";
            this.btnBlockExplorer.UseVisualStyleBackColor = true;
            this.btnBlockExplorer.Click += new System.EventHandler(this.btnBlockExplorer_Click);
            // 
            // cboCoinType
            // 
            this.cboCoinType.FormattingEnabled = true;
            this.cboCoinType.Items.AddRange(new object[] {
            "Bitcoin",
            "Testnet",
            "Namecoin",
            "Litecoin"});
            this.cboCoinType.Location = new System.Drawing.Point(336, 320);
            this.cboCoinType.Name = "cboCoinType";
            this.cboCoinType.Size = new System.Drawing.Size(101, 21);
            this.cboCoinType.TabIndex = 22;
            this.cboCoinType.Text = "Bitcoin";
            this.cboCoinType.SelectionChangeCommitted += new System.EventHandler(this.cboCoinType_SelectionChangeCommitted);
            this.cboCoinType.SelectedValueChanged += new System.EventHandler(this.cboCoinType_SelectionChangeCommitted);
            // 
            // btnShacode
            // 
            this.btnShacode.Location = new System.Drawing.Point(543, 58);
            this.btnShacode.Name = "btnShacode";
            this.btnShacode.Size = new System.Drawing.Size(114, 30);
            this.btnShacode.TabIndex = 4;
            this.btnShacode.Text = "Generate Minikey";
            this.btnShacode.UseVisualStyleBackColor = true;
            this.btnShacode.Click += new System.EventHandler(this.btnShacode_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem,
            this.editToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.publicKeyToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(670, 24);
            this.menuStrip1.TabIndex = 25;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.base58CalcToolStripMenuItem,
            this.keyCombinerToolStripMenuItem,
            this.mofNCalcToolStripMenuItem,
            this.pPECKeygenToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // base58CalcToolStripMenuItem
            // 
            this.base58CalcToolStripMenuItem.Name = "base58CalcToolStripMenuItem";
            this.base58CalcToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.base58CalcToolStripMenuItem.Text = "Base58 Calculator";
            this.base58CalcToolStripMenuItem.Click += new System.EventHandler(this.base58CalcToolStripMenuItem_Click);
            // 
            // keyCombinerToolStripMenuItem
            // 
            this.keyCombinerToolStripMenuItem.Name = "keyCombinerToolStripMenuItem";
            this.keyCombinerToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.keyCombinerToolStripMenuItem.Text = "Key Combiner";
            this.keyCombinerToolStripMenuItem.Click += new System.EventHandler(this.keyCombinerToolStripMenuItem_Click);
            // 
            // mofNCalcToolStripMenuItem
            // 
            this.mofNCalcToolStripMenuItem.Name = "mofNCalcToolStripMenuItem";
            this.mofNCalcToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.mofNCalcToolStripMenuItem.Text = "M-of-N Calculator";
            this.mofNCalcToolStripMenuItem.Click += new System.EventHandler(this.mofNCalcToolStripMenuItem_Click);
            // 
            // pPECKeygenToolStripMenuItem
            // 
            this.pPECKeygenToolStripMenuItem.Name = "pPECKeygenToolStripMenuItem";
            this.pPECKeygenToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.pPECKeygenToolStripMenuItem.Text = "Intermediate Generator";
            this.pPECKeygenToolStripMenuItem.Click += new System.EventHandler(this.pPECKeygenToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyMinikeyQRMenuItem,
            this.copyPrivateKeyQRMenuItem,
            this.copyPublicHexQRMenuItem,
            this.copyAddressQRMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // copyMinikeyQRMenuItem
            // 
            this.copyMinikeyQRMenuItem.Name = "copyMinikeyQRMenuItem";
            this.copyMinikeyQRMenuItem.Size = new System.Drawing.Size(327, 22);
            this.copyMinikeyQRMenuItem.Text = "Copy minikey to clipboard as QR code image";
            this.copyMinikeyQRMenuItem.Click += new System.EventHandler(this.copyMinikeyQRMenuItem_Click);
            // 
            // copyPrivateKeyQRMenuItem
            // 
            this.copyPrivateKeyQRMenuItem.Name = "copyPrivateKeyQRMenuItem";
            this.copyPrivateKeyQRMenuItem.Size = new System.Drawing.Size(327, 22);
            this.copyPrivateKeyQRMenuItem.Text = "Copy private key to clipboard as QR code image";
            this.copyPrivateKeyQRMenuItem.Click += new System.EventHandler(this.copyPrivateKeyQRMenuItem_Click);
            // 
            // copyPublicHexQRMenuItem
            // 
            this.copyPublicHexQRMenuItem.Name = "copyPublicHexQRMenuItem";
            this.copyPublicHexQRMenuItem.Size = new System.Drawing.Size(327, 22);
            this.copyPublicHexQRMenuItem.Text = "Copy public hex to clipboard as QR code image";
            this.copyPublicHexQRMenuItem.Click += new System.EventHandler(this.copyPublicHexQRMenuItem_Click);
            // 
            // copyAddressQRMenuItem
            // 
            this.copyAddressQRMenuItem.Name = "copyAddressQRMenuItem";
            this.copyAddressQRMenuItem.Size = new System.Drawing.Size(327, 22);
            this.copyAddressQRMenuItem.Text = "Copy address to clipboard as QR code image";
            this.copyAddressQRMenuItem.Click += new System.EventHandler(this.copyAddressQRMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.spaceBetweenHexBytesToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // spaceBetweenHexBytesToolStripMenuItem
            // 
            this.spaceBetweenHexBytesToolStripMenuItem.Name = "spaceBetweenHexBytesToolStripMenuItem";
            this.spaceBetweenHexBytesToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.spaceBetweenHexBytesToolStripMenuItem.Text = "Space between hex bytes";
            this.spaceBetweenHexBytesToolStripMenuItem.Click += new System.EventHandler(this.spaceBetweenHexBytesToolStripMenuItem_Click);
            // 
            // publicKeyToolStripMenuItem
            // 
            this.publicKeyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compressToolStripMenuItem,
            this.compressPublicKeyToolStripMenuItem,
            this.uncompressPublicKeyToolStripMenuItem,
            this.showFieldsToolStripMenuItem});
            this.publicKeyToolStripMenuItem.Name = "publicKeyToolStripMenuItem";
            this.publicKeyToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.publicKeyToolStripMenuItem.Text = "Public Key";
            // 
            // compressToolStripMenuItem
            // 
            this.compressToolStripMenuItem.Name = "compressToolStripMenuItem";
            this.compressToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.compressToolStripMenuItem.Text = "Generate compressed public keys";
            this.compressToolStripMenuItem.Click += new System.EventHandler(this.compressToolStripMenuItem_Click);
            // 
            // compressPublicKeyToolStripMenuItem
            // 
            this.compressPublicKeyToolStripMenuItem.Name = "compressPublicKeyToolStripMenuItem";
            this.compressPublicKeyToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.compressPublicKeyToolStripMenuItem.Text = "Compress public key";
            this.compressPublicKeyToolStripMenuItem.Click += new System.EventHandler(this.compressPublicKeyToolStripMenuItem_Click);
            // 
            // uncompressPublicKeyToolStripMenuItem
            // 
            this.uncompressPublicKeyToolStripMenuItem.Name = "uncompressPublicKeyToolStripMenuItem";
            this.uncompressPublicKeyToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.uncompressPublicKeyToolStripMenuItem.Text = "Uncompress public key";
            this.uncompressPublicKeyToolStripMenuItem.Click += new System.EventHandler(this.uncompressPublicKeyToolStripMenuItem_Click);
            // 
            // showFieldsToolStripMenuItem
            // 
            this.showFieldsToolStripMenuItem.Name = "showFieldsToolStripMenuItem";
            this.showFieldsToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.showFieldsToolStripMenuItem.Text = "Highlight X-coordinate";
            this.showFieldsToolStripMenuItem.Click += new System.EventHandler(this.showFieldsToolStripMenuItem_Click);
            // 
            // btnSha256ToPrivate
            // 
            this.btnSha256ToPrivate.Location = new System.Drawing.Point(491, 58);
            this.btnSha256ToPrivate.Name = "btnSha256ToPrivate";
            this.btnSha256ToPrivate.Size = new System.Drawing.Size(46, 30);
            this.btnSha256ToPrivate.TabIndex = 3;
            this.btnSha256ToPrivate.Text = "▼▼";
            this.btnSha256ToPrivate.UseVisualStyleBackColor = true;
            this.btnSha256ToPrivate.Click += new System.EventHandler(this.btnSha256ToPrivate_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 39);
            this.label6.TabIndex = 0;
            this.label6.Text = "Minikey / key\r\nfrom SHA256 hash\r\nof a string";
            // 
            // txtMinikey
            // 
            this.txtMinikey.Location = new System.Drawing.Point(101, 36);
            this.txtMinikey.Name = "txtMinikey";
            this.txtMinikey.Size = new System.Drawing.Size(557, 20);
            this.txtMinikey.TabIndex = 2;
            this.txtMinikey.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.txtMinikey.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // lblNotSafe
            // 
            this.lblNotSafe.AutoSize = true;
            this.lblNotSafe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblNotSafe.Location = new System.Drawing.Point(112, 63);
            this.lblNotSafe.Name = "lblNotSafe";
            this.lblNotSafe.Size = new System.Drawing.Size(93, 13);
            this.lblNotSafe.TabIndex = 26;
            this.lblNotSafe.Text = "Warning: Not safe";
            this.lblNotSafe.Visible = false;
            // 
            // lblWhyNot
            // 
            this.lblWhyNot.AutoSize = true;
            this.lblWhyNot.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWhyNot.ForeColor = System.Drawing.Color.Blue;
            this.lblWhyNot.Location = new System.Drawing.Point(211, 63);
            this.lblWhyNot.Name = "lblWhyNot";
            this.lblWhyNot.Size = new System.Drawing.Size(53, 13);
            this.lblWhyNot.TabIndex = 27;
            this.lblWhyNot.Text = "Why not?";
            this.lblWhyNot.Visible = false;
            this.lblWhyNot.Click += new System.EventHandler(this.lblWhyNot_Click);
            // 
            // txtPassphrase
            // 
            this.txtPassphrase.Location = new System.Drawing.Point(101, 115);
            this.txtPassphrase.Name = "txtPassphrase";
            this.txtPassphrase.Size = new System.Drawing.Size(336, 20);
            this.txtPassphrase.TabIndex = 28;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 112);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 26);
            this.label7.TabIndex = 29;
            this.label7.Text = "Encryption phrase\r\nfor private key";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 391);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtPassphrase);
            this.Controls.Add(this.lblWhyNot);
            this.Controls.Add(this.lblNotSafe);
            this.Controls.Add(this.btnSha256ToPrivate);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtMinikey);
            this.Controls.Add(this.btnShacode);
            this.Controls.Add(this.cboCoinType);
            this.Controls.Add(this.btnBlockExplorer);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnPrivWIFToHex);
            this.Controls.Add(this.btnPrivHexToWIF);
            this.Controls.Add(this.btnPrivToPub);
            this.Controls.Add(this.btnPubHexToHash);
            this.Controls.Add(this.btnPubHashToAddress);
            this.Controls.Add(this.btnAddressToPubHash);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBtcAddr);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPubHash);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPubHex);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPrivHex);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPrivWIF);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Bitcoin Address Utility by Casascius (Beta, No Warranty)";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPrivWIF;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPrivHex;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPubHex;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPubHash;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBtcAddr;
        private System.Windows.Forms.Button btnAddressToPubHash;
        private System.Windows.Forms.Button btnPubHashToAddress;
        private System.Windows.Forms.Button btnPubHexToHash;
        private System.Windows.Forms.Button btnPrivToPub;
        private System.Windows.Forms.Button btnPrivWIFToHex;
        private System.Windows.Forms.Button btnPrivHexToWIF;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnBlockExplorer;
        private System.Windows.Forms.ComboBox cboCoinType;
        private System.Windows.Forms.Button btnShacode;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Button btnSha256ToPrivate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMinikey;
        private System.Windows.Forms.Label lblNotSafe;
        private System.Windows.Forms.Label lblWhyNot;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem base58CalcToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mofNCalcToolStripMenuItem;
        private System.Windows.Forms.TextBox txtPassphrase;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spaceBetweenHexBytesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem publicKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compressToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compressPublicKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uncompressPublicKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showFieldsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keyCombinerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyPrivateKeyQRMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyAddressQRMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyMinikeyQRMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyPublicHexQRMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem pPECKeygenToolStripMenuItem;

    }
}


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
            this.walletGeneratorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.whatIsASHAcodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPrivWIF
            // 
            this.txtPrivWIF.Location = new System.Drawing.Point(103, 28);
            this.txtPrivWIF.Name = "txtPrivWIF";
            this.txtPrivWIF.Size = new System.Drawing.Size(557, 20);
            this.txtPrivWIF.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Private Key (WIF)";
            // 
            // txtPrivHex
            // 
            this.txtPrivHex.Location = new System.Drawing.Point(103, 86);
            this.txtPrivHex.Name = "txtPrivHex";
            this.txtPrivHex.Size = new System.Drawing.Size(557, 20);
            this.txtPrivHex.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Private Key (Hex)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 147);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Public Key (Hex)";
            // 
            // txtPubHex
            // 
            this.txtPubHex.Location = new System.Drawing.Point(103, 144);
            this.txtPubHex.Multiline = true;
            this.txtPubHex.Name = "txtPubHex";
            this.txtPubHex.Size = new System.Drawing.Size(557, 43);
            this.txtPubHex.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 232);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Public Key (Hash)";
            // 
            // txtPubHash
            // 
            this.txtPubHash.Location = new System.Drawing.Point(103, 229);
            this.txtPubHash.Name = "txtPubHash";
            this.txtPubHash.Size = new System.Drawing.Size(557, 20);
            this.txtPubHash.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 290);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Address";
            // 
            // txtBtcAddr
            // 
            this.txtBtcAddr.Location = new System.Drawing.Point(103, 287);
            this.txtBtcAddr.Name = "txtBtcAddr";
            this.txtBtcAddr.Size = new System.Drawing.Size(557, 20);
            this.txtBtcAddr.TabIndex = 15;
            // 
            // btnAddressToPubHash
            // 
            this.btnAddressToPubHash.Location = new System.Drawing.Point(286, 251);
            this.btnAddressToPubHash.Name = "btnAddressToPubHash";
            this.btnAddressToPubHash.Size = new System.Drawing.Size(46, 30);
            this.btnAddressToPubHash.TabIndex = 18;
            this.btnAddressToPubHash.Text = "▲";
            this.btnAddressToPubHash.UseVisualStyleBackColor = true;
            this.btnAddressToPubHash.Click += new System.EventHandler(this.btnAddressToPubHash_Click);
            // 
            // btnPubHashToAddress
            // 
            this.btnPubHashToAddress.Location = new System.Drawing.Point(334, 251);
            this.btnPubHashToAddress.Name = "btnPubHashToAddress";
            this.btnPubHashToAddress.Size = new System.Drawing.Size(46, 30);
            this.btnPubHashToAddress.TabIndex = 19;
            this.btnPubHashToAddress.Text = "▼";
            this.btnPubHashToAddress.UseVisualStyleBackColor = true;
            this.btnPubHashToAddress.Click += new System.EventHandler(this.btnPubHashToAddress_Click);
            // 
            // btnPubHexToHash
            // 
            this.btnPubHexToHash.Location = new System.Drawing.Point(334, 193);
            this.btnPubHexToHash.Name = "btnPubHexToHash";
            this.btnPubHexToHash.Size = new System.Drawing.Size(46, 30);
            this.btnPubHexToHash.TabIndex = 21;
            this.btnPubHexToHash.Text = "▼";
            this.btnPubHexToHash.UseVisualStyleBackColor = true;
            this.btnPubHexToHash.Click += new System.EventHandler(this.btnPubHexToHash_Click);
            // 
            // btnPrivToPub
            // 
            this.btnPrivToPub.Location = new System.Drawing.Point(334, 108);
            this.btnPrivToPub.Name = "btnPrivToPub";
            this.btnPrivToPub.Size = new System.Drawing.Size(46, 30);
            this.btnPrivToPub.TabIndex = 23;
            this.btnPrivToPub.Text = "▼";
            this.btnPrivToPub.UseVisualStyleBackColor = true;
            this.btnPrivToPub.Click += new System.EventHandler(this.btnPrivToPub_Click);
            // 
            // btnPrivWIFToHex
            // 
            this.btnPrivWIFToHex.Location = new System.Drawing.Point(334, 50);
            this.btnPrivWIFToHex.Name = "btnPrivWIFToHex";
            this.btnPrivWIFToHex.Size = new System.Drawing.Size(46, 30);
            this.btnPrivWIFToHex.TabIndex = 25;
            this.btnPrivWIFToHex.Text = "▼";
            this.btnPrivWIFToHex.UseVisualStyleBackColor = true;
            this.btnPrivWIFToHex.Click += new System.EventHandler(this.btnPrivWIFToHex_Click);
            // 
            // btnPrivHexToWIF
            // 
            this.btnPrivHexToWIF.Location = new System.Drawing.Point(286, 50);
            this.btnPrivHexToWIF.Name = "btnPrivHexToWIF";
            this.btnPrivHexToWIF.Size = new System.Drawing.Size(46, 30);
            this.btnPrivHexToWIF.TabIndex = 24;
            this.btnPrivHexToWIF.Text = "▲";
            this.btnPrivHexToWIF.UseVisualStyleBackColor = true;
            this.btnPrivHexToWIF.Click += new System.EventHandler(this.btnPrivHexToWIF_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(420, 50);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(116, 30);
            this.btnGenerate.TabIndex = 26;
            this.btnGenerate.Text = "Generate Address";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnBlockExplorer
            // 
            this.btnBlockExplorer.Location = new System.Drawing.Point(492, 251);
            this.btnBlockExplorer.Name = "btnBlockExplorer";
            this.btnBlockExplorer.Size = new System.Drawing.Size(167, 29);
            this.btnBlockExplorer.TabIndex = 27;
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
            "Namecoin"});
            this.cboCoinType.Location = new System.Drawing.Point(382, 255);
            this.cboCoinType.Name = "cboCoinType";
            this.cboCoinType.Size = new System.Drawing.Size(101, 21);
            this.cboCoinType.TabIndex = 29;
            this.cboCoinType.Text = "Bitcoin";
            // 
            // btnShacode
            // 
            this.btnShacode.Location = new System.Drawing.Point(542, 50);
            this.btnShacode.Name = "btnShacode";
            this.btnShacode.Size = new System.Drawing.Size(116, 30);
            this.btnShacode.TabIndex = 30;
            this.btnShacode.Text = "Generate SHAcode";
            this.btnShacode.UseVisualStyleBackColor = true;
            this.btnShacode.Click += new System.EventHandler(this.btnShacode_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.walletGeneratorToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(670, 24);
            this.menuStrip1.TabIndex = 31;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // walletGeneratorToolStripMenuItem
            // 
            this.walletGeneratorToolStripMenuItem.Name = "walletGeneratorToolStripMenuItem";
            this.walletGeneratorToolStripMenuItem.Size = new System.Drawing.Size(107, 20);
            this.walletGeneratorToolStripMenuItem.Text = "&Wallet Generator";
            this.walletGeneratorToolStripMenuItem.Click += new System.EventHandler(this.walletGeneratorToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.whatIsASHAcodeToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // whatIsASHAcodeToolStripMenuItem
            // 
            this.whatIsASHAcodeToolStripMenuItem.Name = "whatIsASHAcodeToolStripMenuItem";
            this.whatIsASHAcodeToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.whatIsASHAcodeToolStripMenuItem.Text = "What is a SHAcode?";
            this.whatIsASHAcodeToolStripMenuItem.Click += new System.EventHandler(this.whatIsASHAcodeToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 317);
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
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Bitcoin Address Utility by Casascius (Beta, No Warranty)";
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
        private System.Windows.Forms.ToolStripMenuItem walletGeneratorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem whatIsASHAcodeToolStripMenuItem;

    }
}


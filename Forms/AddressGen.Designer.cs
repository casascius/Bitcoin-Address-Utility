namespace BtcAddress.Forms {
    partial class AddressGen {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddressGen));
            this.rdoDeterministicWallet = new System.Windows.Forms.RadioButton();
            this.txtTextInput = new System.Windows.Forms.TextBox();
            this.rdoRandomWallet = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.numGenCount = new System.Windows.Forms.NumericUpDown();
            this.rdoMiniKeys = new System.Windows.Forms.RadioButton();
            this.btnGenerateAddresses = new System.Windows.Forms.Button();
            this.lblTextInput = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoEncrypted = new System.Windows.Forms.RadioButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.chkRetainPrivKey = new System.Windows.Forms.CheckBox();
            this.rdoTwoFactor = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.numGenCount)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rdoDeterministicWallet
            // 
            this.rdoDeterministicWallet.AutoSize = true;
            this.rdoDeterministicWallet.Location = new System.Drawing.Point(14, 84);
            this.rdoDeterministicWallet.Name = "rdoDeterministicWallet";
            this.rdoDeterministicWallet.Size = new System.Drawing.Size(114, 17);
            this.rdoDeterministicWallet.TabIndex = 5;
            this.rdoDeterministicWallet.Text = "Deterministic (WIF)";
            this.rdoDeterministicWallet.UseVisualStyleBackColor = true;
            this.rdoDeterministicWallet.CheckedChanged += new System.EventHandler(this.rdoWalletType_CheckedChanged);
            // 
            // txtTextInput
            // 
            this.txtTextInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTextInput.Location = new System.Drawing.Point(8, 148);
            this.txtTextInput.Name = "txtTextInput";
            this.txtTextInput.Size = new System.Drawing.Size(365, 20);
            this.txtTextInput.TabIndex = 0;
            this.txtTextInput.Visible = false;
            // 
            // rdoRandomWallet
            // 
            this.rdoRandomWallet.AutoSize = true;
            this.rdoRandomWallet.Location = new System.Drawing.Point(14, 33);
            this.rdoRandomWallet.Name = "rdoRandomWallet";
            this.rdoRandomWallet.Size = new System.Drawing.Size(102, 17);
            this.rdoRandomWallet.TabIndex = 4;
            this.rdoRandomWallet.Text = "Full-length (WIF)";
            this.rdoRandomWallet.UseVisualStyleBackColor = true;
            this.rdoRandomWallet.CheckedChanged += new System.EventHandler(this.rdoWalletType_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(164, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Number of addresses to generate";
            // 
            // numGenCount
            // 
            this.numGenCount.Location = new System.Drawing.Point(51, 34);
            this.numGenCount.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numGenCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numGenCount.Name = "numGenCount";
            this.numGenCount.Size = new System.Drawing.Size(90, 20);
            this.numGenCount.TabIndex = 2;
            this.numGenCount.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // rdoMiniKeys
            // 
            this.rdoMiniKeys.AutoSize = true;
            this.rdoMiniKeys.Checked = true;
            this.rdoMiniKeys.Location = new System.Drawing.Point(14, 16);
            this.rdoMiniKeys.Name = "rdoMiniKeys";
            this.rdoMiniKeys.Size = new System.Drawing.Size(62, 17);
            this.rdoMiniKeys.TabIndex = 5;
            this.rdoMiniKeys.TabStop = true;
            this.rdoMiniKeys.Text = "MiniKey";
            this.rdoMiniKeys.UseVisualStyleBackColor = true;
            this.rdoMiniKeys.CheckedChanged += new System.EventHandler(this.rdoWalletType_CheckedChanged);
            // 
            // btnGenerateAddresses
            // 
            this.btnGenerateAddresses.Location = new System.Drawing.Point(109, 182);
            this.btnGenerateAddresses.Name = "btnGenerateAddresses";
            this.btnGenerateAddresses.Size = new System.Drawing.Size(173, 36);
            this.btnGenerateAddresses.TabIndex = 6;
            this.btnGenerateAddresses.Text = "Generate Addresses";
            this.btnGenerateAddresses.UseVisualStyleBackColor = true;
            this.btnGenerateAddresses.Click += new System.EventHandler(this.btnGenerateAddresses_Click);
            // 
            // lblTextInput
            // 
            this.lblTextInput.AutoSize = true;
            this.lblTextInput.Location = new System.Drawing.Point(6, 132);
            this.lblTextInput.Name = "lblTextInput";
            this.lblTextInput.Size = new System.Drawing.Size(161, 13);
            this.lblTextInput.TabIndex = 10;
            this.lblTextInput.Text = "Seed for deterministic generation";
            this.lblTextInput.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoTwoFactor);
            this.groupBox1.Controls.Add(this.rdoEncrypted);
            this.groupBox1.Controls.Add(this.rdoMiniKeys);
            this.groupBox1.Controls.Add(this.rdoRandomWallet);
            this.groupBox1.Controls.Add(this.rdoDeterministicWallet);
            this.groupBox1.Location = new System.Drawing.Point(194, 18);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(179, 111);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Private key type";
            // 
            // rdoEncrypted
            // 
            this.rdoEncrypted.AutoSize = true;
            this.rdoEncrypted.Location = new System.Drawing.Point(14, 50);
            this.rdoEncrypted.Name = "rdoEncrypted";
            this.rdoEncrypted.Size = new System.Drawing.Size(131, 17);
            this.rdoEncrypted.TabIndex = 6;
            this.rdoEncrypted.Text = "Passphrase Encrypted";
            this.rdoEncrypted.UseVisualStyleBackColor = true;
            this.rdoEncrypted.CheckedChanged += new System.EventHandler(this.rdoWalletType_CheckedChanged);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 232);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(384, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Visible = false;
            // 
            // chkRetainPrivKey
            // 
            this.chkRetainPrivKey.AutoSize = true;
            this.chkRetainPrivKey.Location = new System.Drawing.Point(8, 102);
            this.chkRetainPrivKey.Name = "chkRetainPrivKey";
            this.chkRetainPrivKey.Size = new System.Drawing.Size(174, 17);
            this.chkRetainPrivKey.TabIndex = 14;
            this.chkRetainPrivKey.Text = "Retain unencrypted private key";
            this.chkRetainPrivKey.UseVisualStyleBackColor = true;
            this.chkRetainPrivKey.Visible = false;
            // 
            // rdoTwoFactor
            // 
            this.rdoTwoFactor.AutoSize = true;
            this.rdoTwoFactor.Location = new System.Drawing.Point(14, 67);
            this.rdoTwoFactor.Name = "rdoTwoFactor";
            this.rdoTwoFactor.Size = new System.Drawing.Size(130, 17);
            this.rdoTwoFactor.TabIndex = 7;
            this.rdoTwoFactor.Text = "Two-Factor Encrypted";
            this.rdoTwoFactor.UseVisualStyleBackColor = true;
            this.rdoTwoFactor.CheckedChanged += new System.EventHandler(this.rdoWalletType_CheckedChanged);
            // 
            // AddressGen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 254);
            this.Controls.Add(this.chkRetainPrivKey);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTextInput);
            this.Controls.Add(this.btnGenerateAddresses);
            this.Controls.Add(this.numGenCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTextInput);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddressGen";
            this.Text = "Generate Addresses";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddressGen_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numGenCount)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoDeterministicWallet;
        private System.Windows.Forms.TextBox txtTextInput;
        private System.Windows.Forms.RadioButton rdoRandomWallet;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numGenCount;
        private System.Windows.Forms.RadioButton rdoMiniKeys;
        private System.Windows.Forms.Button btnGenerateAddresses;
        private System.Windows.Forms.Label lblTextInput;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoEncrypted;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.CheckBox chkRetainPrivKey;
        private System.Windows.Forms.RadioButton rdoTwoFactor;
    }
}
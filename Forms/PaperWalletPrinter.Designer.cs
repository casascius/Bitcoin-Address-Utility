namespace BtcAddress {
    partial class PaperWalletPrinter {
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboNumPerPage = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboColor = new System.Windows.Forms.ComboBox();
            this.lblDenomination = new System.Windows.Forms.Label();
            this.txtDenomination = new System.Windows.Forms.TextBox();
            this.rdoBitcoinBanknote = new System.Windows.Forms.RadioButton();
            this.btnPrintWallet = new System.Windows.Forms.Button();
            this.rdoWalletPrivQR = new System.Windows.Forms.RadioButton();
            this.rdoWalletPubPrivQR = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.chkEncrypt = new System.Windows.Forms.CheckBox();
            this.lblGenCount = new System.Windows.Forms.Label();
            this.txtEncryptionPassword = new System.Windows.Forms.TextBox();
            this.btnGenerateAddresses = new System.Windows.Forms.Button();
            this.rdoDeterministicWallet = new System.Windows.Forms.RadioButton();
            this.rdoRandomWallet = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.numGenCount = new System.Windows.Forms.NumericUpDown();
            this.lblInputDescription = new System.Windows.Forms.Label();
            this.txtPassphrase = new System.Windows.Forms.TextBox();
            this.btnSaveAddresses = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnSortKeys = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdoSavePrivKeys = new System.Windows.Forms.RadioButton();
            this.rdoSendmany = new System.Windows.Forms.RadioButton();
            this.rdoSaveAddressesOnly = new System.Windows.Forms.RadioButton();
            this.chkMiniKeys = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGenCount)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cboNumPerPage);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cboColor);
            this.groupBox1.Controls.Add(this.lblDenomination);
            this.groupBox1.Controls.Add(this.txtDenomination);
            this.groupBox1.Controls.Add(this.rdoBitcoinBanknote);
            this.groupBox1.Controls.Add(this.btnPrintWallet);
            this.groupBox1.Controls.Add(this.rdoWalletPrivQR);
            this.groupBox1.Controls.Add(this.rdoWalletPubPrivQR);
            this.groupBox1.Location = new System.Drawing.Point(486, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(313, 211);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Printing Style";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Max vouchers per page";
            // 
            // cboNumPerPage
            // 
            this.cboNumPerPage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNumPerPage.FormattingEnabled = true;
            this.cboNumPerPage.Items.AddRange(new object[] {
            "3",
            "2",
            "1"});
            this.cboNumPerPage.Location = new System.Drawing.Point(157, 145);
            this.cboNumPerPage.Name = "cboNumPerPage";
            this.cboNumPerPage.Size = new System.Drawing.Size(121, 21);
            this.cboNumPerPage.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(120, 124);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Color";
            // 
            // cboColor
            // 
            this.cboColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboColor.FormattingEnabled = true;
            this.cboColor.Items.AddRange(new object[] {
            "Yellow",
            "Blue",
            "Purple",
            "Green",
            "Greyscale"});
            this.cboColor.Location = new System.Drawing.Point(157, 121);
            this.cboColor.Name = "cboColor";
            this.cboColor.Size = new System.Drawing.Size(121, 21);
            this.cboColor.TabIndex = 15;
            // 
            // lblDenomination
            // 
            this.lblDenomination.AutoSize = true;
            this.lblDenomination.Location = new System.Drawing.Point(79, 101);
            this.lblDenomination.Name = "lblDenomination";
            this.lblDenomination.Size = new System.Drawing.Size(72, 13);
            this.lblDenomination.TabIndex = 14;
            this.lblDenomination.Text = "Denomination";
            // 
            // txtDenomination
            // 
            this.txtDenomination.Location = new System.Drawing.Point(157, 98);
            this.txtDenomination.Name = "txtDenomination";
            this.txtDenomination.Size = new System.Drawing.Size(81, 20);
            this.txtDenomination.TabIndex = 13;
            // 
            // rdoBitcoinBanknote
            // 
            this.rdoBitcoinBanknote.AutoSize = true;
            this.rdoBitcoinBanknote.Location = new System.Drawing.Point(27, 77);
            this.rdoBitcoinBanknote.Name = "rdoBitcoinBanknote";
            this.rdoBitcoinBanknote.Size = new System.Drawing.Size(159, 17);
            this.rdoBitcoinBanknote.TabIndex = 12;
            this.rdoBitcoinBanknote.Text = "Bitcoin \"Banknote\" Voucher";
            this.rdoBitcoinBanknote.UseVisualStyleBackColor = true;
            this.rdoBitcoinBanknote.CheckedChanged += new System.EventHandler(this.rdoBitcoinBanknote_CheckedChanged);
            // 
            // btnPrintWallet
            // 
            this.btnPrintWallet.Location = new System.Drawing.Point(82, 169);
            this.btnPrintWallet.Name = "btnPrintWallet";
            this.btnPrintWallet.Size = new System.Drawing.Size(173, 36);
            this.btnPrintWallet.TabIndex = 11;
            this.btnPrintWallet.Text = "Print the wallet";
            this.btnPrintWallet.UseVisualStyleBackColor = true;
            this.btnPrintWallet.Click += new System.EventHandler(this.btnPrintWallet_Click);
            // 
            // rdoWalletPrivQR
            // 
            this.rdoWalletPrivQR.AutoSize = true;
            this.rdoWalletPrivQR.Location = new System.Drawing.Point(27, 54);
            this.rdoWalletPrivQR.Name = "rdoWalletPrivQR";
            this.rdoWalletPrivQR.Size = new System.Drawing.Size(222, 17);
            this.rdoWalletPrivQR.TabIndex = 1;
            this.rdoWalletPrivQR.Text = "16 wallets per page, Spend QR code only";
            this.rdoWalletPrivQR.UseVisualStyleBackColor = true;
            // 
            // rdoWalletPubPrivQR
            // 
            this.rdoWalletPubPrivQR.AutoSize = true;
            this.rdoWalletPubPrivQR.Checked = true;
            this.rdoWalletPubPrivQR.Location = new System.Drawing.Point(27, 31);
            this.rdoWalletPubPrivQR.Name = "rdoWalletPubPrivQR";
            this.rdoWalletPubPrivQR.Size = new System.Drawing.Size(228, 17);
            this.rdoWalletPubPrivQR.TabIndex = 0;
            this.rdoWalletPubPrivQR.TabStop = true;
            this.rdoWalletPubPrivQR.Text = "8 wallets per page, Load/Spend QR codes";
            this.rdoWalletPubPrivQR.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkMiniKeys);
            this.groupBox2.Controls.Add(this.lblPassword);
            this.groupBox2.Controls.Add(this.chkEncrypt);
            this.groupBox2.Controls.Add(this.lblGenCount);
            this.groupBox2.Controls.Add(this.txtEncryptionPassword);
            this.groupBox2.Controls.Add(this.btnGenerateAddresses);
            this.groupBox2.Controls.Add(this.rdoDeterministicWallet);
            this.groupBox2.Controls.Add(this.rdoRandomWallet);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.numGenCount);
            this.groupBox2.Controls.Add(this.lblInputDescription);
            this.groupBox2.Controls.Add(this.txtPassphrase);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(468, 211);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Address Generation Options";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(9, 109);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(105, 13);
            this.lblPassword.TabIndex = 9;
            this.lblPassword.Text = "Encryption password";
            this.lblPassword.Visible = false;
            // 
            // chkEncrypt
            // 
            this.chkEncrypt.AutoSize = true;
            this.chkEncrypt.Location = new System.Drawing.Point(293, 31);
            this.chkEncrypt.Name = "chkEncrypt";
            this.chkEncrypt.Size = new System.Drawing.Size(122, 17);
            this.chkEncrypt.TabIndex = 8;
            this.chkEncrypt.Text = "Encrypt private keys";
            this.chkEncrypt.UseVisualStyleBackColor = true;
            this.chkEncrypt.CheckedChanged += new System.EventHandler(this.chkEncrypt_CheckedChanged);
            // 
            // lblGenCount
            // 
            this.lblGenCount.AutoSize = true;
            this.lblGenCount.Location = new System.Drawing.Point(9, 184);
            this.lblGenCount.Name = "lblGenCount";
            this.lblGenCount.Size = new System.Drawing.Size(172, 13);
            this.lblGenCount.TabIndex = 7;
            this.lblGenCount.Text = "0 addresses have been generated.";
            // 
            // txtEncryptionPassword
            // 
            this.txtEncryptionPassword.Location = new System.Drawing.Point(120, 106);
            this.txtEncryptionPassword.Name = "txtEncryptionPassword";
            this.txtEncryptionPassword.Size = new System.Drawing.Size(341, 20);
            this.txtEncryptionPassword.TabIndex = 8;
            this.txtEncryptionPassword.Visible = false;
            // 
            // btnGenerateAddresses
            // 
            this.btnGenerateAddresses.Location = new System.Drawing.Point(288, 170);
            this.btnGenerateAddresses.Name = "btnGenerateAddresses";
            this.btnGenerateAddresses.Size = new System.Drawing.Size(173, 36);
            this.btnGenerateAddresses.TabIndex = 6;
            this.btnGenerateAddresses.Text = "Generate Addresses";
            this.btnGenerateAddresses.UseVisualStyleBackColor = true;
            this.btnGenerateAddresses.Click += new System.EventHandler(this.btnGenerateAddresses_Click);
            // 
            // rdoDeterministicWallet
            // 
            this.rdoDeterministicWallet.AutoSize = true;
            this.rdoDeterministicWallet.Location = new System.Drawing.Point(20, 77);
            this.rdoDeterministicWallet.Name = "rdoDeterministicWallet";
            this.rdoDeterministicWallet.Size = new System.Drawing.Size(115, 17);
            this.rdoDeterministicWallet.TabIndex = 5;
            this.rdoDeterministicWallet.Text = "Deterministic wallet";
            this.rdoDeterministicWallet.UseVisualStyleBackColor = true;
            this.rdoDeterministicWallet.CheckedChanged += new System.EventHandler(this.rdoDeterministicWallet_CheckedChanged);
            // 
            // rdoRandomWallet
            // 
            this.rdoRandomWallet.AutoSize = true;
            this.rdoRandomWallet.Checked = true;
            this.rdoRandomWallet.Location = new System.Drawing.Point(20, 54);
            this.rdoRandomWallet.Name = "rdoRandomWallet";
            this.rdoRandomWallet.Size = new System.Drawing.Size(153, 17);
            this.rdoRandomWallet.TabIndex = 4;
            this.rdoRandomWallet.TabStop = true;
            this.rdoRandomWallet.Text = "Randomly generated wallet";
            this.rdoRandomWallet.UseVisualStyleBackColor = true;
            this.rdoRandomWallet.CheckedChanged += new System.EventHandler(this.rdoRandomWallet_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(164, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Number of addresses to generate";
            // 
            // numGenCount
            // 
            this.numGenCount.Location = new System.Drawing.Point(187, 28);
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
            // lblInputDescription
            // 
            this.lblInputDescription.AutoSize = true;
            this.lblInputDescription.Location = new System.Drawing.Point(9, 129);
            this.lblInputDescription.Name = "lblInputDescription";
            this.lblInputDescription.Size = new System.Drawing.Size(284, 13);
            this.lblInputDescription.TabIndex = 1;
            this.lblInputDescription.Text = "Enter some random text with your keyboard to add entropy.";
            // 
            // txtPassphrase
            // 
            this.txtPassphrase.Location = new System.Drawing.Point(12, 145);
            this.txtPassphrase.Name = "txtPassphrase";
            this.txtPassphrase.Size = new System.Drawing.Size(450, 20);
            this.txtPassphrase.TabIndex = 0;
            // 
            // btnSaveAddresses
            // 
            this.btnSaveAddresses.Location = new System.Drawing.Point(76, 108);
            this.btnSaveAddresses.Name = "btnSaveAddresses";
            this.btnSaveAddresses.Size = new System.Drawing.Size(173, 36);
            this.btnSaveAddresses.TabIndex = 9;
            this.btnSaveAddresses.Text = "Save addresses to a text file";
            this.btnSaveAddresses.UseVisualStyleBackColor = true;
            this.btnSaveAddresses.Click += new System.EventHandler(this.btnSaveAddresses_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnSortKeys
            // 
            this.btnSortKeys.Location = new System.Drawing.Point(300, 357);
            this.btnSortKeys.Name = "btnSortKeys";
            this.btnSortKeys.Size = new System.Drawing.Size(173, 36);
            this.btnSortKeys.TabIndex = 11;
            this.btnSortKeys.Text = "Sort the keys by Bitcoin address";
            this.btnSortKeys.UseVisualStyleBackColor = true;
            this.btnSortKeys.Click += new System.EventHandler(this.btnSortKeys_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdoSavePrivKeys);
            this.groupBox3.Controls.Add(this.rdoSendmany);
            this.groupBox3.Controls.Add(this.rdoSaveAddressesOnly);
            this.groupBox3.Controls.Add(this.btnSaveAddresses);
            this.groupBox3.Location = new System.Drawing.Point(486, 231);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(312, 162);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Save text file";
            // 
            // rdoSavePrivKeys
            // 
            this.rdoSavePrivKeys.AutoSize = true;
            this.rdoSavePrivKeys.Location = new System.Drawing.Point(46, 49);
            this.rdoSavePrivKeys.Name = "rdoSavePrivKeys";
            this.rdoSavePrivKeys.Size = new System.Drawing.Size(155, 17);
            this.rdoSavePrivKeys.TabIndex = 2;
            this.rdoSavePrivKeys.Text = "Addresses and private keys";
            this.rdoSavePrivKeys.UseVisualStyleBackColor = true;
            // 
            // rdoSendmany
            // 
            this.rdoSendmany.AutoSize = true;
            this.rdoSendmany.Location = new System.Drawing.Point(46, 72);
            this.rdoSendmany.Name = "rdoSendmany";
            this.rdoSendmany.Size = new System.Drawing.Size(162, 30);
            this.rdoSendmany.TabIndex = 1;
            this.rdoSendmany.Text = "bitcoind sendmany command\r\n(for batch funding)\r\n";
            this.rdoSendmany.UseVisualStyleBackColor = true;
            // 
            // rdoSaveAddressesOnly
            // 
            this.rdoSaveAddressesOnly.AutoSize = true;
            this.rdoSaveAddressesOnly.Checked = true;
            this.rdoSaveAddressesOnly.Location = new System.Drawing.Point(46, 26);
            this.rdoSaveAddressesOnly.Name = "rdoSaveAddressesOnly";
            this.rdoSaveAddressesOnly.Size = new System.Drawing.Size(108, 17);
            this.rdoSaveAddressesOnly.TabIndex = 0;
            this.rdoSaveAddressesOnly.TabStop = true;
            this.rdoSaveAddressesOnly.Text = "Bitcoin addresses";
            this.rdoSaveAddressesOnly.UseVisualStyleBackColor = true;
            // 
            // chkMiniKeys
            // 
            this.chkMiniKeys.AutoSize = true;
            this.chkMiniKeys.Location = new System.Drawing.Point(293, 55);
            this.chkMiniKeys.Name = "chkMiniKeys";
            this.chkMiniKeys.Size = new System.Drawing.Size(117, 17);
            this.chkMiniKeys.TabIndex = 10;
            this.chkMiniKeys.Text = "Use MiniKey format";
            this.chkMiniKeys.UseVisualStyleBackColor = true;
            this.chkMiniKeys.CheckedChanged += new System.EventHandler(this.chkMiniKeys_CheckedChanged);
            // 
            // PaperWalletPrinter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 409);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnSortKeys);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "PaperWalletPrinter";
            this.Text = "Paper Wallet Printer";
            this.Load += new System.EventHandler(this.PaperWalletPrinter_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGenCount)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoWalletPrivQR;
        private System.Windows.Forms.RadioButton rdoWalletPubPrivQR;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdoDeterministicWallet;
        private System.Windows.Forms.RadioButton rdoRandomWallet;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numGenCount;
        private System.Windows.Forms.Label lblInputDescription;
        private System.Windows.Forms.TextBox txtPassphrase;
        private System.Windows.Forms.Label lblGenCount;
        private System.Windows.Forms.Button btnGenerateAddresses;
        private System.Windows.Forms.Button btnPrintWallet;
        private System.Windows.Forms.Button btnSaveAddresses;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnSortKeys;
        private System.Windows.Forms.RadioButton rdoBitcoinBanknote;
        private System.Windows.Forms.Label lblDenomination;
        private System.Windows.Forms.TextBox txtDenomination;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdoSavePrivKeys;
        private System.Windows.Forms.RadioButton rdoSendmany;
        private System.Windows.Forms.RadioButton rdoSaveAddressesOnly;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.CheckBox chkEncrypt;
        private System.Windows.Forms.TextBox txtEncryptionPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboNumPerPage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboColor;
        private System.Windows.Forms.CheckBox chkMiniKeys;

    }
}
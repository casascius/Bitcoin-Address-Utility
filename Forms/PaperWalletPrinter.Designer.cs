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
            this.btnPrintWallet = new System.Windows.Forms.Button();
            this.rdoWalletPrivQR = new System.Windows.Forms.RadioButton();
            this.rdoWalletPubPrivQR = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkMiniKeys = new System.Windows.Forms.CheckBox();
            this.lblGenCount = new System.Windows.Forms.Label();
            this.btnGenerateAddresses = new System.Windows.Forms.Button();
            this.rdoDeterministicWallet = new System.Windows.Forms.RadioButton();
            this.rdoRandomWallet = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.numGenCount = new System.Windows.Forms.NumericUpDown();
            this.lblInputDescription = new System.Windows.Forms.Label();
            this.txtPassphrase = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnSortKeys = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGenCount)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSortKeys);
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
            // btnPrintWallet
            // 
            this.btnPrintWallet.Location = new System.Drawing.Point(82, 92);
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
            this.groupBox2.Controls.Add(this.lblGenCount);
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
            // chkMiniKeys
            // 
            this.chkMiniKeys.AutoSize = true;
            this.chkMiniKeys.Location = new System.Drawing.Point(288, 32);
            this.chkMiniKeys.Name = "chkMiniKeys";
            this.chkMiniKeys.Size = new System.Drawing.Size(117, 17);
            this.chkMiniKeys.TabIndex = 10;
            this.chkMiniKeys.Text = "Use MiniKey format";
            this.chkMiniKeys.UseVisualStyleBackColor = true;
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
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnSortKeys
            // 
            this.btnSortKeys.Location = new System.Drawing.Point(82, 161);
            this.btnSortKeys.Name = "btnSortKeys";
            this.btnSortKeys.Size = new System.Drawing.Size(173, 36);
            this.btnSortKeys.TabIndex = 11;
            this.btnSortKeys.Text = "Sort the keys by Bitcoin address";
            this.btnSortKeys.UseVisualStyleBackColor = true;
            // 
            // PaperWalletPrinter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 235);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "PaperWalletPrinter";
            this.Text = "Paper Wallet Printer";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGenCount)).EndInit();
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
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnSortKeys;
        private System.Windows.Forms.CheckBox chkMiniKeys;

    }
}
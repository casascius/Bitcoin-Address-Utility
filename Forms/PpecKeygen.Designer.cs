namespace BtcAddress {
    partial class PpecKeygen {
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
            this.txtPassphrase = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPassphraseCode = new System.Windows.Forms.TextBox();
            this.btnGenerateKey = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEncryptedKey = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBitcoinAddress = new System.Windows.Forms.TextBox();
            this.btnEncode = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtPassphrase
            // 
            this.txtPassphrase.Location = new System.Drawing.Point(110, 9);
            this.txtPassphrase.Name = "txtPassphrase";
            this.txtPassphrase.Size = new System.Drawing.Size(710, 20);
            this.txtPassphrase.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Passphrase";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Passphrase Code";
            // 
            // txtPassphraseCode
            // 
            this.txtPassphraseCode.Location = new System.Drawing.Point(110, 70);
            this.txtPassphraseCode.Name = "txtPassphraseCode";
            this.txtPassphraseCode.Size = new System.Drawing.Size(710, 20);
            this.txtPassphraseCode.TabIndex = 2;
            // 
            // btnGenerateKey
            // 
            this.btnGenerateKey.Location = new System.Drawing.Point(112, 99);
            this.btnGenerateKey.Name = "btnGenerateKey";
            this.btnGenerateKey.Size = new System.Drawing.Size(128, 29);
            this.btnGenerateKey.TabIndex = 4;
            this.btnGenerateKey.Text = "Generate Key";
            this.btnGenerateKey.UseVisualStyleBackColor = true;
            this.btnGenerateKey.Click += new System.EventHandler(this.btnGenerateKey_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 164);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Encrypted Key";
            // 
            // txtEncryptedKey
            // 
            this.txtEncryptedKey.Location = new System.Drawing.Point(110, 160);
            this.txtEncryptedKey.Name = "txtEncryptedKey";
            this.txtEncryptedKey.Size = new System.Drawing.Size(710, 20);
            this.txtEncryptedKey.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Bitcoin address";
            // 
            // txtBitcoinAddress
            // 
            this.txtBitcoinAddress.Location = new System.Drawing.Point(110, 134);
            this.txtBitcoinAddress.Name = "txtBitcoinAddress";
            this.txtBitcoinAddress.Size = new System.Drawing.Size(710, 20);
            this.txtBitcoinAddress.TabIndex = 5;
            // 
            // btnEncode
            // 
            this.btnEncode.Location = new System.Drawing.Point(112, 35);
            this.btnEncode.Name = "btnEncode";
            this.btnEncode.Size = new System.Drawing.Size(128, 29);
            this.btnEncode.TabIndex = 9;
            this.btnEncode.Text = "Encode passphrase";
            this.btnEncode.UseVisualStyleBackColor = true;
            this.btnEncode.Click += new System.EventHandler(this.btnEncode_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(107, 183);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(387, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Use the main Bitcoin Address Utility screen to decrypt and obtain the private key" +
    ".";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(265, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(524, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Generating passphrases and encrypting and decrypting keys is slow by design. Expe" +
    "ct several seconds delay.";
            // 
            // PpecKeygen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 211);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnEncode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtEncryptedKey);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtBitcoinAddress);
            this.Controls.Add(this.btnGenerateKey);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPassphraseCode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPassphrase);
            this.Name = "PpecKeygen";
            this.Text = "PpecKeygen";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPassphrase;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPassphraseCode;
        private System.Windows.Forms.Button btnGenerateKey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtEncryptedKey;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBitcoinAddress;
        private System.Windows.Forms.Button btnEncode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}
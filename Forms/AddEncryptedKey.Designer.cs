namespace BtcAddress.Forms {
    partial class AddEncryptedKey {
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
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddEncryptedKey));
            this.label1 = new System.Windows.Forms.Label();
            this.txtBoxPlainPrivKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBoxPassphrase = new System.Windows.Forms.TextBox();
            this.chkBoxCompress = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(459, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter an existing plaintext private key to encrypt (any standard format except al" +
    "ready encrypted):";
            // 
            // txtBoxPlainPrivKey
            // 
            this.txtBoxPlainPrivKey.Location = new System.Drawing.Point(15, 25);
            this.txtBoxPlainPrivKey.Name = "txtBoxPlainPrivKey";
            this.txtBoxPlainPrivKey.Size = new System.Drawing.Size(598, 20);
            this.txtBoxPlainPrivKey.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(291, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Enter the passphrase or 2nd factor to use as encryption key:";
            // 
            // txtBoxPassphrase
            // 
            this.txtBoxPassphrase.Location = new System.Drawing.Point(15, 77);
            this.txtBoxPassphrase.Name = "txtBoxPassphrase";
            this.txtBoxPassphrase.Size = new System.Drawing.Size(595, 20);
            this.txtBoxPassphrase.TabIndex = 3;
            // 
            // chkBoxCompress
            // 
            this.chkBoxCompress.AutoSize = true;
            this.chkBoxCompress.Location = new System.Drawing.Point(14, 116);
            this.chkBoxCompress.Name = "chkBoxCompress";
            this.chkBoxCompress.Size = new System.Drawing.Size(149, 17);
            this.chkBoxCompress.TabIndex = 4;
            this.chkBoxCompress.Text = "Compress public address?";
            this.chkBoxCompress.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(329, 116);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(178, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "Encrypt Key and Add to Collection";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(535, 116);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // AddEncryptedKey
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(626, 153);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkBoxCompress);
            this.Controls.Add(this.txtBoxPassphrase);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtBoxPlainPrivKey);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddEncryptedKey";
            this.Text = "Add Encrypted Key";
            this.Load += new System.EventHandler(this.AddEncryptedKey_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBoxPlainPrivKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBoxPassphrase;
        private System.Windows.Forms.CheckBox chkBoxCompress;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}
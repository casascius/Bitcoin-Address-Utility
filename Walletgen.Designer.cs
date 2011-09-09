﻿namespace BtcAddress {
    partial class Walletgen {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Walletgen));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPassphrase = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtWallet = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(677, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Deterministic Wallet Generator generates a Paper Bitcoin Wallet from a passphrase" +
                ".  Entering the same passphrase generates the same wallet.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(547, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "To use it, either accept the random default passphrase or enter a secure one of y" +
                "our choice.  Then click Generate.";
            // 
            // txtPassphrase
            // 
            this.txtPassphrase.Location = new System.Drawing.Point(8, 72);
            this.txtPassphrase.Name = "txtPassphrase";
            this.txtPassphrase.Size = new System.Drawing.Size(720, 20);
            this.txtPassphrase.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Passphrase:";
            // 
            // txtWallet
            // 
            this.txtWallet.Location = new System.Drawing.Point(12, 129);
            this.txtWallet.Multiline = true;
            this.txtWallet.Name = "txtWallet";
            this.txtWallet.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtWallet.Size = new System.Drawing.Size(715, 474);
            this.txtWallet.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(606, 95);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 28);
            this.button1.TabIndex = 2;
            this.button1.Text = "Generate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 608);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(520, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Generation formula: PrivKey = SHA256(n + \"/\" + passphrase + \"/\" + n + \"/BITCOIN) " +
                "where n = \"1\" thru \"10\"";
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(12, 624);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(661, 45);
            this.textBox1.TabIndex = 8;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(59, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Number of Addresses";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(9, 72);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(719, 20);
            this.progressBar1.TabIndex = 12;
            this.progressBar1.Visible = false;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(8, 96);
            this.textBox2.MaxLength = 3;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(45, 20);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "10";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(678, 625);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(62, 41);
            this.button2.TabIndex = 13;
            this.button2.Text = "Print";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Walletgen
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 678);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtWallet);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPassphrase);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Walletgen";
            this.Text = "Deterministic Wallet Generator";
            this.Shown += new System.EventHandler(this.Walletgen_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPassphrase;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtWallet;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button2;
    }
}
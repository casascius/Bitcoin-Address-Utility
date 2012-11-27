namespace BtcAddress {
    partial class KeyCombiner {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeyCombiner));
            this.txtInput1 = new System.Windows.Forms.TextBox();
            this.txtInput2 = new System.Windows.Forms.TextBox();
            this.txtOutputAddress = new System.Windows.Forms.TextBox();
            this.txtOutputPriv = new System.Windows.Forms.TextBox();
            this.btnCombine = new System.Windows.Forms.Button();
            this.txtOutputPubkey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoAdd = new System.Windows.Forms.RadioButton();
            this.rdoMul = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtInput1
            // 
            this.txtInput1.Location = new System.Drawing.Point(99, 153);
            this.txtInput1.Multiline = true;
            this.txtInput1.Name = "txtInput1";
            this.txtInput1.Size = new System.Drawing.Size(493, 42);
            this.txtInput1.TabIndex = 0;
            // 
            // txtInput2
            // 
            this.txtInput2.Location = new System.Drawing.Point(99, 202);
            this.txtInput2.Multiline = true;
            this.txtInput2.Name = "txtInput2";
            this.txtInput2.Size = new System.Drawing.Size(493, 42);
            this.txtInput2.TabIndex = 1;
            // 
            // txtOutputAddress
            // 
            this.txtOutputAddress.Location = new System.Drawing.Point(98, 336);
            this.txtOutputAddress.Name = "txtOutputAddress";
            this.txtOutputAddress.Size = new System.Drawing.Size(493, 20);
            this.txtOutputAddress.TabIndex = 2;
            // 
            // txtOutputPriv
            // 
            this.txtOutputPriv.Location = new System.Drawing.Point(99, 425);
            this.txtOutputPriv.Name = "txtOutputPriv";
            this.txtOutputPriv.Size = new System.Drawing.Size(493, 20);
            this.txtOutputPriv.TabIndex = 3;
            // 
            // btnCombine
            // 
            this.btnCombine.Location = new System.Drawing.Point(99, 247);
            this.btnCombine.Name = "btnCombine";
            this.btnCombine.Size = new System.Drawing.Size(109, 41);
            this.btnCombine.TabIndex = 4;
            this.btnCombine.Text = "Combine";
            this.btnCombine.UseVisualStyleBackColor = true;
            this.btnCombine.Click += new System.EventHandler(this.btnCombine_Click);
            // 
            // txtOutputPubkey
            // 
            this.txtOutputPubkey.Location = new System.Drawing.Point(98, 368);
            this.txtOutputPubkey.Multiline = true;
            this.txtOutputPubkey.Name = "txtOutputPubkey";
            this.txtOutputPubkey.Size = new System.Drawing.Size(493, 42);
            this.txtOutputPubkey.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 156);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Input Key 1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 205);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Input Key 2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 330);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 26);
            this.label3.TabIndex = 8;
            this.label3.Text = "Resulting\r\nBitcoin Address";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 368);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 26);
            this.label4.TabIndex = 9;
            this.label4.Text = "Resulting\r\nPublic Key Hex";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 419);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 26);
            this.label5.TabIndex = 10;
            this.label5.Text = "Resulting\r\nPrivate Key";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.linkLabel1);
            this.groupBox1.Controls.Add(this.rdoAdd);
            this.groupBox1.Controls.Add(this.rdoMul);
            this.groupBox1.Location = new System.Drawing.Point(230, 247);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(361, 75);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Combining Method";
            // 
            // rdoAdd
            // 
            this.rdoAdd.AutoSize = true;
            this.rdoAdd.Location = new System.Drawing.Point(15, 43);
            this.rdoAdd.Name = "rdoAdd";
            this.rdoAdd.Size = new System.Drawing.Size(262, 17);
            this.rdoAdd.TabIndex = 1;
            this.rdoAdd.Text = "EC Addition (for use only with Vanity Address Pool)";
            this.rdoAdd.UseVisualStyleBackColor = true;
            // 
            // rdoMul
            // 
            this.rdoMul.AutoSize = true;
            this.rdoMul.Checked = true;
            this.rdoMul.Location = new System.Drawing.Point(15, 20);
            this.rdoMul.Name = "rdoMul";
            this.rdoMul.Size = new System.Drawing.Size(306, 17);
            this.rdoMul.TabIndex = 0;
            this.rdoMul.TabStop = true;
            this.rdoMul.Text = "EC Multiplication (two-factor physical Bitcoins) (most secure)";
            this.rdoMul.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(534, 130);
            this.label6.TabIndex = 12;
            this.label6.Text = resources.GetString("label6.Text");
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(274, 45);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(38, 13);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "(why?)";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // KeyCombiner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 463);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtOutputPubkey);
            this.Controls.Add(this.btnCombine);
            this.Controls.Add(this.txtOutputPriv);
            this.Controls.Add(this.txtOutputAddress);
            this.Controls.Add(this.txtInput2);
            this.Controls.Add(this.txtInput1);
            this.Name = "KeyCombiner";
            this.Text = "Key Combiner";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtInput1;
        private System.Windows.Forms.TextBox txtInput2;
        private System.Windows.Forms.TextBox txtOutputAddress;
        private System.Windows.Forms.TextBox txtOutputPriv;
        private System.Windows.Forms.Button btnCombine;
        private System.Windows.Forms.TextBox txtOutputPubkey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoAdd;
        private System.Windows.Forms.RadioButton rdoMul;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}
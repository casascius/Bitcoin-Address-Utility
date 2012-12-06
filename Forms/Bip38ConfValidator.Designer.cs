namespace BtcAddress.Forms {
    partial class Bip38ConfValidator {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Bip38ConfValidator));
            this.label1 = new System.Windows.Forms.Label();
            this.txtPassphrase = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtConfCode = new System.Windows.Forms.TextBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.lblAddressHeader = new System.Windows.Forms.Label();
            this.lblAddressItself = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(676, 104);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // txtPassphrase
            // 
            this.txtPassphrase.Location = new System.Drawing.Point(115, 125);
            this.txtPassphrase.Name = "txtPassphrase";
            this.txtPassphrase.Size = new System.Drawing.Size(688, 20);
            this.txtPassphrase.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Passphrase";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Confirmation code";
            // 
            // txtConfCode
            // 
            this.txtConfCode.Location = new System.Drawing.Point(115, 151);
            this.txtConfCode.Name = "txtConfCode";
            this.txtConfCode.Size = new System.Drawing.Size(688, 20);
            this.txtConfCode.TabIndex = 3;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(115, 177);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnConfirm.TabIndex = 5;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // lblAddressHeader
            // 
            this.lblAddressHeader.AutoSize = true;
            this.lblAddressHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddressHeader.Location = new System.Drawing.Point(12, 216);
            this.lblAddressHeader.Name = "lblAddressHeader";
            this.lblAddressHeader.Size = new System.Drawing.Size(98, 13);
            this.lblAddressHeader.TabIndex = 6;
            this.lblAddressHeader.Text = "Bitcoin address:";
            this.lblAddressHeader.Visible = false;
            // 
            // lblAddressItself
            // 
            this.lblAddressItself.AutoSize = true;
            this.lblAddressItself.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddressItself.ForeColor = System.Drawing.Color.Green;
            this.lblAddressItself.Location = new System.Drawing.Point(112, 216);
            this.lblAddressItself.Name = "lblAddressItself";
            this.lblAddressItself.Size = new System.Drawing.Size(170, 13);
            this.lblAddressItself.TabIndex = 7;
            this.lblAddressItself.Text = "Address xxxxxxxxxxxxxxxxxxx";
            this.lblAddressItself.Visible = false;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResult.ForeColor = System.Drawing.Color.Green;
            this.lblResult.Location = new System.Drawing.Point(112, 238);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(394, 13);
            this.lblResult.TabIndex = 8;
            this.lblResult.Text = "It is confirmed that this Bitcoin address depends on this passphrase.";
            this.lblResult.Visible = false;
            // 
            // Bip38ConfValidator
            // 
            this.AcceptButton = this.btnConfirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 268);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.lblAddressItself);
            this.Controls.Add(this.lblAddressHeader);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtConfCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPassphrase);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Bip38ConfValidator";
            this.Text = "Confirmation Code Validator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPassphrase;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtConfCode;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Label lblAddressHeader;
        private System.Windows.Forms.Label lblAddressItself;
        private System.Windows.Forms.Label lblResult;
    }
}
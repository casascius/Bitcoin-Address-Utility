namespace BtcAddress {
    partial class Base58Calc {
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
            this.txtHex = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBase58 = new System.Windows.Forms.TextBox();
            this.lblByteCounts = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtHex
            // 
            this.txtHex.Location = new System.Drawing.Point(66, 35);
            this.txtHex.Name = "txtHex";
            this.txtHex.Size = new System.Drawing.Size(585, 20);
            this.txtHex.TabIndex = 0;
            this.txtHex.TextChanged += new System.EventHandler(this.txtHex_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Hex";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Base58";
            // 
            // txtBase58
            // 
            this.txtBase58.Location = new System.Drawing.Point(66, 61);
            this.txtBase58.Name = "txtBase58";
            this.txtBase58.Size = new System.Drawing.Size(585, 20);
            this.txtBase58.TabIndex = 2;
            this.txtBase58.TextChanged += new System.EventHandler(this.txtBase58_TextChanged);
            // 
            // lblByteCounts
            // 
            this.lblByteCounts.AutoSize = true;
            this.lblByteCounts.Location = new System.Drawing.Point(63, 84);
            this.lblByteCounts.Name = "lblByteCounts";
            this.lblByteCounts.Size = new System.Drawing.Size(156, 13);
            this.lblByteCounts.TabIndex = 4;
            this.lblByteCounts.Text = "Byte count: 0  Base58 length: 0";
            // 
            // Base58Calc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 123);
            this.Controls.Add(this.lblByteCounts);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtBase58);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtHex);
            this.Name = "Base58Calc";
            this.Text = "Base58Calc";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtHex;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBase58;
        private System.Windows.Forms.Label lblByteCounts;
    }
}
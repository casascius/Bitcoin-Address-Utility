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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.modeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useChecksumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
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
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(685, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // modeToolStripMenuItem
            // 
            this.modeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.useChecksumToolStripMenuItem});
            this.modeToolStripMenuItem.Name = "modeToolStripMenuItem";
            this.modeToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.modeToolStripMenuItem.Text = "Mode";
            // 
            // useChecksumToolStripMenuItem
            // 
            this.useChecksumToolStripMenuItem.Checked = true;
            this.useChecksumToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useChecksumToolStripMenuItem.Name = "useChecksumToolStripMenuItem";
            this.useChecksumToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.useChecksumToolStripMenuItem.Text = "Use Checksum";
            this.useChecksumToolStripMenuItem.Click += new System.EventHandler(this.useChecksumToolStripMenuItem_Click);
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
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Base58Calc";
            this.Text = "Base 58 Calculator";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtHex;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBase58;
        private System.Windows.Forms.Label lblByteCounts;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem modeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useChecksumToolStripMenuItem;
    }
}
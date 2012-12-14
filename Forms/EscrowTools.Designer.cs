namespace BtcAddress.Forms {
    partial class EscrowTools {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EscrowTools));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabHowItWorks = new System.Windows.Forms.TabPage();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPayeeGeneratedAddress = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.txtPayeeGeneratedInvite = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGenPayee = new System.Windows.Forms.Button();
            this.txtPayeeCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnPayerDone = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPayerAddress = new System.Windows.Forms.TextBox();
            this.lblPayerHereIs = new System.Windows.Forms.Label();
            this.btnPayerPrint = new System.Windows.Forms.Button();
            this.btnPayerSave = new System.Windows.Forms.Button();
            this.txtPayerCode2 = new System.Windows.Forms.TextBox();
            this.txtPayerCode1 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.btnEscrowPrint = new System.Windows.Forms.Button();
            this.btnEscrowSave = new System.Windows.Forms.Button();
            this.txtEscrowForPayee = new System.Windows.Forms.TextBox();
            this.lblEscrowHead = new System.Windows.Forms.Label();
            this.btnGenerateEscrowInvitation = new System.Windows.Forms.Button();
            this.txtEscrowForPayer = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.txtRedeemPrivKey = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtRedeemAddress = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btnRedeem = new System.Windows.Forms.Button();
            this.txtRedeemCode3 = new System.Windows.Forms.TextBox();
            this.txtRedeemCode2 = new System.Windows.Forms.TextBox();
            this.txtRedeemCode1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabHowItWorks.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabHowItWorks);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(731, 428);
            this.tabControl1.TabIndex = 0;
            // 
            // tabHowItWorks
            // 
            this.tabHowItWorks.Controls.Add(this.linkLabel1);
            this.tabHowItWorks.Controls.Add(this.richTextBox1);
            this.tabHowItWorks.Location = new System.Drawing.Point(4, 22);
            this.tabHowItWorks.Name = "tabHowItWorks";
            this.tabHowItWorks.Padding = new System.Windows.Forms.Padding(3);
            this.tabHowItWorks.Size = new System.Drawing.Size(723, 402);
            this.tabHowItWorks.TabIndex = 4;
            this.tabHowItWorks.Text = "How it works";
            this.tabHowItWorks.UseVisualStyleBackColor = true;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(562, 386);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(155, 13);
            this.linkLabel1.TabIndex = 1;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "DISCLAIMER OF WARRANTY";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(7, 7);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(710, 376);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.txtPayeeGeneratedAddress);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.button4);
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.txtPayeeGeneratedInvite);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.btnGenPayee);
            this.tabPage1.Controls.Add(this.txtPayeeCode);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(723, 402);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Be a Payee";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 221);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(688, 169);
            this.label5.TabIndex = 9;
            this.label5.Text = resources.GetString("label5.Text");
            // 
            // txtPayeeGeneratedAddress
            // 
            this.txtPayeeGeneratedAddress.Location = new System.Drawing.Point(19, 198);
            this.txtPayeeGeneratedAddress.Name = "txtPayeeGeneratedAddress";
            this.txtPayeeGeneratedAddress.Size = new System.Drawing.Size(680, 20);
            this.txtPayeeGeneratedAddress.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 182);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(619, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Here is the Bitcoin address that belongs to the transaction.  Give both the Bitco" +
    "in address and the payment invitation to the payee.";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(615, 130);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(84, 23);
            this.button4.TabIndex = 6;
            this.button4.Text = "Print";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(525, 130);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(84, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Save";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // txtPayeeGeneratedInvite
            // 
            this.txtPayeeGeneratedInvite.Location = new System.Drawing.Point(19, 156);
            this.txtPayeeGeneratedInvite.Name = "txtPayeeGeneratedInvite";
            this.txtPayeeGeneratedInvite.Size = new System.Drawing.Size(680, 20);
            this.txtPayeeGeneratedInvite.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(477, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "A new payment invitation has been created.  Before you can see it, you must eithe" +
    "r save it or print it.";
            // 
            // btnGenPayee
            // 
            this.btnGenPayee.Location = new System.Drawing.Point(19, 109);
            this.btnGenPayee.Name = "btnGenPayee";
            this.btnGenPayee.Size = new System.Drawing.Size(214, 23);
            this.btnGenPayee.TabIndex = 2;
            this.btnGenPayee.Text = "Generate Payment Invitation";
            this.btnGenPayee.UseVisualStyleBackColor = true;
            this.btnGenPayee.Click += new System.EventHandler(this.btnGenPayee_Click);
            // 
            // txtPayeeCode
            // 
            this.txtPayeeCode.Location = new System.Drawing.Point(19, 85);
            this.txtPayeeCode.Name = "txtPayeeCode";
            this.txtPayeeCode.Size = new System.Drawing.Size(680, 20);
            this.txtPayeeCode.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(681, 65);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnPayerDone);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.txtPayerAddress);
            this.tabPage2.Controls.Add(this.lblPayerHereIs);
            this.tabPage2.Controls.Add(this.btnPayerPrint);
            this.tabPage2.Controls.Add(this.btnPayerSave);
            this.tabPage2.Controls.Add(this.txtPayerCode2);
            this.tabPage2.Controls.Add(this.txtPayerCode1);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(723, 402);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Be a Payer";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnPayerDone
            // 
            this.btnPayerDone.Location = new System.Drawing.Point(20, 190);
            this.btnPayerDone.Name = "btnPayerDone";
            this.btnPayerDone.Size = new System.Drawing.Size(84, 23);
            this.btnPayerDone.TabIndex = 20;
            this.btnPayerDone.Text = "Done";
            this.btnPayerDone.UseVisualStyleBackColor = true;
            this.btnPayerDone.Click += new System.EventHandler(this.btnPayerDone_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 217);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(669, 156);
            this.label6.TabIndex = 19;
            this.label6.Text = resources.GetString("label6.Text");
            // 
            // txtPayerAddress
            // 
            this.txtPayerAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPayerAddress.Location = new System.Drawing.Point(20, 165);
            this.txtPayerAddress.Name = "txtPayerAddress";
            this.txtPayerAddress.Size = new System.Drawing.Size(680, 20);
            this.txtPayerAddress.TabIndex = 18;
            this.txtPayerAddress.Visible = false;
            // 
            // lblPayerHereIs
            // 
            this.lblPayerHereIs.AutoSize = true;
            this.lblPayerHereIs.Location = new System.Drawing.Point(17, 136);
            this.lblPayerHereIs.Name = "lblPayerHereIs";
            this.lblPayerHereIs.Size = new System.Drawing.Size(658, 26);
            this.lblPayerHereIs.TabIndex = 17;
            this.lblPayerHereIs.Text = resources.GetString("lblPayerHereIs.Text");
            this.lblPayerHereIs.Visible = false;
            // 
            // btnPayerPrint
            // 
            this.btnPayerPrint.Location = new System.Drawing.Point(616, 190);
            this.btnPayerPrint.Name = "btnPayerPrint";
            this.btnPayerPrint.Size = new System.Drawing.Size(84, 23);
            this.btnPayerPrint.TabIndex = 16;
            this.btnPayerPrint.Text = "Print";
            this.btnPayerPrint.UseVisualStyleBackColor = true;
            this.btnPayerPrint.Visible = false;
            // 
            // btnPayerSave
            // 
            this.btnPayerSave.Location = new System.Drawing.Point(526, 190);
            this.btnPayerSave.Name = "btnPayerSave";
            this.btnPayerSave.Size = new System.Drawing.Size(84, 23);
            this.btnPayerSave.TabIndex = 15;
            this.btnPayerSave.Text = "Save";
            this.btnPayerSave.UseVisualStyleBackColor = true;
            this.btnPayerSave.Visible = false;
            // 
            // txtPayerCode2
            // 
            this.txtPayerCode2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPayerCode2.Location = new System.Drawing.Point(20, 113);
            this.txtPayerCode2.Name = "txtPayerCode2";
            this.txtPayerCode2.Size = new System.Drawing.Size(680, 20);
            this.txtPayerCode2.TabIndex = 14;
            // 
            // txtPayerCode1
            // 
            this.txtPayerCode1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPayerCode1.Location = new System.Drawing.Point(20, 84);
            this.txtPayerCode1.Name = "txtPayerCode1";
            this.txtPayerCode1.Size = new System.Drawing.Size(680, 20);
            this.txtPayerCode1.TabIndex = 11;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(640, 78);
            this.label9.TabIndex = 10;
            this.label9.Text = resources.GetString("label9.Text");
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.btnEscrowPrint);
            this.tabPage3.Controls.Add(this.btnEscrowSave);
            this.tabPage3.Controls.Add(this.txtEscrowForPayee);
            this.tabPage3.Controls.Add(this.lblEscrowHead);
            this.tabPage3.Controls.Add(this.btnGenerateEscrowInvitation);
            this.tabPage3.Controls.Add(this.txtEscrowForPayer);
            this.tabPage3.Controls.Add(this.label12);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(723, 402);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Be an Escrow Agent";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 205);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(703, 169);
            this.label8.TabIndex = 19;
            this.label8.Text = resources.GetString("label8.Text");
            // 
            // btnEscrowPrint
            // 
            this.btnEscrowPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEscrowPrint.Location = new System.Drawing.Point(614, 115);
            this.btnEscrowPrint.Name = "btnEscrowPrint";
            this.btnEscrowPrint.Size = new System.Drawing.Size(84, 23);
            this.btnEscrowPrint.TabIndex = 16;
            this.btnEscrowPrint.Text = "Print";
            this.btnEscrowPrint.UseVisualStyleBackColor = true;
            // 
            // btnEscrowSave
            // 
            this.btnEscrowSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEscrowSave.Location = new System.Drawing.Point(524, 115);
            this.btnEscrowSave.Name = "btnEscrowSave";
            this.btnEscrowSave.Size = new System.Drawing.Size(84, 23);
            this.btnEscrowSave.TabIndex = 15;
            this.btnEscrowSave.Text = "Save";
            this.btnEscrowSave.UseVisualStyleBackColor = true;
            this.btnEscrowSave.Visible = false;
            // 
            // txtEscrowForPayee
            // 
            this.txtEscrowForPayee.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEscrowForPayee.Location = new System.Drawing.Point(18, 173);
            this.txtEscrowForPayee.Name = "txtEscrowForPayee";
            this.txtEscrowForPayee.Size = new System.Drawing.Size(680, 20);
            this.txtEscrowForPayee.TabIndex = 14;
            // 
            // lblEscrowHead
            // 
            this.lblEscrowHead.AutoSize = true;
            this.lblEscrowHead.Location = new System.Drawing.Point(15, 125);
            this.lblEscrowHead.Name = "lblEscrowHead";
            this.lblEscrowHead.Size = new System.Drawing.Size(504, 13);
            this.lblEscrowHead.TabIndex = 13;
            this.lblEscrowHead.Text = "A new escrow invitation has been created.  Before you can see or save it, you mus" +
    "t print a paper backup.";
            // 
            // btnGenerateEscrowInvitation
            // 
            this.btnGenerateEscrowInvitation.Location = new System.Drawing.Point(18, 95);
            this.btnGenerateEscrowInvitation.Name = "btnGenerateEscrowInvitation";
            this.btnGenerateEscrowInvitation.Size = new System.Drawing.Size(214, 23);
            this.btnGenerateEscrowInvitation.TabIndex = 12;
            this.btnGenerateEscrowInvitation.Text = "Generate Escrow Invitation";
            this.btnGenerateEscrowInvitation.UseVisualStyleBackColor = true;
            this.btnGenerateEscrowInvitation.Click += new System.EventHandler(this.btnGenerateEscrowInvitation_Click);
            // 
            // txtEscrowForPayer
            // 
            this.txtEscrowForPayer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEscrowForPayer.Location = new System.Drawing.Point(18, 147);
            this.txtEscrowForPayer.Name = "txtEscrowForPayer";
            this.txtEscrowForPayer.Size = new System.Drawing.Size(680, 20);
            this.txtEscrowForPayer.TabIndex = 11;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(15, 14);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(672, 78);
            this.label12.TabIndex = 10;
            this.label12.Text = resources.GetString("label12.Text");
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.txtRedeemPrivKey);
            this.tabPage4.Controls.Add(this.label15);
            this.tabPage4.Controls.Add(this.txtRedeemAddress);
            this.tabPage4.Controls.Add(this.label14);
            this.tabPage4.Controls.Add(this.btnRedeem);
            this.tabPage4.Controls.Add(this.txtRedeemCode3);
            this.tabPage4.Controls.Add(this.txtRedeemCode2);
            this.tabPage4.Controls.Add(this.txtRedeemCode1);
            this.tabPage4.Controls.Add(this.label3);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(723, 402);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Collect your funds";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // txtRedeemPrivKey
            // 
            this.txtRedeemPrivKey.Location = new System.Drawing.Point(21, 283);
            this.txtRedeemPrivKey.Name = "txtRedeemPrivKey";
            this.txtRedeemPrivKey.Size = new System.Drawing.Size(680, 20);
            this.txtRedeemPrivKey.TabIndex = 9;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(18, 267);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(367, 13);
            this.label15.TabIndex = 8;
            this.label15.Text = "The code (private key) needed to collect the funds into your Bitcoin wallet is:";
            // 
            // txtRedeemAddress
            // 
            this.txtRedeemAddress.Location = new System.Drawing.Point(21, 223);
            this.txtRedeemAddress.Name = "txtRedeemAddress";
            this.txtRedeemAddress.Size = new System.Drawing.Size(680, 20);
            this.txtRedeemAddress.TabIndex = 7;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(18, 207);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(114, 13);
            this.label14.TabIndex = 6;
            this.label14.Text = "The Bitcoin address is:";
            // 
            // btnRedeem
            // 
            this.btnRedeem.Location = new System.Drawing.Point(21, 163);
            this.btnRedeem.Name = "btnRedeem";
            this.btnRedeem.Size = new System.Drawing.Size(75, 23);
            this.btnRedeem.TabIndex = 5;
            this.btnRedeem.Text = "Redeem";
            this.btnRedeem.UseVisualStyleBackColor = true;
            this.btnRedeem.Click += new System.EventHandler(this.btnRedeem_Click);
            // 
            // txtRedeemCode3
            // 
            this.txtRedeemCode3.Location = new System.Drawing.Point(21, 137);
            this.txtRedeemCode3.Name = "txtRedeemCode3";
            this.txtRedeemCode3.Size = new System.Drawing.Size(680, 20);
            this.txtRedeemCode3.TabIndex = 4;
            // 
            // txtRedeemCode2
            // 
            this.txtRedeemCode2.Location = new System.Drawing.Point(21, 111);
            this.txtRedeemCode2.Name = "txtRedeemCode2";
            this.txtRedeemCode2.Size = new System.Drawing.Size(680, 20);
            this.txtRedeemCode2.TabIndex = 3;
            // 
            // txtRedeemCode1
            // 
            this.txtRedeemCode1.Location = new System.Drawing.Point(21, 85);
            this.txtRedeemCode1.Name = "txtRedeemCode1";
            this.txtRedeemCode1.Size = new System.Drawing.Size(680, 20);
            this.txtRedeemCode1.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(470, 65);
            this.label3.TabIndex = 0;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // EscrowTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 452);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EscrowTools";
            this.Text = "Escrow Tools";
            this.Load += new System.EventHandler(this.EscrowTools_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabHowItWorks.ResumeLayout(false);
            this.tabHowItWorks.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabHowItWorks;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox txtPayeeCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button btnRedeem;
        private System.Windows.Forms.TextBox txtRedeemCode3;
        private System.Windows.Forms.TextBox txtRedeemCode2;
        private System.Windows.Forms.TextBox txtRedeemCode1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPayeeGeneratedAddress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txtPayeeGeneratedInvite;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnGenPayee;
        private System.Windows.Forms.Button btnPayerDone;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPayerAddress;
        private System.Windows.Forms.Label lblPayerHereIs;
        private System.Windows.Forms.Button btnPayerPrint;
        private System.Windows.Forms.Button btnPayerSave;
        private System.Windows.Forms.TextBox txtPayerCode2;
        private System.Windows.Forms.TextBox txtPayerCode1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnEscrowPrint;
        private System.Windows.Forms.Button btnEscrowSave;
        private System.Windows.Forms.TextBox txtEscrowForPayee;
        private System.Windows.Forms.Label lblEscrowHead;
        private System.Windows.Forms.Button btnGenerateEscrowInvitation;
        private System.Windows.Forms.TextBox txtEscrowForPayer;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtRedeemPrivKey;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtRedeemAddress;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}
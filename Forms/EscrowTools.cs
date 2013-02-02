// Copyright 2012 Mike Caldwell (Casascius)
// This file is part of Bitcoin Address Utility.

// Bitcoin Address Utility is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// Bitcoin Address Utility is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with Bitcoin Address Utility.  If not, see http://www.gnu.org/licenses/.


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Security.Cryptography;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Casascius.Bitcoin;

namespace BtcAddress.Forms {
    public partial class EscrowTools : Form {
        public EscrowTools() {
            InitializeComponent();
        }

        private void EscrowTools_Load(object sender, EventArgs e) {
            string crlf = "\r\n";

            richTextBox1.Rtf = 
@"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Calibri;}}" + crlf +
@"{\*\generator Msftedit 5.41.21.2510;}\viewkind4\uc1\pard\sa200\sl276\slmult1\lang9\b\f0\fs28 How Three-Party Escrow Works\fs22\par" + crlf +
@"Escrow\b0  allows two people to transact in Bitcoin while leaving their funds visible to everybody and accessible to nobody until somebody releases them.  It allows the payer or the payee to release funds to one another, and also lets a third person decide for them if the two can't agree.  The third person never has access to take the funds, and is only needed to release the funds if the original two can't agree who gets them.  Whoever gets a copy of all three ""invitations"" gets access to the funds.\par" + crlf +
@"Let's pretend that Alice wants to pay Bob, and they agree to use Eddie as their escrow agent.\par" + crlf +
@"\b First\b0 , Eddie creates a pair of Escrow Invitation codes.  This is a matched pair of codes representing a single invitation.  These codes can be used by someone else in a future transaction to give Eddie the authority to act as the escrow agent.  He gives one code to Alice and the other to Bob, and keeps a copy for himself.\par" + crlf +
@"\b Second\b0 , Bob creates a Payment Invitation and gives it only to Alice, but keeps a copy for himself.  When Alice and Bob use the escrow tool to combine their individual Escrow Invitation codes with the Payment Invitation, they'll get the same Bitcoin address.  Alice and Bob must agree they have generated the same address.\par" + crlf +
@"\b Third,\b0  Alice sends Bitcoins to that address.  Now, nobody can get them until someone releases them.\par" + crlf +
@"\b Alice\b0  can release the Bitcoins to Bob by giving a copy of her Escrow Invitation code to Bob (so that he now has both halves, as well as his Payment Invitation).  He'll use the ""Collect Your Funds"" tab to enter all three, and will receive the private key needed to claim the funds.  The private key can be imported into a Bitcoin client or web wallet.\par" + crlf +
@"\b Bob\b0  can give a refund to Alice by giving her a copy of his Escrow Invitation code.\par" + crlf +
@"\b Eddie\b0  can also force the payment to be awarded to Alice or Bob by giving them both Escrow Invitation codes.  Eddie can't claim the payment himself because he would also need the Payment Invitation, which he doesn't have.\par" + crlf +
@"}";
 

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            MessageBox.Show(
"BECAUSE THIS SOFTWARE IS LICENSED FREE OF CHARGE, THERE IS NO WARRANTY FOR THE SOFTWARE, TO THE EXTENT PERMITTED BY APPLICABLE LAW. EXCEPT WHEN OTHERWISE STATED IN WRITING THE COPYRIGHT HOLDERS AND/OR OTHER PARTIES PROVIDE THE SOFTWARE \"AS IS\" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. THE ENTIRE RISK AS TO THE QUALITY AND PERFORMANCE OF THE SOFTWARE IS WITH YOU. SHOULD THE SOFTWARE PROVE DEFECTIVE, YOU ASSUME THE COST OF ALL NECESSARY SERVICING, REPAIR, OR CORRECTION.\r\n\r\n" +
"IN NO EVENT UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING WILL ANY COPYRIGHT HOLDER, OR ANY OTHER PARTY WHO MAY MODIFY AND/OR REDISTRIBUTE THE SOFTWARE AS PERMITTED BY THE ABOVE LICENCE, BE LIABLE TO YOU FOR DAMAGES, INCLUDING ANY GENERAL, SPECIAL, INCIDENTAL, OR CONSEQUENTIAL DAMAGES ARISING OUT OF THE USE OR INABILITY TO USE THE SOFTWARE (INCLUDING BUT NOT LIMITED TO LOSS OF DATA OR DATA BEING RENDERED INACCURATE OR LOSSES SUSTAINED BY YOU OR THIRD PARTIES OR A FAILURE OF THE SOFTWARE TO OPERATE WITH ANY OTHER SOFTWARE), EVEN IF SUCH HOLDER OR OTHER PARTY HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGES.",
            "Disclaimer of Warranty",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);




        }

        private void btnGenerateEscrowInvitation_Click(object sender, EventArgs e) {

            EscrowCodeSet cs = new EscrowCodeSet();

            txtEscrowForPayer.Text = cs.EscrowInvitationCodeA;
            txtEscrowForPayee.Text = cs.EscrowInvitationCodeB;
        }

        private void btnGenPayee_Click(object sender, EventArgs e) {


            try {
                txtPayeeCode.Text = Util.Base58Trim(txtPayeeCode.Text);
                EscrowCodeSet cs = new EscrowCodeSet(txtPayeeCode.Text);
                txtPayeeGeneratedInvite.Text = cs.PaymentInvitationCode;
                txtPayeeGeneratedAddress.Text = cs.BitcoinAddress;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }

        }

        private void setPayerElementsVisible(bool visible) {
            lblPayerHereIs.Visible = visible;
            txtPayerAddress.Visible = visible;
            btnPayerSave.Visible = visible;
            btnPayerPrint.Visible = visible;


        }


        private void btnPayerDone_Click(object sender, EventArgs e) {
            if (btnPayerDone.Text == "Reset") {
                setPayerElementsVisible(false);
                btnPayerDone.Text = "Done";
                return;
            }


            try {
                txtPayerCode1.Text = Util.Base58Trim(txtPayerCode1.Text);
                txtPayerCode2.Text = Util.Base58Trim(txtPayerCode2.Text);
                EscrowCodeSet cs = new EscrowCodeSet(txtPayerCode1.Text, txtPayerCode2.Text);
                txtPayerAddress.Text = cs.BitcoinAddress;
                setPayerElementsVisible(true);
                btnPayerDone.Text = "Reset";
                if (cs.SamePartyWarningApplies) {
                    MessageBox.Show("The Payment Invitation Code appears to have been generated from the same Escrow Invitation Code you entered, " +
                        "and not its mate.  You might be verifying a Payment Invitation you produced yourself, rather than one " +
                        "produced by your trading partner.", "Are you verifying the wrong thing?", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }




        }

        private void btnRedeem_Click(object sender, EventArgs e) {
            try {
                txtRedeemCode1.Text = Util.Base58Trim(txtRedeemCode1.Text);
                txtRedeemCode2.Text = Util.Base58Trim(txtRedeemCode2.Text);
                txtRedeemCode3.Text = Util.Base58Trim(txtRedeemCode3.Text);

                EscrowCodeSet cs = new EscrowCodeSet(txtRedeemCode1.Text, txtRedeemCode2.Text, txtRedeemCode3.Text);
                txtRedeemAddress.Text = cs.BitcoinAddress;
                txtRedeemPrivKey.Text = cs.PrivateKey;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }

        }
    }
}

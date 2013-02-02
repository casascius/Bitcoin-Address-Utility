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
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Casascius.Bitcoin;

namespace BtcAddress {
    public partial class KeyCombiner : Form {
        public KeyCombiner() {
            InitializeComponent();
        }

        private void btnCombine_Click(object sender, EventArgs e) {
            // What is input #1?

            string input1 = txtInput1.Text;
            string input2 = txtInput2.Text;
            PublicKey pub1 = null, pub2 = null;
            KeyPair kp1 = null, kp2 = null;


            if (KeyPair.IsValidPrivateKey(input1)) {
                pub1 = kp1 = new KeyPair(input1);
            } else if (PublicKey.IsValidPublicKey(input1)) {
                pub1 = new PublicKey(input1);
            } else {
                MessageBox.Show("Input key #1 is not a valid Public Key or Private Key Hex", "Can't combine", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (KeyPair.IsValidPrivateKey(input2)) {
                pub2 = kp2 = new KeyPair(input2);
            } else if (PublicKey.IsValidPublicKey(input2)) {
                pub2 = new PublicKey(input2);
            } else {
                MessageBox.Show("Input key #2 is not a valid Public Key or Private Key Hex", "Can't combine", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (kp1 == null && kp2 == null && rdoAdd.Checked == false) {
                MessageBox.Show("Can't multiply two public keys.  At least one of the keys must be a private key.", 
                    "Can't combine", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (pub1.IsCompressedPoint != pub2.IsCompressedPoint) {
                MessageBox.Show("Can't combine a compressed key with an uncompressed key.", "Can't combine", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (pub1.AddressBase58 == pub2.AddressBase58) {
                if (MessageBox.Show("Both of the key inputs have the same public key hash.  You can continue, but " +
                   "the results are probably going to be wrong.  You might have provided the wrong " +
                   "information, such as two parts from the same side of the transaction, instead " +
                    "of one part from each side.  Continue anyway?", "Duplicate Key Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) {
                    return;
                }

            }

            var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");

            // Combining two private keys?
            if (kp1 != null && kp2 != null) {

                BigInteger e1 = new BigInteger(1, kp1.PrivateKeyBytes);
                BigInteger e2 = new BigInteger(1, kp2.PrivateKeyBytes);
                BigInteger ecombined = (rdoAdd.Checked ? e1.Add(e2) : e1.Multiply(e2)).Mod(ps.N);
                

                System.Diagnostics.Debug.WriteLine(kp1.PublicKeyHex);
                System.Diagnostics.Debug.WriteLine(kp2.PublicKeyHex);
                KeyPair kpcombined = new KeyPair(Util.Force32Bytes(ecombined.ToByteArrayUnsigned()), compressed: kp1.IsCompressedPoint);

                txtOutputAddress.Text = kpcombined.AddressBase58;
                txtOutputPubkey.Text = kpcombined.PublicKeyHex.Replace(" ", "");
                txtOutputPriv.Text = kpcombined.PrivateKeyBase58;


            } else if (kp1 != null || kp2 != null) {
                // Combining one public and one private

                KeyPair priv = (kp1 == null) ? kp2 : kp1;
                PublicKey pub = (kp1 == null) ? pub1 : pub2;

                ECPoint point = pub.GetECPoint();
                
                ECPoint combined = rdoAdd.Checked ? point.Add(priv.GetECPoint()) : point.Multiply(new BigInteger(1, priv.PrivateKeyBytes));
                ECPoint combinedc = ps.Curve.CreatePoint(combined.X.ToBigInteger(), combined.Y.ToBigInteger(), priv.IsCompressedPoint);
                PublicKey pkcombined = new PublicKey(combinedc.GetEncoded());
                txtOutputAddress.Text = pkcombined.AddressBase58;
                txtOutputPubkey.Text = pkcombined.PublicKeyHex.Replace(" ", "");
                txtOutputPriv.Text = "Only available when combining two private keys";
            } else {
                // Adding two public keys
                ECPoint combined = pub1.GetECPoint().Add(pub2.GetECPoint());
                ECPoint combinedc = ps.Curve.CreatePoint(combined.X.ToBigInteger(), combined.Y.ToBigInteger(), pub1.IsCompressedPoint);
                PublicKey pkcombined = new PublicKey(combinedc.GetEncoded());
                txtOutputAddress.Text = pkcombined.AddressBase58;
                txtOutputPubkey.Text = pkcombined.PublicKeyHex.Replace(" ", "");
                txtOutputPriv.Text = "Only available when combining two private keys";
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            MessageBox.Show("EC Addition should not be used for two-factor storage.  " +
                "Use multiplication instead. " +
                "Addition is safe when employing a vanity pool to generate vanity addresses, " +
                "and is required for vanity address generators to achieve GPU-accelerated performance.  " +
                "For some other uses, addition is unsafe due to its reversibility, " +
                "so always use multiplication instead wherever possible.");
        }
    }
}

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
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math;
using CryptSharp.Utility;
using Casascius.Bitcoin;

namespace BtcAddress.Forms {
    public partial class Bip38ConfValidator : Form {
        public Bip38ConfValidator() {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e) {
            lblAddressHeader.Visible = false;
            lblAddressItself.Visible = false;
            lblResult.Visible = false;
            

            // check for null entry
            if (txtPassphrase.Text == "") {
                MessageBox.Show("Passphrase is required.", "Passphrase required", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtConfCode.Text == "") {
                MessageBox.Show("Confirmation code is required.", "Confirmation code required", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Parse confirmation code.
            byte[] confbytes = Util.Base58CheckToByteArray(txtConfCode.Text.Trim());
            if (confbytes == null) {
                // is it even close?
                if (txtConfCode.Text.StartsWith("cfrm38")) {
                    MessageBox.Show("This is not a valid confirmation code.  It has the right prefix, but " +
                        "doesn't contain valid confirmation data.  Possible typo or incomplete?", 
                        "Invalid confirmation code", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                MessageBox.Show("This is not a valid confirmation code.", "Invalid confirmation code", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (confbytes.Length != 51 || confbytes[0] != 0x64 || confbytes[1] != 0x3B || confbytes[2] != 0xF6 ||
                confbytes[3] != 0xA8 || confbytes[4] != 0x9A || confbytes[18] < 0x02 || confbytes[18] > 0x03) {

                // Unrecognized Base58 object.  Do we know what this is?  Tell the user.
                object result = StringInterpreter.Interpret(txtConfCode.Text.Trim());
                if (result != null) {

                    // did we actually get an encrypted private key?  if so, just try to decrypt it.
                    if (result is PassphraseKeyPair) {
                        PassphraseKeyPair ppkp = result as PassphraseKeyPair;
                        if (ppkp.DecryptWithPassphrase(txtPassphrase.Text)) {
                            confirmIsValid(ppkp.GetAddress().AddressBase58);
                            MessageBox.Show("What you provided contains a private key, not just a confirmation. " +
                                "Confirmation is successful, and with this correct passphrase, " +
                                "you are also able to spend the funds from the address.", "This is actually a private key",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        } else {
                            MessageBox.Show("This is not a valid confirmation code.  It looks like an " +
                                "encrypted private key.  Decryption was attempted but the passphrase couldn't decrypt it", "Invalid confirmation code", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }

                    string objectKind = result.GetType().Name;
                    if (objectKind == "AddressBase") {
                        objectKind = "an Address";
                    } else {
                        objectKind = "a " + objectKind;
                    }

                    MessageBox.Show("This is not a valid confirmation code.  Instead, it looks like " + objectKind +
                      ".  Perhaps you entered the wrong thing?  Confirmation codes " +
                    "start with \"cfrm\".", "Invalid confirmation code", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                MessageBox.Show("This is not a valid confirmation code.", "Invalid confirmation code", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
                
            }

            // extract ownersalt and get an intermediate
            byte[] ownersalt = new byte[8];
            Array.Copy(confbytes, 10, ownersalt, 0, 8);

            bool includeHashStep = (confbytes[5] & 0x04) == 0x04;
            Bip38Intermediate intermediate = new Bip38Intermediate(txtPassphrase.Text, ownersalt, includeHashStep);

            // derive the 64 bytes we need
            // get ECPoint from passpoint            
            PublicKey pk = new PublicKey(intermediate.passpoint);

            byte[] addresshashplusownersalt = new byte[12];
            Array.Copy(confbytes, 6, addresshashplusownersalt, 0, 4);
            Array.Copy(intermediate.ownerentropy, 0, addresshashplusownersalt, 4, 8);

            // derive encryption key material
            byte[] derived = new byte[64];
            SCrypt.ComputeKey(intermediate.passpoint, addresshashplusownersalt, 1024, 1, 1, 1, derived);

            byte[] derivedhalf2 = new byte[32];
            Array.Copy(derived, 32, derivedhalf2, 0, 32);

            byte[] unencryptedpubkey = new byte[33];
            // recover the 0x02 or 0x03 prefix
            unencryptedpubkey[0] = (byte)(confbytes[18] ^ (derived[63] & 0x01));

            // decrypt
            var aes = Aes.Create();
            aes.KeySize = 256;
            aes.Mode = CipherMode.ECB;
            aes.Key = derivedhalf2;
            ICryptoTransform decryptor = aes.CreateDecryptor();

            decryptor.TransformBlock(confbytes, 19, 16, unencryptedpubkey, 1);
            decryptor.TransformBlock(confbytes, 19, 16, unencryptedpubkey, 1);
            decryptor.TransformBlock(confbytes, 19 + 16, 16, unencryptedpubkey, 17);
            decryptor.TransformBlock(confbytes, 19 + 16, 16, unencryptedpubkey, 17);

            // xor out the padding
            for (int i = 0; i < 32; i++) unencryptedpubkey[i + 1] ^= derived[i];

            // reconstitute the ECPoint
            var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");
            ECPoint point;
            try {
                point = ps.Curve.DecodePoint(unencryptedpubkey);

                // multiply passfactor.  Result is going to be compressed.
                ECPoint pubpoint = point.Multiply(new BigInteger(1, intermediate.passfactor));

                // Do we want it uncompressed?  then we will have to uncompress it.
                byte flagbyte = confbytes[5];
                if ((flagbyte & 0x20) == 0x00) {
                    pubpoint = ps.Curve.CreatePoint(pubpoint.X.ToBigInteger(), pubpoint.Y.ToBigInteger(), false);
                }

                // Convert to bitcoin address and check address hash.
                PublicKey generatedaddress = new PublicKey(pubpoint);

                // get addresshash
                UTF8Encoding utf8 = new UTF8Encoding(false);
                Sha256Digest sha256 = new Sha256Digest();
                byte[] generatedaddressbytes = utf8.GetBytes(generatedaddress.AddressBase58);
                sha256.BlockUpdate(generatedaddressbytes, 0, generatedaddressbytes.Length);
                byte[] addresshashfull = new byte[32];
                sha256.DoFinal(addresshashfull, 0);
                sha256.BlockUpdate(addresshashfull, 0, 32);
                sha256.DoFinal(addresshashfull, 0);

                for (int i = 0; i < 4; i++) {
                    if (addresshashfull[i] != confbytes[i + 6]) {
                        MessageBox.Show("This passphrase is wrong or does not belong to this confirmation code.", "Invalid passphrase", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }

                confirmIsValid(generatedaddress.AddressBase58);
            } catch {
                // Might throw an exception - not every 256-bit integer is a valid X coordinate
                MessageBox.Show("This passphrase is wrong or does not belong to this confirmation code.", "Invalid passphrase", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

        }

        private void confirmIsValid(string address) {
            lblAddressHeader.Visible = true;
            lblAddressItself.Text = address;
            lblAddressItself.Visible = true;
            lblResult.Visible = true;

        }

    }
}

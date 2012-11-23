using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using CryptSharp.Utility;
using Org.BouncyCastle.Math;


namespace BtcAddress {
    public partial class PpecKeygen : Form {
        public PpecKeygen() {
            InitializeComponent();
        }

        private byte[] magic = new byte[] { 0x2C, 0xE9, 0xB3, 0xE1, 0xFF, 0x39, 0xE2, 0x53 };

        private void btnEncode_Click(object sender, EventArgs e) {
            if ((txtPassphrase.Text ?? "") == "") {
                MessageBox.Show("Enter a passphrase first.");
                return;
            }

            byte[] ownersalt = new byte[8];

            // Get 8 random bytes to use as salt
            SecureRandom sr = new SecureRandom();            
            sr.NextBytes(ownersalt);

            UTF8Encoding utf8 = new UTF8Encoding(false);
            byte[] passfactor = new byte[32];
            SCrypt.ComputeKey(utf8.GetBytes(txtPassphrase.Text), ownersalt, 16384, 8, 8, 8, passfactor);

            // make a compressed key out of it just by using the existing bitcoin address classes
            KeyPair kp = new KeyPair(passfactor, compressed: true);

            byte[] passpoint = kp.PublicKeyBytes;

            byte[] result = new byte[49];

            // 8 bytes are a constant, responsible for making the result start with the characters "passphrase"
            Array.Copy(magic, 0, result, 0, 8);
            Array.Copy(ownersalt, 0, result, 8, 8);
            Array.Copy(passpoint, 0, result, 16, 33);
            txtPassphraseCode.Text = Bitcoin.ByteArrayToBase58Check(result);

        }

        private void btnGenerateKey_Click(object sender, EventArgs e) {
            // get passphrase code
            byte[] ppcode = Bitcoin.Base58CheckToByteArray(txtPassphraseCode.Text);
            if (ppcode == null) {
                MessageBox.Show("Passphrase code is not valid.");
                return;
            }
            
            // check length
            if (ppcode.Length != 49) {
                MessageBox.Show("This is not a passphrase code.");
                return;
            }

            // check magic
            for (int i = 0; i < 8; i++) {
                if (magic[i] != ppcode[i]) {
                    MessageBox.Show("This is not a passphrase code.");
                    return;
                }
            }

            // get ownersalt and passpoint
            byte[] ownersalt = new byte[8];
            byte[] passpoint = new byte[33];
            Array.Copy(ppcode, 8, ownersalt, 0, 8);
            Array.Copy(ppcode, 16, passpoint, 0, 33);

            // generate seedb
            byte[] seedb = new byte[24];
            SecureRandom sr = new SecureRandom();
            sr.NextBytes(seedb);

            // get factorb as sha256(sha256(seedb))
            Sha256Digest sha256 = new Sha256Digest();
            sha256.BlockUpdate(seedb, 0, 24);
            byte[] factorb = new byte[32];
            sha256.DoFinal(factorb, 0);
            sha256.BlockUpdate(factorb, 0, 32);
            sha256.DoFinal(factorb, 0);

            // get ECPoint from passpoint            
            PublicKey pk;
            try {
                pk = new PublicKey(passpoint);
            } catch (ArgumentException ae) {
                MessageBox.Show("Passphrase code is not valid: " + ae.Message);
                return;
            }

            ECPoint generatedpoint = pk.GetECPoint().Multiply(new BigInteger(1, factorb));
            byte[] generatedpointbytes = generatedpoint.GetEncoded();
            PublicKey generatedaddress = new PublicKey(generatedpointbytes);

            // get addresshash
            UTF8Encoding utf8 = new UTF8Encoding(false);
            byte[] generatedaddressbytes = utf8.GetBytes(generatedaddress.AddressBase58);
            sha256.BlockUpdate(generatedaddressbytes, 0, generatedaddressbytes.Length);
            byte[] addresshashfull = new byte[32];
            sha256.DoFinal(addresshashfull, 0);
            sha256.BlockUpdate(addresshashfull, 0, 32);
            sha256.DoFinal(addresshashfull, 0);

            byte[] addresshashplusownersalt = new byte[12];
            Array.Copy(addresshashfull, 0, addresshashplusownersalt, 0, 4);
            Array.Copy(ownersalt, 0, addresshashplusownersalt, 4, 8);

            // derive encryption key material
            byte[] derived = new byte[64];
            SCrypt.ComputeKey(passpoint, addresshashplusownersalt, 1024, 1, 1, 1, derived);

            byte[] derivedhalf2 = new byte[32];
            Array.Copy(derived, 32, derivedhalf2, 0, 32);

            byte[] unencryptedpart1 = new byte[16];
            for (int i = 0; i < 16; i++) {
                unencryptedpart1[i] = (byte)(seedb[i] ^ derived[i]);
            }
            byte[] encryptedpart1 = new byte[16];

            // encrypt it
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.KeySize = 256;
            aes.Mode = CipherMode.ECB;
            aes.Key = derivedhalf2;
            ICryptoTransform encryptor = aes.CreateEncryptor();

            encryptor.TransformBlock(unencryptedpart1, 0, 16, encryptedpart1, 0);
            encryptor.TransformBlock(unencryptedpart1, 0, 16, encryptedpart1, 0);

            byte[] unencryptedpart2 = new byte[16];
            for (int i = 0; i < 8; i++) {
                unencryptedpart2[i] = (byte)(encryptedpart1[i+8] ^ derived[i+16]);
            }
            for (int i = 0; i < 8; i++) {
                unencryptedpart2[i+8] = (byte)(seedb[i + 16] ^ derived[i + 24]);
            }

            byte[] encryptedpart2 = new byte[16];
            encryptor.TransformBlock(unencryptedpart2, 0, 16, encryptedpart2, 0);
            encryptor.TransformBlock(unencryptedpart2, 0, 16, encryptedpart2, 0);

            byte[] result = new byte[39];
            result[0] = 0x01;
            result[1] = 0x43;
            result[2] = generatedaddress.IsCompressedPoint ? (byte)0x20 : (byte)0x00;
            Array.Copy(addresshashfull, 0, result, 3, 4);
            Array.Copy(ownersalt, 0, result, 7, 8);
            Array.Copy(encryptedpart1, 0, result, 15, 8);
            Array.Copy(encryptedpart2, 0, result, 23, 16);

            txtEncryptedKey.Text = Bitcoin.ByteArrayToBase58Check(result);
            txtBitcoinAddress.Text = generatedaddress.AddressBase58;
        }



    }
}

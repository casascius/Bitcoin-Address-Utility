using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;

namespace BtcAddress {

    /// <summary>
    /// Represents a printable report producing the materials to go in a two-factor physical Bitcoin piece.
    /// </summary>
    class CoinInsert : System.Drawing.Printing.PrintDocument {

        private static bool UbuntuFontLoaded = false;

        private Font fontbig = new Font("Courier New", 13);
        private Font font = new Font("Courier New", 10);
        private Font fontsmall = new Font("Courier New", 4.5F);

        private Font ubuntufont = null;
        private Font ubuntumid = null;
        private Font ubuntubig = null;

        public List<KeyCollectionItem> keys;


        protected override void OnBeginPrint(System.Drawing.Printing.PrintEventArgs e) {
            base.OnBeginPrint(e);
        }


        protected override void OnPrintPage(System.Drawing.Printing.PrintPageEventArgs e) {
            base.OnPrintPage(e);
            int printHeight;
            int printWidth;
            int leftMargin;
            int rightMargin;
            Int32 lines;
            Int32 chars;

            //Set print area size and margins
            {
                printHeight = base.DefaultPageSettings.PaperSize.Height - base.DefaultPageSettings.Margins.Top - base.DefaultPageSettings.Margins.Bottom;
                printWidth = base.DefaultPageSettings.PaperSize.Width - base.DefaultPageSettings.Margins.Left - base.DefaultPageSettings.Margins.Right;
                leftMargin = base.DefaultPageSettings.Margins.Left;
                //X
                rightMargin = base.DefaultPageSettings.Margins.Top;
                //Y
            }


            for (int i = 0; i < 8; i++) {
                int eachheight = 120;
                if (keys.Count == 0) break;

                KeyCollectionItem kci = keys[0];
                string address = kci.GetAddressBase58();

                string privkey = kci.PrivateKey;
                string confcode = "";
                if (kci.EncryptedKeyPair != null && kci.EncryptedKeyPair is Bip38KeyPair) {
                    confcode = ((Bip38KeyPair)kci.EncryptedKeyPair).GetConfirmationCode() ?? "";
                    //if (confcode != "") confcode = "Confirmation code:\r\n" + confcode;
                }

                keys.RemoveAt(0);

                int thiscodeX = 0; //  50;
                int thiscodeY = 50 + eachheight * i;

                // ----------------------------------------------------------------
                // Coin insert with public and private QR codes.  Fits 8 to a page.
                // ----------------------------------------------------------------


                float CircleDiameterInches = (7F / 16F); // 7/16"

                // draw the private key circle
                using (Pen blackpen = new Pen(Color.Black)) {
                    blackpen.Width = (1F / 72F);

                    e.Graphics.DrawEllipse(blackpen, thiscodeX + 30F, thiscodeY + 10F, CircleDiameterInches * 100F, CircleDiameterInches * 100F);

                    // Over 30 characters? do a folding insert at 95% diameter away
                    if (privkey.Length > 30) {
                        e.Graphics.DrawEllipse(blackpen, thiscodeX + 30F, thiscodeY + 10F + (CircleDiameterInches * 95F), CircleDiameterInches * 100F, CircleDiameterInches * 100F);
                        e.Graphics.FillEllipse(Brushes.White, thiscodeX + 30F, thiscodeY + 10F + (CircleDiameterInches * 95F), CircleDiameterInches * 100F, CircleDiameterInches * 100F);
                    }
                    e.Graphics.FillEllipse(Brushes.White, thiscodeX + 30F, thiscodeY + 10F, CircleDiameterInches * 100F, CircleDiameterInches * 100F);
                }



                int[] charsPerLine = new int[] { 4, 7, 8, 7, 4, 0, 4, 7, 8, 7, 4 };
                string privkeyleft = privkey;
                // if it's going to take two circles, add hyphens
                if (privkeyleft.Length > 30) privkeyleft = privkeyleft.Substring(0, 29) + "--" + privkeyleft.Substring(29);
                string privkeytoprint = "";
                for (int c = 0; c < 11; c++) {
                    if (charsPerLine[c] == 0) {
                        privkeytoprint += "\r\n";
                    } else {
                        if (privkeyleft.Length > charsPerLine[c]) {
                            privkeytoprint += privkeyleft.Substring(0, charsPerLine[c]) + "\r\n";
                            privkeyleft = privkeyleft.Substring(charsPerLine[c]);
                        } else {
                            privkeytoprint += privkeyleft + "\r\n";
                            privkeyleft = "";
                        }
                    }
                }
                using (StringFormat sfcenter = new StringFormat()) {
                    sfcenter.Alignment = StringAlignment.Center;
                    e.Graphics.DrawString(privkeytoprint, fontsmall, Brushes.Black, thiscodeX + 30F + (CircleDiameterInches * 100F / 2F), thiscodeY + 14F, sfcenter);
                }
                




                // draw the address QR code
                using (Bitmap b2 = Bitcoin.EncodeQRCode(address)) {
                    e.Graphics.DrawImage(b2, thiscodeX + 100, thiscodeY, 100, 100);
                }
            
                e.Graphics.DrawString("Bitcoin address:\r\n" + address, font, Brushes.Black, thiscodeX + 210, thiscodeY);

                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Far; // right justify

                if (confcode != "") {
                    // Print the confirmation QR code
                    using (Bitmap b = Bitcoin.EncodeQRCode(confcode)) {
                        e.Graphics.DrawImage(b, thiscodeX + 600, thiscodeY, 100, 100);

                        string whattoprint = "Confirmation code:\r\n" + confcode.Substring(0, 38) + "\r\n" + confcode.Substring(38);

                        e.Graphics.DrawString(whattoprint, font, Brushes.Black, thiscodeX + 597, thiscodeY + 55, sf);
                    }
                }


            }
            if (keys.Count != 0) {
                e.HasMorePages = true;
            }
        }






    }
}
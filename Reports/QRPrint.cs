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
using System.Linq;
using System.Text;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;
using Casascius.Bitcoin;

namespace BtcAddress {
    class QRPrint : System.Drawing.Printing.PrintDocument {

        private static bool UbuntuFontLoaded = false;

        private Font fontbig = new Font("Arial", 13);
        private Font font = new Font("Arial", 10);
        private Font fontsmall = new Font("Arial", 6);

        private Font ubuntufont = null;
        private Font ubuntumid = null;
        private Font ubuntubig = null;

        public List<KeyCollectionItem> keys;

        public PrintModes PrintMode = PrintModes.PubPrivQR;

        public string Denomination = "";

        public string ImageFilename = "note-yellow.png";

        public int NotesPerPage = 3;

        public bool PrintMiniKeysWith1DBarcode = false;

        public bool PreferUnencryptedPrivateKeys = false;

        private Image BitcoinNote = null;

        public enum PrintModes {
            /// <summary>
            /// Paper wallet with only private key QR code.  Fits 16 to a page.
            /// </summary>
            PrivQR,

            /// <summary>
            /// Paper wallet with public and private QR codes.  Fits 8 to a page.
            /// </summary>
            PubPrivQR,

            /// <summary>
            /// Banknote image loaded from PNG file.  Fits 3 to a page.
            /// </summary>
            PsyBanknote
        }

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


            for (int i = 0; i < 16; i++) {

                int eachheight=120;

                switch (PrintMode) {

                    case PrintModes.PubPrivQR:
                        if (i >= 8) i = 999;
                        eachheight = 120;
                        break;

                    case PrintModes.PsyBanknote:
                        if (i >= 3) i = 999;
                        eachheight = 365;
                        break;
                }

                if (i == 999) break;




                if (PrintMode == PrintModes.PubPrivQR && i >= 8) break;

                if (PrintMode == PrintModes.PsyBanknote && i >= NotesPerPage) break;

                if (keys.Count == 0) break;

                int thiscodeX = 50;
                int thiscodeY = 50 + eachheight * i;
                if (i >= 8) {
                    thiscodeX = 450;
                    thiscodeY = 50 + eachheight * (i - 8);
                }

                //    T-------------------------------|
                //    |                               |
                //    |                               |
                //    |                               |
                //    |                               |
                //    |                               |
                //    |-------------------------------|
                //
                //    T = thiscodeX,thiscodeY
                //


                // Load the Ubuntu font directly from a file so it doesn't need to be installed on the system.
                if (UbuntuFontLoaded == false) {
                    UbuntuFontLoaded = true;
                    try {
                        System.Drawing.Text.PrivateFontCollection pfc = new System.Drawing.Text.PrivateFontCollection();
                        pfc.AddFontFile("Ubuntu-R.ttf");
                    } catch { }
                }

                ubuntufont = new Font("Ubuntu", 6);
                ubuntumid = new Font("Ubuntu", 9);
                ubuntubig = new Font("Ubuntu", 17);


                KeyCollectionItem k = (KeyCollectionItem)keys[0];
                keys.RemoveAt(0);

                string privkey = k.PrivateKey;
                if (PreferUnencryptedPrivateKeys) {
                    if (k.EncryptedKeyPair != null && k.EncryptedKeyPair.IsUnencryptedPrivateKeyAvailable()) {
                        privkey = k.EncryptedKeyPair.GetUnencryptedPrivateKey().PrivateKey;
                    }
                }

                Bitmap b = QR.EncodeQRCode(privkey);

                if (PrintMode == PrintModes.PsyBanknote) {

                    if (BitcoinNote == null) {
                        BitcoinNote = Image.FromFile(ImageFilename);

                    }

                    float desiredScale = 550F;

                    float scalefactor = (desiredScale / 650.0F);
                    float leftOffset = (float)printWidth - desiredScale;
                    

                    // draw the note
                    e.Graphics.DrawImage(BitcoinNote, 
                                        leftOffset + scalefactor * (float)thiscodeX, 
                                        scalefactor * (float)thiscodeY, (float)650F * scalefactor, 
                                        (float)650F * scalefactor * (float)BitcoinNote.Height / (float)BitcoinNote.Width);

                    // draw the private QR
                    e.Graphics.DrawImage(b, leftOffset + scalefactor * (float)(thiscodeX + 472), 
                        scalefactor * (float)(thiscodeY + 140), 
                        scalefactor * 145F, 
                        scalefactor * 147F);
                    
                    // draw the public QR
                    Bitmap b2 = QR.EncodeQRCode(k.GetAddressBase58());
                    e.Graphics.DrawImage(b2,
                        leftOffset + scalefactor * (float)(thiscodeX + 39),
                        scalefactor * (float)(thiscodeY + 90), scalefactor * 128F, scalefactor * 128F);

                    // write bitcoin address
                    StringFormat sf = new StringFormat();
                    //sf.FormatFlags |= StringFormatFlags.DirectionVertical | StringFormatFlags.DirectionRightToLeft;

                    e.Graphics.RotateTransform(-90F);
                    e.Graphics.DrawString("Bitcoin Address\r\n" + k.GetAddressBase58(), ubuntumid, Brushes.Black,
                        -scalefactor * (float)(thiscodeY + 338),
                        leftOffset + scalefactor * (float)(thiscodeX + 170), 

                        sf);

                    // write private key
                    string whattoprint;
                    if (privkey.Length > 30) {
                        whattoprint = privkey.Substring(0, 25) + "\r\n" + privkey.Substring(25);
                    } else {
                        whattoprint = "\r\n" + privkey;
                    }
                    float xpos =  444;
                    if (privkey.StartsWith("6")) {
                        whattoprint = "Password Required\r\n" + whattoprint;
                        xpos -=  10;
                    }

                    e.Graphics.DrawString(whattoprint, ubuntufont, Brushes.Black,
                        -scalefactor * (float)(thiscodeY + 290),
                        leftOffset + scalefactor * (float)(thiscodeX + xpos),
                        sf);


                    e.Graphics.RotateTransform(90F);

                    // write denomination, if any
                    if ((Denomination ?? "") != "") {
                        e.Graphics.DrawString(Denomination, ubuntubig, Brushes.Black,
                            leftOffset + scalefactor * (float)(thiscodeX + 330),
                            scalefactor * (float)(thiscodeY + 310)
                            
                            );
                    }

                    if (PrintMiniKeysWith1DBarcode && k.Address is MiniKeyPair) {
                        Bitmap barcode1d = Barcode128b.GetBarcode(k.PrivateKey);
                        float aspect1d = (float)barcode1d.Width / (float)barcode1d.Height;
                        e.Graphics.DrawImage(barcode1d, leftOffset + scalefactor * (float)(thiscodeX + 231F),
                            scalefactor * (float)(thiscodeY + 293),
                            scalefactor * 420F,
                            scalefactor * 50F);

                    }

                } else if (PrintMode == PrintModes.PrivQR) {

                    // ----------------------------------------------------------------
                    // Paper wallet with only private key QR code.  Fits 16 to a page.
                    // ----------------------------------------------------------------
                    e.Graphics.DrawImage(b, thiscodeX, thiscodeY, 100, 100);

                    e.Graphics.DrawString("Bitcoin address: " + k.GetAddressBase58(), fontsmall, Brushes.Black, thiscodeX + 110, thiscodeY);

                    string whattowrite;
                    if (privkey.Length > 30) {
                        whattowrite = privkey.Substring(0, 25) + "\r\n" + privkey.Substring(25);
                    } else {
                        whattowrite = "\r\n" + privkey;
                    }
                    if (privkey.StartsWith("6")) {
                        whattowrite = whattowrite + "\r\nPassword Required";
                    }


                    e.Graphics.DrawString(whattowrite, font, Brushes.Black, thiscodeX + 110, thiscodeY + 15);

                    if ((Denomination ?? "") != "") {
                        e.Graphics.DrawString(Denomination + " BTC", fontbig, Brushes.Black, thiscodeX + 110, thiscodeY + 75); 
                    }
                    

                } else if (PrintMode == PrintModes.PubPrivQR) {

                    // ----------------------------------------------------------------
                    // Paper wallet with public and private QR codes.  Fits 8 to a page.
                    // ----------------------------------------------------------------

                    e.Graphics.DrawImage(b, thiscodeX + 600, thiscodeY, 100, 100);
                    QRCodeEncoder qr2 = new QRCodeEncoder();
                    qr2.QRCodeVersion = 3;

                    Bitmap b2 = qr2.Encode(k.GetAddressBase58());
                    e.Graphics.DrawImage(b2, thiscodeX, thiscodeY, 100, 100);

                    e.Graphics.DrawString("Bitcoin address:\r\n" + k.GetAddressBase58(), font, Brushes.Black, thiscodeX + 110, thiscodeY);

                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Far; // right justify
                    string whattoprint = privkey;
                    if (privkey.StartsWith("6")) {
                        whattoprint = whattoprint + "\r\nPassword Required";
                    }

                    e.Graphics.DrawString("Private key:\r\n" + whattoprint, font, Brushes.Black, thiscodeX + 597, thiscodeY + 65, sf);

                }

            }

            e.HasMorePages = keys.Count > 0;



        }



    }
}

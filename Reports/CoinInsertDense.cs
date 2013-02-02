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

    /// <summary>
    /// Represents a printable report producing the materials to go in a two-factor physical Bitcoin piece.
    /// </summary>
    class CoinInsertDense : CoinInsert {

        /*
         *
        private static bool UbuntuFontLoaded = false;

        private Font fontbig = new Font("Courier New", 13);
        private Font font = new Font("Courier New", 10);
        private Font fontsmall = new Font("Courier New", 4.5F);

        private Font ubuntufont = null;
        private Font ubuntumid = null;
        private Font ubuntubig = null;

        public List<KeyCollectionItem> keys;
        */

        protected override void OnBeginPrint(System.Drawing.Printing.PrintEventArgs e) {
            base.OnBeginPrint(e);
        }


        protected override void OnPrintPage(System.Drawing.Printing.PrintPageEventArgs e) {
            baseOnPrintPage(e);
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

            int startwidth = 0;
            int startheight = 50;

            for (int i = 0; i < 96; i++) {
                int eachheight = 60, eachwidth = 130;
                if (keys.Count == 0) break;

                KeyCollectionItem kci = keys[0];
                string address = kci.GetAddressBase58();

                string privkey = kci.PrivateKey;

                keys.RemoveAt(0);

                int thiscodeX = startwidth + eachwidth * (i / 16);
                int thiscodeY = startheight + eachheight * (i % 16);

                // ----------------------------------------------------------------
                // Coin insert with public and private QR codes.  Fits 8 to a page.
                // ----------------------------------------------------------------


                float CircleDiameterInches = (7F / 16F); // 7/16"

                // draw the private key circle
                using (Pen blackpen = new Pen(Color.Black)) {

                    // print some alignment marks for use in laser cutting
                    if (i == 0) {
                        e.Graphics.FillRectangle(Brushes.Black, startwidth + eachwidth * 3F, startheight, 0.01F, 0.01F);
                        e.Graphics.FillRectangle(Brushes.Black, startwidth + eachwidth * 3F, (float)startheight + (float)eachheight * 8.5F, 0.01F, 0.01F);
                        e.Graphics.FillRectangle(Brushes.Black, startwidth + eachwidth * 3F, (float)startheight + (float)eachheight * 17F, 0.01F, 0.01F);
                    }


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
                using (Bitmap b2 = QR.EncodeQRCode(address)) {
                    e.Graphics.DrawImage(b2, thiscodeX + 80, thiscodeY + 10, 50, 50);
                }

                e.Graphics.RotateTransform(-90F);
                // transform 90 degrees changes our coordinate space so we can do sideways text.
                // must swap xy and value supplied as x parameter must be negative
                // instead of             it's now
                //        -Y                  +X
                //        |                   |
                //  -X-------+X        -Y----------+Y
                //        | PRINT             | PRINT
                //        +Y                  -X


                using (StringFormat sfright = new StringFormat()) {
                    sfright.Alignment = StringAlignment.Far;
                    e.Graphics.DrawString(address.Substring(0, 12) + "\r\n" + address.Substring(12, 12) + "\r\n" + address.Substring(24), fontsmall, Brushes.Black,
                        -(float)(thiscodeY + 10),
                        (float)(thiscodeX + 130), sfright);

                }
                // get out of sideways mode
                e.Graphics.RotateTransform(90F);



            }
            if (keys.Count != 0) {
                e.HasMorePages = true;
            }
        }






    }
}
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

namespace BtcAddress {
    public class Barcode128b {
        /// <summary>
        /// There are 107 total patterns.  Patterns 0-94 correspond to ASCII codes 32-126.
        /// Patterns 104 and 106 are the start and stop codes for 128-B.
        /// Each pattern is the width of bar-space-bar-space-bar-space (+bar for the end pattern 106).
        /// </summary>
        private static string[] patterns =  {"212222","222122","222221","121223","121322","131222","122213","122312","132212",
                                              "221213","221312","231212","112232","122132","122231","113222","123122","123221",
"223211","221132","221231","213212","223112","312131","311222","321122","321221","312212","322112",
"322211","212123","212321","232121","111323","131123","131321","112313","132113","132311","211313",
"231113","231311","112133","112331","132131","113123","113321","133121","313121","211331","231131",
"213113","213311","213131","311123","311321","331121","312113","312311","332111","314111","221411",
"431111","111224","111422","121124","121421","141122","141221","112214","112412","122114","122411",
"142112","142211","241211","221114","413111","241112","134111","111242","121142","121241","114212",
"124112","124211","411212","421112","421211","212141","214121","412121","111143","111341","131141",
"114113","114311","411113","411311","113141","114131","311141","411131","211412","211214","211232",
"2331112"};



        private static string getPatternsForMessage(string message) {
            StringBuilder rv = new StringBuilder();
            
            // add in the start code for 128-B, as well as pre-seed the checksum
            rv.Append(patterns[104]);
            int runningChecksum = 104;
            int posNumber = 1;

            foreach (char c in message.ToCharArray()) {
                if (c >= ' ' && c <= '~') { // chars 32 thru 126
                    int sym = c - 0x20;
                    rv.Append(patterns[sym]);
                    runningChecksum = (runningChecksum + sym * posNumber) % 103;
                    posNumber++;
                }
            }

            // append the checksum and the stop code
            rv.Append(patterns[runningChecksum]);
            rv.Append(patterns[106]);
            return rv.ToString();
        }

        public static Bitmap GetBarcode(string message) {

            int pixelspermodule = 1;
            int margininmodules = 12;
            int heightinmodules = 30;

            string pattern = getPatternsForMessage(message);

            // get number of modules.  Pretty simple, just add up all the digits in the pattern.
            int modulecount=0;
            foreach (char c in pattern.ToCharArray()) modulecount += (c - '0');

            int neededWidth = pixelspermodule * (margininmodules + margininmodules + modulecount);
            int neededHeight = pixelspermodule * heightinmodules;
            Bitmap b = new Bitmap(neededWidth + 1, neededHeight + 1);
            SolidBrush brush = new SolidBrush(Color.White);

            Graphics gr = Graphics.FromImage(b);
            
            // start with a white background
            gr.FillRectangle(brush, new Rectangle(0, 0, neededWidth, neededHeight));

            brush.Color = Color.Black;

            int currentmodule = margininmodules;
            bool nowBlack = true;
            foreach (char c in pattern.ToCharArray()) {
                int modulewidth = (c - '0');
                if (nowBlack) {
                    gr.FillRectangle(brush, currentmodule * pixelspermodule, 0, modulewidth * pixelspermodule, neededHeight);
                }
                nowBlack = !nowBlack;
                currentmodule += modulewidth;
            }

            return b;
        }

    }
}

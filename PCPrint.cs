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


//*****************************************************************************************
//                           LICENSE INFORMATION
//*****************************************************************************************
//   PCPrint Version 1.0.0.0
//   Class file for printing in VB.Net. Inherits from the PrintDocument Class, and includes
//   all its functionality
//
//   Copyright (C) 2008  
//   Richard L. McCutchen 
//   Email: psychocoder@dreamincode.net
//   Created: 25FEB08
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with this program.  If not, see <http://www.gnu.org/licenses/>.
//*****************************************************************************************
using System;
using System.Drawing;
using System.Drawing.Printing;


namespace PC
{
    public class PCPrint : System.Drawing.Printing.PrintDocument
    {
        #region  Property Variables
        /// <summary>
        /// Property variable for the Font the user wishes to use
        /// </summary>
        /// <remarks></remarks>
        private Font _font;

        /// <summary>
        /// Property variable for the text to be printed
        /// </summary>
        /// <remarks></remarks>
        private string _text;
        #endregion

        #region Static Local Variables
        /// <summary>
        /// Static variable to hold the current character
        /// we're currently dealing with.
        /// </summary>
        static int curChar;
        #endregion

        #region  Class Properties
        /// <summary>
        /// Property to hold the text that is to be printed
        /// </summary>
        /// <value></value>
        /// <returns>A string</returns>
        /// <remarks></remarks>
        public string TextToPrint
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// Property to hold the font the users wishes to use
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Font PrinterFont
        {
            // Allows the user to override the default font
            get { return _font; }
            set { _font = value; }
        }
        #endregion

        #region  Class Constructors
        /// <summary>
        /// Empty constructor
        /// </summary>
        /// <remarks></remarks>
        public PCPrint()
            : base()
        {
            //Set the file stream
            //Instantiate out Text property to an empty string
            _text = string.Empty;
        }

        /// <summary>
        /// Constructor to initialize our printing object
        /// and the text it's supposed to be printing
        /// </summary>
        /// <param name=str>Text that will be printed</param>
        /// <remarks></remarks>
        public PCPrint(string str)
            : base()
        {
            //Set the file stream
            //Set our Text property value
            _text = str;
        }
        #endregion

        #region  OnBeginPrint
        /// <summary>
        /// Override the default OnBeginPrint method of the PrintDocument Object
        /// </summary>
        /// <param name=e></param>
        /// <remarks></remarks>
        protected override void OnBeginPrint(System.Drawing.Printing.PrintEventArgs e)
        {
            // Run base code
            base.OnBeginPrint(e);

            //Check to see if the user provided a font
            //if they didnt then we default to Times New Roman
            if (_font == null)
            {
                //Create the font we need
                _font = new Font("Times New Roman", 10);
            }
        }
        #endregion

        #region  OnPrintPage
        /// <summary>
        /// Override the default OnPrintPage method of the PrintDocument
        /// </summary>
        /// <param name=e></param>
        /// <remarks>This provides the print logic for our document</remarks>
        protected override void OnPrintPage(System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Run base code
            base.OnPrintPage(e);

            //Declare local variables needed

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

            //Check if the user selected to print in Landscape mode
            //if they did then we need to swap height/width parameters
            if (base.DefaultPageSettings.Landscape)
            {
                int tmp;
                tmp = printHeight;
                printHeight = printWidth;
                printWidth = tmp;
            }

            //Now we need to determine the total number of lines
            //we're going to be printing
            Int32 numLines = (int)printHeight / PrinterFont.Height;

            //Create a rectangle printing are for our document
            RectangleF printArea = new RectangleF(leftMargin, rightMargin, printWidth, printHeight);

            //Use the StringFormat class for the text layout of our document
            StringFormat format = new StringFormat(StringFormatFlags.LineLimit);

            //Fit as many characters as we can into the print area      

            e.Graphics.MeasureString(_text.Substring(RemoveZeros(ref curChar)), PrinterFont, new SizeF(printWidth, printHeight), format, out chars, out lines);

            //Print the page
            e.Graphics.DrawString(_text.Substring(RemoveZeros(ref curChar)), PrinterFont, Brushes.Black, printArea, format);

            //Increase current char count
            curChar += chars;

            //Detemine if there is more text to print, if
            //there is the tell the printer there is more coming
            if (curChar + 1 < _text.Length)
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
                curChar = 0;
            }
        }

        #endregion

        #region  RemoveZeros
        /// <summary>
        /// Function to replace any zeros in the size to a 1
        /// Zero's will mess up the printing area
        /// </summary>
        /// <param name=value>Value to check</param>
        /// <returns></returns>
        /// <remarks></remarks>

        public int RemoveZeros(ref int value)
        {
            //As 0 (ZERO) being sent to DrawString will create unexpected
            //problems when printing we need to search for the first
            //non-zero character in the string.
            while (_text[value] == '\0')
            {
                value++;
            }
            return value;
        }

        #endregion
    }

}

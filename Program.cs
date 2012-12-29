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
using System.Windows.Forms;

namespace BtcAddress {
    static class Program {

        public static Form1 AddressUtility = null;

        public static Base58Calc Base58Calc = null;

        public static MofNcalc MofNcalc = null;

        public static PpecKeygen IntermediateGen = null;

        public static KeyCombiner KeyCombiner = null;

        public static BtcAddress.Forms.DecryptKey DecryptKey = null;

        public static BtcAddress.Forms.Bip38ConfValidator ConfValidator = null;

        public static BtcAddress.Forms.EscrowTools EscrowTools = null;

        public static void ShowAddressUtility() {
            AddressUtility = showForm<Form1>(AddressUtility);
        }

        public static void ShowBase58Calc() {
            Base58Calc = showForm<Base58Calc>(Base58Calc);
        }

        public static void ShowMofNcalc() {
            MofNcalc = showForm<MofNcalc>(MofNcalc);
        }

        public static void ShowIntermediateGen() {
            IntermediateGen = showForm<PpecKeygen>(IntermediateGen);
        }

        public static void ShowKeyCombiner() {
            KeyCombiner = showForm<KeyCombiner>(KeyCombiner);
        }

        public static void ShowConfValidator() {
            ConfValidator = showForm<BtcAddress.Forms.Bip38ConfValidator>(ConfValidator);
        }

        public static void ShowKeyDecrypter() {
            DecryptKey = showForm<BtcAddress.Forms.DecryptKey>(DecryptKey);
        }

        public static void ShowEscrowTools() {
            EscrowTools = showForm<BtcAddress.Forms.EscrowTools>(EscrowTools);

        }

        private static T showForm<T>(T currentform) where T : Form, new() {
            if (currentform == null || currentform.Visible == false) {
                T rv = new T();
                rv.Show();
                return rv;
            } else {
                currentform.Focus();
                return currentform;
            }
        }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Application.Run(new BtcAddress.Forms.KeyCollectionView());
        }
    }
}

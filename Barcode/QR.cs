using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Security.Cryptography;
using System.IO;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Math.EC;
using ThoughtWorks.QRCode.Codec;
using System.Text.RegularExpressions;

namespace BtcAddress {
    public class QR {

        /// <summary>
        /// Encodes a QR code, making the best choice based on string length
        /// (apparently not provided by QR lib?)
        /// </summary>
        public static Bitmap EncodeQRCode(string what) {
            if (what == null || what == "") return null;

            // Determine if we can use alphanumeric encoding (e.g. public key hex)
            Regex r = new Regex("^[0-9A-F]{63,154}$");
            bool IsAlphanumeric = r.IsMatch(what);

            QRCodeEncoder qr = new QRCodeEncoder();
            if (IsAlphanumeric) {
                qr.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
                if (what.Length > 154) {
                    return null;
                } else if (what.Length > 67) {
                    // 5L is good to 154 alphanumeric characters
                    qr.QRCodeVersion = 5;
                    qr.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                } else {
                    // 4Q is good to 67 alphanumeric characters
                    qr.QRCodeVersion = 4;
                    qr.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                }
            } else {
                if (what.Length > 84) {
                    // We don't intend to encode any alphanumeric strings longer than confirmation codes at 75 characters
                    return null;
                } else if (what.Length > 62) {
                    // 5M is good to 84 characters
                    qr.QRCodeVersion = 5;
                    qr.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                } else if (what.Length > 34) {
                    // 4M is good to 62 characters
                    qr.QRCodeVersion = 4;
                    qr.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                } else if (what.Length > 32) {
                    // 4H is good to 34 characters
                    qr.QRCodeVersion = 4;
                    qr.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                } else {
                    // 3Q is good to 32 characters
                    qr.QRCodeVersion = 3;
                    qr.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                }
            }

            return qr.Encode(what);
        }

    }
}

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
using System.Security.Cryptography;
using System.Diagnostics;
using System.IO;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math;
using CryptSharp.Utility;

namespace Casascius.Bitcoin {
    /// <summary>
    /// Represents a private key encrypted with AES using the SHA256 of a passphrase as key material.
    /// Such a private key is a xx character string starting with "6p" (lowercase p).
    /// This methodology uses no key hardening, so it should only be used when the "passphrase" is
    /// not a user-chosen password.  It's suitable for fast wallet encryption/decryption given key
    /// material that has been properly derived elsewhere.
    /// </summary>
    public class ShaPassphraseKeyPair : PassphraseKeyPair {

        /// <summary>
        /// Load constructor (in preparation for decryption)
        /// </summary>
        public ShaPassphraseKeyPair(string encryptedkey) : base() {
            this._encryptedKey = encryptedkey;

            byte[] hex = Util.Base58CheckToByteArray(encryptedkey);

            if (hex == null) {
                throw new ArgumentException("Not a valid key");
            } else if (hex.Length == 36 && hex[0] == 0x02 && hex[1] == 0x05 && hex[2] <= 0x80) {
                // Apparently valid. Read the compressed point flag.
                if ((hex[3] & 0x01) == 0x01) {
                    IsCompressedPoint = true;
                }
            } else {
                throw new ArgumentException("Not a valid key");
            }
        }

        /// <summary>
        /// Encrypt constructor.
        /// Creates a new encrypted key pair with a passphrase.
        /// The resulting key pair record retains the public key and bitcoin address
        /// but not the passphrase or the unencrypted private key.
        /// </summary>
        public ShaPassphraseKeyPair(KeyPair key, string passphrase) {
            if (passphrase == null || passphrase == "") {
                throw new ArgumentException("Passphrase is required");
            }

            if (key == null) throw new ArgumentException("Key is required");
            IsCompressedPoint = key.IsCompressedPoint;
            _addressType = key.AddressType;
            this._hash160 = key.Hash160;
            this._pubKey = key.PublicKeyBytes;

            var aes = Aes.Create();
            aes.KeySize = 256;
            aes.Mode = CipherMode.ECB;

            byte[] encryptionKey = Util.ComputeSha256(passphrase);
            aes.Key = encryptionKey;
            ICryptoTransform encryptor = aes.CreateEncryptor();

            byte[] rv = new byte[36];

            encryptor.TransformBlock(_privKey, 0, 16, rv, 4);
            encryptor.TransformBlock(_privKey, 0, 16, rv, 4);
            byte[] interblock = new byte[16];
            Array.Copy(rv, 4, interblock, 0, 16);
            for (int x = 0; x < 16; x++) interblock[x] ^= _privKey[16 + x];
            encryptor.TransformBlock(interblock, 0, 16, rv, 20);
            encryptor.TransformBlock(interblock, 0, 16, rv, 20);

            // put header
            rv[0] = 0x02;
            rv[1] = 0x05;

            byte[] checksum = Util.ComputeSha256(passphrase + "?");

            rv[2] = (byte)(checksum[0] & 0x7F);
            rv[3] = (byte)(checksum[1] & 0xFE);
            if (key.IsCompressedPoint) rv[3]++;
            this._encryptedKey = Util.ByteArrayToBase58Check(rv);
        }

        public override bool DecryptWithPassphrase(string passphrase) {

            byte[] hex = Util.Base58CheckToByteArray(_encryptedKey);

            if (passphrase == null || passphrase == "") {
                return false;
            }

            byte[] checksum = Util.ComputeSha256(passphrase + "?");

            if (hex[2] != 0x80) {
                if ((checksum[0] & 0x7f) != hex[2] || (checksum[1] & 0x7e) != (hex[3] & 0x7e)) {
                    return false;
                }
            }

            var aes = Aes.Create();
            aes.KeySize = 256;
            aes.Mode = CipherMode.ECB;

            byte[] encryptionKey = Util.ComputeSha256(passphrase);
            aes.Key = encryptionKey;
            ICryptoTransform decryptor = aes.CreateDecryptor();

            byte[] decrypted = new byte[33];
            decrypted[0] = 0x80;

            decryptor.TransformBlock(hex, 4, 16, decrypted, 1);
            decryptor.TransformBlock(hex, 4, 16, decrypted, 1);
            decryptor.TransformBlock(hex, 20, 16, decrypted, 17);
            decryptor.TransformBlock(hex, 20, 16, decrypted, 17);
            for (int x = 0; x < 16; x++) decrypted[17 + x] ^= hex[4 + x];

            _privKey = new byte[32];
            Array.Copy(decrypted, 1, _privKey, 0, 32);
            return true;
        }
    }
}

#region License
/*
Illusory Studios C# Crypto Library (CryptSharp)
Copyright (c) 2010 James F. Bellinger <jfb@zer7.com>

Permission to use, copy, modify, and/or distribute this software for any
purpose with or without fee is hereby granted, provided that the above
copyright notice and this permission notice appear in all copies.

THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
*/
#endregion

using System;
using System.Diagnostics;

namespace CryptSharp.Utility {
    static class Helper {
        public static void CheckBounds<T>(string valueName,
            T[] value, int offset, int count) {
            CheckNull(valueName, value);
            if (offset < 0 || count < 0 || count > value.Length - offset) { throw new ArgumentOutOfRangeException(); }
        }

        public static void CheckNull<T>(string valueName, T value) {
            if (value == null) {
                if (valueName == null) { throw new ArgumentNullException("valueName"); }
                throw new ArgumentNullException(valueName);
            }
        }

        public static void CheckRange(string valueName,
            int value, int minimum, int maximum) {
            if (value < minimum || value > maximum) {
                throw new ArgumentOutOfRangeException(valueName,
                    string.Format("Value must be in the range [{0}, {1}].",
                    minimum, maximum));
            }
        }

        public static void CheckRange<T>(string valueName,
            T[] value, int minimum, int maximum) {
            CheckNull(valueName, value);
            if (value.Length < minimum || value.Length > maximum) {
                throw new ArgumentOutOfRangeException(valueName,
                    string.Format("Length must be in the range [{0}, {1}].",
                    minimum, maximum));
            }
        }

        public static uint BytesToUInt32(byte[] bytes, int offset) {
            return
                (uint)bytes[offset + 0] << 24 |
                (uint)bytes[offset + 1] << 16 |
                (uint)bytes[offset + 2] << 8 |
                (uint)bytes[offset + 3];
        }

        public static uint BytesToUInt32LE(byte[] bytes, int offset) {
            return
                (uint)bytes[offset + 3] << 24 |
                (uint)bytes[offset + 2] << 16 |
                (uint)bytes[offset + 1] << 8 |
                (uint)bytes[offset + 0];
        }

        public static void UInt32ToBytes(uint value, byte[] bytes, int offset) {
            bytes[offset + 0] = (byte)(value >> 24);
            bytes[offset + 1] = (byte)(value >> 16);
            bytes[offset + 2] = (byte)(value >> 8);
            bytes[offset + 3] = (byte)(value);
        }

        public static void UInt32ToBytesLE(uint value, byte[] bytes, int offset) {
            bytes[offset + 3] = (byte)(value >> 24);
            bytes[offset + 2] = (byte)(value >> 16);
            bytes[offset + 1] = (byte)(value >> 8);
            bytes[offset + 0] = (byte)(value);
        }
    }
}

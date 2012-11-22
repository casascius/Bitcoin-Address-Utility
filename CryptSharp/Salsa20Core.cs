#region License
/*
Illusory Studios C# Crypto Library (CryptSharp)
Copyright (c) 2011 James F. Bellinger <jfb@zer7.com>

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
using System.Security.Cryptography;

namespace CryptSharp.Utility {
    public static class Salsa20Core {
        // Source: http://cr.yp.to/salsa20.html
        static uint R(uint a, int b) { return (a << b) | (a >> (32 - b)); }

        public static void Compute(int rounds,
            uint[] input, int inputOffset, uint[] output, int outputOffset,
            uint[] x) {
            if (rounds < 1 || rounds > 20 || (rounds & 1) == 1) { throw new ArgumentOutOfRangeException("rounds"); }

            try {
                int i;
                for (i = 0; i < 16; i++) { x[i] = input[i + inputOffset]; }
                for (i = rounds; i > 0; i -= 2) {
                    x[4] ^= R(x[0] + x[12], 7); x[8] ^= R(x[4] + x[0], 9);
                    x[12] ^= R(x[8] + x[4], 13); x[0] ^= R(x[12] + x[8], 18);
                    x[9] ^= R(x[5] + x[1], 7); x[13] ^= R(x[9] + x[5], 9);
                    x[1] ^= R(x[13] + x[9], 13); x[5] ^= R(x[1] + x[13], 18);
                    x[14] ^= R(x[10] + x[6], 7); x[2] ^= R(x[14] + x[10], 9);
                    x[6] ^= R(x[2] + x[14], 13); x[10] ^= R(x[6] + x[2], 18);
                    x[3] ^= R(x[15] + x[11], 7); x[7] ^= R(x[3] + x[15], 9);
                    x[11] ^= R(x[7] + x[3], 13); x[15] ^= R(x[11] + x[7], 18);
                    x[1] ^= R(x[0] + x[3], 7); x[2] ^= R(x[1] + x[0], 9);
                    x[3] ^= R(x[2] + x[1], 13); x[0] ^= R(x[3] + x[2], 18);
                    x[6] ^= R(x[5] + x[4], 7); x[7] ^= R(x[6] + x[5], 9);
                    x[4] ^= R(x[7] + x[6], 13); x[5] ^= R(x[4] + x[7], 18);
                    x[11] ^= R(x[10] + x[9], 7); x[8] ^= R(x[11] + x[10], 9);
                    x[9] ^= R(x[8] + x[11], 13); x[10] ^= R(x[9] + x[8], 18);
                    x[12] ^= R(x[15] + x[14], 7); x[13] ^= R(x[12] + x[15], 9);
                    x[14] ^= R(x[13] + x[12], 13); x[15] ^= R(x[14] + x[13], 18);
                }
                for (i = 0; i < 16; i++) { output[i + outputOffset] = x[i] + input[i + inputOffset]; }
            } catch {
                Helper.CheckNull("input", input); Helper.CheckBounds("input", input, inputOffset, 16);
                Helper.CheckNull("output", output); Helper.CheckBounds("output", output, outputOffset, 16);
                Helper.CheckNull("x", x); Helper.CheckBounds("x", x, 0, 16);
                throw;
            }
        }
    }
}

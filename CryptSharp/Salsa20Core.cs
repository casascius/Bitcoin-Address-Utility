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
                uint x0 = input[0 + inputOffset];
                uint x1 = input[1 + inputOffset];
                uint x2 = input[2 + inputOffset];
                uint x3 = input[3 + inputOffset];
                uint x4 = input[4 + inputOffset];
                uint x5 = input[5 + inputOffset];
                uint x6 = input[6 + inputOffset];
                uint x7 = input[7 + inputOffset];
                uint x8 = input[8 + inputOffset];
                uint x9 = input[9 + inputOffset];
                uint x10 = input[10 + inputOffset];
                uint x11 = input[11 + inputOffset];
                uint x12 = input[12 + inputOffset];
                uint x13 = input[13 + inputOffset];
                uint x14 = input[14 + inputOffset];
                uint x15 = input[15 + inputOffset];

                for (i = rounds; i > 0; i -= 2) {
                    //x4 ^= R(x0 + x12, 7); x8 ^= R(x4 + x0, 9);
                    x4 ^= (x0 + x12 << 7) | (x0 + x12 >> (32 - 7));
                    x8 ^= (x4 + x0 << 9) | (x4 + x0 >> (32 - 9));

                    //x12 ^= R(x8 + x4, 13); x0 ^= R(x12 + x8, 18);
                    x12 ^= (x8 + x4 << 13) | (x8 + x4 >> (32 - 13));
                    x0 ^= (x12 + x8 << 18) | (x12 + x8 >> (32 - 18));

                    //x9 ^= R(x5 + x1, 7); x13 ^= R(x9 + x5, 9);
                    x9 ^= (x5 + x1 << 7) | (x5 + x1 >> (32 - 7));
                    x13 ^= (x9 + x5 << 9) | (x9 + x5 >> (32 - 9));

                    //x1 ^= R(x13 + x9, 13); x5 ^= R(x1 + x13, 18);
                    x1 ^= (x13 + x9 << 13) | (x13 + x9 >> (32 - 13));
                    x5 ^= (x1 + x13 << 18) | (x1 + x13 >> (32 - 18));

                    //x14 ^= R(x10 + x6, 7); x2 ^= R(x14 + x10, 9);
                    x14 ^= (x10 + x6 << 7) | (x10 + x6 >> (32 - 7));
                    x2 ^= (x14 + x10 << 9) | (x14 + x10 >> (32 - 9));

                    //x6 ^= R(x2 + x14, 13); x10 ^= R(x6 + x2, 18);
                    x6 ^= (x2 + x14 << 13) | (x2 + x14 >> (32 - 13));
                    x10 ^= (x6 + x2 << 18) | (x6 + x2 >> (32 - 18));

                    //x3 ^= R(x15 + x11, 7); x7 ^= R(x3 + x15, 9);
                    x3 ^= (x15 + x11 << 7) | (x15 + x11 >> (32 - 7));
                    x7 ^= (x3 + x15 << 9) | (x3 + x15 >> (32 - 9));

                    //x11 ^= R(x7 + x3, 13); x15 ^= R(x11 + x7, 18);
                    x11 ^= (x7 + x3 << 13) | (x7 + x3 >> (32 - 13));
                    x15 ^= (x11 + x7 << 18) | (x11 + x7 >> (32 - 18));

                    //x1 ^= R(x0 + x3, 7); x2 ^= R(x1 + x0, 9);
                    x1 ^= (x0 + x3 << 7) | (x0 + x3 >> (32 - 7));
                    x2 ^= (x1 + x0 << 9) | (x1 + x0 >> (32 - 9));

                    //x3 ^= R(x2 + x1, 13); x0 ^= R(x3 + x2, 18);
                    x3 ^= (x2 + x1 << 13) | (x2 + x1 >> (32 - 13));
                    x0 ^= (x3 + x2 << 18) | (x3 + x2 >> (32 - 18));

                    //x6 ^= R(x5 + x4, 7); x7 ^= R(x6 + x5, 9);
                    x6 ^= (x5 + x4 << 7) | (x5 + x4 >> (32 - 7));
                    x7 ^= (x6 + x5 << 9) | (x6 + x5 >> (32 - 9));

                    //x4 ^= R(x7 + x6, 13); x5 ^= R(x4 + x7, 18);
                    x4 ^= (x7 + x6 << 13) | (x7 + x6 >> (32 - 13));
                    x5 ^= (x4 + x7 << 18) | (x4 + x7 >> (32 - 18));

                    //x11 ^= R(x10 + x9, 7); x8 ^= R(x11 + x10, 9);
                    x11 ^= (x10 + x9 << 7) | (x10 + x9 >> (32 - 7));
                    x8 ^= (x11 + x10 << 9) | (x11 + x10 >> (32 - 9));

                    //x9 ^= R(x8 + x11, 13); x10 ^= R(x9 + x8, 18);
                    x9 ^= (x8 + x11 << 13) | (x8 + x11 >> (32 - 13));
                    x10 ^= (x9 + x8 << 18) | (x9 + x8 >> (32 - 18));

                    //x12 ^= R(x15 + x14, 7); x13 ^= R(x12 + x15, 9);
                    x12 ^= (x15 + x14 << 7) | (x15 + x14 >> (32 - 7));
                    x13 ^= (x12 + x15 << 9) | (x12 + x15 >> (32 - 9));

                    //x14 ^= R(x13 + x12, 13); x15 ^= R(x14 + x13, 18);
                    x14 ^= (x13 + x12 << 13) | (x13 + x12 >> (32 - 13));
                    x15 ^= (x14 + x13 << 18) | (x14 + x13 >> (32 - 18));

                }
                output[0 + outputOffset] = x0 + input[0 + inputOffset];
                output[1 + outputOffset] = x1 + input[1 + inputOffset];
                output[2 + outputOffset] = x2 + input[2 + inputOffset];
                output[3 + outputOffset] = x3 + input[3 + inputOffset];
                output[4 + outputOffset] = x4 + input[4 + inputOffset];
                output[5 + outputOffset] = x5 + input[5 + inputOffset];
                output[6 + outputOffset] = x6 + input[6 + inputOffset];
                output[7 + outputOffset] = x7 + input[7 + inputOffset];
                output[8 + outputOffset] = x8 + input[8 + inputOffset];
                output[9 + outputOffset] = x9 + input[9 + inputOffset];
                output[10 + outputOffset] = x10 + input[10 + inputOffset];
                output[11 + outputOffset] = x11 + input[11 + inputOffset];
                output[12 + outputOffset] = x12 + input[12 + inputOffset];
                output[13 + outputOffset] = x13 + input[13 + inputOffset];
                output[14 + outputOffset] = x14 + input[14 + inputOffset];
                output[15 + outputOffset] = x15 + input[15 + inputOffset];

            } catch {
                Helper.CheckNull("input", input); Helper.CheckBounds("input", input, inputOffset, 16);
                Helper.CheckNull("output", output); Helper.CheckBounds("output", output, outputOffset, 16);
                Helper.CheckNull("x", x); Helper.CheckBounds("x", x, 0, 16);
                throw;
            }
        }
    }
}

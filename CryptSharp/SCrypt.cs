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
using System.Threading;

namespace CryptSharp.Utility {
    // See http://www.tarsnap.com/scrypt/scrypt.pdf for algorithm details.
    // TODO: Test on a big-endian machine and make sure it works.
    // TODO: Feel hatred for whatever genius decided C# wouldn't have 'safe'
    //       stack-allocated arrays. He has stricken ugliness upon a thousand codes.
    public static class SCrypt {
        const int hLen = 32;
        static readonly Pbkdf2.ComputeHmacCallback _hmacCallback =
            Pbkdf2.CallbackFromHmac<HMACSHA256>();

        public static void ComputeKey(byte[] key, byte[] salt,
            int cost, int blockSize, int parallel, int? maxThreads, byte[] output) {
            using (Pbkdf2 kdf = GetStream(key, salt, cost, blockSize, parallel, maxThreads)) { kdf.Read(output); }
        }

        public static byte[] GetEffectivePbkdf2Salt(byte[] key, byte[] salt,
            int cost, int blockSize, int parallel, int? maxThreads) {
            Helper.CheckNull("key", key); Helper.CheckNull("salt", salt);
            return MFcrypt(key, salt, cost, blockSize, parallel, maxThreads);
        }

        public static Pbkdf2 GetStream(byte[] key, byte[] salt,
            int cost, int blockSize, int parallel, int? maxThreads) {
            byte[] B = GetEffectivePbkdf2Salt(key, salt, cost, blockSize, parallel, maxThreads);
            Pbkdf2 kdf = new Pbkdf2(key, B, 1, _hmacCallback, hLen);
            Clear(B); return kdf;
        }

        static void Clear(Array arr) {
            Array.Clear(arr, 0, arr.Length);
        }

        static byte[] MFcrypt(byte[] P, byte[] S,
            int cost, int blockSize, int parallel, int? maxThreads) {
            int MFLen = blockSize * 128;
            if (maxThreads == null) { maxThreads = int.MaxValue; }

            if (cost <= 0 || (cost & (cost - 1)) != 0) { throw new ArgumentOutOfRangeException("cost", "Cost must be a positive power of 2."); }
            Helper.CheckRange("blockSize", blockSize, 1, int.MaxValue / 32);
            Helper.CheckRange("parallel", parallel, 1, int.MaxValue / MFLen);
            Helper.CheckRange("maxThreads", (int)maxThreads, 1, int.MaxValue);

            byte[] B = new byte[parallel * MFLen];
            Pbkdf2.ComputeKey(P, S, 1, _hmacCallback, hLen, B);

            uint[] B0 = new uint[B.Length / 4];
            for (int i = 0; i < B0.Length; i++) { B0[i] = Helper.BytesToUInt32LE(B, i * 4); } // code is easier with uint[]
            ThreadSMixCalls(B0, MFLen, cost, blockSize, parallel, (int)maxThreads);
            for (int i = 0; i < B0.Length; i++) { Helper.UInt32ToBytesLE(B0[i], B, i * 4); }
            Clear(B0);

            return B;
        }

        static void ThreadSMixCalls(uint[] B0, int MFLen,
            int cost, int blockSize, int parallel, int maxThreads) {
            int current = 0;
            ThreadStart workerThread = delegate() {
                while (true) {
                    int j = Interlocked.Increment(ref current) - 1;
                    if (j >= parallel) { break; }

                    SMix(B0, j * MFLen / 4, B0, j * MFLen / 4, (uint)cost, blockSize);
                }
            };

            int threadCount = Math.Max(1, Math.Min(Environment.ProcessorCount, Math.Min(maxThreads, parallel)));
            Thread[] threads = new Thread[threadCount - 1];
            for (int i = 0; i < threads.Length; i++) { (threads[i] = new Thread(workerThread, 8192)).Start(); }
            workerThread();
            for (int i = 0; i < threads.Length; i++) { threads[i].Join(); }
        }

        static void SMix(uint[] B, int Boffset, uint[] Bp, int Bpoffset, uint N, int r) {
            uint Nmask = N - 1; int Bs = 16 * 2 * r;
            uint[] scratch1 = new uint[16], scratch2 = new uint[16];
            uint[] scratchX = new uint[16], scratchY = new uint[Bs];
            uint[] scratchZ = new uint[Bs];

            uint[] x = new uint[Bs]; uint[][] v = new uint[N][];
            for (int i = 0; i < v.Length; i++) { v[i] = new uint[Bs]; }

            Array.Copy(B, Boffset, x, 0, Bs);
            for (uint i = 0; i < N; i++) {
                Array.Copy(x, v[i], Bs);
                BlockMix(x, 0, x, 0, scratchX, scratchY, scratch1, scratch2, r);
            }
            for (uint i = 0; i < N; i++) {
                uint j = x[Bs - 16] & Nmask; uint[] vj = v[j];
                for (int k = 0; k < scratchZ.Length; k++) { scratchZ[k] = x[k] ^ vj[k]; }
                BlockMix(scratchZ, 0, x, 0, scratchX, scratchY, scratch1, scratch2, r);
            }
            Array.Copy(x, 0, Bp, Bpoffset, Bs);

            for (int i = 0; i < v.Length; i++) { Clear(v[i]); }
            Clear(v); Clear(x);
            Clear(scratchX); Clear(scratchY); Clear(scratchZ);
            Clear(scratch1); Clear(scratch2);
        }

        static void BlockMix
            (uint[] B,        // 16*2*r
             int Boffset,
             uint[] Bp,       // 16*2*r
             int Bpoffset,
             uint[] x,        // 16
             uint[] y,        // 16*2*r -- unnecessary but it allows us to alias B and Bp
             uint[] scratch1, // 16
             uint[] scratch2, // 16
             int r) {
            int k = Boffset, m = 0, n = 16 * r;
            Array.Copy(B, (2 * r - 1) * 16, x, 0, 16);

            for (int i = 0; i < r; i++) {
                for (int j = 0; j < scratch1.Length; j++) { scratch1[j] = x[j] ^ B[j + k]; }
                Salsa20Core.Compute(8, scratch1, 0, x, 0, scratch2);
                Array.Copy(x, 0, y, m, 16);
                k += 16;

                for (int j = 0; j < scratch1.Length; j++) { scratch1[j] = x[j] ^ B[j + k]; }
                Salsa20Core.Compute(8, scratch1, 0, x, 0, scratch2);
                Array.Copy(x, 0, y, m + n, 16);
                k += 16;

                m += 16;
            }

            Array.Copy(y, 0, Bp, Bpoffset, y.Length);
        }
    }
}

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
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace CryptSharp.Utility {
    public class Pbkdf2 : Stream {
        #region PBKDF2
        public delegate void ComputeHmacCallback(byte[] key, byte[] data, byte[] output);

        byte[] _key, _saltBuf, _block, _blockT1, _blockT2;
        ComputeHmacCallback _computeHmacCallback;
        int _iterations;

        public Pbkdf2(byte[] key, byte[] salt, int iterations,
            ComputeHmacCallback computeHmacCallback, int hmacLength) {
            Reopen(key, salt, iterations, computeHmacCallback, hmacLength);
        }

        static void Clear(Array arr) {
            Array.Clear(arr, 0, arr.Length);
        }

        public void Read(byte[] output) {
            Helper.CheckNull("output", output);

            int bytes = Read(output, 0, output.Length);
            if (bytes < output.Length) {
                throw new ArgumentException("Can only return "
                    + output.Length.ToString() + " bytes.", "output");
            }
        }

        public static void ComputeKey(byte[] key, byte[] salt, int iterations,
            ComputeHmacCallback computeHmacCallback, int hmacLength, byte[] output) {
            using (Pbkdf2 kdf = new Pbkdf2
                (key, salt, iterations, computeHmacCallback, hmacLength)) {
                kdf.Read(output);
            }
        }

        public static ComputeHmacCallback CallbackFromHmac<T>() where T : KeyedHashAlgorithm, new() {
            return delegate(byte[] key, byte[] data, byte[] output) {
                using (T hmac = new T()) {
                    Helper.CheckNull("key", key); Helper.CheckNull("data", data);
                    hmac.Key = key; byte[] hmacOutput = hmac.ComputeHash(data);

                    try {
                        Helper.CheckRange("output", output, hmacOutput.Length, hmacOutput.Length);
                        Array.Copy(hmacOutput, output, output.Length);
                    } finally {
                        Clear(hmacOutput);
                    }
                }
            };
        }

        public void Reopen(byte[] key, byte[] salt, int iterations,
            ComputeHmacCallback computeHmacCallback, int hmacLength) {
            Helper.CheckNull("key", key);
            Helper.CheckNull("salt", salt);
            Helper.CheckNull("computeHmacCallback", computeHmacCallback);
            Helper.CheckRange("salt", salt, 0, int.MaxValue - 4);
            Helper.CheckRange("iterations", iterations, 1, int.MaxValue);
            Helper.CheckRange("hmacLength", hmacLength, 1, int.MaxValue);
            _key = new byte[key.Length]; Array.Copy(key, _key, key.Length);
            _saltBuf = new byte[salt.Length + 4]; Array.Copy(salt, _saltBuf, salt.Length);
            _iterations = iterations; _computeHmacCallback = computeHmacCallback;
            _block = new byte[hmacLength]; _blockT1 = new byte[hmacLength]; _blockT2 = new byte[hmacLength];
            ReopenStream();
        }

        public override void Close() {
            Clear(_key); Clear(_saltBuf); Clear(_block);
        }

        void ComputeBlock(uint pos) {
            Helper.UInt32ToBytes(pos, _saltBuf, _saltBuf.Length - 4);
            ComputeHmac(_saltBuf, _blockT1);
            Array.Copy(_blockT1, _block, _blockT1.Length);

            for (int i = 1; i < _iterations; i++) {
                ComputeHmac(_blockT1, _blockT2); // let's not require aliasing support
                Array.Copy(_blockT2, _blockT1, _blockT2.Length);
                for (int j = 0; j < _block.Length; j++) { _block[j] ^= _blockT1[j]; }
            }

            Clear(_blockT1); Clear(_blockT2);
        }

        void ComputeHmac(byte[] data, byte[] output) {
            Debug.Assert(data != null && output != null);
            _computeHmacCallback(_key, data, output);
        }
        #endregion

        #region Stream
        long _blockStart, _blockEnd, _pos;

        void ReopenStream() {
            _blockStart = _blockEnd = _pos = 0;
        }

        public override void Flush() {

        }

        public override int Read(byte[] buffer, int offset, int count) {
            Helper.CheckBounds("buffer", buffer, offset, count); int bytes = 0;

            while (count > 0) {
                if (Position < _blockStart || Position >= _blockEnd) {
                    if (Position >= Length) { break; }

                    long pos = Position / _block.Length;
                    ComputeBlock((uint)(pos + 1));
                    _blockStart = pos * _block.Length;
                    _blockEnd = _blockStart + _block.Length;
                }

                int bytesSoFar = (int)(Position - _blockStart);
                int bytesThisTime = (int)Math.Min(_block.Length - bytesSoFar, count);
                Array.Copy(_block, bytesSoFar, buffer, bytes, bytesThisTime);
                count -= bytesThisTime; bytes += bytesThisTime; Position += bytesThisTime;
            }

            return bytes;
        }

        public override long Seek(long offset, SeekOrigin origin) {
            long pos;

            switch (origin) {
                case SeekOrigin.Begin: pos = offset; break;
                case SeekOrigin.Current: pos = Position + offset; break;
                case SeekOrigin.End: pos = Length + offset; break;
                default: throw new ArgumentException("Unknown seek type.", "origin");
            }

            if (pos < 0) { throw new ArgumentException("Seeking before the stream start.", "offset"); }
            Position = pos; return pos;
        }

        public override void SetLength(long value) {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count) {
            throw new NotSupportedException();
        }

        public override bool CanRead {
            get { return true; }
        }

        public override bool CanSeek {
            get { return true; }
        }

        public override bool CanWrite {
            get { return false; }
        }

        public override long Length {
            get { return (long)_block.Length * uint.MaxValue; }
        }

        public override long Position {
            get { return _pos; }
            set {
                if (_pos < 0) { throw new ArgumentOutOfRangeException("value"); }
                _pos = value;
            }
        }
        #endregion
    }
}

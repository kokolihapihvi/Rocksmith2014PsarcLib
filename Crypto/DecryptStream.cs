using Org.BouncyCastle.Utilities.Zlib;
using Rocksmith2014PsarcLib.Psarc.Asset;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Rocksmith2014PsarcLib.Crypto
{
    public class DecryptStream : IDisposable
    {
        public static readonly byte[] PsarcKey = new byte[32]
        {
            0xC5, 0x3D, 0xB2, 0x38, 0x70, 0xA1, 0xA2, 0xF7,
            0x1C, 0xAE, 0x64, 0x06, 0x1F, 0xDD, 0x0E, 0x11,
            0x57, 0x30, 0x9D, 0xC8, 0x52, 0x04, 0xD4, 0xC5,
            0xBF, 0xDF, 0x25, 0x09, 0x0D, 0xF2, 0x57, 0x2C
        };

        public static readonly byte[] PsarcIV = new byte[16]
        {
            0xE9, 0x15, 0xAA, 0x01,
            0x8F, 0xEF, 0x71, 0xFC,
            0x50, 0x81, 0x32, 0xE4,
            0xBB, 0x4C, 0xEB, 0x42
        };

        public static byte[] SngKeyPC = new byte[32]
        {
            0xCB, 0x64, 0x8D, 0xF3, 0xD1, 0x2A, 0x16, 0xBF,
            0x71, 0x70, 0x14, 0x14, 0xE6, 0x96, 0x19, 0xEC,
            0x17, 0x1C, 0xCA, 0x5D, 0x2A, 0x14, 0x2E, 0x3E,
            0x59, 0xDE, 0x7A, 0xDD, 0xA1, 0x8A, 0x3A, 0x30
        };

        public static byte[] PCMetaDatKey = new byte[32]
        {
            0x5F, 0xB0, 0x23, 0xEF, 0x19, 0xD5, 0xDC, 0x37,
            0xAD, 0xDA, 0xC8, 0xF0, 0x17, 0xF8, 0x8F, 0x0E,
            0x98, 0x18, 0xA3, 0xAC, 0x2F, 0x72, 0x46, 0x96,
            0xA5, 0x9D, 0xE2, 0xBF, 0x05, 0x25, 0x12, 0xEB
        };

        public enum DecryptMode
        {
            PSARC,
            SNG
        }

        public DecryptMode Mode { get; private set; }

        public BinaryReader Reader { get; private set; }

        private MemoryStream _decryptedStream;

        public DecryptStream(Stream input, DecryptMode mode, long position, long length)
        {
            Mode = mode;

            switch (Mode)
            {
                case DecryptMode.PSARC:
                    InitializePsarcDecryptor(input, position, length);
                    break;
                case DecryptMode.SNG:
                    InitializeSngDecryptor(input, position, length);
                    break;
                default:
                    throw new ArgumentException("Invalid decrypt mode", "mode");
            }
        }

        private void InitializePsarcDecryptor(Stream input, long position, long length)
        {
            _decryptedStream = new MemoryStream((int)length);

            //Setup decrypting stream
            using (var decryptor = new RijndaelManaged())
            {
                decryptor.Mode = CipherMode.CFB;
                decryptor.Padding = PaddingMode.None;
                decryptor.BlockSize = 128;
                decryptor.IV = new byte[16];
                decryptor.Key = PsarcKey;

                using (var decryptStream = new CryptoStream(_decryptedStream, decryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    var buffer = new byte[512];
                    var pad = buffer.Length - (int)(length % buffer.Length);

                    while (input.Position < length)
                    {
                        var size = (int)Math.Min(length - input.Position, buffer.Length);
                        input.Read(buffer, 0, size);
                        decryptStream.Write(buffer, 0, size);
                    }

                    if (pad > 0)
                        decryptStream.Write(new byte[pad], 0, pad);

                    decryptStream.Flush();
                    _decryptedStream = new MemoryStream(_decryptedStream.ToArray());
                }
            }

            Reader = new BinaryReader(_decryptedStream);
        }

        private void InitializeSngDecryptor(Stream input, long position, long length)
        {
            var sngFlags = SngAsset.AssetFlags.None;

            var decryptIV = new byte[16];

            //Check and prepare for sng decrypting
            using (var rdr = new BinaryReader(input, System.Text.Encoding.Default, true))
            {
                //Identifier?
                if (rdr.ReadUInt32() != 0x4A)
                {
                    throw new InvalidDataException("Not a valid sng file");
                }

                //Read asset flags
                sngFlags = (SngAsset.AssetFlags)rdr.ReadUInt32();

                //Read encryption IV
                decryptIV = rdr.ReadBytes(16);

                //Adjust length by removing header size
                length -= 24;
            }

            _decryptedStream = new MemoryStream();

            // Decrypt using aes counter mode
            AesCtrTransform(SngKeyPC, decryptIV, input, _decryptedStream);

            Reader = new BinaryReader(_decryptedStream);
            _decryptedStream.Seek(0, SeekOrigin.Begin);

            if (sngFlags.HasFlag(SngAsset.AssetFlags.Compressed))
            {
                //Uncompress
                long uncompressedSize = Reader.ReadUInt32();

                _decryptedStream.Seek(4, SeekOrigin.Begin);
                using (var zOutputStream = new ZInputStream(_decryptedStream))
                {
                    var unzippedStream = new MemoryStream();

                    zOutputStream.CopyTo(unzippedStream);

                    unzippedStream.Seek(0, SeekOrigin.Begin);

                    // Dispose the previous reader
                    Reader.Dispose();

                    Reader = new BinaryReader(unzippedStream);
                }

                _decryptedStream.Dispose();
            }
        }

        /// <summary>
        /// AES counter mode transform
        /// https://stackoverflow.com/a/51188472
        /// </summary>
        /// <param name="key"></param>
        /// <param name="salt"></param>
        /// <param name="inputStream"></param>
        /// <param name="outputStream"></param>
        private static void AesCtrTransform(byte[] key, byte[] salt, Stream inputStream, Stream outputStream)
        {
            SymmetricAlgorithm aes = new AesManaged { Mode = CipherMode.ECB, Padding = PaddingMode.None };

            var blockSize = aes.BlockSize / 8;

            if (salt.Length != blockSize)
            {
                throw new ArgumentException($"Salt size must be same as block size (actual: {salt.Length}, expected: {blockSize})");
            }

            var counter = (byte[])salt.Clone();

            var xorMask = new Queue<byte>();

            var zeroIv = new byte[blockSize];
            var counterEncryptor = aes.CreateEncryptor(key, zeroIv);

            int b;
            while ((b = inputStream.ReadByte()) != -1)
            {
                if (xorMask.Count == 0)
                {
                    var counterModeBlock = new byte[blockSize];

                    counterEncryptor.TransformBlock(
                        counter, 0, counter.Length, counterModeBlock, 0);

                    for (var i2 = counter.Length - 1; i2 >= 0; i2--)
                    {
                        if (++counter[i2] != 0)
                        {
                            break;
                        }
                    }

                    foreach (var b2 in counterModeBlock)
                    {
                        xorMask.Enqueue(b2);
                    }
                }

                var mask = xorMask.Dequeue();
                outputStream.WriteByte((byte)(((byte)b) ^ mask));
            }
        }

        #region Disposable
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    Reader.Dispose();
                    _decryptedStream.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~DecryptStream()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}

using Rocksmith2014PsarcLib.Crypto;
using Rocksmith2014PsarcLib.ReaderExtensions;
using System;
using System.Collections.Generic;
using System.IO;

namespace Rocksmith2014PsarcLib.Psarc
{
    public class PsarcTOC : IDisposable
    {
        private bool disposedValue;

        public List<PsarcTOCEntry> Entries { get; private set; }
        public bool Encrypted { get; private set; }

        public DecryptStream Decryptor { get; private set; }

        public uint[] ZipBlockSizes { get; private set; }

        private BinaryReader _reader;

        public PsarcTOC(PsarcFile psarcFile)
        {
            Entries = new List<PsarcTOCEntry>();

            Encrypted = psarcFile.Header.ArchiveFlags.HasFlag(PsarcArchiveFlags.TOC_ENCRYPTED);

            _reader = psarcFile._reader;
            if (Encrypted)
            {
                //Setup decrypting stream
                Decryptor = new DecryptStream(psarcFile._fileStream, DecryptStream.DecryptMode.PSARC, 0, psarcFile.Header.TOCSize);

                _reader = Decryptor.Reader;
            }

            for (int i = 0; i < psarcFile.Header.EntryCount; i++)
            {
                Entries.Add(new PsarcTOCEntry
                {
                    Index = i,
                    Hash = BitConverter.ToString(_reader.ReadBytes(16)).Replace("-", ""),
                    StartBlock = _reader.ReadUInt32BE(),
                    Length = _reader.ReadUInt40BE(),
                    Offset = _reader.ReadUInt40BE()
                });
            }

            int bNum = (int)Math.Log(psarcFile.Header.BlockSize, 256);

            int tocChunkSize = (int)(psarcFile.Header.EntryCount * psarcFile.Header.TOCEntrySize);//(int)_reader.BaseStream.Position //don't alter this with. causes issues
            int zNum = (int)((psarcFile.Header.TOCSize - 32u - tocChunkSize) / bNum);
            ZipBlockSizes = new uint[zNum];

            for (int i = 0; i < zNum; i++)
            {
                switch (bNum)
                {
                    case 2://64KB
                        ZipBlockSizes[i] = _reader.ReadUInt16BE();
                        break;
                    case 3://16MB
                        ZipBlockSizes[i] = _reader.ReadUInt24BE();
                        break;
                    case 4://4GB
                        ZipBlockSizes[i] = _reader.ReadUInt32BE();
                        break;
                }
            }

            Decryptor?.Dispose();
            Decryptor = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    Decryptor?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~PsarcTOC()
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
    }
}

using Rocksmith2014PsarcLib.Crypto;
using Rocksmith2014PsarcLib.ReaderExtensions;
using System;
using System.Collections.Generic;

namespace Rocksmith2014PsarcLib.Psarc
{
    public class PsarcTOC : IDisposable
    {
        public List<PsarcTOCEntry> Entries { get; }
        public bool Encrypted { get; }

        private DecryptStream Decryptor { get; }

        public uint[] ZipBlockSizes { get; }

        public PsarcTOC(PsarcFile psarcFile)
        {
            Entries = new List<PsarcTOCEntry>((int)psarcFile.Header.EntryCount);

            Encrypted = psarcFile.Header.ArchiveFlags.HasFlag(PsarcArchiveFlags.TOC_ENCRYPTED);

            var reader = psarcFile.Reader;
            if (Encrypted)
            {
                //Setup decrypting stream
                Decryptor = new DecryptStream(psarcFile.FileStream, DecryptStream.DecryptMode.PSARC, psarcFile.Header.TOCSize);

                reader = Decryptor.Reader;
            }

            for (var i = 0; i < psarcFile.Header.EntryCount; i++)
            {
                Entries.Add(new PsarcTOCEntry
                {
                    Index = i,
                    Hash = BitConverter.ToString(reader.ReadBytes(16)).Replace("-", ""),
                    StartBlock = reader.ReadUInt32Be(),
                    Length = reader.ReadUInt40Be(),
                    Offset = reader.ReadUInt40Be()
                });
            }

            var bNum = (int)Math.Log(psarcFile.Header.BlockSize, 256);

            var tocChunkSize = (int)(psarcFile.Header.EntryCount * psarcFile.Header.TOCEntrySize);//(int)_reader.BaseStream.Position //don't alter this with. causes issues
            var zNum = (int)((psarcFile.Header.TOCSize - 32u - tocChunkSize) / bNum);
            ZipBlockSizes = new uint[zNum];

            for (var i = 0; i < zNum; i++)
            {
                switch (bNum)
                {
                    case 2://64KB
                        ZipBlockSizes[i] = reader.ReadUInt16Be();
                        break;
                    case 3://16MB
                        ZipBlockSizes[i] = reader.ReadUInt24Be();
                        break;
                    case 4://4GB
                        ZipBlockSizes[i] = reader.ReadUInt32Be();
                        break;
                }
            }

            Decryptor?.Dispose();
            Decryptor = null;
        }

        public void Dispose()
        {
            Decryptor?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

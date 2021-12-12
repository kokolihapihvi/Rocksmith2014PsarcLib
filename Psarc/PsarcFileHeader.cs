using Rocksmith2014PsarcLib.ReaderExtensions;
using System.IO;
using System.Text;

namespace Rocksmith2014PsarcLib.Psarc
{
    public class PsarcFileHeader
    {
        public string Identifier { get; private set; }
        public uint Version { get; private set; }
        public string Compression { get; private set; }
        public uint TOCSize { get; private set; }
        public uint TOCEntrySize { get; private set; }
        public long TOCOffset { get; private set; }
        public uint EntryCount { get; private set; }
        public uint BlockSize { get; private set; }
        public PsarcArchiveFlags ArchiveFlags { get; private set; }

        public PsarcFileHeader(PsarcFile psarcFile)
        {
            var reader = psarcFile.Reader;

            //Seek to the beginning
            reader.BaseStream.Seek(0, SeekOrigin.Begin);

            Identifier = Encoding.ASCII.GetString(reader.ReadBytes(4));
            Version = reader.ReadUInt32Be();
            Compression = Encoding.ASCII.GetString(reader.ReadBytes(4));
            TOCSize = reader.ReadUInt32Be();
            TOCEntrySize = reader.ReadUInt32Be(); // -32?
            EntryCount = reader.ReadUInt32Be();
            BlockSize = reader.ReadUInt32Be();
            ArchiveFlags = (PsarcArchiveFlags)reader.ReadUInt32Be();

            TOCOffset = reader.BaseStream.Position;
        }
    }
}

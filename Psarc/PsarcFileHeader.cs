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
            var _reader = psarcFile._reader;

            //Seek to the beginning
            _reader.BaseStream.Seek(0, SeekOrigin.Begin);

            Identifier = Encoding.ASCII.GetString(_reader.ReadBytes(4));
            Version = _reader.ReadUInt32BE();
            Compression = Encoding.ASCII.GetString(_reader.ReadBytes(4));
            TOCSize = _reader.ReadUInt32BE();
            TOCEntrySize = _reader.ReadUInt32BE(); // -32?
            EntryCount = _reader.ReadUInt32BE();
            BlockSize = _reader.ReadUInt32BE();
            ArchiveFlags = (PsarcArchiveFlags)_reader.ReadUInt32BE();

            TOCOffset = _reader.BaseStream.Position;
        }
    }
}

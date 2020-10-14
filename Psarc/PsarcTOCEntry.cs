namespace Rocksmith2014PsarcLib.Psarc
{
    public class PsarcTOCEntry
    {
        public int Index { get; internal set; }
        public string Hash { get; internal set; }
        public uint StartBlock { get; internal set; }
        public ulong Length { get; internal set; }
        public ulong Offset { get; internal set; }
        public string Path { get; internal set; }
    }
}

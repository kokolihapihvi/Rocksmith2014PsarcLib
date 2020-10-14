using System.Runtime.InteropServices;

namespace Rocksmith2014PsarcLib.Psarc.Models.Sng
{
    public struct Phrase
    {
        public byte Solo;
        public byte Disparity;
        public byte Ignore;
        public byte Padding;

        public int MaxDifficulty;
        public int PhraseIterationLinks;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string Name;
    }
}

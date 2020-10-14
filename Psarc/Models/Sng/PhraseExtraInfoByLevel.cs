using System.Runtime.InteropServices;

namespace Rocksmith2014PsarcLib.Psarc.Models.Sng
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PhraseExtraInfoByLevel
    {
        public int PhraseId;
        public int Difficulty;
        public int Empty;
        public byte LevelJump;
        public short Redundant;
        public byte Padding;
    }
}

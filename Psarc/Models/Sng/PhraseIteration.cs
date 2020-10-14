using System.Runtime.InteropServices;

namespace Rocksmith2014PsarcLib.Psarc.Models.Sng
{
    public struct PhraseIteration
    {
        public int PhraseId;
        public float StartTime;
        public float NextPhraseTime;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public int[] Difficulty;
    }
}

using System.Runtime.InteropServices;

namespace Rocksmith2014PsarcLib.Psarc.Models.Sng
{
    public struct Section
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string Name;

        public int Number;
        public float StartTime;
        public float EndTime;
        public int StartPhraseIterationId;
        public int EndPhraseIterationId;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string StringMask;
    }
}

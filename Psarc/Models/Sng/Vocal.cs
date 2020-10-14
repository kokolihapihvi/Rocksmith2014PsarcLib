using System.Runtime.InteropServices;

namespace Rocksmith2014PsarcLib.Psarc.Models.Sng
{
    public struct Vocal
    {
        public float Time;
        public int Note;
        public float Length;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
        public string Lyric;
    }
}

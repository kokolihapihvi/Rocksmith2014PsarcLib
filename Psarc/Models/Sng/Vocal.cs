using System.Runtime.InteropServices;

namespace Rocksmith2014PsarcLib.Psarc.Models.Sng
{
    public struct Vocal
    {
        public float Time;
        public int Note;
        public float Length;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public byte[] LyricBytes;
        
        public string Lyric => System.Text.Encoding.UTF8.GetString(LyricBytes);
    }
}

using System.Runtime.InteropServices;

namespace Rocksmith2014PsarcLib.Psarc.Models.Sng
{
    public struct Chord
    {
        public uint Mask;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] Frets;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] Fingers;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public int[] Notes;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string Name;
    }
}

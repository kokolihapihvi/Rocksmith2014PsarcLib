using System.Runtime.InteropServices;

namespace Rocksmith2014PsarcLib.Psarc.Models.Sng
{
    public struct ChordNotes
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public int[] NoteMask;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public BendData[] BendData;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] SlideTo;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] SlideUnpitchTo;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public short[] Vibrato;
    }
}

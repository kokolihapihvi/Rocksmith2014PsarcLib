using System.Runtime.InteropServices;

namespace Rocksmith2014PsarcLib.Psarc.Models.Sng
{
    public struct BendData32
    {
        public float Time;
        public float Step;
        public short Unk3_0;
        public byte Unk4_0;
        public byte Unk5;
    }

    public struct BendData
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public BendData32[] BendData32;

        public int UsedCount;
    }
}

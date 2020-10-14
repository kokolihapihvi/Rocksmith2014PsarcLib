using System.Runtime.InteropServices;

namespace Rocksmith2014PsarcLib.Psarc.Models.Sng
{
    public struct Rect
    {
        public float yMin;
        public float xMin;
        public float yMax;
        public float xMax;
    }

    public struct SymbolDefinition
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string Text;
        public Rect Rect_Outter;
        public Rect Rect_Inner;
    }
}

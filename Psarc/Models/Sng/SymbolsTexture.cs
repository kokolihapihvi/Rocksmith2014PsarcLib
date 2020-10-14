using System.Runtime.InteropServices;

namespace Rocksmith2014PsarcLib.Psarc.Models.Sng
{
    public struct SymbolsTexture
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string Font;

        public int FontpathLength;
        public int Unk1_0;
        public int Width;
        public int Height;
    }
}

using System.Runtime.InteropServices;

namespace Rocksmith2014PsarcLib.Psarc.Models.Sng
{
    public struct Action
    {
        public float Time;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string ActionName;
    }
}

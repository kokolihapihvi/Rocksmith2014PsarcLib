using System;

namespace Rocksmith2014PsarcLib.Psarc
{
    [Flags]
    public enum PsarcArchiveFlags
    {
        NONE = 0,
        UNK1 = 1,
        UNK2 = 2,
        TOC_ENCRYPTED = 4,
        UNK8 = 8,
        UNK16 = 16,
        UNK32 = 32,
        UNK64 = 64,
        UNK128 = 128
    }
}

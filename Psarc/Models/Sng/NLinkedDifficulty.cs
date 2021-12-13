using System.IO;
using Rocksmith2014PsarcLib.ReaderExtensions;

namespace Rocksmith2014PsarcLib.Psarc.Models.Sng
{
    public struct NLinkedDifficulty : IBinarySerializable
    {
        public int LevelBreak;
        public int PhraseCount;

        public int[] NLD_Phrase;

        public IBinarySerializable Read(BinaryReader reader)
        {
            LevelBreak = reader.ReadInt32();
            PhraseCount = reader.ReadInt32();

            NLD_Phrase = reader.ReadIntArray(PhraseCount);

            return this;
        }
    }
}

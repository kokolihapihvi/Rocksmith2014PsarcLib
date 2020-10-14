using System.IO;

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

            NLD_Phrase = new int[PhraseCount];

            for (int i = 0; i < NLD_Phrase.Length; i++) NLD_Phrase[i] = reader.ReadInt32();

            return this;
        }
    }
}

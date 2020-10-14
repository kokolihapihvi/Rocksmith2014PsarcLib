using Rocksmith2014PsarcLib.ReaderExtensions;
using System.IO;

namespace Rocksmith2014PsarcLib.Psarc.Models.Sng
{
    public struct Metadata : IBinarySerializable
    {
        public double MaxScore;
        public double MaxNotesAndChords;
        public double MaxNotesAndChords_Real;
        public double PointsPerNote;
        public float FirstBeatLength;
        public float StartTime;
        public byte CapoFretId;
        public string LastConversionDateTime;
        public short Part;
        public float SongLength;
        public int StringCount;
        public short[] Tuning;
        public float Unk11_FirstNoteTime;
        public float Unk12_FirstNoteTime;
        public int MaxDifficulty;

        public IBinarySerializable Read(BinaryReader reader)
        {
            MaxScore = reader.ReadDouble();
            MaxNotesAndChords = reader.ReadDouble();
            MaxNotesAndChords_Real = reader.ReadDouble();
            PointsPerNote = reader.ReadDouble();
            FirstBeatLength = reader.ReadSingle();
            StartTime = reader.ReadSingle();
            CapoFretId = reader.ReadByte();
            LastConversionDateTime = reader.ReadString(32);
            Part = reader.ReadInt16();
            SongLength = reader.ReadSingle();

            StringCount = reader.ReadInt32();
            Tuning = reader.ReadShortArray(StringCount);

            Unk11_FirstNoteTime = reader.ReadSingle();
            Unk12_FirstNoteTime = reader.ReadSingle();
            MaxDifficulty = reader.ReadInt32();

            return this;
        }
    }
}

using Rocksmith2014PsarcLib.ReaderExtensions;
using System.IO;
using System.Runtime.InteropServices;

namespace Rocksmith2014PsarcLib.Psarc.Models.Sng
{
    public struct Anchor
    {
        public float StartBeatTime;
        public float EndBeatTime;
        public float Unk3_FirstNoteTime;
        public float Unk4_LastNoteTime;
        public byte FretId;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] Padding;

        public int Width;
        public int PhraseIterationId;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AnchorExtension
    {
        public float BeatTime;
        public byte FretId;
        public int Unk2_0;
        public short Unk3_0;
        public byte Unk4_0;
    }

    public struct Fingerprint
    {
        public int ChordId;
        public float StartTime;
        public float EndTime;
        public float Unk3_FirstNoteTime;
        public float Unk4_LastNoteTime;
    }

    public struct Note : IBinarySerializable
    {
        public uint NoteMask;
        public uint NoteFlags;
        public uint Hash;
        public float Time;
        public byte StringIndex;
        public byte FretId;
        public byte AnchorFretId;
        public byte AnchorWidth;
        public int ChordId;
        public int ChordNotesId;
        public int PhraseId;
        public int PhraseIterationId;
        public short[] FingerPrintId;
        public short NextIterNote;
        public short PrevIterNote;
        public short ParentPrevNote;
        public byte SlideTo;
        public byte SlideUnpitchTo;
        public byte LeftHand;
        public byte Tap;
        public byte PickDirection;
        public byte Slap;
        public byte Pluck;
        public short Vibrato;
        public float Sustain;
        public float MaxBend;
        public BendData32[] BendData;

        public IBinarySerializable Read(BinaryReader reader)
        {
            NoteMask = reader.ReadUInt32();
            NoteFlags = reader.ReadUInt32();
            Hash = reader.ReadUInt32();
            Time = reader.ReadSingle();
            StringIndex = reader.ReadByte();
            FretId = reader.ReadByte();
            AnchorFretId = reader.ReadByte();
            AnchorWidth = reader.ReadByte();
            ChordId = reader.ReadInt32();
            ChordNotesId = reader.ReadInt32();
            PhraseId = reader.ReadInt32();
            PhraseIterationId = reader.ReadInt32();

            FingerPrintId = new short[2];
            for (var i = 0; i < 2; i++) FingerPrintId[i] = reader.ReadInt16();

            NextIterNote = reader.ReadInt16();
            PrevIterNote = reader.ReadInt16();
            ParentPrevNote = reader.ReadInt16();
            SlideTo = reader.ReadByte();
            SlideUnpitchTo = reader.ReadByte();
            LeftHand = reader.ReadByte();
            Tap = reader.ReadByte();
            PickDirection = reader.ReadByte();
            Slap = reader.ReadByte();
            Pluck = reader.ReadByte();
            Vibrato = reader.ReadInt16();
            Sustain = reader.ReadSingle();
            MaxBend = reader.ReadSingle();
            BendData = reader.ReadStructArray<BendData32>();

            return this;
        }
    }

    public struct Arrangement : IBinarySerializable
    {
        public int Difficulty;
        public Anchor[] Anchors;
        public AnchorExtension[] AnchorExtensions;
        public Fingerprint[] Fingerprints1;
        public Fingerprint[] Fingerprints2;
        public Note[] Notes;

        public int PhraseCount;
        public float[] AverageNotesPerIteration;
        public int PhraseIterationCount1;
        public int[] NotesInIteration1;
        public int PhraseIterationCount2;
        public int[] NotesInIteration2;

        public IBinarySerializable Read(BinaryReader reader)
        {
            Difficulty = reader.ReadInt32();

            Anchors = reader.ReadStructArray<Anchor>();
            AnchorExtensions = reader.ReadStructArray<AnchorExtension>();
            Fingerprints1 = reader.ReadStructArray<Fingerprint>();
            Fingerprints2 = reader.ReadStructArray<Fingerprint>();
            Notes = reader.ReadStructArray<Note>();

            PhraseCount = reader.ReadInt32();
            AverageNotesPerIteration = reader.ReadFloatArray(PhraseCount);

            PhraseIterationCount1 = reader.ReadInt32();
            NotesInIteration1 = reader.ReadIntArray(PhraseIterationCount1);

            PhraseIterationCount2 = reader.ReadInt32();
            NotesInIteration2 = reader.ReadIntArray(PhraseIterationCount2);

            return this;
        }
    }
}

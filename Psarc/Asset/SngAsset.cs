using Rocksmith2014PsarcLib.Crypto;
using Rocksmith2014PsarcLib.Psarc.Models.Sng;
using Rocksmith2014PsarcLib.ReaderExtensions;
using System;
using System.IO;
using Action = Rocksmith2014PsarcLib.Psarc.Models.Sng.Action;

namespace Rocksmith2014PsarcLib.Psarc.Asset
{
    public class SngAsset : PsarcAsset
    {
        [Flags]
        public enum AssetFlags
        {
            None = 0,
            Compressed = 1,
            Encrypted = 2
        }

        public Bpm[] BPMs;
        public Phrase[] Phrases;
        public Chord[] Chords;
        public ChordNotes[] ChordNotes;
        public Vocal[] Vocals;
        public SymbolsHeader[] SymbolHeaders;
        public SymbolsTexture[] SymbolTextures;
        public SymbolDefinition[] SymbolDefinitions;
        public PhraseIteration[] PhraseIterations;
        public PhraseExtraInfoByLevel[] PhraseExtraInfo;
        public NLinkedDifficulty[] NLD;
        public Action[] Actions;
        public Event[] Events;
        public Tone[] Tones;
        public Dna[] DNAs;
        public Section[] Sections;
        public Arrangement[] Arrangements;
        public Metadata Metadata;

        public override void ReadFrom(MemoryStream stream)
        {
            base.ReadFrom(stream);

            //Decrypt/uncompress
            using (var dcStream = new DecryptStream(stream, DecryptStream.DecryptMode.SNG, stream.Length))
            {
                //Read beats
                BPMs = dcStream.Reader.ReadStructArray<Bpm>();

                //Read phrases
                Phrases = dcStream.Reader.ReadStructArray<Phrase>();

                //Read chords
                Chords = dcStream.Reader.ReadStructArray<Chord>();

                //Read chord notes
                ChordNotes = dcStream.Reader.ReadStructArray<ChordNotes>();

                //Read vocals
                Vocals = dcStream.Reader.ReadStructArray<Vocal>();

                //Read symbols if there were vocals
                if (Vocals.Length > 0)
                {
                    //Read symbols header
                    SymbolHeaders = dcStream.Reader.ReadStructArray<SymbolsHeader>();

                    //Read symbols texture
                    SymbolTextures = dcStream.Reader.ReadStructArray<SymbolsTexture>();

                    //Read symbols definition
                    SymbolDefinitions = dcStream.Reader.ReadStructArray<SymbolDefinition>();
                }

                //Read phrase iterations
                PhraseIterations = dcStream.Reader.ReadStructArray<PhraseIteration>();

                //Read phrase extra info
                PhraseExtraInfo = dcStream.Reader.ReadStructArray<PhraseExtraInfoByLevel>();

                //Read linked difficulties
                NLD = dcStream.Reader.ReadStructArray<NLinkedDifficulty>();

                //Read actions
                Actions = dcStream.Reader.ReadStructArray<Action>();

                //Read events
                Events = dcStream.Reader.ReadStructArray<Event>();

                //Read tones switch events
                Tones = dcStream.Reader.ReadStructArray<Tone>();

                //Read DNAs
                DNAs = dcStream.Reader.ReadStructArray<Dna>();

                //Read sections
                Sections = dcStream.Reader.ReadStructArray<Section>();

                //Read arrangements
                Arrangements = dcStream.Reader.ReadStructArray<Arrangement>();

                //Read metadata
                Metadata = dcStream.Reader.ReadStruct<Metadata>();
            }
        }
    }
}

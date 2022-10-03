using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rocksmith2014PsarcLib.Psarc.Models.Json
{
    [Serializable]
    public class SongArrangement
    {
        [Serializable]
        public class ArrangementAttributes
        {
            [Serializable]
            public class Properties
            {
                [JsonProperty("represent")]
                public int Represent { get; set; }
                [JsonProperty("bonusArr")]
                public int BonusArr { get; set; }
                [JsonProperty("standardTuning")]
                public int StandardTuning { get; set; }
                [JsonProperty("nonStandardChords")]
                public int NonStandardChords { get; set; }
                [JsonProperty("barreChords")]
                public int BarreChords { get; set; }
                [JsonProperty("powerChords")]
                public int PowerChords { get; set; }
                [JsonProperty("dropDPower")]
                public int DropDPower { get; set; }
                [JsonProperty("openChords")]
                public int OpenChords { get; set; }
                [JsonProperty("fingerPicking")]
                public int FingerPicking { get; set; }
                [JsonProperty("pickDirection")]
                public int PickDirection { get; set; }
                [JsonProperty("doubleStops")]
                public int DoubleStops { get; set; }
                [JsonProperty("palmMutes")]
                public int PalmMutes { get; set; }
                [JsonProperty("harmonics")]
                public int Harmonics { get; set; }
                [JsonProperty("pinchHarmonics")]
                public int PinchHarmonics { get; set; }
                [JsonProperty("hopo")]
                public int Hopo { get; set; }
                [JsonProperty("tremolo")]
                public int Tremolo { get; set; }
                [JsonProperty("slides")]
                public int Slides { get; set; }
                [JsonProperty("unpitchedSlides")]
                public int UnpitchedSlides { get; set; }
                [JsonProperty("bends")]
                public int Bends { get; set; }
                [JsonProperty("tapping")]
                public int Tapping { get; set; }
                [JsonProperty("vibrato")]
                public int Vibrato { get; set; }
                [JsonProperty("fretHandMutes")]
                public int FretHandMutes { get; set; }
                [JsonProperty("slapPop")]
                public int SlapPop { get; set; }
                [JsonProperty("twoFingerPicking")]
                public int TwoFingerPicking { get; set; }
                [JsonProperty("fifthsAndOctaves")]
                public int FifthsAndOctaves { get; set; }
                [JsonProperty("syncopation")]
                public int Syncopation { get; set; }
                [JsonProperty("bassPick")]
                public int BassPick { get; set; }
                [JsonProperty("sustain")]
                public int Sustain { get; set; }
                [JsonProperty("pathLead")]
                public int PathLead { get; set; }
                [JsonProperty("pathRhythm")]
                public int PathRhythm { get; set; }
                [JsonProperty("pathBass")]
                public int PathBass { get; set; }
                [JsonProperty("routeMask")]
                public int RouteMask { get; set; }
            }

            [Serializable]
            public class PhraseIteration
            {
                public int PhraseIndex { get; set; }
                public int MaxDifficulty { get; set; }

                public string Name { get; set; }

                public float StartTime { get; set; }
                public float EndTime { get; set; }
            }

            [Serializable]
            public class Phrase
            {
                public int MaxDifficulty { get; set; }

                public string Name { get; set; }

                public int IterationCount { get; set; }
            }

            [Serializable]
            public class Section
            {
                public string Name { get; set; }
                public string UIName { get; set; }

                public int Number { get; set; }

                public float StartTime { get; set; }
                public float EndTime { get; set; }

                public int StartPhraseIterationIndex { get; set; }
                public int EndPhraseIterationIndex { get; set; }

                public bool IsSolo { get; set; }
            }

            [Serializable]
            public class Tone
            {
                [Serializable]
                public class ToneGear
                {
                    [JsonProperty("Type")]
                    public string Type { get; set; }
                    public string Key { get; set; }
                    public string Category { get; set; }

                    public Dictionary<string, float> KnobValues { get; set; }
                }

                public Dictionary<string, Dictionary<string, ToneGear>> GearList { get; set; }
                public List<string> ToneDescriptors { get; set; }

                public string NameSeparator { get; set; }
                public bool IsCustom { get; set; }
                public string Volume { get; set; }
                public string Key { get; set; }
                public string Name { get; set; }
                public float SortOrder { get; set; }
            }

            public class ArrangementTuning
            {
                [JsonProperty("string0")]
                public int String0 { get; set; }
                [JsonProperty("string1")]
                public int String1 { get; set; }
                [JsonProperty("string2")]
                public int String2 { get; set; }
                [JsonProperty("string3")]
                public int String3 { get; set; }
                [JsonProperty("string4")]
                public int String4 { get; set; }
                [JsonProperty("string5")]
                public int String5 { get; set; }
            }

            public string AlbumArt { get; set; }
            public string AlbumName { get; set; }
            public string AlbumNameSort { get; set; }
            public string ArrangementName { get; set; }

            public Properties ArrangementProperties { get; set; }

            public int ArrangementSort { get; set; }
            public int ArrangementType { get; set; }

            public string ArtistName { get; set; }
            public string ArtistNameSort { get; set; }
            public string BlockAsset { get; set; }

            public float CapoFret { get; set; }
            public float CentOffset { get; set; }

            public bool DLC { get; set; }

            public float DNA_Chords { get; set; }
            public float DNA_Riffs { get; set; }
            public float DNA_Solo { get; set; }

            public List<float> DynamicVisualDensity { get; set; }

            public float EasyMastery { get; set; }

            public string FullName { get; set; }
            public string LastConversionDateTime { get; set; }

            public int? LeaderboardChallengeRating { get; set; }

            public string ManifestUrn { get; set; }

            public int MasterID_PS3 { get; set; }
            public int MasterID_RDV { get; set; }
            public int MasterID_XBox360 { get; set; }
            public int MaxPhraseDifficulty { get; set; }

            public float MediumMastery { get; set; }
            public float NotesEasy { get; set; }
            public float NotesHard { get; set; }
            public float NotesMedium { get; set; }

            public List<PhraseIteration> PhraseIterations { get; set; }
            public List<Phrase> Phrases { get; set; }

            public string PreviewBankPath { get; set; }

            public int RelativeDifficulty { get; set; }

            public float Score_MaxNotes { get; set; }
            public float Score_PNV { get; set; }

            public List<Section> Sections { get; set; }

            public bool Shipping { get; set; }

            public string ShowlightsXML { get; set; }
            public string SKU { get; set; }
            public string SongAsset { get; set; }

            public float SongAverageTempo { get; set; }

            public string SongBank { get; set; }

            public float SongDiffEasy { get; set; }
            public float SongDiffHard { get; set; }
            public float SongDifficulty { get; set; }
            public float SongDiffMed { get; set; }

            public string SongEvent { get; set; }
            public string SongKey { get; set; }

            public float SongLength { get; set; }

            public string SongName { get; set; }
            public string SongNameSort { get; set; }

            public float SongOffset { get; set; }

            public int SongPartition { get; set; }

            public string SongXml { get; set; }

            public int SongYear { get; set; }

            public long TargetScore { get; set; }

            public Dictionary<string, Dictionary<string, List<int>>> Techniques { get; set; }

            public string Tone_A { get; set; }
            public string Tone_B { get; set; }
            public string Tone_Base { get; set; }
            public string Tone_C { get; set; }
            public string Tone_D { get; set; }
            public string Tone_Multiplayer { get; set; }

            //public List<Tone> Tones { get; set; }

            public ArrangementTuning Tuning { get; set; }

            public string PersistentID { get; set; }
        }

        public ArrangementAttributes Attributes { get; private set; }

        /// <summary>
        /// This is probably illegal in more than 100 countries, but who needs json converters when you can do this
        /// </summary>
        public Dictionary<string, Dictionary<string, ArrangementAttributes>> Entries
        {
            set
            {
                Attributes = value.First().Value.First().Value;
            }
        }

        public string ModelName { get; set; }
        public int IterationVersion { get; set; }
        public string InsertRoot { get; set; }
    }
}

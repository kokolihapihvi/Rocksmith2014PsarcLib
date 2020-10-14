using System.IO;

namespace Rocksmith2014PsarcLib.Psarc.Asset
{
    public class TextPsarcAsset : PsarcAsset
    {
        /// <summary>
        /// The text contents of this asset
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Line separators for the <see cref="Lines"/> property
        /// </summary>
        public string[] LineSeparators { get; set; } = new string[] { "\r\n", "\n" };

        /// <summary>
        /// The text contents of this asset, split into an array of lines.
        /// <para></para>
        /// Uses <see cref="LineSeparators"/>
        /// </summary>
        public string[] Lines
        {
            get
            {
                return Text.Split(LineSeparators, System.StringSplitOptions.None);
            }
        }

        public override void ReadFrom(MemoryStream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                Text = reader.ReadToEnd();
            }
        }
    }
}

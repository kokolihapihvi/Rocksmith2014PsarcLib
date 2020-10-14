namespace Rocksmith2014PsarcLib.Psarc.Models
{
    /// <summary>
    /// For CDLC provides information about Toolkit version,
    /// the package author, the package version and a comment.
    /// </summary>
    //https://github.com/rscustom/rocksmith-custom-song-toolkit/blob/master/RocksmithToolkitLib/DLCPackage/ToolkitInfo.cs
    public class ToolkitInfo
    {
        public string ToolkitVersion { get; set; }
        public string PackageAuthor { get; set; }
        public string PackageVersion { get; set; }
        public string PackageComment { get; set; }
        public string PackageRating { get; set; }

        /// <summary>
        /// Was the psarc file created with the rs custom toolkit
        /// </summary>
        public bool IsCustom
        {
            get
            {
                return ToolkitVersion != "Null";
            }
        }

        public ToolkitInfo()
        {
            ToolkitVersion = "Null";
            PackageAuthor = "Ubisoft";
            PackageVersion = "0";
            PackageComment = "Null";
            PackageRating = "5";
        }
    }
}

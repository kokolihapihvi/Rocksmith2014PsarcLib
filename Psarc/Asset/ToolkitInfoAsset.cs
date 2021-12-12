using Rocksmith2014PsarcLib.Psarc.Models;
using System;
using System.IO;

namespace Rocksmith2014PsarcLib.Psarc.Asset
{
    public class ToolkitInfoAsset : TextPsarcAsset
    {
        public ToolkitInfo ToolkitInfo { get; private set; } = new ToolkitInfo();

        public override void ReadFrom(MemoryStream stream)
        {
            base.ReadFrom(stream);

            //Single line means only toolkit version is included
            if (Lines.Length == 1)
            {
                ToolkitInfo.ToolkitVersion = Lines[0].Trim();
            }
            else
            {
                //Go through all lines and parse "key: value" pairs
                foreach (var line in Lines)
                {
                    var tokens = line.Split(':');

                    if (tokens.Length != 2)
                    {
                        Console.WriteLine("  Notice: Unrecognized line in toolkit.version: {0}", line);
                        continue;
                    }

                    var key = tokens[0].Trim().ToLower();

                    switch (key)
                    {
                        case "toolkit version":
                            ToolkitInfo.ToolkitVersion = tokens[1];
                            break;
                        case "package author":
                            ToolkitInfo.PackageAuthor = tokens[1];
                            break;
                        case "package version":
                            ToolkitInfo.PackageVersion = tokens[1];
                            break;
                        case "package comment":
                            ToolkitInfo.PackageComment = tokens[1];
                            break;
                        case "package rating":
                            ToolkitInfo.PackageRating = tokens[1];
                            break;
                        default:
                            Console.WriteLine("  Notice: Unknown key in toolkit.version: {0}", key);
                            break;
                    }
                }
            }
        }
    }
}

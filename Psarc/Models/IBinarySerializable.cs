using System.IO;

namespace Rocksmith2014PsarcLib.Psarc.Models
{
    public interface IBinarySerializable
    {
        IBinarySerializable Read(BinaryReader reader);
    }
}

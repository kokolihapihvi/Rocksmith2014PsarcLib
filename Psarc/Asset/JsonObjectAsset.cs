using Newtonsoft.Json;
using System.IO;

namespace Rocksmith2014PsarcLib.Psarc.Asset
{
    public class JsonObjectAsset<T> : PsarcAsset
    {
        public T JsonObject { get; private set; }

        private static JsonSerializer Serializer { get; } = new JsonSerializer
        {
            TypeNameHandling = TypeNameHandling.None,
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore
        };

        public override void ReadFrom(MemoryStream stream)
        {
            base.ReadFrom(stream);
            
            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);

            JsonObject = Serializer.Deserialize<T>(jsonReader);
        }
    }
}
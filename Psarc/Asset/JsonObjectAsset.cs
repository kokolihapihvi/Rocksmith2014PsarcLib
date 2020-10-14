using Newtonsoft.Json;
using System.IO;

namespace Rocksmith2014PsarcLib.Psarc.Asset
{
    public class JsonObjectAsset<T> : PsarcAsset
    {
        public T JsonObject { get; private set; }

        public override void ReadFrom(MemoryStream stream)
        {
            base.ReadFrom(stream);

            using (var reader = new StreamReader(stream))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    var serializer = new JsonSerializer
                    {
                        TypeNameHandling = TypeNameHandling.None,
                        MetadataPropertyHandling = MetadataPropertyHandling.Ignore
                    };

                    JsonObject = serializer.Deserialize<T>(jsonReader);
                }
            }
        }
    }
}

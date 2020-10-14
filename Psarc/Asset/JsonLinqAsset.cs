using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Rocksmith2014PsarcLib.Psarc.Asset
{
    public class JsonLinqAsset : PsarcAsset
    {
        public JObject JsonObject { get; private set; }

        public override void ReadFrom(MemoryStream stream)
        {
            base.ReadFrom(stream);

            using (var reader = new StreamReader(stream))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    var serializer = new JsonSerializer();

                    JsonObject = serializer.Deserialize<JObject>(jsonReader);
                }
            }
        }
    }
}

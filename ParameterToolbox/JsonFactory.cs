using System.IO;
using Newtonsoft.Json;

namespace ParameterToolbox
{
    public class JsonFactory : IJson
    {
        public JsonSerializer CreateJsonSerializer()
        {
            return new JsonSerializer {Formatting = Formatting.Indented};
        }

        public JsonWriter CreateJsonWriter(StreamWriter streamWriter)
        {
            return new JsonTextWriter(streamWriter);
        }

        public JsonReader CreateJsonReader(StreamReader streamReader)
        {
            return new JsonTextReader(streamReader);
        }
    }
}
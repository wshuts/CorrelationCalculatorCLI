using System.IO;
using Newtonsoft.Json;

namespace ParameterToolbox
{
    public interface IJson
    {
        public JsonSerializer CreateJsonSerializer();
        public JsonWriter CreateJsonWriter(StreamWriter streamWriter);
        public JsonReader CreateJsonReader(StreamReader streamReader);
    }
}
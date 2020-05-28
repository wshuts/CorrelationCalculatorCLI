using Newtonsoft.Json;

namespace ParameterToolbox
{
    public static class JsonFactory
    {
        public static JsonSerializer CreateJsonSerializer()
        {
            return new JsonSerializer {Formatting = Formatting.Indented};
        }
    }
}
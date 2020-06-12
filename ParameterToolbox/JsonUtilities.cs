namespace ParameterToolbox
{
    public static class JsonUtilities
    {
        public static IStream StreamFactory { get; set; } = new StreamFactory();
        public static IJson JsonFactory { get; set; } = new JsonFactory();

        public static void Serialize(string fullFileName, object source)
        {
            using var streamWriter = StreamFactory.CreateStreamWriter(fullFileName);
            using var jsonWriter = JsonFactory.CreateJsonWriter(streamWriter);
            var jsonSerializer = JsonFactory.CreateJsonSerializer();
            jsonSerializer.Serialize(jsonWriter, source);
        }

        public static T Deserialize<T>(string fullFileName)
        {
            using var streamReader = StreamFactory.CreateStreamReader(fullFileName);
            using var jsonReader = JsonFactory.CreateJsonReader(streamReader);
            var jsonSerializer = JsonFactory.CreateJsonSerializer();
            return jsonSerializer.Deserialize<T>(jsonReader);
        }
    }
}
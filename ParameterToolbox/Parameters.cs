using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ParameterToolbox
{
    public class Parameters
    {
        private const string RelativeParametersFileName = @"Parameters\Parameters001.json";

        public Parameters()
        {
            StreamFactory = new StreamFactory();
            JsonFactory = new JsonFactory();
        }

        public IStream StreamFactory { get; }
        public IJson JsonFactory { get; }

        public DateTime EndDate { get; set; }
        public IList<Fund> Funds { get; set; } = new List<Fund>();
        public DateTime StartDate { get; set; }
        [JsonIgnore] public static string FullParametersFileName { get; private set; }

        public void Serialize()
        {
            InitializeFullParametersFileName();
            using var streamWriter = StreamFactory.CreateStreamWriter(FullParametersFileName);
            using var jsonWriter = JsonFactory.CreateJsonWriter(streamWriter);
            var jsonSerializer = JsonFactory.CreateJsonSerializer();
            jsonSerializer.Serialize(jsonWriter, this);
        }

        private static void InitializeFullParametersFileName()
        {
            var currentDomain = AppDomain.CurrentDomain;
            var baseDirectory = currentDomain.BaseDirectory;
            FullParametersFileName = Path.Combine(baseDirectory ?? string.Empty, RelativeParametersFileName);
        }

        public Parameters Deserialize()
        {
            InitializeFullParametersFileName();
            using var streamReader = StreamFactory.CreateStreamReader(FullParametersFileName);
            using var jsonReader = JsonFactory.CreateJsonReader(streamReader);
            var jsonSerializer = JsonFactory.CreateJsonSerializer();
            return jsonSerializer.Deserialize<Parameters>(jsonReader);
        }
    }
}
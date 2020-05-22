using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ParameterToolbox
{
    public class Parameters
    {
        private const string RelativeParametersFileName = @"Parameters\Parameters001.json";
        private static readonly JsonSerializer Serializer = new JsonSerializer {Formatting = Formatting.Indented};

        public DateTime EndDate { get; set; }
        public IList<Fund> Funds { get; set; } = new List<Fund>();
        public DateTime StartDate { get; set; }
        [JsonIgnore] public static string FullParametersFileName { get; private set; }

        public void Serialize()
        {
            InitializeFullParametersFileName();

            using var sw = new StreamWriter(FullParametersFileName);
            using JsonWriter writer = new JsonTextWriter(sw);

            Serializer.Serialize(writer, this);
        }

        private static void InitializeFullParametersFileName()
        {
            var currentDomain = AppDomain.CurrentDomain;
            var baseDirectory = currentDomain.BaseDirectory;
            FullParametersFileName = Path.Combine(baseDirectory ?? string.Empty, RelativeParametersFileName);
        }

        public static Parameters Deserialize()
        {
            InitializeFullParametersFileName();

            using var sr = new StreamReader(FullParametersFileName);
            using JsonReader reader = new JsonTextReader(sr);

            return Serializer.Deserialize<Parameters>(reader);
        }
    }
}
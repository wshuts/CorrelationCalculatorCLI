using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ParameterToolbox
{
    public class Parameters
    {
        public Parameters()
        {
            serializer = new JsonSerializer {Formatting = Formatting.Indented};
        }

        private const string RelativeParametersFileName = @"Parameters\Parameters001.json";
        private readonly JsonSerializer serializer;

        public DateTime EndDate { get; set; }
        public IList Funds { get; set; } = new List<Fund>();
        public DateTime StartDate { get; set; }
        public string FullParametersFileName { get; private set; }

        public void Serialize()
        {
            InitializeFullParametersFileName();

            using var sw = new StreamWriter(FullParametersFileName);
            using JsonWriter writer = new JsonTextWriter(sw);

            serializer.Serialize(writer, this);
        }

        private void InitializeFullParametersFileName()
        {
            var currentDomain = AppDomain.CurrentDomain;
            var baseDirectory = currentDomain.BaseDirectory;
            FullParametersFileName = Path.Combine(baseDirectory ?? string.Empty, RelativeParametersFileName);
        }

        public Parameters Deserialize()
        {
            InitializeFullParametersFileName();

            using var sr = new StreamReader(FullParametersFileName);
            using JsonReader reader = new JsonTextReader(sr);

            return serializer.Deserialize<Parameters>(reader);
        }
    }
}
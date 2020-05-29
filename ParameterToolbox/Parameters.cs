using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ParameterToolbox
{
    public class Parameters
    {
        private const string RelativeParametersFileName = @"Parameters\Parameters001.json";

        public DateTime EndDate { get; set; }
        public IList<Fund> Funds { get; set; } = new List<Fund>();
        public DateTime StartDate { get; set; }
        [JsonIgnore] public static string FullParametersFileName { get; private set; }

        public void Serialize()
        {
            SetFullParametersFileName();
            JsonUtilities.Serialize(FullParametersFileName, this);
        }

        private static void SetFullParametersFileName()
        {
            FullParametersFileName = FileUtilities.ConvertToAbsolutePath(RelativeParametersFileName);
        }

        public Parameters Deserialize()
        {
            SetFullParametersFileName();
            return JsonUtilities.Deserialize<Parameters>(FullParametersFileName);
        }
    }
}
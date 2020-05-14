using System.IO;
using System.Runtime.Serialization.Json;

namespace CalculationParameterReader
{
    public class AllCalculationParametersReader
    {
        public CalculationParameters[] AllCalculationParameters { get; private set; }
        public FileStream CalculationParametersFileStream { get; set; }

        public void Initialize()
        {            
            DeserializeAllCalculationParameters();
        }

        private void DeserializeAllCalculationParameters()
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(CalculationParameters[]));
            CalculationParametersFileStream.Position = 0;
            AllCalculationParameters = (CalculationParameters[])jsonSerializer.ReadObject(CalculationParametersFileStream);
        }
    }
}
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ParameterToolbox
{
    public class Fund
    {
        public Fund(string fundName, string fundNumber)
        {
            FundName = fundName;
            FundNumber = fundNumber;
        }

        public string FundName { get; set; }
        public string FundNumber { get; set; }
        [JsonIgnore] public IDictionary<string, double> CorrelationCoefficients { get; set; } = new SortedList<string, double>();
        [JsonIgnore] public IList<double> PriceVector { get; set; } = new List<double>();
        [JsonIgnore] public IDictionary<string, string> PriceInfo { get; set; } = new SortedList<string, string>();

        public void InitializePriceVector()
        {
            PriceVector.Clear();
            foreach (string price in PriceInfo.Values) PriceVector.Add(double.Parse(price));
        }
    }
}
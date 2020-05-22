using System.Collections;
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
        [JsonIgnore] public IDictionary CorrelationCoefficients { get; set; } = new SortedList();
        [JsonIgnore] public IList<double> PriceVector { get; set; } = new List<double>();
        [JsonIgnore] public IDictionary PriceInfo { get; set; } = new SortedList();

        public void InitializePriceVector()
        {
            PriceVector.Clear();
            foreach (string price in PriceInfo.Values) PriceVector.Add(double.Parse(price));
        }
    }
}
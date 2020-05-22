using System.Collections;
using System.Collections.Generic;

namespace ParameterToolbox
{
    public class Fund
    {
        public string FundName { get; set; }
        public string FundNumber { get; set; }
        public IDictionary CorrelationCoefficients { get; set; } = new SortedList();
        public IList<double> PriceVector { get; set; }

        public Fund(string fundName, string fundNumber)
        {
            FundName = fundName;
            FundNumber = fundNumber;
        }
    }
}
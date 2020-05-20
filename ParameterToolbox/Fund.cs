using System.Collections;

namespace ParameterToolbox
{
    public class Fund
    {
        public string FundName { get; set; }
        public string FundNumber { get; set; }
        public IDictionary CorrelationCoefficients { get; set; }
        public IList Vector { get; set; }

        public Fund(string fundName, string fundNumber)
        {
            FundName = fundName;
            FundNumber = fundNumber;
        }
    }
}
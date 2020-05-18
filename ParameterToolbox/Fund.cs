namespace ParameterToolbox
{
    public class Fund
    {
        public string FundName { get; set; }
        public string FundNumber { get; set; }

        public Fund(string fundName, string fundNumber)
        {
            FundName = fundName;
            FundNumber = fundNumber;
        }
    }
}
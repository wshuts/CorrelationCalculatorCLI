using System.Collections;

namespace PriceCorrelationCalculator
{
    public class Calculator
    {
        public void CalculateCorrelation()
        {
            throw new System.NotImplementedException();
        }

        public void InitializeFundTable()
        {
            PriceServer.GetFundTable();

            FundTable.Clear();
            foreach (var dictionaryEntry in PriceServer.FundTable)
            {
                FundTable.Add(dictionaryEntry.Key, dictionaryEntry.Value);
            }

            RemoveMoneyMarketFunds();
        }

        private void RemoveMoneyMarketFunds()
        {
            throw new System.NotImplementedException();
        }

        public PriceServer PriceServer { get; set; }

        public IDictionary FundTable { get; set; }
    }
}
using System;
using System.Collections;
using System.IO;
using CalculationParameterReader;

namespace PriceCorrelationCalculator
{
    public class Calculator
    {
        private readonly FileStream calculationParametersFileStream;
        private CalculationParameters[] allCalculationParameters;

        public Calculator(FileStream calculationParametersFileStream)
        {
            this.calculationParametersFileStream = calculationParametersFileStream;
            PriceServer = new PriceServer();
        }

        public PriceServer PriceServer { get; set; }

        public IDictionary FundTable { get; set; } = new SortedList();

        public IList SelectedFunds { get; set; } = new ArrayList();

        public void CalculateCorrelation()
        {
            throw new NotImplementedException();
        }

        public void InitializeFundTable()
        {
            PriceServer.GetFundTable();

            FundTable.Clear();
            foreach (DictionaryEntry dictionaryEntry in PriceServer.FundTable)
                FundTable.Add(dictionaryEntry.Key, dictionaryEntry.Value);

            RemoveMoneyMarketFunds();
        }

        private void RemoveMoneyMarketFunds()
        {
            var keysToRemove = new ArrayList();
            foreach (string fundName in FundTable.Keys)
                if (fundName.IndexOf("Money Market", StringComparison.Ordinal) != -1)
                    keysToRemove.Add(fundName);

            foreach (string fundName in keysToRemove) FundTable.Remove(fundName);
        }

        public void ReadCalculationParameters()
        {
            InitializeAllCalculationParameters();
        }

        private void InitializeAllCalculationParameters()
        {
            var allCalculationParametersReader =
                new AllCalculationParametersReader {CalculationParametersFileStream = calculationParametersFileStream};
            allCalculationParametersReader.Initialize();
            allCalculationParameters = allCalculationParametersReader.AllCalculationParameters;
        }

        public void SerializeFundTable()
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ParameterToolbox;

namespace PriceCorrelationCalculator
{
    public class Calculator
    {
        private const string RelativeFundTableFileName = @"FundTable\FundTable.json";
        private readonly JsonSerializer serializer;

        public Calculator()
        {
            PriceServer = new PriceServer();
            serializer = new JsonSerializer {Formatting = Formatting.Indented};
        }

        public string FullFundTableFileName { get; private set; }

        public PriceServer PriceServer { get; set; }

        public IDictionary FundTable { get; set; } = new SortedList();

        public IList<Fund> Funds { get; set; } = new List<Fund>();

        public DateTime EndDate { get; set; }

        public DateTime StartDate { get; set; }

        public void CalculateCorrelation()
        {
            foreach(Fund firstFund in Funds)
            {
                firstFund.CorrelationCoefficients.Clear();
                foreach(Fund secondFund in Funds)
                {
                    var correlationCoefficient=Statistics.Correlation(firstFund.Vector,secondFund.Vector);
                    firstFund.CorrelationCoefficients.Add(secondFund.FundName,correlationCoefficient);
                }
            }
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
            var parameters = Parameters.Deserialize();
            Funds = parameters.Funds;
            StartDate = parameters.StartDate;
            EndDate = parameters.EndDate;
        }

        public void SerializeFundTable()
        {
            InitializeFullFundTableFileName();

            using var sw = new StreamWriter(FullFundTableFileName);
            using JsonWriter writer = new JsonTextWriter(sw);

            serializer.Serialize(writer, FundTable);
        }

        private void InitializeFullFundTableFileName()
        {
            var currentDomain = AppDomain.CurrentDomain;
            var baseDirectory = currentDomain.BaseDirectory;
            FullFundTableFileName = Path.Combine(baseDirectory ?? string.Empty, RelativeFundTableFileName);
        }

        public IDictionary DeserializeFundTable()
        {
            InitializeFullFundTableFileName();

            using var sr = new StreamReader(FullFundTableFileName);
            using JsonReader reader = new JsonTextReader(sr);

            return serializer.Deserialize<IDictionary>(reader);
        }
    }
}
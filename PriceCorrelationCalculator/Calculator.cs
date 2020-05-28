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
        private const string RelativeOutputFileName = @"Output\CorrelationCoefficients.xls";
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

        public string FullOutputFileName { get; set; }

        public void CalculateCorrelation()
        {
            foreach (var firstFund in Funds)
            {
                firstFund.CorrelationCoefficients.Clear();
                foreach (var secondFund in Funds)
                {
                    var correlationCoefficient = Statistics.Correlation(firstFund.PriceVector, secondFund.PriceVector);
                    firstFund.CorrelationCoefficients.Add(secondFund.FundName, correlationCoefficient);
                }
            }
        }

        public void InitializeFundTable()
        {
            PriceServer.GetFundTable();

            FundTable.Clear();
            foreach (var (key, value) in PriceServer.FundTable)
                FundTable.Add(key, value);

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
            var calculationParameters = new Parameters(JsonSerializer.Create());
            var parameters = calculationParameters.Deserialize();
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

        private void InitializeFullOutputFileName()
        {
            var currentDomain = AppDomain.CurrentDomain;
            var baseDirectory = currentDomain.BaseDirectory;
            FullOutputFileName = Path.Combine(baseDirectory ?? string.Empty, RelativeOutputFileName);
        }

        public IDictionary DeserializeFundTable()
        {
            InitializeFullFundTableFileName();

            using var sr = new StreamReader(FullFundTableFileName);
            using JsonReader reader = new JsonTextReader(sr);

            return serializer.Deserialize<IDictionary>(reader);
        }

        public void RetrievePriceInfo()
        {
            foreach (var fund in Funds)
            {
                var fundNumber = fund.FundNumber;
                PriceServer.RetrievePriceInfo(fundNumber, StartDate, EndDate);

                fund.PriceInfo.Clear();
                foreach (var (key, value) in PriceServer.PriceInfo) fund.PriceInfo.Add(key, value);

                InitializePriceVector(fund);
            }
        }

        private static void InitializePriceVector(Fund fund)
        {
            fund.InitializePriceVector();
        }

        public void GenerateOutputFile()
        {
            InitializeFullOutputFileName();

            using var sw = new StreamWriter(FullOutputFileName);
            sw.WriteLine("\t" + StartDate.ToShortDateString() + "\t" + EndDate.ToShortDateString());

            foreach (var fund in Funds) sw.Write("\t" + fund.FundName);
            sw.Write("\n");

            foreach (var fund in Funds)
            {
                sw.Write(fund.FundName);
                foreach (var correlationCoefficient in fund.CorrelationCoefficients.Values)
                    sw.Write("\t" + correlationCoefficient.ToString("0.00"));
                sw.Write("\n");
            }
        }
    }
}
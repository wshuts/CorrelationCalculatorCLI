using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ParameterToolbox;

namespace PriceCorrelationCalculator
{
    public class Calculator
    {
        private const string RelativeFundTableFileName = @"FundTable\FundTable.json";
        private const string RelativeOutputFileName = @"Output\CorrelationCoefficients.xls";

        public Calculator()
        {
            PriceServer = new PriceServer();
            StreamFactory = new StreamFactory();
            JsonFactory = new JsonFactory();
        }

        public IStream StreamFactory { get; }
        public IJson JsonFactory { get; }
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
            var calculationParameters = new Parameters();
            var parameters = calculationParameters.Deserialize();
            Funds = parameters.Funds;
            StartDate = parameters.StartDate;
            EndDate = parameters.EndDate;
        }

        public void SerializeFundTable()
        {
            SetFullFundTableFileName();
            using var streamWriter = StreamFactory.CreateStreamWriter(FullFundTableFileName);
            using var jsonWriter = JsonFactory.CreateJsonWriter(streamWriter);
            var jsonSerializer = JsonFactory.CreateJsonSerializer();
            jsonSerializer.Serialize(jsonWriter, FundTable);
        }

        private void SetFullFundTableFileName()
        {
            FullFundTableFileName = FileUtilities.ConvertToAbsolutePath(RelativeFundTableFileName);
        }

        private void SetFullOutputFileName()
        {
            FullOutputFileName = FileUtilities.ConvertToAbsolutePath(RelativeOutputFileName);
        }

        public IDictionary DeserializeFundTable()
        {
            SetFullFundTableFileName();
            using var streamReader = StreamFactory.CreateStreamReader(FullFundTableFileName);
            using var jsonReader = JsonFactory.CreateJsonReader(streamReader);
            var jsonSerializer = JsonFactory.CreateJsonSerializer();
            return jsonSerializer.Deserialize<IDictionary>(jsonReader);
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
            SetFullOutputFileName();

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
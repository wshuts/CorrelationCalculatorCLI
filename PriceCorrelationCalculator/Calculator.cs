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
            StreamFactory = new StreamFactory();

            PriceServerFactory = new PriceServerFactory();
            PriceServer = PriceServerFactory.CreatePriceServer();
        }

        public PriceServer PriceServer { get; }

        public string FullFundTableFileName { get; private set; }
        public IPriceServerFactory PriceServerFactory { get; set; }
        public IStreamFactory StreamFactory { get; }
        public IDictionary FundTable { get; set; } = new SortedList();
        public IList<Fund> Funds { get; set; } = new List<Fund>();
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public string FullOutputFileName { get; set; }

        public void CalculateCorrelation()
        {
            foreach (var firstFund in Funds)
            {
                var firstFundCorrelationCoefficients = firstFund.CorrelationCoefficients;
                firstFundCorrelationCoefficients.Clear();

                foreach (var secondFund in Funds)
                {
                    var firstFundPriceVector = firstFund.PriceVector;
                    var secondFundPriceVector = secondFund.PriceVector;
                    var correlationCoefficient = Statistics.Correlation(firstFundPriceVector, secondFundPriceVector);

                    var secondFundFundName = secondFund.FundName;
                    firstFundCorrelationCoefficients.Add(secondFundFundName, correlationCoefficient);
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
            JsonUtilities.Serialize(FullFundTableFileName, FundTable);
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
            return JsonUtilities.Deserialize<IDictionary>(FullFundTableFileName);
        }

        public void RetrievePriceInfo()
        {
            foreach (var fund in Funds)
            {
                var fundNumber = fund.FundNumber;
                PriceServer.RetrievePriceInfo(fundNumber, StartDate, EndDate);

                var fundPriceInfo = fund.PriceInfo;
                fundPriceInfo.Clear();
                foreach (var (key, value) in PriceServer.PriceInfo) fundPriceInfo.Add(key, value);

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
            using var streamWriter = StreamFactory.CreateStreamWriter(FullOutputFileName);

            WriteDates(streamWriter);
            WriteColumnHeadings(streamWriter);
            WriteRows(streamWriter);
        }

        private void WriteDates(TextWriter streamWriter)
        {
            streamWriter.WriteLine("\t" + StartDate.ToShortDateString() + "\t" + EndDate.ToShortDateString());
        }

        private void WriteColumnHeadings(TextWriter streamWriter)
        {
            foreach (var fund in Funds) streamWriter.Write("\t" + fund.FundName);
            streamWriter.Write("\n");
        }

        private void WriteRows(TextWriter streamWriter)
        {
            foreach (var fund in Funds)
            {
                streamWriter.Write(fund.FundName);
                foreach (var correlationCoefficient in fund.CorrelationCoefficients.Values)
                    streamWriter.Write("\t" + correlationCoefficient.ToString("0.00"));
                streamWriter.Write("\n");
            }
        }
    }
}
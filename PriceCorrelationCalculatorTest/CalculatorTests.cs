using NUnit.Framework;
using ParameterToolbox;
using ParameterToolboxTest;
using PriceCorrelationCalculator;

namespace PriceCorrelationCalculatorTest
{
    [TestFixture]
    public class CalculatorTests
    {
        [SetUp]
        public void Setup()
        {
            Calculator = new Calculator();
            ParametersForTesting = ParametersFactory.CreateParametersForTesting();
        }

        public Calculator Calculator { get; private set; }
        public Parameters ParametersForTesting { get; private set; }

        [Test]
        public void CalculateCorrelationIsImplemented()
        {
            Assert.DoesNotThrow(Calculator.CalculateCorrelation);
        }

        [Test]
        public void CanReadCalculationParameters()
        {
            Calculator.ReadCalculationParameters(ParametersForTesting);

            var funds = Calculator.Funds;
            var fund = funds[0];
            Assert.NotNull(fund);

            const string expected = "Windsor II Fund Adm      ";
            var actual = fund.FundName;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanRetrievePriceInfo()
        {
            Calculator.ReadCalculationParameters(ParametersForTesting);

            Calculator.RetrievePriceInfo();

            var priceServer = Calculator.PriceServer;
            var priceInfo = priceServer.PriceInfo;
            const int expected = 84;
            var actual = priceInfo.Count;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanSerializeFundTable()
        {
            Calculator.InitializeFundTable();
            Calculator.SerializeFundTable();

            var fundTable = Calculator.DeserializeFundTable();

            Assert.NotNull(fundTable);

            const string expected = "0573";
            var actual = fundTable["Windsor II Fund Adm      "];
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FundTableGetsInitialized()
        {
            Calculator.InitializeFundTable();
            const int expected = 0;
            var actual = Calculator.FundTable.Count;
            Assert.AreNotEqual(expected, actual);
        }

        [Test]
        public void GenerateOutputFileIsImplemented()
        {
            Calculator.ReadCalculationParameters(ParametersForTesting);
            Calculator.RetrievePriceInfo();
            Calculator.CalculateCorrelation();

            Assert.DoesNotThrow(Calculator.GenerateOutputFile);
        }

        [Test]
        public void VerifyCorrelationCoefficients()
        {
            Calculator.ReadCalculationParameters(ParametersForTesting);
            Calculator.RetrievePriceInfo();
            Calculator.CalculateCorrelation();

            var firstFund = Calculator.Funds[0];
            Assert.NotNull(firstFund);

            var secondFund = Calculator.Funds[1];
            Assert.NotNull(secondFund);

            var firstFundCorrelationCoefficients = firstFund.CorrelationCoefficients;

            const double expected1 = 1.0;
            var actual1 = firstFundCorrelationCoefficients[firstFund.FundName];
            Assert.AreEqual(expected1, actual1);

            const double expected2 = 0.9967;
            var actual2 = firstFundCorrelationCoefficients[secondFund.FundName];
            const double delta = 0.0001;
            Assert.AreEqual(expected2, actual2, delta);
        }
    }
}
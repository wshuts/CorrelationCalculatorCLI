using NUnit.Framework;
using PriceCorrelationCalculator;

namespace PriceCorrelationCalculatorTest
{
    [TestFixture]
    public class CalculatorTests
    {
        [SetUp]
        public void Setup()
        {
            calculator = new Calculator();
        }

        private Calculator calculator;

        [Test]
        public void CalculateCorrelationIsImplemented()
        {
            Assert.DoesNotThrow(calculator.CalculateCorrelation);
        }

        [Test]
        public void CanReadCalculationParameters()
        {
            calculator.ReadCalculationParameters();

            var funds = calculator.Funds;
            var fund = funds[0];
            Assert.NotNull(fund);

            const string expected = "Windsor II Fund Adm      ";
            var actual = fund.FundName;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanRetrievePriceInfo()
        {
            calculator.ReadCalculationParameters();

            calculator.RetrievePriceInfo();

            var priceServer = calculator.PriceServer;
            var priceInfo = priceServer.PriceInfo;
            const int expected = 84;
            var actual = priceInfo.Count;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanSerializeFundTable()
        {
            calculator.InitializeFundTable();
            calculator.SerializeFundTable();

            var fundTable = calculator.DeserializeFundTable();

            Assert.NotNull(fundTable);

            const string expected = "0573";
            var actual = fundTable["Windsor II Fund Adm      "];
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FundTableGetsInitialized()
        {
            calculator.InitializeFundTable();
            const int expected = 0;
            var actual = calculator.FundTable.Count;
            Assert.AreNotEqual(expected, actual);
        }

        [Test]
        public void GenerateOutputFileIsImplemented()
        {
            calculator.ReadCalculationParameters();
            calculator.RetrievePriceInfo();
            calculator.CalculateCorrelation();

            Assert.DoesNotThrow(calculator.GenerateOutputFile);
        }

        [Test]
        public void VerifyCorrelationCoefficients()
        {
            calculator.ReadCalculationParameters();
            calculator.RetrievePriceInfo();
            calculator.CalculateCorrelation();

            var firstFund = calculator.Funds[0];
            Assert.NotNull(firstFund);

            var secondFund = calculator.Funds[1];
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
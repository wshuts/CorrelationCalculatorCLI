using System.Diagnostics;
using NUnit.Framework;
using ParameterToolbox;
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

            var fund = (Fund)funds[0];
            Assert.NotNull(fund);

            const string expected = "Windsor II Fund Adm      ";
            var actual = fund.FundName;
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
    }
}
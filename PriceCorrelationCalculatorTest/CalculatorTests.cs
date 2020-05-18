using System.IO;
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
            const string parameterFileName = ParametersFolder + ParameterFileNameArg;
            ParametersFileStream = new FileStream(parameterFileName, FileMode.Open, FileAccess.Read);
            calculator = new Calculator(ParametersFileStream);
        }

        private const string ParametersFolder = @"Parameters\";
        private const string ParameterFileNameArg = @"Parameters001.json";
        private Calculator calculator;

        public static FileStream ParametersFileStream { get; private set; }

        [Test]
        public void CalculateCorrelationIsImplemented()
        {
            Assert.DoesNotThrow(calculator.CalculateCorrelation);
        }

        [Test]
        public void CanReadCalculationParameters()
        {
            calculator.ReadCalculationParameters();
            const int expected = 5;
            var actual = calculator.SelectedFunds.Count;
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
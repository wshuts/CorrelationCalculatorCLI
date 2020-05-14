using System.IO;
using NUnit.Framework;
using PriceCorrelationCalculator;

namespace PriceCorrelationCalculatorTest
{
    public class CalculatorTests
    {
        private const string CalculationParametersFolder = @"CalculationParameters\";
        private const string ParameterFileNameArg = @"CalculationParameters001.json";
        private Calculator calculator;

        public static FileStream CalculationParametersFileStream { get; private set; }

        [SetUp]
        public void Setup()
        {
            const string parameterFileName = CalculationParametersFolder + ParameterFileNameArg;
            CalculationParametersFileStream = new FileStream(parameterFileName, FileMode.Open, FileAccess.Read);
            calculator = new Calculator(CalculationParametersFileStream);
        }

        [Test]
        public void CalculateCorrelationIsImplemented()
        {
            Assert.DoesNotThrow(calculator.CalculateCorrelation);
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
            Assert.DoesNotThrow(calculator.SerializeFundTable);
        }
    }
}
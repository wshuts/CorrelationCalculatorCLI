using NUnit.Framework;
using PriceCorrelationCalculator;

namespace PriceCorrelationCalculatorTest
{
    public class CalculatorTests
    {
        private Calculator calculator;

        [SetUp]
        public void Setup()
        {
            calculator = new Calculator();
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
    }
}
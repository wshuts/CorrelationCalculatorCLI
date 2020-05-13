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
    }
}
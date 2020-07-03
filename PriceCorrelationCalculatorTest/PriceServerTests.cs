using NUnit.Framework;
using PriceCorrelationCalculator;

namespace PriceCorrelationCalculatorTest
{
    [TestFixture]
    public class PriceServerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        private const string ExpectedFundTableQuery =
            "https://personal.vanguard.com/us/funds/tools/pricehistorysearch?Sc=1";

        [Test]
        public void CanBuildFundTableQuery()
        {
            var fundTableQuery = PriceServer.BuildFundTableQuery();

            Assert.AreEqual(ExpectedFundTableQuery, fundTableQuery);
        }
    }
}
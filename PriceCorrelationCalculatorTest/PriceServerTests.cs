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
            PriceServer = new PriceServer();
        }

        public PriceServer PriceServer { get; private set; }

        private const string ExpectedFundTableQuery =
            "https://personal.vanguard.com/us/funds/tools/pricehistorysearch?Sc=1";

        [Test]
        public void CanBuildFundTableQuery()
        {
            var fundTableQuery = PriceServer.BuildFundTableQuery();

            Assert.AreEqual(ExpectedFundTableQuery, fundTableQuery);
        }

        [Test]
        public void CanReadFromWeb()
        {
            const string requestUri = ExpectedFundTableQuery;
            var responseFromServer = PriceServer.ReadFromWeb(requestUri);

            const string expectedResponseFromServer = "";
            Assert.AreEqual(expectedResponseFromServer, responseFromServer);
        }
    }
}
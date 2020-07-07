using System;
using System.IO;
using System.Net;
using System.Text;
using Moq;
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
            PriceServer = new PriceServer {WebCommunicator = WebCommunicatorMock.Object};
            WebCommunicatorMockSetup();
        }

        private readonly WebRequest webRequest = It.IsAny<WebRequest>();
        private readonly HttpWebResponse httpWebResponse = It.IsAny<HttpWebResponse>();

        private readonly Stream stream = new MemoryStream(Encoding.UTF8.GetBytes("Foo"));

        private void WebCommunicatorMockSetup()
        {
            WebCommunicatorMock.Setup(s => s.SetSecurityProtocol());
            WebCommunicatorMock.Setup(s => s.CreateWebRequest(It.IsAny<string>()))
                .Returns(webRequest);
            WebCommunicatorMock.Setup(s => s.InitializeWebRequest(webRequest));
            WebCommunicatorMock.Setup(s => s.GetWebResponse(webRequest))
                .Returns(httpWebResponse);
            WebCommunicatorMock.Setup(s => s.GetResponseStream(httpWebResponse))
                .Returns(stream);
        }

        public PriceServer PriceServer { get; private set; }

        public Mock<IWebCommunicator> WebCommunicatorMock { get; } =
            new Mock<IWebCommunicator>(MockBehavior.Strict);

        private const string ExpectedFundTableQuery =
            "https://personal.vanguard.com/us/funds/tools/pricehistorysearch?Sc=1";

        private const string ExpectedPriceInfoQuery =
            "https://personal.vanguard.com/us/funds/tools/pricehistorysearch?radio=1&results=get&FundType=VanguardFunds" +
            "&FundIntExt=INT&FundId=0540&Sc=1&fundName=0540&fundValue=0540&radiobutton2=1&beginDate=4%2F1%2F2020&endDate=6%2F30%2F2020";

        [Test]
        public void CanBuildFundTableQuery()
        {
            var fundTableQuery = PriceServer.BuildFundTableQuery();

            Assert.AreEqual(ExpectedFundTableQuery, fundTableQuery);
        }

        [Test]
        public void CanBuildPriceInfoQuery()
        {
            const string id = "0540";
            var startDate = new DateTime(2020, 04, 01);
            var endDate = new DateTime(2020, 06, 30);
            var priceInfoQuery = PriceServer.BuildPriceInfoQuery(id, startDate, endDate);

            Assert.AreEqual(ExpectedPriceInfoQuery, priceInfoQuery);
        }

        [Test]
        public void CanParseFundTable()
        {
            const string responseFromServer =
                "Junk\n" +
                "</option>\n" +
                "</option>\n" +
                "</option>\n" +
                "</option>\n" +
                "</option>\n" +
                "<option value=\"0540\" >500 Index Fund Adm       </option>\n" +
                "<option value=\"0502\" >Balanced Index Fund Adm  </option>\n" +
                "<option value=\"5100\" >CA IT Tax-Exempt Admiral </option>\n" +
                "Junk";

            PriceServer.ParseFundTable(responseFromServer);

            const string expectedFirstValue = "0540";
            var fundTable = PriceServer.FundTable;
            var firstValue = fundTable["500 Index Fund Adm       "];
            Assert.AreEqual(expectedFirstValue, firstValue);
        }

        [Test]
        public void CanReadFromWeb()
        {
            const string requestUri = ExpectedFundTableQuery;
            var responseFromServer = PriceServer.ReadFromWeb(requestUri);

            WebCommunicatorMock.Verify(v => v.SetSecurityProtocol(), Times.Once);
            WebCommunicatorMock.Verify(v => v.CreateWebRequest(requestUri), Times.Once);
            WebCommunicatorMock.Verify(v => v.InitializeWebRequest(webRequest), Times.Once);
            WebCommunicatorMock.Verify(v => v.GetWebResponse(webRequest), Times.Once);
            WebCommunicatorMock.Verify(v => v.GetResponseStream(httpWebResponse), Times.Once);

            const string expectedResponseFromServer = "Foo";
            Assert.AreEqual(expectedResponseFromServer, responseFromServer);
        }
    }
}
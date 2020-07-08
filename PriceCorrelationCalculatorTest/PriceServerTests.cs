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
            WebCommunicatorMock = new Mock<IWebCommunicator>(MockBehavior.Strict);
            PriceServer = new PriceServer {WebCommunicator = WebCommunicatorMock.Object};
        }

        private WebRequest webRequest;
        private HttpWebResponse httpWebResponse;
        private string requestUri;
        private Stream stream;

        private void WebCommunicatorMockSetup()
        {
            WebCommunicatorMock.Setup(s => s.SetSecurityProtocol());
            WebCommunicatorMock.Setup(s => s.CreateWebRequest(requestUri))
                .Returns(webRequest);
            WebCommunicatorMock.Setup(s => s.InitializeWebRequest(webRequest));
            WebCommunicatorMock.Setup(s => s.GetWebResponse(webRequest))
                .Returns(httpWebResponse);
            WebCommunicatorMock.Setup(s => s.GetResponseStream(httpWebResponse))
                .Returns(stream);
        }

        public PriceServer PriceServer { get; private set; }

        public Mock<IWebCommunicator> WebCommunicatorMock { get; private set; }

        private const string ExpectedFundTableQuery =
            "https://personal.vanguard.com/us/funds/tools/pricehistorysearch?Sc=1";

        private const string ExpectedPriceInfoQuery =
            "https://personal.vanguard.com/us/funds/tools/pricehistorysearch?radio=1&results=get&FundType=VanguardFunds" +
            "&FundIntExt=INT&FundId=0540&Sc=1&fundName=0540&fundValue=0540&radiobutton2=1&beginDate=4%2F1%2F2020&endDate=6%2F30%2F2020";

        private const string ExpectedFundTableFromServer = "Junk\n" +
                                                           "</option>\n" +
                                                           "</option>\n" +
                                                           "</option>\n" +
                                                           "</option>\n" +
                                                           "</option>\n" +
                                                           "<option value=\"0540\" >500 Index Fund Adm       </option>\n" +
                                                           "<option value=\"0502\" >Balanced Index Fund Adm  </option>\n" +
                                                           "<option value=\"5100\" >CA IT Tax-Exempt Admiral </option>\n" +
                                                           "Junk";

        private const string ExpectedPriceInfoFromServer =
            "Junk\n" +
            "<tr class=\"ar\"><td align=\"left\">05/15/2020</td><td>$26.66</td><td class=\"nr\">&#8212;</td></tr>\n" +
            "<tr class=\"wr\"><td align=\"left\">05/18/2020</td><td>$27.55</td><td class=\"nr\">&#8212;</td></tr>\n" +
            "<tr class=\"wr\"><td align=\"left\">06/12/2020</td><td>$281.94</td><td class=\"nr\">1.84%</td></tr>\n" +
            "<tr class=\"ar\"><td align=\"left\">06/15/2020</td><td>$284.30</td><td class=\"nr\">1.84%</td></tr>\n" +
            "Junk";

        private void CanBuildFundTableQuery()
        {
            var fundTableQuery = PriceServer.FundTableQuery;
            Assert.AreEqual(ExpectedFundTableQuery, fundTableQuery);
        }

        private void CanReadFundTableFromWeb()
        {
            WebCommunicatorMockVerify();

            var responseFromServer = PriceServer.ResponseFromServer;
            Assert.AreEqual(ExpectedFundTableFromServer, responseFromServer);
        }

        private void CanReadPriceInfoFromWeb()
        {
            WebCommunicatorMockVerify();

            var responseFromServer = PriceServer.ResponseFromServer;
            Assert.AreEqual(ExpectedPriceInfoFromServer, responseFromServer);
        }

        private void WebCommunicatorMockVerify()
        {
            WebCommunicatorMock.Verify(v => v.SetSecurityProtocol(), Times.Once);
            WebCommunicatorMock.Verify(v => v.CreateWebRequest(requestUri), Times.Once);
            WebCommunicatorMock.Verify(v => v.InitializeWebRequest(webRequest), Times.Once);
            WebCommunicatorMock.Verify(v => v.GetWebResponse(webRequest), Times.Once);
            WebCommunicatorMock.Verify(v => v.GetResponseStream(httpWebResponse), Times.Once);
        }

        private void CanParseFundTable()
        {
            const string expectedFirstValue = "0540";
            var fundTable = PriceServer.FundTable;
            var firstValue = fundTable["500 Index Fund Adm       "];
            Assert.AreEqual(expectedFirstValue, firstValue);
        }

        private void CanBuildPriceInfoQuery()
        {
            var priceInfoQuery = PriceServer.PriceInfoQuery;
            Assert.AreEqual(ExpectedPriceInfoQuery, priceInfoQuery);
        }

        private void CanParsePriceInfo()
        {
            const string expectedFirstValue = "281.94";
            var priceInfo = PriceServer.PriceInfo;
            var firstValue = priceInfo["06/12/2020"];
            Assert.AreEqual(expectedFirstValue, firstValue);
        }

        [Test]
        public void CanGetFundTable()
        {
            webRequest = It.IsAny<WebRequest>();
            httpWebResponse = It.IsAny<HttpWebResponse>();
            requestUri = ExpectedFundTableQuery;
            var utf8 = Encoding.UTF8;
            var buffer = utf8.GetBytes(ExpectedFundTableFromServer);
            stream = new MemoryStream(buffer);
            WebCommunicatorMockSetup();

            PriceServer.GetFundTable();

            CanBuildFundTableQuery();
            CanReadFundTableFromWeb();
            CanParseFundTable();
        }

        [Test]
        public void CanRetrievePriceInfo()
        {
            webRequest = It.IsAny<WebRequest>();
            httpWebResponse = It.IsAny<HttpWebResponse>();
            requestUri = ExpectedPriceInfoQuery;
            var utf8 = Encoding.UTF8;
            var buffer = utf8.GetBytes(ExpectedPriceInfoFromServer);
            stream = new MemoryStream(buffer);
            WebCommunicatorMockSetup();

            const string id = "0540";
            var startDate = new DateTime(2020, 04, 01);
            var endDate = new DateTime(2020, 06, 30);
            PriceServer.RetrievePriceInfo(id, startDate, endDate);

            CanBuildPriceInfoQuery();
            CanReadPriceInfoFromWeb();
            CanParsePriceInfo();
        }
    }
}
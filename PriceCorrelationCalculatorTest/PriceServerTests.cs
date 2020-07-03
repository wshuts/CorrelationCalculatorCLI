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
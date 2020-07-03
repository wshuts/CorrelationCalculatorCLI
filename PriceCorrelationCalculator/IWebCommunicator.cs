using System.IO;
using System.Net;

namespace PriceCorrelationCalculator
{
    public interface IWebCommunicator
    {
        public Stream GetResponseStream(HttpWebResponse response);
        public HttpWebResponse GetWebResponse(WebRequest request);
        public void InitializeWebRequest(WebRequest request);
        public WebRequest CreateWebRequest(string requestUri);
        public void SetSecurityProtocol();
    }
}
using System.IO;
using System.Net;
using System.Security.Authentication;

namespace PriceCorrelationCalculator
{
    public class WebCommunicator
    {
        public static Stream GetResponseStream(HttpWebResponse response)
        {
            var dataStream = response.GetResponseStream();
            return dataStream;
        }

        public static HttpWebResponse GetWebResponse(WebRequest request)
        {
            var response = (HttpWebResponse) request.GetResponse();
            return response;
        }

        public static void InitializeWebRequest(WebRequest request)
        {
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = 10000;
        }

        public static WebRequest CreateWebRequest(string requestUri)
        {
            var request = WebRequest.Create(requestUri);
            return request;
        }

        public static void SetSecurityProtocol()
        {
            const SslProtocols sslProtocol = (SslProtocols) 0x00000C00;
            const SecurityProtocolType securityProtocolType = (SecurityProtocolType) sslProtocol;
            ServicePointManager.SecurityProtocol = securityProtocolType;
        }
    }
}
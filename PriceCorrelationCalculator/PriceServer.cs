using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Authentication;

namespace PriceCorrelationCalculator
{
    public class PriceServer
    {
        public void GetFundTable()
        {
            var requestUri = BuildQuery();
            var responseFromServer = ReadFromWeb(requestUri);
            ParseFundTable(responseFromServer);
        }

        private void ParseFundTable(string responseFromServer)
        {
            throw new System.NotImplementedException();
        }

        private static string ReadFromWeb(string requestUri)
        {
            const SslProtocols sslProtocol = (SslProtocols) 0x00000C00;
            const SecurityProtocolType securityProtocolType = (SecurityProtocolType) sslProtocol;
            ServicePointManager.SecurityProtocol = securityProtocolType;

            var request = WebRequest.Create(requestUri);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = 10000;
            var response = (HttpWebResponse) request.GetResponse();

            var dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream ?? throw new InvalidOperationException("Could not get response from the price server."));
            var responseFromServer = reader.ReadToEnd();

            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }

        private static string BuildQuery()
        {
            const string absolutePath = "https://personal.vanguard.com/us/funds/tools/pricehistorysearch";
            const string sc = "?Sc=1";
            const string requestUri = absolutePath + sc;
            return requestUri;
        }

        public IEnumerable<DictionaryEntry> FundTable { get; set; }
    }
}
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
        public IDictionary<string, string> FundTable { get; set; } = new SortedList<string, string>();
        public IDictionary<string, string> PriceInfo { get; set; } = new SortedList<string, string>();

        public void GetFundTable()
        {
            var requestUri = BuildQuery();
            var responseFromServer = ReadFromWeb(requestUri);
            ParseFundTable(responseFromServer);
        }

        private void ParseFundTable(string responseFromServer)
        {
            IList fundTableLines = new ArrayList();

            var parsedStrings = responseFromServer.Split('\n');

            fundTableLines.Clear();
            foreach (var line in parsedStrings)
                if (line.IndexOf("</option>", StringComparison.Ordinal) != -1)
                    fundTableLines.Add(line);

            for (var count = 0; count < 5; count++)
                if (fundTableLines.Count > 0)
                    fundTableLines.RemoveAt(0);

            FundTable.Clear();
            foreach (string line in fundTableLines)
            {
                var chunks = line.Split('\"', '>', '<');
                FundTable.Add(chunks[4], chunks[2]);
            }
        }

        private static string ReadFromWeb(string requestUri)
        {
            SetSecurityProtocol();

            var request = CreateWebRequest(requestUri);
            InitializeWebRequest(request);
            var response = GetWebResponse(request);

            var dataStream = GetResponseStream(response);
            var reader = CreateStreamReader(dataStream);
            var responseFromServer = reader.ReadToEnd();

            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }

        private static StreamReader CreateStreamReader(Stream dataStream)
        {
            var reader = new StreamReader(dataStream ??
                                          throw new InvalidOperationException(
                                              "Could not get response from the price server."));
            return reader;
        }

        private static Stream GetResponseStream(HttpWebResponse response)
        {
            var dataStream = response.GetResponseStream();
            return dataStream;
        }

        private static HttpWebResponse GetWebResponse(WebRequest request)
        {
            var response = (HttpWebResponse) request.GetResponse();
            return response;
        }

        private static void InitializeWebRequest(WebRequest request)
        {
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = 10000;
        }

        private static WebRequest CreateWebRequest(string requestUri)
        {
            var request = WebRequest.Create(requestUri);
            return request;
        }

        private static void SetSecurityProtocol()
        {
            const SslProtocols sslProtocol = (SslProtocols) 0x00000C00;
            const SecurityProtocolType securityProtocolType = (SecurityProtocolType) sslProtocol;
            ServicePointManager.SecurityProtocol = securityProtocolType;
        }

        private static string BuildQuery()
        {
            const string absolutePath = "https://personal.vanguard.com/us/funds/tools/pricehistorysearch";
            const string sc = "?Sc=1";
            const string requestUri = absolutePath + sc;
            return requestUri;
        }

        public string BuildQuery(string id, DateTime startDate, DateTime endDate)
        {
            const string absolutePath = "https://personal.vanguard.com/us/funds/tools/pricehistorysearch";
            const string radio = "?radio=1";
            const string results = "&results=get";
            const string fundType = "&FundType=VanguardFunds";
            const string fundIntExt = "&FundIntExt=INT";
            var fundId = "&FundId=" + id;
            const string sc = "&Sc=1";
            var fundName = "&fundName=" + id;
            var fundValue = "&fundValue=" + id;
            const string radiobutton2 = "&radiobutton2=1";
            var beginDate = "&beginDate=" + startDate.Month + "%2F" + startDate.Day + "%2F" + startDate.Year;
            var finalDate = "&endDate=" + endDate.Month + "%2F" + endDate.Day + "%2F" + endDate.Year;
            var requestUri = absolutePath + radio + results + fundType + fundIntExt + fundId + sc + fundName +
                             fundValue + radiobutton2 + beginDate + finalDate;
            return requestUri;
        }

        public void RetrievePriceInfo(string fundNumber, in DateTime startDate, in DateTime endDate)
        {
            var requestUri = BuildQuery(fundNumber, startDate, endDate);
            var responseFromServer = ReadFromWeb(requestUri);
            ParsePriceInfo(responseFromServer);
        }

        private void ParsePriceInfo(string responseFromServer)
        {
            IList priceLines = new ArrayList();

            var parsedStrings = responseFromServer.Split('\n');

            priceLines.Clear();
            foreach (var line in parsedStrings)
                if (line.IndexOf("$", StringComparison.Ordinal) != -1 &&
                    (line.IndexOf("%", StringComparison.Ordinal) != -1 ||
                     line.IndexOf("&#8212;", StringComparison.Ordinal) != -1))
                    priceLines.Add(line);

            PriceInfo.Clear();
            foreach (string line in priceLines)
            {
                var chunks = line.Split('$', '>', '<');
                PriceInfo.Add(chunks[4], chunks[9]);
            }
        }
    }
}
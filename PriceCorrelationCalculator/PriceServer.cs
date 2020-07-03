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
        private const string AbsolutePath = "https://personal.vanguard.com/us/funds/tools/pricehistorysearch";
        public IDictionary<string, string> FundTable { get; set; } = new SortedList<string, string>();
        public IDictionary<string, string> PriceInfo { get; set; } = new SortedList<string, string>();

        public void GetFundTable()
        {
            var requestUri = BuildFundTableQuery();
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
            using var response = GetWebResponse(request);

            using var dataStream = GetResponseStream(response);
            using var reader = CreateStreamReader(dataStream);
            var responseFromServer = reader.ReadToEnd();

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

        public static string BuildFundTableQuery()
        {
            const string sc = "?Sc=1";
            const string requestUri = AbsolutePath + sc;
            return requestUri;
        }

        public string BuildQuery(string id, DateTime startDate, DateTime endDate)
        {
            const string radio = "?radio=1";
            const string results = "&results=get";
            const string fundType = "&FundType=VanguardFunds";
            const string fundIntExt = "&FundIntExt=INT";
            const string radiobutton2 = "&radiobutton2=1";
            const string sc = "&Sc=1";

            var fundId = "&FundId=" + id;
            var fundName = "&fundName=" + id;
            var fundValue = "&fundValue=" + id;
            var beginDate = "&beginDate=" + startDate.Month + "%2F" + startDate.Day + "%2F" + startDate.Year;
            var finalDate = "&endDate=" + endDate.Month + "%2F" + endDate.Day + "%2F" + endDate.Year;
            var requestUri = AbsolutePath + radio + results + fundType + fundIntExt + fundId + sc + fundName +
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
using System;
using System.Collections;
using System.Collections.Generic;
using ParameterToolbox;

namespace PriceCorrelationCalculator
{
    public class PriceServer
    {
        private const string AbsolutePath = "https://personal.vanguard.com/us/funds/tools/pricehistorysearch";
        public string ResponseFromServer { get; private set; }
        public string FundTableQuery { get; private set; }
        public IStreamFactory StreamFactory { get; } = new StreamFactory();
        public IWebCommunicator WebCommunicator { get; set; } = new WebCommunicator();
        public IDictionary<string, string> FundTable { get; } = new SortedList<string, string>();
        public IDictionary<string, string> PriceInfo { get; } = new SortedList<string, string>();

        public void GetFundTable()
        {
            FundTableQuery = BuildFundTableQuery();
            ResponseFromServer = ReadFromWeb(FundTableQuery);
            ParseFundTable(ResponseFromServer);
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

        private string ReadFromWeb(string requestUri)
        {
            WebCommunicator.SetSecurityProtocol();
            var request = WebCommunicator.CreateWebRequest(requestUri);
            WebCommunicator.InitializeWebRequest(request);
            using var response = WebCommunicator.GetWebResponse(request);
            using var dataStream = WebCommunicator.GetResponseStream(response);

            using var reader = StreamFactory.CreateStreamReader(dataStream);
            var responseFromServer = reader.ReadToEnd();
            return responseFromServer;
        }

        private static string BuildFundTableQuery()
        {
            const string sc = "?Sc=1";
            const string requestUri = AbsolutePath + sc;
            return requestUri;
        }

        public string BuildPriceInfoQuery(string id, DateTime startDate, DateTime endDate)
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
            var requestUri = BuildPriceInfoQuery(fundNumber, startDate, endDate);
            var responseFromServer = ReadFromWeb(requestUri);
            ParsePriceInfo(responseFromServer);
        }

        public void ParsePriceInfo(string responseFromServer)
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
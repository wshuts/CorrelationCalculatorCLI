using System;
using System.Collections.Generic;

namespace ParameterToolbox
{
    public static class ParametersFactory
    {
        public static Parameters CreateParametersForTesting()
        {
            var startDate = CreateStartDate();
            var endDate = CreateEndDate();
            var funds = CreateFunds();
            var parameters = new Parameters {StartDate = startDate, EndDate = endDate, Funds = funds};
            return parameters;
        }

        private static DateTime CreateStartDate()
        {
            const int startYear = 2020;
            const int startMonth = 01;
            const int startDay = 01;
            var startDate = CreateDateTime(startYear, startMonth, startDay);
            return startDate;
        }

        private static DateTime CreateEndDate()
        {
            const int endYear = 2020;
            const int endMonth = 05;
            const int endDay = 01;
            var endDate = CreateDateTime(endYear, endMonth, endDay);
            return endDate;
        }

        private static DateTime CreateDateTime(int year, int month, int day)
        {
            var startDate = new DateTime(year, month, day);
            return startDate;
        }

        private static List<Fund> CreateFunds()
        {
            var fund1 = CreateFund1();
            var fund2 = CreateFund2();
            var funds = new List<Fund> {fund1, fund2};
            return funds;
        }

        private static Fund CreateFund1()
        {
            const string fund1Name = "Windsor II Fund Adm      ";
            const string fund1Number = "0573";
            var fund1 = new Fund(fund1Name, fund1Number);
            return fund1;
        }

        private static Fund CreateFund2()
        {
            const string fund2Name = "Total Stock Mkt Idx Adm  ";
            const string fund2Number = "0585";
            var fund2 = new Fund(fund2Name, fund2Number);
            return fund2;
        }
    }
}
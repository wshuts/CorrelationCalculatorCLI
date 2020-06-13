using System;
using System.Collections.Generic;

namespace ParameterToolbox
{
    public static class ParametersFactory
    {
        public static Parameters CreateParameters()
        {
            var startDate = new DateTime(2020, 01, 01);
            var endDate = new DateTime(2020, 05, 01);

            const string fund1Name = "Windsor II Fund Adm      ";
            const string fund1Number = "0573";
            var fund1 = new Fund(fund1Name, fund1Number);

            const string fund2Name = "Total Stock Mkt Idx Adm  ";
            const string fund2Number = "0585";
            var fund2 = new Fund(fund2Name, fund2Number);

            var funds = new List<Fund> {fund1, fund2};

            return new Parameters {StartDate = startDate, EndDate = endDate, Funds = funds};
        }
    }
}
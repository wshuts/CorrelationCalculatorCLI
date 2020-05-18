using System;
using System.Collections.Generic;
using NUnit.Framework;
using ParameterToolbox;

namespace ParameterToolboxTest
{
    [TestFixture]
    public class ParametersTests
    {
        public Parameters Parameters { get; private set; }

        [Test]
        public void CanSerializeParameters()
        {
            InitializeParameters();
            Parameters.Serialize();

            var parameters = Parameters.Deserialize();

            var funds = parameters.Funds;
            Assert.NotNull(funds);
            
            var expected = Parameters.StartDate;
            var actual = parameters.StartDate;
            Assert.AreEqual(expected, actual);
        }

        private void InitializeParameters()
        {
            var startDate = new DateTime(2020, 01, 01);
            var endDate = new DateTime(2020, 05, 01);

            const string fundName = "Windsor II Fund Adm      ";
            const string fundNumber = "0573";
            var fund = new Fund(fundName, fundNumber);
            var funds = new List<Fund> {fund};

            Parameters = new Parameters {StartDate = startDate, EndDate = endDate, Funds = funds};
        }
    }
}
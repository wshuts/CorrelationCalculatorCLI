using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using NUnit.Framework;
using ParameterToolbox;

namespace ParameterToolboxTest
{
    [TestFixture]
    public class ParametersTests
    {
        public Parameters Parameters { get; private set; }

        private void InitializeParameters()
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

            Parameters = new Parameters {StartDate = startDate, EndDate = endDate, Funds = funds};
        }

        [Test]
        public void GivenStreamFactoryMockCanSerializeParameters()
        {
            var streamFactoryMock = new Mock<IStream>(MockBehavior.Strict);
            var memoryBuffer = new byte[512];

            var writeMemoryStream = new MemoryStream(memoryBuffer);
            streamFactoryMock.Setup(s => s.CreateStreamWriter(It.IsAny<string>()))
                .Returns(new StreamWriter(writeMemoryStream));

            var readMemoryStream = new MemoryStream(memoryBuffer);
            streamFactoryMock.Setup(s => s.CreateStreamReader(It.IsAny<string>()))
                .Returns(new StreamReader(readMemoryStream));

            JsonUtilities.StreamFactory = streamFactoryMock.Object;

            InitializeParameters();
            Parameters.Serialize();

            var parameters = Parameters.Deserialize();

            var funds = parameters.Funds;
            Assert.NotNull(funds);

            var expected = Parameters.StartDate;
            var actual = parameters.StartDate;
            Assert.AreEqual(expected, actual);
        }
    }
}
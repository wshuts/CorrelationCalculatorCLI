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
            Parameters = ParametersFactory.CreateParametersForTesting();

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
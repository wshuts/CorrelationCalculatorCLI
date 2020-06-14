using System.IO;
using Moq;
using NUnit.Framework;
using ParameterToolbox;

namespace ParameterToolboxTest
{
    [TestFixture]
    public class ParametersTests
    {
        public byte[] MemoryBuffer { get; } = new byte[512];
        public Mock<IStream> StreamFactoryMock { get; } = new Mock<IStream>(MockBehavior.Strict);
        public Parameters Parameters { get; private set; }

        [Test]
        public void GivenStreamFactoryMockCanSerializeParameters()
        {
            StreamFactoryMockSetup();
            JsonUtilities.StreamFactory = StreamFactoryMock.Object;
            Parameters = ParametersFactory.CreateParametersForTesting();

            Parameters.Serialize();

            var parameters = Parameters.Deserialize();
            var funds = parameters.Funds;
            Assert.NotNull(funds);

            var expected = Parameters.StartDate;
            var actual = parameters.StartDate;
            Assert.AreEqual(expected, actual);
        }

        private void StreamFactoryMockSetup()
        {
            var writeMemoryStream = new MemoryStream(MemoryBuffer);
            StreamFactoryMock.Setup(s => s.CreateStreamWriter(It.IsAny<string>()))
                .Returns(new StreamWriter(writeMemoryStream));

            var readMemoryStream = new MemoryStream(MemoryBuffer);
            StreamFactoryMock.Setup(s => s.CreateStreamReader(It.IsAny<string>()))
                .Returns(new StreamReader(readMemoryStream));
        }
    }
}
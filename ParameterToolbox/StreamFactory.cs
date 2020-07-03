using System.IO;

namespace ParameterToolbox
{
    public class StreamFactory : IStreamFactory
    {
        public StreamWriter CreateStreamWriter(string path)
        {
            return new StreamWriter(path);
        }

        public StreamReader CreateStreamReader(string path)
        {
            return new StreamReader(path);
        }

        public StreamReader CreateStreamReader(Stream dataStream)
        {
            return new StreamReader(dataStream);
        }
    }
}
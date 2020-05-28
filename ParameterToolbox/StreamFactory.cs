using System.IO;

namespace ParameterToolbox
{
    public class StreamFactory : IStream
    {
        public StreamWriter CreateStreamWriter(string path)
        {
            return new StreamWriter(path);
        }

        public StreamReader CreateStreamReader(string path)
        {
            return new StreamReader(path);
        }
    }
}
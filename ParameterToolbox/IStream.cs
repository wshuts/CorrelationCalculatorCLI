using System.IO;

namespace ParameterToolbox
{
    public interface IStream
    {
        public StreamWriter CreateStreamWriter(string path);
        public StreamReader CreateStreamReader(string path);
    }
}
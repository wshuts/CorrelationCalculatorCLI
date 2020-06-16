using System.IO;

namespace ParameterToolbox
{
    public interface IStreamFactory
    {
        public StreamWriter CreateStreamWriter(string path);
        public StreamReader CreateStreamReader(string path);
    }
}
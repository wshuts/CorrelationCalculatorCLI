using System;
using System.IO;

namespace ParameterToolbox
{
    public static class FileUtilities
    {
        public static string ConvertToAbsolutePath(string relativeFileName)
        {
            var currentDomain = AppDomain.CurrentDomain;
            var baseDirectory = currentDomain.BaseDirectory;
            return Path.Combine(baseDirectory ?? string.Empty, relativeFileName);
        }
    }
}
using System.IO;
using System.IO.Compression;

namespace CTP
{
    public static class DeflateHelper
    {
        public static byte[] Compress(byte[] data)
        {
            var ms = new MemoryStream();
            using (var gz = new DeflateStream(ms, CompressionMode.Compress))
            {
                gz.Write(data, 0, data.Length);
            }
            return ms.ToArray();
        }
    }
}

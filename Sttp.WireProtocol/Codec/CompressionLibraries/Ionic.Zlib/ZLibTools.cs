using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
//using Ionic.Zlib;

namespace Sttp.Codec.CompressionLibraries.Ionic.Zlib
{
    public class ZLibTools
    {
        public static byte[] Compress(byte[] data)
        {
            var ms = new MemoryStream();
            using (var gz = new DeflateStream(ms, CompressionMode.Compress))
            {
                gz.Write(data, 0, data.Length);
            }
            return ms.ToArray();
            //var ms = new MemoryStream();
            //using (var gz = new DeflateStream(ms, CompressionMode.Compress, CompressionLevel.Level1))
            //{
            //    gz.Write(data,0,data.Length);
            //}
            //return ms.ToArray();
        }
    }
}


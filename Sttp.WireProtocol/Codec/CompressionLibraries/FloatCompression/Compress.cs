using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec.CompressionLibraries.FloatCompression
{
    /// <summary>
    /// Compresses 32-bit floating point values. This custom method will attempt to squeeze 1 or 2 bytes out of a 32-bit float.
    /// </summary>
    public class Compress
    {
        private uint[] m_history = new uint[16];
        private int m_index = 0;

        public void Clear()
        {

        }

        public void Pack(float value)
        {

        }
    }

    /// <summary>
    /// Compresses 32-bit floating point values. This custom method will attempt to squeeze 1 or 2 bytes out of a 32-bit float.
    /// </summary>
    public class Decompress
    {
        private uint[] m_history = new uint[16];
        private int m_index = 0;

        public float Unpack()
        {
            return 0;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp
{
    public class SttpBuffer
    {
        public byte[] Data;

        public SttpBuffer(byte[] data)
        {
            Data = (byte[])data.Clone();
        }
    }
}

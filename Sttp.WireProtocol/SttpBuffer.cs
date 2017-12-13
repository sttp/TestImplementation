using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp
{
    public class SttpBuffer
    {
        private byte[] Data;

        public SttpBuffer(byte[] data)
        {
            Data = (byte[])data.Clone();
        }

        public SttpBuffer(ByteReader data)
        {
            throw new NotImplementedException();
        }

        public byte[] ToBuffer()
        {
            throw new NotImplementedException();
        }

        public void Write(ByteWriter byteWriter)
        {
            throw new NotImplementedException();
        }
    }
}

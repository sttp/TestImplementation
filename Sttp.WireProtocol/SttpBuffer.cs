using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp
{
    public class SttpBuffer
    {
        private byte[] m_data;

        public SttpBuffer(byte[] data)
        {
            m_data = (byte[])data.Clone();
        }

        public SttpBuffer(ByteReader reader)
        {
            m_data = reader.ReadBytes();
        }

        public byte[] ToBuffer()
        {
            return (byte[])m_data.Clone();
        }

        public void Write(ByteWriter writer)
        {
            writer.Write(m_data);
        }

        public static bool operator ==(SttpBuffer a, SttpBuffer b)
        {
            return a.m_data.SequenceEqual(b.m_data);
        }

        public static bool operator !=(SttpBuffer a, SttpBuffer b)
        {
            return !(a == b);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTP
{
    public class CtpBuffer
    {
        private byte[] m_data;

        public CtpBuffer(byte[] data)
        {
            m_data = (byte[])data.Clone();
        }

        public CtpBuffer(ByteReader reader)
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

        public static bool operator ==(CtpBuffer a, CtpBuffer b)
        {
            return a.m_data.SequenceEqual(b.m_data);
        }

        public static bool operator !=(CtpBuffer a, CtpBuffer b)
        {
            return !(a == b);
        }
    }
}

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
            m_data = data;
        }

        public int Length => m_data.Length;

        /// <summary>
        /// Copies the internal buffer to the provided byte array.
        /// Be sure to call <see cref="Length"/> to ensure that the destination buffer
        /// has enough space to receive the copy.
        /// </summary>
        /// <param name="buffer">the buffer to copy to.</param>
        /// <param name="offset">the offset position of <see pref="buffer"/></param>
        public void CopyTo(byte[] buffer, int offset)
        {
            Array.Copy(m_data, 0, buffer, offset, m_data.Length); // write data
        }

        public static bool operator ==(CtpBuffer a, CtpBuffer b)
        {
            return a.m_data.SequenceEqual(b.m_data);
        }

        public static bool operator !=(CtpBuffer a, CtpBuffer b)
        {
            return !(a == b);
        }

        public byte[] ToBuffer()
        {
            return (byte[])m_data.Clone();
        }
    }
}

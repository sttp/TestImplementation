using System;

namespace CTP
{
    /// <summary>
    /// An immutable Buffer
    /// </summary>
    public class CtpBuffer : IEquatable<CtpBuffer>
    {
        private readonly byte[] m_data;

        /// <summary>
        /// Creates a new <see cref="CtpBuffer"/> cloning the provided buffer.
        /// </summary>
        /// <param name="data"></param>
        public CtpBuffer(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            m_data = (byte[])data.Clone();
        }

        /// <summary>
        /// Gets the size of the Buffer;
        /// </summary>
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
            if (ReferenceEquals(a, b))
                return true;
            if ((object)a == null || (object)b == null)
                return false;
            if (a.m_data.Length != b.m_data.Length)
                return false;

            for (int x = 0; x < a.m_data.Length; x++)
            {
                if (a.m_data[x] != b.m_data[x])
                    return false;
            }
            return true;
        }

        public static bool operator !=(CtpBuffer a, CtpBuffer b)
        {
            return !(a == b);
        }

        public byte[] ToBuffer()
        {
            return (byte[])m_data.Clone();
        }

        public bool Equals(CtpBuffer other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((CtpBuffer)obj);
        }

        public override int GetHashCode()
        {
            int hashCode = 27;
            for (int x = 0; x < m_data.Length; x++)
            {
                hashCode = hashCode * 13 + m_data[x];
            }

            return hashCode;
        }
    }
}

using System;
using System.IO;

namespace Sttp.WireProtocol
{
    public abstract class StreamBase
    {
        protected static readonly byte[] Empty = new byte[0];
        public byte[] Buffer;

        public int Position;
        protected int m_length;

        public int Length => m_length;

        protected StreamBase(ushort initialSize = 512)
        {
            Buffer = initialSize > 0 ? new byte[initialSize] : new byte[512];
        }

        protected void ExtendToPosition()
        {
            if (Position > m_length)
            {
                m_length = Position;
            }
        }

        public void Clear()
        {
            Position = 0;
            m_length = 0;

            Array.Clear(Buffer, 0, Buffer.Length);
        }

        public void Fill(byte[] data, int position, int length)
        {
            while (length + m_length >= Buffer.Length)
            {
                Grow();
            }
            Array.Copy(data, position, Buffer, m_length, length);
            m_length += length;
        }

        public void Fill(Stream stream, int length)
        {
            while (length + m_length >= Buffer.Length)
            {
                Grow();
            }

            stream.Read(Buffer, 0, length);
            m_length += length;
        }

        protected void Fill()
        {
            throw new EndOfStreamException();
        }

        public void Compact()
        {
            if (Position > 0 && Position != m_length)
            {
                // Compact - trims all data before current position if position is in middle of stream
                Array.Copy(Buffer, Position, Buffer, 0, m_length - Position);
            }
            m_length = m_length - Position;
            Position = 0;
        }

        protected void Grow(int neededBytes)
        {
            if (Position + neededBytes >= Buffer.Length)
            {
                Grow();
            }
        }

        protected void Grow()
        {
            byte[] newBuffer = new byte[Buffer.Length * 2];
            Buffer.CopyTo(newBuffer, 0);
            Buffer = newBuffer;
        }

        public byte[] ToArray()
        {
            byte[] rv = new byte[m_length];
            Array.Copy(Buffer, 0, rv, 0, m_length);
            return rv;
        }

        public void CopyTo(Array desination, int destinationPosition)
        {
            Array.Copy(Buffer, 0, desination, 0, m_length);
        }

        public void CopyTo(Stream stream)
        {
            stream.Write(Buffer, 0, m_length);
        }
    }
}
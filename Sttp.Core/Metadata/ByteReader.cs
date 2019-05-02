using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Sttp.Metadata
{
    internal unsafe class ByteReader
    {
        private static readonly byte[] Empty = new byte[0];

        public byte[] Data;
        public int Position;
        private int m_length;

        public ByteReader(byte[] data)
        {
            Data = data;
            m_length = Data.Length;
            Position = 0;
        }

        public int Length => Data.Length;
        public bool IsEos => Position == Data.Length;

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ThrowEndOfStreamException()
        {
            throw new EndOfStreamException();
        }

        public float ReadSingle()
        {
            var value = ReadBits32();
            return *(float*)&value;
        }

        public byte[] ReadBytes(int length)
        {
            if (length == 0)
            {
                return Empty;
            }

            if (Position + length > m_length)
            {
                ThrowEndOfStreamException();
            }

            byte[] rv = new byte[length];
            Array.Copy(Data, Position, rv, 0, length);
            Position += length;
            return rv;
        }

        public string ReadString(int length, Encoding encoding)
        {
            byte[] data = ReadBytes(length);
            return encoding.GetString(data);
        }

        #region [ Read Bits ]


        public byte ReadByte()
        {
            return (byte)ReadBits8();
        }

        public ushort ReadUInt16()
        {
            return (ushort)ReadBits16();
        }

        public short ReadInt16()
        {
            return (short)ReadBits16();
        }

        public uint ReadUInt32()
        {
            return (uint)ReadBits32();
        }

        public uint ReadBits8()
        {
            if (Position + 1 > m_length)
            {
                ThrowEndOfStreamException();
            }
            byte rv = Data[Position];
            Position++;
            return rv;
        }

        public uint ReadBits16()
        {
            if (Position + 2 > m_length)
            {
                ThrowEndOfStreamException();
            }
            uint rv = (uint)Data[Position] << 8
                      | (uint)Data[Position + 1];
            Position += 2;
            return rv;
        }

        private uint ReadBits32()
        {
            if (Position + 4 > m_length)
            {
                ThrowEndOfStreamException();
            }
            uint rv = (uint)Data[Position] << 24
                      | (uint)Data[Position + 1] << 16
                      | (uint)Data[Position + 2] << 8
                      | (uint)Data[Position + 3];
            Position += 4;
            return rv;
        }

        #endregion
    }
}
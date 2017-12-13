using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sttp.Codec;

namespace Sttp
{
    public unsafe class ByteReader
    {
        private static readonly byte[] Empty = new byte[0];

        private byte[] m_buffer;
        private int m_currentPosition;
        private int m_startingPosition;
        private int m_lastPosition;
        private int m_bitCount;
        private uint m_cache;

        public int Length { get; private set; }

        public ByteReader()
        {
            m_buffer = Empty;
        }



        public ByteReader(byte[] data)
        {
            SetBuffer(data, 0, data.Length);
        }

        public ByteReader(byte[] data, int position, int length)
        {
            SetBuffer(data, position, length);
        }

        public int Position
        {
            get
            {
                return m_currentPosition - m_startingPosition;
            }
            set
            {
                if (value < 0 || value > Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Must be between 0 and Length");
                }
                m_currentPosition = value + m_startingPosition;
            }
        }

        public void SetBuffer(byte[] data)
        {
            SetBuffer(data, 0, data.Length);
        }

        public void SetBuffer(byte[] data, int position, int length)
        {
            m_buffer = data;
            m_startingPosition = position;
            m_currentPosition = position;
            m_lastPosition = length + position;
            Length = length;
            m_bitCount = 0;
            m_cache = 0;
        }

        private void ThrowEndOfStreamException()
        {
            throw new EndOfStreamException();
        }

        private void EnsureCapacity(int length)
        {
            if (m_currentPosition + length > m_lastPosition)
            {
                ThrowEndOfStreamException();
            }
        }

        #region [ Read Methods ]

        #region [ 1 byte values ]

        public byte ReadByte()
        {
            EnsureCapacity(1);
            byte rv = m_buffer[m_currentPosition];
            m_currentPosition++;
            return rv;
        }

        public bool ReadBoolean()
        {
            return ReadByte() != 0;
        }

        public sbyte ReadSByte()
        {
            return (sbyte)ReadByte();
        }

        #endregion

        #region [ 2-byte values ]

        public short ReadInt16()
        {
            EnsureCapacity(2);
            short rv = (short)(m_buffer[m_currentPosition] << 8 | m_buffer[m_currentPosition + 1]);
            m_currentPosition += 2;
            return rv;
        }

        public ushort ReadUInt16()
        {
            return (ushort)ReadInt16();
        }

        public char ReadChar()
        {
            return (char)ReadInt16();
        }

        #endregion

        #region [ 4-byte values ]

        public int ReadInt32()
        {
            if (m_currentPosition + 4 > m_lastPosition)
            {
                ThrowEndOfStreamException();
            }
            int rv = m_buffer[m_currentPosition + 0] << 24 |
                     m_buffer[m_currentPosition + 1] << 16 |
                     m_buffer[m_currentPosition + 2] << 8 |
                     m_buffer[m_currentPosition + 3];
            m_currentPosition += 4;
            return rv;
        }

        public uint ReadUInt32()
        {
            return (uint)ReadInt32();
        }

        public float ReadSingle()
        {
            var value = ReadInt32();
            return *(float*)&value;
        }

        #endregion

        #region [ 8-byte values ]

        public long ReadInt64()
        {
            if (m_currentPosition + 8 > m_lastPosition)
            {
                ThrowEndOfStreamException();
            }
            long rv = (long)m_buffer[m_currentPosition + 0] << 56 |
                      (long)m_buffer[m_currentPosition + 1] << 48 |
                      (long)m_buffer[m_currentPosition + 2] << 40 |
                      (long)m_buffer[m_currentPosition + 3] << 32 |
                      (long)m_buffer[m_currentPosition + 4] << 24 |
                      (long)m_buffer[m_currentPosition + 5] << 16 |
                      (long)m_buffer[m_currentPosition + 6] << 8 |
                      (long)m_buffer[m_currentPosition + 7];
            m_currentPosition += 8;
            return rv;
        }

        public double ReadDouble()
        {
            var value = ReadInt64();
            return *(double*)&value;
        }

        public ulong ReadUInt64()
        {
            return (ulong)ReadInt64();
        }

        public DateTime ReadDateTime()
        {
            return new DateTime(ReadInt64());
        }

        #endregion

        #region [ 16-byte values ]

        public decimal ReadDecimal()
        {
            if (m_currentPosition + 16 > m_lastPosition)
            {
                ThrowEndOfStreamException();
            }
            decimal rv = BigEndian.ToDecimal(m_buffer, m_currentPosition);
            m_currentPosition += 16;
            return rv;
        }

        public Guid ReadGuid()
        {
            if (m_currentPosition + 16 > m_lastPosition)
            {
                ThrowEndOfStreamException();
            }

            Guid rv = GuidExtensions.ToRfcGuid(m_buffer, m_currentPosition);
            m_currentPosition += 16;
            return rv;
        }

        #endregion

        #region [ Variable Length ]

        public byte[] ReadRawBytes(int length)
        {
            byte[] rv = new byte[length];
            Array.Copy(m_buffer, m_currentPosition, rv, 0, length);

            m_currentPosition += length;
            return rv;
        }

        public byte[] ReadBytes()
        {
            int length = (int)ReadUInt7Bit();
            if (length == 0)
            {
                return null;
            }
            if (length == 1)
            {
                return Empty;
            }

            length--; // minus one, because 1 added to len before writing since (1) is used for empty.
            EnsureCapacity(length);

            byte[] rv = new byte[length];
            Array.Copy(m_buffer, m_currentPosition, rv, 0, length);
            m_currentPosition += length;
            return rv;
        }

        public string ReadString()
        {
            byte[] rv = ReadBytes();
            if (rv == null)
                return null;
            if (rv.Length == 0)
                return string.Empty;

            return Encoding.UTF8.GetString(rv);
        }

        #endregion

        #region [ ReadBits ]

        public int ReadInt7Bit()
        {
            uint u = ReadUInt7Bit();
            return *(int*)&u;
        }

        public uint ReadUInt7Bit()
        {
            Read7BitEnsureCapacity();
            return Encoding7Bit.ReadUInt32(m_buffer, ref m_currentPosition);
        }

        private void Read7BitEnsureCapacity()
        {
            if (m_buffer[m_currentPosition] < 128)
            {
                return;
            }
            if (m_buffer[m_currentPosition + 1] < 128)
            {
                EnsureCapacity(1);
                return;
            }
            if (m_buffer[m_currentPosition + 2] < 128)
            {
                EnsureCapacity(2);
                return;
            }
            if (m_buffer[m_currentPosition + 3] < 128)
            {
                EnsureCapacity(3);
                return;
            }
            EnsureCapacity(4);
        }

        #endregion

        #endregion

        public List<SttpDataPointID> ReadListSttpDataPointID()
        {
            var rv = new List<SttpDataPointID>();
            int len = ReadInt7Bit();
            while (len > 0)
            {
                rv.Add(new SttpDataPointID(this));
                len--;
            }
            return rv;
        }

        public List<MetadataSchemaTables> ReadListMetadataSchemaTables()
        {
            var rv = new List<MetadataSchemaTables>();
            int len = ReadInt7Bit();
            while (len > 0)
            {
                rv.Add(new MetadataSchemaTables(this));
                len--;
            }
            return rv;
        }

        public List<MetadataSchemaTableUpdate> ReadListMetadataSchemaTableUpdate()
        {
            var rv = new List<MetadataSchemaTableUpdate>();
            int len = ReadInt7Bit();
            while (len > 0)
            {
                rv.Add(new MetadataSchemaTableUpdate(this));
                len--;
            }
            return rv;
        }

        public List<MetadataColumn> ReadListMetadataColumn()
        {
            var rv = new List<MetadataColumn>();
            int len = ReadInt7Bit();
            while (len > 0)
            {
                rv.Add(new MetadataColumn(this));
                len--;
            }
            return rv;
        }

        public List<int> ReadListInt()
        {
            var rv = new List<int>();
            int len = ReadInt7Bit();
            while (len > 0)
            {
                rv.Add(ReadInt32());
                len--;
            }
            return rv;
        }

        public int? ReadNullInt32()
        {
            if (ReadBoolean())
                return ReadInt32();
            return null;
        }

        public List<string> ReadListString()
        {
            var rv = new List<string>();
            int len = ReadInt7Bit();
            while (len > 0)
            {
                rv.Add(ReadString());
                len--;
            }
            return rv;
        }

        public SttpMarkup ReadSttpMarkup()
        {
            return new SttpMarkup(this);
        }

        public List<MetadataForeignKey> ReadListMetadataForeignKey()
        {
            var rv = new List<MetadataForeignKey>();
            int len = ReadInt7Bit();
            while (len > 0)
            {
                rv.Add(new MetadataForeignKey(this));
                len--;
            }
            return rv;
        }

        public SttpBuffer ReadSttpBuffer()
        {
            return new SttpBuffer(this);
        }

        public DateTimeOffset ReadDateTimeOffset()
        {
            throw new NotImplementedException();
        }

        public SttpTime ReadSttpTime()
        {
            return new SttpTime(this);
        }

        public TimeSpan ReadTimeSpan()
        {
            throw new NotImplementedException();
        }

        public List<SttpMarkup> ReadListSttpMarkup()
        {
            var rv = new List<SttpMarkup>();
            int len = ReadInt7Bit();
            while (len > 0)
            {
                rv.Add(new SttpMarkup(this));
                len--;
            }
            return rv;
        }

        public SttpBulkTransport ReadSttpBulkTransport()
        {
            throw new NotImplementedException();
        }



        #region [ Read Bits ]

        public uint ReadBits0()
        {
            return 0;
        }

        public uint ReadBits1()
        {
            const int bits = 1;
            if (m_bitCount < bits)
                ReadMoreBits();
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits2()
        {
            const int bits = 2;
            if (m_bitCount < bits)
                ReadMoreBits();
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }

        public uint ReadBits3()
        {
            const int bits = 3;
            if (m_bitCount < bits)
                ReadMoreBits();
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits4()
        {
            const int bits = 4;
            if (m_bitCount < bits)
                ReadMoreBits();
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits5()
        {
            const int bits = 5;
            if (m_bitCount < bits)
                ReadMoreBits();
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits6()
        {
            const int bits = 6;
            if (m_bitCount < bits)
                ReadMoreBits();
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits7()
        {
            const int bits = 7;
            if (m_bitCount < bits)
                ReadMoreBits();
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits8()
        {
            if (m_currentPosition + 1 > m_lastPosition)
            {
                ThrowEndOfStreamException();
            }
            byte rv = m_buffer[m_currentPosition];
            m_currentPosition++;
            return rv;
        }
        public uint ReadBits9()
        {
            //Note, .NET evaluates Left to Right. If ported, be sure to correct this for the target language.
            return ReadBits8() | (ReadBits1() << 8);
        }
        public uint ReadBits10()
        {
            return ReadBits8() | (ReadBits2() << 8);
        }
        public uint ReadBits11()
        {
            return ReadBits8() | (ReadBits3() << 8);
        }
        public uint ReadBits12()
        {
            return ReadBits8() | (ReadBits4() << 8);
        }
        public uint ReadBits13()
        {
            return ReadBits8() | (ReadBits5() << 8);
        }
        public uint ReadBits14()
        {
            return ReadBits8() | (ReadBits6() << 8);
        }
        public uint ReadBits15()
        {
            return ReadBits8() | (ReadBits7() << 8);
        }
        public uint ReadBits16()
        {
            if (m_currentPosition + 2 > m_lastPosition)
            {
                ThrowEndOfStreamException();
            }
            uint rv = (uint)m_buffer[m_currentPosition] << 8
                    | (uint)m_buffer[m_currentPosition + 1];
            m_currentPosition += 2;
            return rv;
        }
        public uint ReadBits17()
        {
            return ReadBits16() | (ReadBits1() << 16);

        }
        public uint ReadBits18()
        {
            return ReadBits16() | (ReadBits2() << 16);

        }
        public uint ReadBits19()
        {
            return ReadBits16() | (ReadBits3() << 16);

        }

        public uint ReadBits20()
        {
            return ReadBits16() | (ReadBits4() << 16);

        }
        public uint ReadBits21()
        {
            return ReadBits16() | (ReadBits5() << 16);

        }
        public uint ReadBits22()
        {
            return ReadBits16() | (ReadBits6() << 16);
        }

        public uint ReadBits23()
        {
            return ReadBits16() | (ReadBits7() << 16);

        }
        public uint ReadBits24()
        {
            if (m_currentPosition + 3 > m_lastPosition)
            {
                ThrowEndOfStreamException();
            }
            uint rv = (uint)m_buffer[m_currentPosition] << 16
                      | (uint)m_buffer[m_currentPosition + 1] << 8
                      | (uint)m_buffer[m_currentPosition + 2];
            m_currentPosition += 3;
            return rv;
        }
        public uint ReadBits25()
        {
            return ReadBits24() | (ReadBits1() << 24);
        }
        public uint ReadBits26()
        {
            return ReadBits24() | (ReadBits2() << 24);
        }
        public uint ReadBits27()
        {
            return ReadBits24() | (ReadBits3() << 24);
        }
        public uint ReadBits28()
        {
            return ReadBits24() | (ReadBits4() << 24);
        }
        public uint ReadBits29()
        {
            return ReadBits24() | (ReadBits5() << 24);
        }

        public uint ReadBits30()
        {
            return ReadBits24() | (ReadBits6() << 24);
        }

        public uint ReadBits31()
        {
            return ReadBits24() | (ReadBits7() << 24);
        }

        public uint ReadBits32()
        {
            if (m_currentPosition + 4 > m_lastPosition)
            {
                ThrowEndOfStreamException();
            }
            uint rv = (uint)m_buffer[m_currentPosition] << 24
                      | (uint)m_buffer[m_currentPosition + 1] << 16
                      | (uint)m_buffer[m_currentPosition + 2] << 8
                      | (uint)m_buffer[m_currentPosition + 3];
            m_currentPosition += 4;
            return rv;
        }

        private void ReadMoreBits()
        {
            m_bitCount += 8;
            m_cache = (m_cache << 8) | ReadByte();
        }

        public ulong Read8BitSegments()
        {
            ulong value = 0;
            int bits = 0;
            while (ReadBits1() == 1)
            {
                value = value | ((ulong)ReadByte() << bits);
                bits += 8;
            }
            return value;
        }
        public ulong Read4BitSegments()
        {
            ulong value = 0;
            int bits = 0;
            while (ReadBits1() == 1)
            {
                value = value | ((ulong)ReadBits4() << bits);
                bits += 4;
            }
            return value;
        }

        #endregion
    }
}

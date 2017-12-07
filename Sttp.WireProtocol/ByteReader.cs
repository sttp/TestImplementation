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
        public int Length { get; private set; }

        public ByteReader()
        {
            m_buffer = Empty;
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

        public void SetBuffer(byte[] data, int position, int length)
        {
            m_buffer = data;
            m_startingPosition = position;
            m_currentPosition = position;
            m_lastPosition = length + position;
            Length = length;
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
            if (m_currentPosition + 1 > m_lastPosition)
            {
                ThrowEndOfStreamException();
            }
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
            if (m_currentPosition + 2 > m_lastPosition)
            {
                ThrowEndOfStreamException();
            }
            short rv = BigEndian.ToInt16(m_buffer, m_currentPosition);
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
            int rv = BigEndian.ToInt32(m_buffer, m_currentPosition);
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
            long rv = BigEndian.ToInt64(m_buffer, m_currentPosition);
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

        public SttpNamedSet ReadSttpNamedSet()
        {
            return new SttpNamedSet(this);
        }

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

        public SttpValue ReadSttpValue()
        {
            return SttpValue.Load(this);
        }

        public SttpValueSet ReadSttpValueSet()
        {
            return new SttpValueSet(this);
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

        public List<SttpValue> ReadListSttpValue()
        {
            var rv = new List<SttpValue>();
            int len = ReadInt7Bit();
            while (len > 0)
            {
                rv.Add(SttpValue.Load(this));
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
            throw new NotImplementedException();
        }

        public SttpTimeOffset ReadSttpTimeOffset()
        {
            throw new NotImplementedException();
        }

        public TimeSpan ReadTimeSpan()
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sttp.Codec
{
    public unsafe class PayloadReader
    {
        private static readonly byte[] Empty = new byte[0];

        private byte[] m_buffer;
        private int m_currentPosition;
        private int m_startingPosition;
        private int m_lastPosition;

        private SessionDetails m_sessionDetails;
        public CommandCode Command { get; private set; }
        public int Length { get; private set; }

        public PayloadReader(SessionDetails sessionDetails)
        {
            m_sessionDetails = sessionDetails;
            m_buffer = new byte[512];
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

        internal void SetBuffer(CommandCode code, byte[] data, int position, int length)
        {
            Command = code;
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

        //#region [ Generics ]

        //public List<T> ReadList<T>()
        //{
        //    int length = (int)ReadUInt16();
        //    var collection = new List<T>();

        //    for (int i = 0; i < length; i++)
        //    {
        //        T item1 = Read<T>();

        //        collection.Add(item1);
        //    }

        //    return collection;
        //}
        //public List<Tuple<T1, T2>> ReadList<T1, T2>()
        //{
        //    int length = (int)ReadUInt16();
        //    var collection = new List<Tuple<T1, T2>>();

        //    for (int i = 0; i < length; i++)
        //    {
        //        T1 item1 = Read<T1>();
        //        T2 item2 = Read<T2>();

        //        collection.Add(new Tuple<T1, T2>(item1, item2));
        //    }

        //    return collection;
        //}

        //public List<Tuple<T1, T2, T3>> ReadList<T1, T2, T3>()
        //{
        //    int length = (int)ReadUInt16();
        //    var collection = new List<Tuple<T1, T2, T3>>();

        //    for (int i = 0; i < length; i++)
        //    {
        //        T1 item1 = Read<T1>();
        //        T2 item2 = Read<T2>();
        //        T3 item3 = Read<T3>();

        //        collection.Add(new Tuple<T1, T2, T3>(item1, item2, item3));
        //    }

        //    return collection;
        //}

        ///// <summary>
        ///// Generic interface for reading an Array of values.
        ///// </summary>
        ///// <typeparam name="T">Data type.</typeparam>
        ///// <returns>Array of T[].</returns>
        //public T[] ReadArray<T>()
        //{
        //    int length = (int)ReadUInt16();
        //    var result = new T[length];
        //    for (int i = 0; i < length; i++)
        //    {
        //        result[i] = Read<T>();
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// Generic interface for reading data.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <returns></returns>
        //public T Read<T>()
        //{
        //    var t = typeof(T);

        //    if (t == typeof(Guid))
        //        return (T)(object)ReadGuid();

        //    if (t == typeof(SttpValue))
        //    {
        //        var value = new SttpValue();
        //        value.Load(this);
        //        return (T)(object)value;
        //    }

        //    switch (Type.GetTypeCode(t))
        //    {
        //        case TypeCode.Boolean:
        //            return (T)Convert.ChangeType(ReadBoolean(), typeof(bool));
        //        case TypeCode.Byte:
        //            return (T)Convert.ChangeType(ReadByte(), typeof(byte));
        //        case TypeCode.Char:
        //            return (T)Convert.ChangeType(ReadChar(), typeof(char));
        //        case TypeCode.DateTime:
        //            return (T)Convert.ChangeType(ReadDateTime(), typeof(DateTime));
        //        case TypeCode.Decimal:
        //            return (T)Convert.ChangeType(ReadDecimal(), typeof(Decimal));
        //        case TypeCode.Double:
        //            return (T)Convert.ChangeType(ReadDouble(), typeof(Double));
        //        case TypeCode.Int16:
        //            return (T)Convert.ChangeType(ReadInt16(), typeof(Int16));
        //        case TypeCode.Int32:
        //            return (T)Convert.ChangeType(ReadInt32(), typeof(Int32));
        //        case TypeCode.Int64:
        //            return (T)Convert.ChangeType(ReadInt64(), typeof(Int64));
        //        case TypeCode.SByte:
        //            return (T)Convert.ChangeType(ReadSByte(), typeof(sbyte));
        //        case TypeCode.Single:
        //            return (T)Convert.ChangeType(ReadSingle(), typeof(Single));
        //        case TypeCode.String:
        //            return (T)Convert.ChangeType(ReadString(), typeof(string));
        //        case TypeCode.UInt16:
        //            return (T)Convert.ChangeType(ReadUInt16(), typeof(UInt16));
        //        case TypeCode.UInt32:
        //            return (T)Convert.ChangeType(ReadUInt32(), typeof(UInt32));
        //        case TypeCode.UInt64:
        //            return (T)Convert.ChangeType(ReadUInt64(), typeof(UInt64));
        //        case TypeCode.Object:
        //        case TypeCode.DBNull:
        //        case TypeCode.Empty:
        //        default:
        //            throw new ArgumentException($"Invalid type: {t.FullName}");

        //    }
        //}

        //#endregion Generics

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

        public List<SttpQueryStatement> ReadListSttpQueryStatement()
        {
            var rv = new List<SttpQueryStatement>();
            int len = ReadInt7Bit();
            while (len > 0)
            {
                rv.Add(new SttpQueryStatement(this));
                len--;
            }
            return rv;
        }

        public List<SttpQueryRaw> ReadListSttpQueryRaw()
        {
            var rv = new List<SttpQueryRaw>();
            int len = ReadInt7Bit();
            while (len > 0)
            {
                rv.Add(new SttpQueryRaw(this));
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

        public SttpConnectionString ReadSttpConnectionString()
        {
            return new SttpConnectionString(this);

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
            return new SttpValue(this);
        }

        public SttpValueSet ReadSttpValueSet()
        {
            return new SttpValueSet(this);
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
    }
}

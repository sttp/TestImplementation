using System;
using System.Collections.Generic;
using System.Text;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.WireProtocol
{
    public unsafe class StreamReader : StreamBase
    {
        public int PendingBytes => m_length - Position;

        #region [ 1 byte values ]

        public byte ReadByte()
        {
            if (Position + 1 > m_length)
            {
                Fill();
            }
            byte rv = Buffer[Position];
            Position++;
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
            if (Position + 2 > m_length)
            {
                Fill();
            }
            short rv = BigEndian.ToInt16(Buffer, Position);
            Position += 2;
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
            if (Position + 4 > m_length)
            {
                Fill();
            }
            int rv = BigEndian.ToInt32(Buffer, Position);
            Position += 4;
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
            if (Position + 8 > m_length)
            {
                Fill();
            }
            long rv = BigEndian.ToInt64(Buffer, Position);
            Position += 8;
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
            if (Position + 16 > m_length)
            {
                Fill();
            }
            decimal rv = BigEndian.ToDecimal(Buffer, Position);
            Position += 16;
            return rv;
        }

        public Guid ReadGuid()
        {
            if (Position + 16 > m_length)
            {
                Fill();
            }

            Guid rv = GuidExtensions.ToRfcGuid(Buffer, Position);
            Position += 16;
            return rv;
        }

        #endregion

        private void EnsureCapacity(int length)
        {
            if (Position + length > m_length)
            {
                Fill();
            }
        }

        #region Variable Length

        public byte[] ReadRawBytes(int length)
        {
            byte[] rv = new byte[length];
            Array.Copy(Buffer, Position, rv, 0, length);

            Position += length;
            return rv;
        }

        public byte[] ReadBytes()
        {
            EnsureCapacity(1);
            int length = Buffer[Position];


            if (length == 0)
            {
                Position++;
                return null;
            }
            if (length == 1)
            {
                Position++;
                return StreamBase.Empty;
            }

            if (length >= 128)
            {
                EnsureCapacity(3);
                int pos = uint22.Read(Buffer, Position, out length);
                EnsureCapacity(pos + (length - 1));
                Position += pos;
            }
            else
            {
                EnsureCapacity(length);
                Position++;
            }

            length--;

            byte[] rv = new byte[length];
            Array.Copy(Buffer, Position, rv, 0, length);

            Position += length;
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

        public int ReadInt15()
        {
            EnsureCapacity(1);

            int val = uint15.Read(Buffer, Position, out int len);
            Position += len;

            return val;
        }

        public int ReadInt7Bit()
        {
            uint u = ReadUInt7Bit();
            return *(int*)&u;
        }

        public uint ReadUInt7Bit()
        {
            Read7BitEnsureCapacity();
            return Encoding7Bit.ReadUInt32(Buffer, ref Position);
        }

        private void Read7BitEnsureCapacity()
        {
            if (Buffer[Position] < 128)
            {
                return;
            }
            if (Buffer[Position + 1] < 128)
            {
                EnsureCapacity(1);
                return;
            }
            if (Buffer[Position + 2] < 128)
            {
                EnsureCapacity(2);
                return;
            }
            if (Buffer[Position + 3] < 128)
            {
                EnsureCapacity(3);
                return;
            }
            EnsureCapacity(4);
        }

        #region Generics

        public List<Tuple<T1, T2>> ReadList<T1, T2>()
        {
            int length = (int)ReadUInt16();
            var collection = new List<Tuple<T1, T2>>();

            for (int i = 0; i < length; i++)
            {
                collection.Add(new Tuple<T1, T2>(Read<T1>(), Read<T2>()));
            }

            return collection;
        }

        public List<Tuple<T1, T2, T3>> ReadList<T1, T2, T3>()
        {
            int length = (int)ReadUInt16();
            var collection = new List<Tuple<T1, T2, T3>>();

            for (int i = 0; i < length; i++)
            {
                collection.Add(new Tuple<T1, T2, T3>(Read<T1>(), Read<T2>(),Read<T3>()));
            }

            return collection;
        }

        /// <summary>
        /// Generic interface for reading an Array of values.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <returns>Array of T[].</returns>
        public T[] ReadArray<T>()
        {
            int length = (int)ReadUInt16();
            var result = new T[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = Read<T>();
            }

            return result;
        }


        /// <summary>
        /// Generic interface for reading data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Read<T>()
        {
            var t = typeof(T);

            if (t == typeof(Guid))
                return (T)(object)ReadGuid();

            switch (Type.GetTypeCode(t))
            {
                case TypeCode.Boolean:
                    return (T)Convert.ChangeType(ReadBoolean(), typeof(bool));
                case TypeCode.Byte:
                    return (T)Convert.ChangeType(ReadByte(), typeof(byte));
                case TypeCode.Char:
                    return (T)Convert.ChangeType(ReadChar(), typeof(char));
                case TypeCode.DateTime:
                    return (T)Convert.ChangeType(ReadDateTime(), typeof(DateTime));
                case TypeCode.Decimal:
                    return (T)Convert.ChangeType(ReadDecimal(), typeof(Decimal));
                case TypeCode.Double:
                    return (T)Convert.ChangeType(ReadDouble(), typeof(Double));
                case TypeCode.Int16:
                    return (T)Convert.ChangeType(ReadInt16(), typeof(Int16));
                case TypeCode.Int32:
                    return (T)Convert.ChangeType(ReadInt32(), typeof(Int32));
                case TypeCode.Int64:
                    return (T)Convert.ChangeType(ReadInt64(), typeof(Int64));
                case TypeCode.SByte:
                    return (T)Convert.ChangeType(ReadSByte(), typeof(sbyte));
                case TypeCode.Single:
                    return (T)Convert.ChangeType(ReadSingle(), typeof(Single));
                case TypeCode.String:
                    return (T)Convert.ChangeType(ReadString(), typeof(string));
                case TypeCode.UInt16:
                    return (T)Convert.ChangeType(ReadUInt16(), typeof(UInt16));
                case TypeCode.UInt32:
                    return (T)Convert.ChangeType(ReadUInt32(), typeof(UInt32));
                case TypeCode.UInt64:
                    return (T)Convert.ChangeType(ReadUInt64(), typeof(UInt64));
                case TypeCode.Object:
                case TypeCode.DBNull:
                case TypeCode.Empty:
                default:
                    throw new ArgumentException($"Invalid type: {t.FullName}");

            }
        }

        #endregion Generics
    }
}

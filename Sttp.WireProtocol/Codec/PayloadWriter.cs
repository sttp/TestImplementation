using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Sttp.IO;

namespace Sttp.Codec
{
    public unsafe class PayloadWriter
    {
        private const int UserDataPosition = 15;
        private static readonly byte[] Empty = new byte[0];
        private byte[] m_buffer;
        private int m_position;
        private SessionDetails m_sessionDetails;
        private CommandEncoder m_encoder;

        public PayloadWriter(SessionDetails sessionDetails, CommandEncoder encoder)
        {
            m_encoder = encoder;
            m_sessionDetails = sessionDetails;
            m_buffer = new byte[512];
        }

        public int UserData => m_position - UserDataPosition;

        private void Grow(int neededBytes)
        {
            if (m_position + neededBytes >= m_buffer.Length)
            {
                Grow();
            }
        }

        private void Grow()
        {
            byte[] newBuffer = new byte[m_buffer.Length * 2];
            m_buffer.CopyTo(newBuffer, 0);
            m_buffer = newBuffer;
        }

        public void Send(CommandCode command)
        {
            int length = UserData;
            int offset = UserDataPosition;

            m_encoder.EncodeAndSend(command, m_buffer, offset, length);
            Clear();
        }

        public void Clear()
        {
            m_position = UserDataPosition;
            //Note: Clearing the array isn't required since this class prohibits advancing the position.
            //Array.Clear(m_buffer, 0, m_buffer.Length);
        }




        #region [ Write Methods ]

        #region [ 1 byte values ]

        public void Write(byte value)
        {
            Grow(1);
            m_buffer[m_position] = value;
            m_position++;
        }

        public void Write(bool value)
        {
            if (value)
                Write((byte)1);
            else
                Write((byte)0);
        }

        public void Write(sbyte value)
        {
            Write((byte)value);
        }

        #endregion

        #region [ 2-byte values ]

        public void Write(short value)
        {
            Grow(2);
            m_position += BigEndian.CopyBytes(value, m_buffer, m_position);
        }

        public void Write(ushort value)
        {
            Write((short)value);
        }

        public void Write(char value)
        {
            Write((short)value);
        }

        #endregion

        #region [ 4-byte values ]

        public void Write(int value)
        {
            Grow(4);
            m_position += BigEndian.CopyBytes(value, m_buffer, m_position);
        }

        public void Write(uint value)
        {
            Write((int)value);
        }

        public void Write(float value)
        {
            Write(*(int*)&value);
        }

        #endregion

        #region [ 8-byte values ]

        public void Write(long value)
        {
            Grow(8);
            m_position += BigEndian.CopyBytes(value, m_buffer, m_position);
        }

        public void Write(ulong value)
        {
            Write((long)value);
        }

        public void Write(double value)
        {
            Write(*(long*)&value);
        }

        public void Write(DateTime value)
        {
            Write(value.Ticks);
        }

        #endregion

        #region [ 16-byte values ]

        public void Write(decimal value)
        {
            Grow(16);
            m_position += BigEndian.CopyBytes(value, m_buffer, m_position);
        }

        public void Write(Guid value)
        {
            Grow(16);
            Array.Copy(value.ToRfcBytes(), 0, m_buffer, m_position, 16);
            m_position += 16;
        }

        #endregion

        #region [ Variable Length Types ]

        public void Write(Stream stream, long start, int length)
        {
            if (length > 1024 * 1024)
                throw new ArgumentException("Encoding more than 1MB is prohibited", nameof(length));

            // write null and empty
            Grow(1);

            if (stream == null)
            {
                Write((byte)0);
                return;
            }
            if (length == 0)
            {
                Write((byte)1);
                return;
            }

            Grow(Encoding7Bit.GetSize((uint)(length + 1)) + length);
            WriteUInt7Bit((uint)(length + 1));

            stream.Read(m_buffer, m_position, length);
            m_position += length;
        }

        public void Write(byte[] value, long start, int length)
        {
            if (length > 1024 * 1024)
                throw new ArgumentException("Encoding more than 1MB is prohibited", nameof(length));

            // write null and empty
            Grow(1);

            if (value == null)
            {
                Write((byte)0);
                return;
            }
            if (value.Length == 0)
            {
                Write((byte)1);
                return;
            }

            Grow(Encoding7Bit.GetSize((uint)(length + 1)) + length);
            WriteUInt7Bit((uint)(length + 1));

            Array.Copy(value, start, m_buffer, m_position, length); // write data
            m_position += length;
        }

        public void Write(byte[] value)
        {
            Write(value, 0, value?.Length ?? 0);
        }

        public void Write(string value)
        {
            Grow(1);

            if (value == null)
            {
                Write((byte)0);
                return;
            }
            if (value.Length == 0)
            {
                Write((byte)1);
                return;
            }

            Write(Encoding.UTF8.GetBytes(value));
        }

        #endregion

        #region [ Write Bits ]

        public void WriteInt7Bit(int value)
        {
            WriteUInt7Bit((uint)value);
        }

        public void WriteInt7Bit(long value)
        {
            WriteUInt7Bit((ulong)value);
        }

        public void WriteUInt7Bit(uint value)
        {
            int size = Encoding7Bit.GetSize(value);
            Grow(size);

            // position is incremented within method.
            Encoding7Bit.Write(m_buffer, ref m_position, value);
        }
        public void WriteUInt7Bit(ulong value)
        {
            int size = Encoding7Bit.GetSize(value);
            Grow(size);

            // position is incremented within method.
            Encoding7Bit.Write(m_buffer, ref m_position, value);
        }

        #endregion

        //#region Generics

        //private bool WriteCollectionHeader<T>(IList<T> collection)
        //{
        //    if (collection == null)
        //    {
        //        Write((ushort)0);
        //        return false;
        //    }

        //    if (collection.Count > short.MaxValue)
        //        throw new ArgumentOutOfRangeException(nameof(collection), "Length must be between 0 and 32767");

        //    Write((ushort)collection.Count);
        //    return true;
        //}

        ///// <summary>
        ///// Writes a collection to the buffer.
        ///// </summary>
        ///// <typeparam name="T">Data type.</typeparam>
        ///// <param name="collection"></param>
        //public void WriteArray<T>(T[] collection)
        //{
        //    if (!WriteCollectionHeader(collection)) return;

        //    for (int i = 0; i < collection.Length; i++)
        //    {
        //        Write(collection[i]);
        //    }
        //}

        //public void WriteList<T1, T2>(IList<Tuple<T1, T2>> collection)
        //{
        //    if (!WriteCollectionHeader(collection)) return;

        //    for (var i = 0; i < collection.Count; i++)
        //    {
        //        Write(collection[i].Item1);
        //        Write(collection[i].Item2);
        //    }
        //}

        //public void WriteList<T1, T2, T3>(List<Tuple<T1, T2, T3>> collection)
        //{
        //    if (!WriteCollectionHeader(collection)) return;

        //    for (var i = 0; i < collection.Count; i++)
        //    {
        //        Write(collection[i].Item1);
        //        Write(collection[i].Item2);
        //        Write(collection[i].Item3);
        //    }
        //}


        ///// <summary>
        ///// Generic interface for writing a value.
        ///// </summary>
        ///// <typeparam name="T">Type of value.</typeparam>
        ///// <param name="value">Value to write.</param>
        //public void Write<T>(T value)
        //{
        //    var t = typeof(T);

        //    // special guid handling
        //    if (t == typeof(Guid))
        //    {
        //        Write((Guid)(object)value);
        //        return;
        //    }
        //    if (t == typeof(SttpValue))
        //    {
        //        ((SttpValue)(object)value).Save(this, true);
        //    }
        //    switch (Type.GetTypeCode(t))
        //    {
        //        case TypeCode.Boolean:
        //            Write(Convert.ToBoolean(value));
        //            break;
        //        case TypeCode.Byte:
        //            Write(Convert.ToByte(value));
        //            break;
        //        case TypeCode.Char:
        //            Write(Convert.ToChar(value));
        //            break;
        //        case TypeCode.DateTime:
        //            Write(Convert.ToDateTime(value));
        //            break;
        //        case TypeCode.Decimal:
        //            Write(Convert.ToDecimal(value));
        //            break;
        //        case TypeCode.Double:
        //            Write(Convert.ToDouble(value));
        //            break;
        //        case TypeCode.Int16:
        //            Write(Convert.ToInt16(value));
        //            break;
        //        case TypeCode.Int32:
        //            Write(Convert.ToInt32(value));
        //            break;
        //        case TypeCode.Int64:
        //            Write(Convert.ToInt64(value));
        //            break;
        //        case TypeCode.SByte:
        //            Write(Convert.ToSByte(value));
        //            break;
        //        case TypeCode.Single:
        //            Write(Convert.ToSingle(value));
        //            break;
        //        case TypeCode.String:
        //            Write(Convert.ToString(value));
        //            break;
        //        case TypeCode.UInt16:
        //            Write(Convert.ToUInt16(value));
        //            break;
        //        case TypeCode.UInt32:
        //            Write(Convert.ToUInt32(value));
        //            break;
        //        case TypeCode.UInt64:
        //            Write(Convert.ToUInt64(value));
        //            break;
        //        case TypeCode.Object:
        //        case TypeCode.DBNull:
        //        case TypeCode.Empty:
        //        default:
        //            throw new ArgumentException(nameof(value), $"Invalid type: {t.FullName}");
        //    }
        //}

        //#endregion Generics

        #endregion

        public void Write(List<MetadataColumn> list)
        {
            if (list == null)
            {
                Write((byte)0);
                return;
            }
            WriteInt7Bit(list.Count);
            for (var x = 0; x < list.Count; x++)
            {
                list[x].Save(this);
            }
        }

        public void Write(List<MetadataForeignKey> list)
        {
            if (list == null)
            {
                Write((byte)0);
                return;
            }
            WriteInt7Bit(list.Count);
            for (var x = 0; x < list.Count; x++)
            {
                list[x].Save(this);
            }
        }



        public void Write(List<SttpDataPointID> list)
        {
            if (list == null)
            {
                Write((byte)0);
                return;
            }
            WriteInt7Bit(list.Count);
            for (var x = 0; x < list.Count; x++)
            {
                list[x].Save(this);
            }
        }

        public void Write(SttpNamedSet value)
        {
            value.Save(this);
        }



        public void Write(List<SttpQueryStatement> list)
        {
            if (list == null)
            {
                Write((byte)0);
                return;
            }
            WriteInt7Bit(list.Count);
            for (var x = 0; x < list.Count; x++)
            {
                list[x].Save(this);
            }
        }

        public void Write(List<SttpQueryRaw> list)
        {
            if (list == null)
            {
                Write((byte)0);
                return;
            }
            WriteInt7Bit(list.Count);
            for (var x = 0; x < list.Count; x++)
            {
                list[x].Save(this);
            }
        }

        public void Write(List<MetadataSchemaTables> list)
        {
            if (list == null)
            {
                Write((byte)0);
                return;
            }
            WriteInt7Bit(list.Count);
            for (var x = 0; x < list.Count; x++)
            {
                list[x].Save(this);
            }
        }

        public void Write(List<MetadataSchemaTableUpdate> list)
        {
            if (list == null)
            {
                Write((byte)0);
                return;
            }
            WriteInt7Bit(list.Count);
            for (var x = 0; x < list.Count; x++)
            {
                list[x].Save(this);
            }
        }

        public void Write(SttpValueSet value)
        {
            value.Save(this);
        }

        public void Write(SttpValue value)
        {
            value.Save(this);
        }

        public void Write(SttpConnectionString value)
        {
            value.Save(this);
        }

        public void Write(List<SttpQueryJoinedTable> list)
        {
            if (list == null)
            {
                Write((byte)0);
                return;
            }
            WriteInt7Bit(list.Count);
            for (var x = 0; x < list.Count; x++)
            {
                list[x].Save(this);
            }
        }

        public void Write(List<SttpQueryLiterals> list)
        {
            if (list == null)
            {
                Write((byte)0);
                return;
            }
            WriteInt7Bit(list.Count);
            for (var x = 0; x < list.Count; x++)
            {
                list[x].Save(this);
            }
        }

        public void Write(List<SttpQueryColumn> list)
        {
            if (list == null)
            {
                Write((byte)0);
                return;
            }
            WriteInt7Bit(list.Count);
            for (var x = 0; x < list.Count; x++)
            {
                list[x].Save(this);
            }
        }

        public void Write(List<SttpProcedureStep> list)
        {
            if (list == null)
            {
                Write((byte)0);
                return;
            }
            WriteInt7Bit(list.Count);
            for (var x = 0; x < list.Count; x++)
            {
                list[x].Save(this);
            }
        }

        public void Write(List<SttpOutputColumns> list)
        {
            if (list == null)
            {
                Write((byte)0);
                return;
            }
            WriteInt7Bit(list.Count);
            for (var x = 0; x < list.Count; x++)
            {
                list[x].Save(this);
            }
        }

        public void Write(List<int> list)
        {
            if (list == null)
            {
                Write((byte)0);
                return;
            }
            WriteInt7Bit(list.Count);
            for (var x = 0; x < list.Count; x++)
            {
                Write(list[x]);
            }
        }

        public void Write(List<SttpValue> list)
        {
            if (list == null)
            {
                Write((byte)0);
                return;
            }
            WriteInt7Bit(list.Count);
            for (var x = 0; x < list.Count; x++)
            {
                list[x].Save(this);
            }
        }
    }
}


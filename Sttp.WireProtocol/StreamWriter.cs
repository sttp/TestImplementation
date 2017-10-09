using System;
using System.Text;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.WireProtocol
{
    public unsafe class StreamWriter : StreamBase
    {
        #region [ 1 byte values ]

        public void Write(byte value)
        {
            if (Position + 1 >= Buffer.Length)
            {
                Grow();
            }
            Buffer[Position] = value;
            Position++;
            m_length = Position;
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
            if (Position + 2 >= Buffer.Length)
            {
                Grow();
            }
            Position += BigEndian.CopyBytes(value, Buffer, Position);
            m_length = Position;
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
            if (Position + 4 >= Buffer.Length)
            {
                Grow();
            }
            Position += BigEndian.CopyBytes(value, Buffer, Position);
            m_length = Position;
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
            if (Position + 8 >= Buffer.Length)
            {
                Grow();
            }
            Position += BigEndian.CopyBytes(value, Buffer, Position);
            m_length = Position;
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
            if (Position + 16 >= Buffer.Length)
            {
                Grow();
            }
            Position += BigEndian.CopyBytes(value, Buffer, Position);
            m_length = Position;
        }

        public void Write(Guid value)
        {
            if (Position + 16 >= Buffer.Length)
            {
                Grow();
            }
            Array.Copy(value.ToRfcBytes(), 0, Buffer, Position, 16);
            Position += 16;
            m_length = Position;
        }

        #endregion

        #region Variable Length Types
        public void Write(byte[] value)
        {
            int len = value?.Length ?? 1;
            if (len > 1024 * 1024)
                throw new ArgumentException("Encoding more than 1MB is prohibited", nameof(value));

            // write null and empty
            if (Position + 1 >= Buffer.Length)
            {
                Grow();
            }

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

            while (Position + len + 3 >= Buffer.Length)
            {
                Grow();
            }

            int length = value.Length + 1; //A length of 0 is null

            Position += uint22.Write(Buffer, Position, length); // write len
            Array.Copy(value, 0, Buffer, Position, value.Length); // write data
            Position += len;
            m_length = Position;
        }

        public void WriteRaw(byte[] value)
        {
            Array.Copy(value, 0, Buffer, Position, value.Length); // write data
            Position += value.Length;
            m_length = Position;
        }

        public void Write(string value)
        {
            if (Position + 1 >= Buffer.Length)
            {
                Grow();
            }

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

        public void WriteInt15(int value)
        {
            if (Position + 2 >= Buffer.Length)
            {
                Grow();
            }
            Position += uint15.Write(Buffer, Position, value);
            m_length = Position;
        }

        #region Generics

        /// <summary>
        /// Writes a collection to the buffer.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="collection"></param>
        public void WriteArray<T>(T[] collection)
        {
            if (collection == null)
            {
                WriteInt15(0);
                return;
            }

            if (collection.Length > short.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(collection), "Length must be between 0 and 32767");

            WriteInt15(collection.Length);
            for (int i = 0; i < collection.Length; i++)
            {
                Write(collection[i]);
            }
        }

        /// <summary>
        /// Generic interface for writing a value.
        /// </summary>
        /// <typeparam name="T">Type of value.</typeparam>
        /// <param name="value">Value to write.</param>
        public void Write<T>(T value)
        {
            var t = typeof(T);

            // special guid handling
            if (t == typeof(Guid))
            {
                Write((Guid)(object)value);
                return;
            }

            switch (Type.GetTypeCode(t))
            {
                case TypeCode.Boolean:
                    Write(Convert.ToBoolean(value));
                    break;
                case TypeCode.Byte:
                    Write(Convert.ToByte(value));
                    break;
                case TypeCode.Char:
                    Write(Convert.ToChar(value));
                    break;
                case TypeCode.DateTime:
                    Write(Convert.ToDateTime(value));
                    break;
                case TypeCode.Decimal:
                    Write(Convert.ToDecimal(value));
                    break;
                case TypeCode.Double:
                    Write(Convert.ToDouble(value));
                    break;
                case TypeCode.Int16:
                    Write(Convert.ToInt16(value));
                    break;
                case TypeCode.Int32:
                    Write(Convert.ToInt32(value));
                    break;
                case TypeCode.Int64:
                    Write(Convert.ToInt64(value));
                    break;
                case TypeCode.SByte:
                    Write(Convert.ToSByte(value));
                    break;
                case TypeCode.Single:
                    Write(Convert.ToSingle(value));
                    break;
                case TypeCode.String:
                    Write(Convert.ToString(value));
                    break;
                case TypeCode.UInt16:
                    Write(Convert.ToUInt16(value));
                    break;
                case TypeCode.UInt32:
                    Write(Convert.ToUInt32(value));
                    break;
                case TypeCode.UInt64:
                    Write(Convert.ToUInt64(value));
                    break;
                case TypeCode.Object:
                case TypeCode.DBNull:
                case TypeCode.Empty:
                default:
                    throw new ArgumentException(nameof(value), $"Invalid type: {t.FullName}");
            }
        }

        #endregion Generics

    }
}


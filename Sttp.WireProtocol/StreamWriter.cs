using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Sttp.IO;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.WireProtocol
{
    public unsafe class StreamWriter 
    {
        protected static readonly byte[] Empty = new byte[0];
        public byte[] Buffer;

        public int Position;
        protected int m_length;

        public StreamWriter()
        {
            Buffer = new byte[512];
        }

        public int Length => m_length;

        protected void Grow(int neededBytes)
        {
            if (Position + neededBytes >= Buffer.Length)
            {
                Grow();
            }
        }
        public byte[] ToArray()
        {
            byte[] rv = new byte[m_length];
            Array.Copy(Buffer, 0, rv, 0, m_length);
            return rv;
        }
        public void Clear()
        {
            Position = 0;
            m_length = 0;

            Array.Clear(Buffer, 0, Buffer.Length);
        }
        protected void Grow()
        {
            byte[] newBuffer = new byte[Buffer.Length * 2];
            Buffer.CopyTo(newBuffer, 0);
            Buffer = newBuffer;
        }

        protected void ExtendToPosition()
        {
            if (Position > m_length)
            {
                m_length = Position;
            }
        }

        #region [ 1 byte values ]

        public void Write(byte value)
        {
            Grow(1);
            Buffer[Position] = value;
            Position++;
            ExtendToPosition();
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
            Position += BigEndian.CopyBytes(value, Buffer, Position);
            ExtendToPosition();
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
            Position += BigEndian.CopyBytes(value, Buffer, Position);
            ExtendToPosition();
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
            Position += BigEndian.CopyBytes(value, Buffer, Position);
            ExtendToPosition();
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
            Position += BigEndian.CopyBytes(value, Buffer, Position);
            ExtendToPosition();
        }

        public void Write(Guid value)
        {
            Grow(16);
            Array.Copy(value.ToRfcBytes(), 0, Buffer, Position, 16);
            Position += 16;
            ExtendToPosition();
        }

        #endregion

        #region Variable Length Types

        public void Write(System.IO.Stream stream, long start, int length)
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

            stream.Read(Buffer, Position, length);
            Position += length;
            ExtendToPosition();
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

            Array.Copy(value, start, Buffer, Position, length); // write data
            Position += length;
            ExtendToPosition();
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

        public void WriteInt15(int value)
        {
            Grow(2);
            Position += uint15.Write(Buffer, Position, value);
            ExtendToPosition();
        }

       

        public void WriteInt7Bit(int value)
        {
            uint u = *(uint*)&value;
            WriteUInt7Bit(u);
        }

        public void WriteUInt7Bit(uint value)
        {
            int size = Encoding7Bit.GetSize(value);
            Grow(size);

            // position is incremented within method.
            Encoding7Bit.Write(Buffer, ref Position, value);
            ExtendToPosition();
        }

        #region Generics

        private bool WriteCollectionHeader<T>(IList<T> collection)
        {
            if (collection == null)
            {
                Write((ushort)0);
                return false;
            }

            if (collection.Count > short.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(collection), "Length must be between 0 and 32767");

            Write((ushort)collection.Count);
            return true;
        }

        /// <summary>
        /// Writes a collection to the buffer.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="collection"></param>
        public void WriteArray<T>(T[] collection)
        {
            if (!WriteCollectionHeader(collection)) return;

            for (int i = 0; i < collection.Length; i++)
            {
                Write(collection[i]);
            }
        }

        public void WriteList<T1, T2>(IList<Tuple<T1, T2>> collection)
        {
            if (!WriteCollectionHeader(collection)) return;

            for (var i = 0; i < collection.Count; i++)
            {
                Write(collection[i].Item1);
                Write(collection[i].Item2);
            }
        }

        public void WriteList<T1, T2, T3>(List<Tuple<T1, T2, T3>> collection)
        {
            if (!WriteCollectionHeader(collection)) return;

            for (var i = 0; i < collection.Count; i++)
            {
                Write(collection[i].Item1);
                Write(collection[i].Item2);
                Write(collection[i].Item3);
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


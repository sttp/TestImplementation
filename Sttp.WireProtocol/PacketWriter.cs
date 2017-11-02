using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Sttp.IO;

namespace Sttp.WireProtocol
{
    public unsafe class PacketWriter
    {
        private const int UserDataPosition = 30;
        private static readonly byte[] Empty = new byte[0];
        private byte[] m_buffer;
        private int m_position;
        private CommandCode m_command;
        private SessionDetails m_sessionDetails;

        public PacketWriter(SessionDetails sessionDetails)
        {
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

        public byte[] ToArray()
        {
            byte[] rv = new byte[m_position];
            Array.Copy(m_buffer, 0, rv, 0, m_position);
            return rv;
        }

        public void BeginCommand(CommandCode command)
        {
            Clear();
            m_command = command;
            m_position = UserDataPosition;
        }

        public void EndCommand(Action<byte[], int, int> sendPacket)
        {
            int length = UserData;
            int offset = UserDataPosition;
            if (length == 0)
                return;

            m_position = offset;
            Write(m_command);
            Write(length);

            if (length > m_sessionDetails.Limits.MetadataResponseSizeLimit)
            {
                //ToDo: Properly enforce limits based on packet sizes.
                throw new Exception("Exceeded a limit");
            }

            if (m_sessionDetails.SupportsDeflate)
            {
                SendCompressed(sendPacket, offset, length);
                return;
            }

            if (length + 3 < m_sessionDetails.MaximumSegmentSize)
            {
                offset -= 3;
                length += 3;
                m_position = offset;
                Write(m_command);
                Write((short)length);
                sendPacket(m_buffer, offset, length);
            }
            else
            {
                SendFragmentedPacket(sendPacket, length, length, 0, offset, length);
            }
        }

        private void SendCompressed(Action<byte[], int, int> sendPacket, int offset, int length)
        {
            //ToDo: Determine if and what kind of compression should occur

            int compressedSize;
            using (var ms = new MemoryStream())
            {
                using (var deflate = new DeflateStream(ms, CompressionMode.Compress, true))
                {
                    deflate.Write(m_buffer, offset, length);
                }
                ms.Position = 0;
                compressedSize = (int)ms.Length;
                while (m_buffer.Length < ms.Length + UserDataPosition)
                {
                    Grow();
                }
                ms.ReadAll(m_buffer, UserDataPosition, compressedSize);
            }

            if (compressedSize + 3 + 9 <= m_sessionDetails.MaximumSegmentSize)
            {
                SendCompressedPacket(sendPacket, length, 1, UserDataPosition, compressedSize);
                return;
            }

            SendFragmentedPacket(sendPacket, compressedSize, length, 1, UserDataPosition, compressedSize);
        }

        private void SendFragmentedPacket(Action<byte[], int, int> sendPacket, int totalFragmentedSize, int totalRawSize, byte compressionMode, int offset, int length)
        {
            const int Overhead = 1 + 2 + 4 + 4 + 1 + 1; //13 bytes.
            const int Overhead2 = 1 + 2; //3 bytes.

            int sentLength = Math.Min(m_sessionDetails.MaximumSegmentSize - Overhead, length);

            m_position = offset - Overhead;
            Write(CommandCode.BeginFragment);
            Write((short)(sentLength + Overhead));
            Write(totalFragmentedSize);
            Write(totalRawSize);
            Write(m_command);
            Write(compressionMode);
            sendPacket(m_buffer, offset - Overhead, sentLength + Overhead);

            offset += sentLength;
            while (sentLength < length)
            {
                int nextLength = Math.Min(m_sessionDetails.MaximumSegmentSize - Overhead2, length - sentLength);

                m_position = offset - Overhead2;
                Write(CommandCode.NextFragment);
                Write((short)(nextLength + Overhead2));
                Write(nextLength);

                sendPacket(m_buffer, offset - Overhead2, nextLength + Overhead2);

                sentLength += nextLength;
                offset += nextLength;
            }

        }

        private void SendCompressedPacket(Action<byte[], int, int> sendPacket, int totalSize, byte compressionMode, int offset, int length)
        {
            const int Overhead = 1 + 2 + 4 + 1 + 1; //9 bytes.
            offset -= Overhead;
            length += Overhead;
            m_position = offset - Overhead;
            Write(CommandCode.CompressedPacket);
            Write((short)(length + Overhead));
            Write(totalSize);
            Write(m_command);
            Write(compressionMode);
            sendPacket(m_buffer, offset - Overhead, length + Overhead);
        }

        public void Clear()
        {
            m_position = 0;
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

        public void WriteInt15(int value)
        {
            Grow(2);
            m_position += uint15.Write(m_buffer, m_position, value);
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
            Encoding7Bit.Write(m_buffer, ref m_position, value);
        }

        #endregion

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
            if (t == typeof(SttpValue))
            {
                ((SttpValue)(object)value).Save(this);
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

        #endregion
    }
}


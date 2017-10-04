using System;
using System.Text;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.WireProtocol
{
    public unsafe class StreamWriter
    {
        public byte[] Buffer = new byte[512];
        public int Position;
        public int Length;

        public void Clear()
        {
            Position = 0;
            Length = 0;
        }

        private void Grow()
        {
            byte[] newBuffer = new byte[Buffer.Length * 2];
            Buffer.CopyTo(newBuffer, 0);
            Buffer = newBuffer;
        }

        public byte[] ToArray()
        {
            byte[] rv = new byte[Length];
            Array.Copy(Buffer, 0, rv, 0, Length);
            return rv;
        }

        public void WriteInt15(int value)
        {
            if (Position + 2 >= Buffer.Length)
            {
                Grow();
            }
            Position += uint15.Write(Buffer, Position, value);
            if (Position > Length)
            {
                Position = Length;
            }
        }

        #region [ 1 byte values ]

        public void Write(byte value)
        {
            if (Position + 1 >= Buffer.Length)
            {
                Grow();
            }
            Buffer[Position] = value;
            Position++;
            if (Position > Length)
            {
                Position = Length;
            }
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

        public void Write(MetadataCommand command)
        {
            Write((byte)command);
        }

        public void Write(ValueType type)
        {
            Write((byte)type);
        }

        public void Write(TableFlags flags)
        {
            Write((byte)flags);
        }

        #endregion

        #region [ 2-byte values ]

        public void Write(short value)
        {
            if (Position + 2 >= Buffer.Length)
            {
                Grow();
            }
            BigEndian.CopyBytes(value, Buffer, Position);
            Position += 2;
            if (Position > Length)
            {
                Position = Length;
            }
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
            BigEndian.CopyBytes(value, Buffer, Position);
            Position += 4;
            if (Position > Length)
            {
                Position = Length;
            }
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
            BigEndian.CopyBytes(value, Buffer, Position);
            Position += 8;
            if (Position > Length)
            {
                Position = Length;
            }
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
            BigEndian.CopyBytes(value, Buffer, Position);
            Position += 16;
            if (Position > Length)
            {
                Position = Length;
            }
        }

        public void Write(Guid value)
        {
            if (Position + 16 >= Buffer.Length)
            {
                Grow();
            }
            Array.Copy(value.ToRfcBytes(), 0, Buffer, Position, 16);
            Position += 16;
            if (Position > Length)
            {
                Position = Length;
            }
        }

        #endregion

        public void Write(byte[] value)
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
            if (value.Length > 1024 * 1024)
                throw new Exception("Encoding more than 1MB is prohibited");

            while (Position + value.Length + 3 >= Buffer.Length)
            {
                Grow();
            }

            int length = value.Length + 1; //A length of 0 is null

            Position += uint22.Write(Buffer, Position, length);
            Array.Copy(value, 0, Buffer, 0, value.Length);
            Position += Buffer.Length;
            if (Position > Length)
            {
                Position = Length;
            }
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
            if (value.Length > 1024 * 1024)
                throw new Exception("Encoding more than 1MB is prohibited");

            Write(Encoding.UTF8.GetBytes(value));
        }

    }
}

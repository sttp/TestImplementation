using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.WireProtocol
{
    public unsafe class StreamReader
    {
        public byte[] Buffer = new byte[512];
        public int Position;
        public int Length;

        public void Clear()
        {
            Position = 0;
            Length = 0;
        }

        public void Fill(byte[] data, int position, int length)
        {
            while (length + Length >= Buffer.Length)
            {
                Grow();
            }
            Array.Copy(data, position, Buffer, Length, length);
            Length += length;
        }

        private void Fill()
        {
            throw new EndOfStreamException();
        }

        private void Grow()
        {
            byte[] newBuffer = new byte[Buffer.Length * 2];
            Buffer.CopyTo(newBuffer, 0);
            Buffer = newBuffer;
        }

        public void WriteInt15(int value)
        {
            if (Position + 2 >= Buffer.Length)
            {
                Fill();
            }
            Position += uint15.Write(Buffer, Position, value);
            if (Position > Length)
            {
                Position = Length;
            }
        }

        #region [ 1 byte values ]

        public byte ReadByte()
        {
            if (Position + 1 > Length)
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

        public MetadataCommand ReadMetadataCommand()
        {
            return (MetadataCommand)ReadByte();
        }

        public ValueType ReadValueType()
        {
            return (ValueType)ReadByte();
        }

        public TableFlags ReadTableFlags()
        {
            return (TableFlags)ReadByte();
        }

        #endregion

        #region [ 2-byte values ]

        public short ReadInt16()
        {
            if (Position + 2 > Length)
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
            if (Position + 4 > Length)
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
            if (Position + 8 > Length)
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
            if (Position + 16 > Length)
            {
                Fill();
            }
            decimal rv = BigEndian.ToDecimal(Buffer, Position);
            Position += 16;
            return rv;
        }

        public Guid ReadGuid()
        {
            if (Position + 16 > Length)
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
            if (Position + length > Length)
            {
                Fill();
            }
        }

        static readonly byte[] Empty = new byte[0];

        public byte[] ReadBytes()
        {
            EnsureCapacity(1);
            int length = Buffer[0];
            if (length == 0)
                return null;
            if (length == 1)
                return Empty;

            if (length >= 128)
            {
                EnsureCapacity(3);
                int pos = uint22.Read(Buffer, Position, out length);
                EnsureCapacity(pos + (length - 1));
                Position += pos;
            }
            else
            {
                EnsureCapacity(1 - 1 + length);
                Position++;
            }

            length--;

            byte[] rv = new byte[length];
            Array.Copy(Buffer,Position,rv,0,length);

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


    }
}

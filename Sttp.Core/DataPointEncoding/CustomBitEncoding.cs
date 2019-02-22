using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTP;

namespace Sttp.DataPointEncoding
{
    public unsafe class CustomBitEncoding
    {
        const ulong Bits64 = 0xFFFFFFFFFFFFFFFFu;
        const ulong Bits60 = 0xFFFFFFFFFFFFFFFu;
        const ulong Bits56 = 0xFFFFFFFFFFFFFFu;
        const ulong Bits52 = 0xFFFFFFFFFFFFFu;
        const ulong Bits48 = 0xFFFFFFFFFFFFu;
        const ulong Bits44 = 0xFFFFFFFFFFFu;
        const ulong Bits40 = 0xFFFFFFFFFFu;
        const ulong Bits36 = 0xFFFFFFFFFu;
        const uint Bits32 = 0xFFFFFFFFu;
        const uint Bits28 = 0xFFFFFFFu;
        const uint Bits24 = 0xFFFFFFu;
        const uint Bits20 = 0xFFFFFu;
        const uint Bits16 = 0xFFFFu;
        const uint Bits12 = 0xFFFu;
        const uint Bits8 = 0xFFu;
        const uint Bits4 = 0xFu;

        private BitStreamWriter m_writer;

        public CustomBitEncoding(BitStreamWriter writer)
        {
            m_writer = writer;
        }

        public void Clear()
        {

        }

        public void WriteBitsChanged64(ulong bitsChanged)
        {
            if (bitsChanged <= Bits4)
            {
                m_writer.WriteBits4(0);
                m_writer.WriteBits4((uint)bitsChanged);
            }
            else if (bitsChanged <= Bits8)
            {
                m_writer.WriteBits4(1);
                m_writer.WriteBits8((uint)bitsChanged);
            }
            else if (bitsChanged <= Bits12)
            {
                m_writer.WriteBits4(2);
                m_writer.WriteBits12((uint)bitsChanged);
            }
            else if (bitsChanged <= Bits16)
            {
                m_writer.WriteBits4(3);
                m_writer.WriteBits16((uint)bitsChanged);
            }
            else if (bitsChanged <= Bits20)
            {
                m_writer.WriteBits4(4);
                m_writer.WriteBits20((uint)bitsChanged);
            }
            else if (bitsChanged <= Bits24)
            {
                m_writer.WriteBits4(5);
                m_writer.WriteBits24((uint)bitsChanged);
            }
            else if (bitsChanged <= Bits28)
            {
                m_writer.WriteBits4(6);
                m_writer.WriteBits28((uint)bitsChanged);
            }
            else if (bitsChanged <= Bits32)
            {
                m_writer.WriteBits4(7);
                m_writer.WriteBits32((uint)bitsChanged);
            }
            else if (bitsChanged <= Bits36)
            {
                m_writer.WriteBits4(8);
                m_writer.WriteBits(36, bitsChanged);
            }
            else if (bitsChanged <= Bits40)
            {
                m_writer.WriteBits4(9);
                m_writer.WriteBits(40, bitsChanged);
            }
            else if (bitsChanged <= Bits44)
            {
                m_writer.WriteBits4(10);
                m_writer.WriteBits(44, bitsChanged);
            }
            else if (bitsChanged <= Bits48)
            {
                m_writer.WriteBits4(11);
                m_writer.WriteBits(48, bitsChanged);
            }
            else if (bitsChanged <= Bits52)
            {
                m_writer.WriteBits4(12);
                m_writer.WriteBits(52, bitsChanged);
            }
            else if (bitsChanged <= Bits56)
            {
                m_writer.WriteBits4(13);
                m_writer.WriteBits(56, bitsChanged);
            }
            else if (bitsChanged <= Bits60)
            {
                m_writer.WriteBits4(14);
                m_writer.WriteBits(60, bitsChanged);
            }
            else
            {
                m_writer.WriteBits4(15);
                m_writer.WriteBits(64, bitsChanged);
            }
        }

        public void WriteBitsChanged32(uint bitsChanged)
        {
            if (bitsChanged <= Bits4)
            {
                m_writer.WriteBits3(0);
                m_writer.WriteBits4(bitsChanged);
            }
            else if (bitsChanged <= Bits8)
            {
                m_writer.WriteBits3(1);
                m_writer.WriteBits8(bitsChanged);
            }
            else if (bitsChanged <= Bits12)
            {
                m_writer.WriteBits3(2);
                m_writer.WriteBits12(bitsChanged);
            }
            else if (bitsChanged <= Bits16)
            {
                m_writer.WriteBits3(3);
                m_writer.WriteBits16(bitsChanged);
            }
            else if (bitsChanged <= Bits20)
            {
                m_writer.WriteBits3(4);
                m_writer.WriteBits20(bitsChanged);
            }
            else if (bitsChanged <= Bits24)
            {
                m_writer.WriteBits3(5);
                m_writer.WriteBits24(bitsChanged);
            }
            else if (bitsChanged <= Bits28)
            {
                m_writer.WriteBits3(6);
                m_writer.WriteBits28(bitsChanged);
            }
            else
            {
                m_writer.WriteBits3(7);
                m_writer.WriteBits32(bitsChanged);
            }
        }

        public void WriteInt32(int value)
        {
            if (value < 0)
            {
                m_writer.WriteBits1(1);
                value = ~value;
            }
            else
            {
                m_writer.WriteBits1(0);
            }

            if (value <= Bits4)
            {
                m_writer.WriteBits3(0);
                m_writer.WriteBits4((uint)value);
            }
            else if (value <= Bits8)
            {
                m_writer.WriteBits3(1);
                m_writer.WriteBits8((uint)value);
            }
            else if (value <= Bits12)
            {
                m_writer.WriteBits3(2);
                m_writer.WriteBits12((uint)value);
            }
            else if (value <= Bits16)
            {
                m_writer.WriteBits3(3);
                m_writer.WriteBits16((uint)value);
            }
            else if (value <= Bits20)
            {
                m_writer.WriteBits3(4);
                m_writer.WriteBits20((uint)value);
            }
            else if (value <= Bits24)
            {
                m_writer.WriteBits3(5);
                m_writer.WriteBits24((uint)value);
            }
            else if (value <= Bits28)
            {
                m_writer.WriteBits3(6);
                m_writer.WriteBits28((uint)value);
            }
            else
            {
                m_writer.WriteBits3(7);
                m_writer.WriteBits32((uint)value);
            }
        }

        public void WriteInt64(long value)
        {
            if (value < 0)
            {
                m_writer.WriteBits1(1);
                value = ~value;
            }
            else
            {
                m_writer.WriteBits1(0);
            }

            if (value <= Bits8)
            {
                m_writer.WriteBits3(0);
                m_writer.WriteBits8((uint)(int)value);
            }
            else if (value <= Bits16)
            {
                m_writer.WriteBits3(1);
                m_writer.WriteBits16((uint)(int)value);
            }
            else if (value <= Bits24)
            {
                m_writer.WriteBits3(2);
                m_writer.WriteBits24((uint)(int)value);
            }
            else if (value <= Bits32)
            {
                m_writer.WriteBits3(3);
                m_writer.WriteBits32((uint)(int)value);
            }
            else if (value <= (long)Bits40)
            {
                m_writer.WriteBits3(4);
                m_writer.WriteBits(40, (ulong)value);
            }
            else if (value <= (long)Bits48)
            {
                m_writer.WriteBits3(5);
                m_writer.WriteBits(48, (ulong)value);
            }
            else if (value <= (long)Bits56)
            {
                m_writer.WriteBits3(6);
                m_writer.WriteBits(56, (ulong)value);
            }
            else
            {
                m_writer.WriteBits3(7);
                m_writer.WriteBits(64, (ulong)value);
            }
        }

        public void WriteSingle(float value)
        {
            uint v = *(uint*)&value;
            m_writer.WriteBits32(v);
        }

        public void WriteBoolean(bool value)
        {
            m_writer.WriteBits1(value);
        }

        public void WriteDouble(double value)
        {
            ulong v = *(ulong*)&value;
            m_writer.WriteBits(64,v);
        }
        public void WriteTime(CtpTime value)
        {
            m_writer.WriteBits(64, (ulong)value.Ticks);
        }
    }
}

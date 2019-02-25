using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTP;

namespace Sttp.DataPointEncoding
{
    public static unsafe class CustomBitEncoding
    {
        const ulong Bits64 = 0xFFFFFFFFFFFFFFFFu;
        const ulong Bits60 = 0xFFFFFFFFFFFFFFFu;
        const ulong Bits56 = 0xFFFFFFFFFFFFFFu;
        const ulong Bits52 = 0xFFFFFFFFFFFFFu;
        const ulong Bits48 = 0xFFFFFFFFFFFFu;
        const ulong Bits44 = 0xFFFFFFFFFFFu;
        const ulong Bits40 = 0xFFFFFFFFFFu;
        const ulong Bits37 = 0x1FFFFFFFFFu;
        const ulong Bits36 = 0xFFFFFFFFFu;
        const uint Bits32 = 0xFFFFFFFFu;
        const uint Bits28 = 0xFFFFFFFu;
        const uint Bits25 = 0x1FFFFFFu;
        const uint Bits24 = 0xFFFFFFu;
        const uint Bits21 = 0x1FFFFFu;
        const uint Bits20 = 0xFFFFFu;
        const uint Bits17 = 0x1FFFFu;
        const uint Bits16 = 0xFFFFu;
        const uint Bits12 = 0xFFFu;
        const uint Bits8 = 0xFFu;
        const uint Bits4 = 0xFu;

        public static CtpTime ReadTimeChanged(BitStreamReader stream, CtpTime prev)
        {
            ulong bitsChanged;

            switch (stream.ReadBits3())
            {
                case 0:
                    bitsChanged = stream.ReadBits17();
                    break;
                case 1:
                    bitsChanged = stream.ReadBits21();
                    break;
                case 2:
                    bitsChanged = stream.ReadBits25();
                    break;
                case 3:
                    bitsChanged = stream.ReadBits32();
                    break;
                case 4:
                    bitsChanged = stream.ReadBits(37);
                    break;
                case 5:
                    bitsChanged = stream.ReadBits(44);
                    break;
                case 6:
                    bitsChanged = stream.ReadBits(56);
                    break;
                case 7:
                    bitsChanged = stream.ReadBits(64);
                    break;
                default:
                    throw new Exception(); //Impossible to get here.
            }

            return new CtpTime((long)CompareUInt64.UnCompare(bitsChanged, (ulong)prev.Ticks));
        }

        public static void WriteTimeChanged(BitStreamWriter stream, CtpTime prev, CtpTime current)
        {
            ulong bitsChanged = CompareUInt64.Compare((ulong)current.Ticks, (ulong)prev.Ticks);

            if (bitsChanged <= Bits17) //Changes by up to 6.5ms
            {
                stream.WriteBits3(0);
                stream.WriteBits17((uint)bitsChanged);
            }
            else if (bitsChanged <= Bits21) //Changes up to 104ms
            {
                stream.WriteBits3(1);
                stream.WriteBits21((uint)bitsChanged);
            }
            else if (bitsChanged <= Bits25) //Changes up to 1.6 seconds
            {
                stream.WriteBits3(2);
                stream.WriteBits25((uint)bitsChanged);
            }
            else if (bitsChanged <= Bits32) //changes up to 3.5 Minutes
            {
                stream.WriteBits3(3);
                stream.WriteBits32((uint)bitsChanged);
            }
            else if (bitsChanged <= Bits37) //changes up to 1.9 hours
            {
                stream.WriteBits3(4);
                stream.WriteBits(37, bitsChanged);
            }
            else if (bitsChanged <= Bits44) //changes up to 10 days
            {
                stream.WriteBits3(5);
                stream.WriteBits(44, bitsChanged);
            }
            else if (bitsChanged <= Bits56) //Changes up to 114 Years
            {
                stream.WriteBits3(6);
                stream.WriteBits(56, bitsChanged);
            }
            else
            {
                stream.WriteBits3(7);
                stream.WriteBits(64, bitsChanged);
            }
        }

        public static void WritePointID(BitStreamWriter stream, int value)
        {
            if (value <= Bits8) // up to 256 points
            {
                stream.WriteBits2(0);
                stream.WriteBits8((uint)value);
            }
            else if (value <= Bits12) // up to 4096 points
            {
                stream.WriteBits2(1);
                stream.WriteBits12((uint)value);
            }
            else if (value <= Bits16)  // up to 65536 points
            {
                stream.WriteBits2(2);
                stream.WriteBits16((uint)value);
            }
            else if (value <= Bits20) // Up to 1 million points
            {
                stream.WriteBits2(3);
                stream.WriteBits1(0);
                stream.WriteBits20((uint)value);
            }
            else //up to 4 billion points
            {
                stream.WriteBits2(3);
                stream.WriteBits1(1);
                stream.WriteBits32((uint)value);
            }
        }

        public static int ReadPointID(BitStreamReader stream)
        {
            switch (stream.ReadBits2())
            {
                case 0:
                    return (int)stream.ReadBits8();
                case 1:
                    return (int)stream.ReadBits12();
                case 2:
                    return (int)stream.ReadBits16();
                case 3:
                    {
                        if (stream.ReadBits1() == 0)
                            return (int)stream.ReadBits20();
                        return (int)stream.ReadBits32();
                    }
                default:
                    throw new Exception(); //impossible to get here.
            }
        }

        public static long ReadInt64(BitStreamReader stream)
        {
            ulong negate = stream.ReadBits1() == 0 ? 0 : ulong.MaxValue;

            switch (stream.ReadBits3())
            {
                case 0:
                    return (long)(stream.ReadBits8() ^ negate);
                case 1:
                    return (long)(stream.ReadBits16() ^ negate);
                case 2:
                    return (long)(stream.ReadBits24() ^ negate);
                case 3:
                    return (long)(stream.ReadBits32() ^ negate);
                case 4:
                    return (long)(stream.ReadBits(40) ^ negate);
                case 5:
                    return (long)(stream.ReadBits(48) ^ negate);
                case 6:
                    return (long)(stream.ReadBits(56) ^ negate);
                case 7:
                    return (long)(stream.ReadBits(64) ^ negate);
                default:
                    throw new Exception(); //Impossible to get here.
            }
        }

        public static void WriteInt64(BitStreamWriter stream, long value)
        {
            if (value < 0)
            {
                stream.WriteBits1(1);
                value = ~value;
            }
            else
            {
                stream.WriteBits1(0);
            }

            if (value <= Bits8)
            {
                stream.WriteBits3(0);
                stream.WriteBits8((uint)(int)value);
            }
            else if (value <= Bits16)
            {
                stream.WriteBits3(1);
                stream.WriteBits16((uint)(int)value);
            }
            else if (value <= Bits24)
            {
                stream.WriteBits3(2);
                stream.WriteBits24((uint)(int)value);
            }
            else if (value <= Bits32)
            {
                stream.WriteBits3(3);
                stream.WriteBits32((uint)(int)value);
            }
            else if (value <= (long)Bits40)
            {
                stream.WriteBits3(4);
                stream.WriteBits(40, (ulong)value);
            }
            else if (value <= (long)Bits48)
            {
                stream.WriteBits3(5);
                stream.WriteBits(48, (ulong)value);
            }
            else if (value <= (long)Bits56)
            {
                stream.WriteBits3(6);
                stream.WriteBits(56, (ulong)value);
            }
            else
            {
                stream.WriteBits3(7);
                stream.WriteBits(64, (ulong)value);
            }
        }

        public static float ReadSingle(BitStreamReader stream)
        {
            uint value = stream.ReadBits32();
            return *(float*)&value;
        }

        public static void WriteSingle(BitStreamWriter stream, float value)
        {
            uint v = *(uint*)&value;
            stream.WriteBits32(v);
        }

        public static void WriteDouble(BitStreamWriter stream, double value)
        {
            ulong v = *(ulong*)&value;
            stream.WriteBits(64, v);
        }

        public static double ReadDouble(BitStreamReader stream)
        {
            ulong value = stream.ReadBits(64);
            return *(double*)&value;
        }

        public static void WriteBoolean(BitStreamWriter stream, bool value)
        {
            stream.WriteBits1(value);
        }

        public static bool ReadBoolean(BitStreamReader stream)
        {
            return stream.ReadBits1() == 1;
        }


    }
}

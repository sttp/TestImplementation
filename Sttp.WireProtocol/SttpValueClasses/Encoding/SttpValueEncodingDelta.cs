using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp
{
    public static unsafe class SttpValueEncodingDelta
    {
        public static void Save(ByteWriter wr, SttpValue value, SttpValue reference)
        {
            if (value == null)
                value = SttpValue.Null;
            if (reference == null)
                reference = SttpValue.Null;

            var type = value.ValueTypeCode;
            if (type != reference.ValueTypeCode)
            {
                wr.WriteBits1(1);
                SttpValueEncodingNative.Save(wr, value);
                return;
            }
            wr.WriteBits1(0);

            switch (type)
            {
                case SttpValueTypeCode.Null:
                    break;
                case SttpValueTypeCode.Int64:
                    wr.Write8BitSegments((ulong)PackSign(value.AsInt64 ^ reference.AsInt64));
                    break;
                case SttpValueTypeCode.Single:
                    {
                        float value1 = value.AsSingle;
                        float value2 = reference.AsSingle;
                        wr.Write(*(uint*)&value1 ^ *(uint*)&value2); //Have some other kind of delta write method
                        break;
                    }
                case SttpValueTypeCode.Double:
                    {
                        double value1 = value.AsDouble;
                        double value2 = reference.AsDouble;
                        wr.Write(*(ulong*)&value1 ^ *(ulong*)&value2); //Have some other kind of delta write method
                        break;
                    }
                case SttpValueTypeCode.Decimal:
                    wr.Write(value.AsDecimal);
                    break;
                case SttpValueTypeCode.SttpTime:
                    wr.Write(value.AsSttpTime);
                    break;
                case SttpValueTypeCode.Boolean:
                    wr.WriteBits1(value.AsBoolean);
                    break;
                case SttpValueTypeCode.Guid:
                    wr.Write(value.AsGuid);
                    break;
                case SttpValueTypeCode.String:
                    wr.Write(value.AsString);
                    break;
                case SttpValueTypeCode.SttpBuffer:
                    wr.Write(value.AsSttpBuffer);
                    break;
                case SttpValueTypeCode.SttpMarkup:
                    wr.Write(value.AsSttpMarkup);
                    break;
                case SttpValueTypeCode.SttpBulkTransport:
                    wr.Write(value.AsSttpBulkTransport);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static SttpValue Load(ByteReader rd, SttpValue reference)
        {
            if (rd.ReadBits1() == 1)
            {
                return SttpValueEncodingNative.Load(rd);
            }

            switch (reference.ValueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    return SttpValue.Null;
                case SttpValueTypeCode.Int64:
                    return (SttpValue)((UnPackSign((long)rd.Read8BitSegments()) ^ reference.AsInt64));
                case SttpValueTypeCode.Single:
                    {
                        float value2 = reference.AsSingle;
                        uint result = rd.ReadUInt32() ^ *(uint*)&value2;
                        return (SttpValue)(*(float*)&result);
                    }
                case SttpValueTypeCode.Double:
                    {
                        double value2 = reference.AsDouble;
                        ulong result = rd.ReadUInt64() ^ *(uint*)&value2;
                        return (SttpValue)(*(double*)&result);
                    }
                case SttpValueTypeCode.Decimal:
                    return (SttpValue)rd.ReadDecimal();
                case SttpValueTypeCode.SttpTime:
                    return (SttpValue)rd.ReadSttpTime();
                case SttpValueTypeCode.Boolean:
                    return (SttpValue)(rd.ReadBits1() == 1);
                case SttpValueTypeCode.Guid:
                    return (SttpValue)rd.ReadGuid();
                case SttpValueTypeCode.String:
                    return (SttpValue)rd.ReadString();
                case SttpValueTypeCode.SttpBuffer:
                    return (SttpValue)rd.ReadSttpBuffer();
                case SttpValueTypeCode.SttpMarkup:
                    return (SttpValue)rd.ReadSttpMarkup();
                case SttpValueTypeCode.SttpBulkTransport:
                    return (SttpValue)rd.ReadSttpBulkTransport();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void Load(ByteReader rd, SttpValue reference, SttpValueMutable output)
        {
            if (rd.ReadBits1() == 1)
            {
                SttpValueEncodingNative.Load(rd, output);
                return;
            }

            switch (reference.ValueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    output.SetNull();
                    break;
                case SttpValueTypeCode.Int64:
                    output.SetValue((UnPackSign((long)rd.Read8BitSegments()) ^ reference.AsInt64));
                    break;
                case SttpValueTypeCode.Single:
                    {
                        float value2 = reference.AsSingle;
                        uint result = rd.ReadUInt32() ^ *(uint*)&value2;
                        output.SetValue((*(float*)&result));
                        break;
                    }
                case SttpValueTypeCode.Double:
                    {
                        double value2 = reference.AsDouble;
                        ulong result = rd.ReadUInt64() ^ *(uint*)&value2;
                        output.SetValue((*(double*)&result));
                        break;
                    }
                case SttpValueTypeCode.Decimal:
                    output.SetValue(rd.ReadDecimal());
                    break;
                case SttpValueTypeCode.SttpTime:
                    output.SetValue(rd.ReadSttpTime());
                    break;
                case SttpValueTypeCode.Boolean:
                    output.SetValue((rd.ReadBits1() == 1));
                    break;
                case SttpValueTypeCode.Guid:
                    output.SetValue(rd.ReadGuid());
                    break;
                case SttpValueTypeCode.String:
                    output.SetValue(rd.ReadString());
                    break;
                case SttpValueTypeCode.SttpBuffer:
                    output.SetValue(rd.ReadSttpBuffer());
                    break;
                case SttpValueTypeCode.SttpMarkup:
                    output.SetValue(rd.ReadSttpMarkup());
                    break;
                case SttpValueTypeCode.SttpBulkTransport:
                    output.SetValue(rd.ReadSttpBulkTransport());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }




        private static long PackSign(long value)
        {
            //since negative signed values have leading 1's and positive have leading 0's, 
            //it's important to change it into a common format.
            //Basically, we rotate left to move the leading sign bit to bit0, and if bit 0 is set, we invert bits 1-63.
            if (value >= 0)
                return value << 1;
            return (~value << 1) + 1;
        }
        private static long UnPackSign(long value)
        {
            if ((value & 1) == 0) //If it was positive
                return (value >> 1) & long.MaxValue;  //Clear the upper bit since rightshift might assign a leading bit.
            return (~value >> 1) | long.MinValue; //Set the upper bit.
        }
    }
}

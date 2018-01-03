using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp
{
    public static class SttpValueEncodingNative
    {
        public static void Save(ByteWriter wr, SttpValue value)
        {
            if (value == null)
                value = SttpValue.Null;

            var typeCode = value.ValueTypeCode;
            wr.WriteBits4((byte)typeCode);
            switch (typeCode)
            {
                case SttpValueTypeCode.Null:
                    break;
                case SttpValueTypeCode.Int64:
                    wr.Write8BitSegments((ulong)PackSign(value.AsInt64));
                    break;
                case SttpValueTypeCode.Single:
                    wr.Write(value.AsSingle);
                    break;
                case SttpValueTypeCode.Double:
                    wr.Write(value.AsDouble);
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

        public static SttpValue Load(ByteReader rd)
        {
            SttpValueTypeCode value = (SttpValueTypeCode)rd.ReadBits4();
            switch (value)
            {
                case SttpValueTypeCode.Null:
                    return SttpValue.Null;
                case SttpValueTypeCode.Int64:
                    return (SttpValue)UnPackSign((long)rd.Read8BitSegments());
                case SttpValueTypeCode.Single:
                    return (SttpValue)rd.ReadSingle();
                case SttpValueTypeCode.Double:
                    return (SttpValue)rd.ReadDouble();
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

        public static void Load(ByteReader rd, SttpValueMutable output)
        {
            SttpValueTypeCode value = (SttpValueTypeCode)rd.ReadBits4();
            switch (value)
            {
                case SttpValueTypeCode.Null:
                    output.SetNull();
                    break;
                case SttpValueTypeCode.Int64:
                    output.SetValue(UnPackSign((long)rd.Read8BitSegments()));
                    break;
                case SttpValueTypeCode.Single:
                    output.SetValue(rd.ReadSingle());
                    break;
                case SttpValueTypeCode.Double:
                    output.SetValue(rd.ReadDouble());
                    break;
                case SttpValueTypeCode.SttpTime:
                    output.SetValue(rd.ReadSttpTime());
                    break;
                case SttpValueTypeCode.Boolean:
                    output.SetValue(rd.ReadBits1() == 1);
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

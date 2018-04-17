using System;

namespace CTP
{
    internal static class CtpValueEncodingNative
    {
        internal static void Save(DocumentBitWriter wr, CtpObject value)
        {
            if (value == null)
                value = CtpObject.Null;

            var typeCode = value.ValueTypeCode;
            wr.WriteBits4((byte)typeCode);
            switch (typeCode)
            {
                case CtpTypeCode.Null:
                    break;
                case CtpTypeCode.Int64:
                    wr.Write8BitSegments((ulong)PackSign(value.AsInt64));
                    break;
                case CtpTypeCode.Single:
                    wr.Write(value.AsSingle);
                    break;
                case CtpTypeCode.Double:
                    wr.Write(value.AsDouble);
                    break;
                case CtpTypeCode.CtpTime:
                    wr.Write(value.AsCtpTime);
                    break;
                case CtpTypeCode.Boolean:
                    wr.WriteBits1(value.AsBoolean);
                    break;
                case CtpTypeCode.Guid:
                    wr.Write(value.AsGuid);
                    break;
                case CtpTypeCode.String:
                    wr.Write(value.AsString);
                    break;
                case CtpTypeCode.CtpBuffer:
                    wr.Write(value.AsCtpBuffer);
                    break;
                case CtpTypeCode.CtpDocument:
                    wr.Write(value.AsDocument);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal static void Load(DocumentBitReader rd, CtpObject output)
        {
            CtpTypeCode value = (CtpTypeCode)rd.ReadBits4();
            switch (value)
            {
                case CtpTypeCode.Null:
                    output.SetNull();
                    break;
                case CtpTypeCode.Int64:
                    output.SetValue(UnPackSign((long)rd.Read8BitSegments()));
                    break;
                case CtpTypeCode.Single:
                    output.SetValue(rd.ReadSingle());
                    break;
                case CtpTypeCode.Double:
                    output.SetValue(rd.ReadDouble());
                    break;
                case CtpTypeCode.CtpTime:
                    output.SetValue(rd.ReadSttpTime());
                    break;
                case CtpTypeCode.Boolean:
                    output.SetValue(rd.ReadBits1() == 1);
                    break;
                case CtpTypeCode.Guid:
                    output.SetValue(rd.ReadGuid());
                    break;
                case CtpTypeCode.String:
                    output.SetValue(rd.ReadString());
                    break;
                case CtpTypeCode.CtpBuffer:
                    output.SetValue(rd.ReadSttpBuffer());
                    break;
                case CtpTypeCode.CtpDocument:
                    output.SetValue(rd.ReadSttpMarkup());
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

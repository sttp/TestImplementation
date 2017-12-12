using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Sttp.Codec;
using Sttp.SttpValueClasses;

namespace Sttp
{
    /// <summary>
    /// This class contains the fundamental value for STTP.
    /// </summary>
    public abstract partial class SttpValue : IEquatable<SttpValue>
    {
        public unsafe void SaveDelta(BitStreamWriter wrBits, ByteWriter wr, SttpValue other)
        {
            var value = ValueTypeCode;
            if (other.ValueTypeCode == value)
            {
                wrBits.WriteBits1(0);
                switch (value)
                {
                    case SttpValueTypeCode.Null:
                        break;
                    case SttpValueTypeCode.Int64:
                        //wrBits.Write8BitSegments((ulong)AsInt64);
                        wrBits.Write8BitSegments((ulong)AsInt64 ^ (ulong)other.AsInt64);
                        break;
                    case SttpValueTypeCode.Single:
                        {
                            float value1 = AsSingle;
                            float value2 = AsSingle;
                            wr.Write(*(uint*)&value1 ^ *(uint*)&value2);
                            break;
                        }
                    case SttpValueTypeCode.Double:
                        {
                            double value1 = AsDouble;
                            double value2 = AsDouble;
                            wr.Write(*(ulong*)&value1 ^ *(ulong*)&value2);
                            break;
                        }
                    case SttpValueTypeCode.Decimal:
                        wr.Write(AsDecimal);
                        break;
                    case SttpValueTypeCode.SttpTime:
                        wr.Write(AsSttpTime);
                        break;
                    case SttpValueTypeCode.Boolean:
                        if (AsBoolean)
                        {
                            wrBits.WriteBits1(1);
                        }
                        else
                        {
                            wrBits.WriteBits1(0);
                        }
                        break;
                    case SttpValueTypeCode.Guid:
                        wr.Write(AsGuid);
                        break;
                    case SttpValueTypeCode.String:
                        wr.Write(AsString);
                        break;
                    case SttpValueTypeCode.SttpBuffer:
                        wr.Write(AsSttpBuffer);
                        break;
                    case SttpValueTypeCode.SttpMarkup:
                        wr.Write(AsSttpMarkup);
                        break;
                    case SttpValueTypeCode.SttpBulkTransport:
                        wr.Write(AsSttpBulkTransport);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                wrBits.WriteBits1(1);
                wrBits.WriteBits4((uint)value);
                switch (value)
                {
                    case SttpValueTypeCode.Null:
                        break;
                    case SttpValueTypeCode.Int64:
                        wr.Write(AsInt64);
                        break;
                    case SttpValueTypeCode.Single:
                        wr.Write(AsSingle);
                        break;
                    case SttpValueTypeCode.Double:
                        wr.Write(AsDouble);
                        break;
                    case SttpValueTypeCode.Decimal:
                        wr.Write(AsDecimal);
                        break;
                    case SttpValueTypeCode.SttpTime:
                        wr.Write(AsSttpTime);
                        break;
                    case SttpValueTypeCode.Boolean:
                        wr.Write(AsBoolean);
                        break;
                    case SttpValueTypeCode.Guid:
                        wr.Write(AsGuid);
                        break;
                    case SttpValueTypeCode.String:
                        wr.Write(AsString);
                        break;
                    case SttpValueTypeCode.SttpBuffer:
                        wr.Write(AsSttpBuffer);
                        break;
                    case SttpValueTypeCode.SttpMarkup:
                        wr.Write(AsSttpMarkup);
                        break;
                    case SttpValueTypeCode.SttpBulkTransport:
                        wr.Write(AsSttpBulkTransport);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        


        public void Save(ByteWriter wr)
        {
            var value = ValueTypeCode;
            wr.Write((byte)value);
            switch (value)
            {
                case SttpValueTypeCode.Null:
                    break;
                case SttpValueTypeCode.Int64:
                    wr.Write(AsInt64);
                    break;
                case SttpValueTypeCode.Single:
                    wr.Write(AsSingle);
                    break;
                case SttpValueTypeCode.Double:
                    wr.Write(AsDouble);
                    break;
                case SttpValueTypeCode.Decimal:
                    wr.Write(AsDecimal);
                    break;
                case SttpValueTypeCode.SttpTime:
                    wr.Write(AsSttpTime);
                    break;
                case SttpValueTypeCode.Boolean:
                    wr.Write(AsBoolean);
                    break;
                case SttpValueTypeCode.Guid:
                    wr.Write(AsGuid);
                    break;
                case SttpValueTypeCode.String:
                    wr.Write(AsString);
                    break;
                case SttpValueTypeCode.SttpBuffer:
                    wr.Write(AsSttpBuffer);
                    break;
                case SttpValueTypeCode.SttpMarkup:
                    wr.Write(AsSttpMarkup);
                    break;
                case SttpValueTypeCode.SttpBulkTransport:
                    wr.Write(AsSttpBulkTransport);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static SttpValue Load(ByteReader rd)
        {
            SttpValueTypeCode value = (SttpValueTypeCode)rd.ReadByte();
            switch (value)
            {
                case SttpValueTypeCode.Null:
                    return SttpValue.Null;
                case SttpValueTypeCode.Int64:
                    return new SttpValueInt64(rd.ReadInt64());
                case SttpValueTypeCode.Single:
                    return new SttpValueSingle(rd.ReadSingle());
                case SttpValueTypeCode.Double:
                    return new SttpValueDouble(rd.ReadDouble());
                case SttpValueTypeCode.Decimal:
                    return new SttpValueDecimal(rd.ReadDecimal());
                case SttpValueTypeCode.SttpTime:
                    return new SttpValueSttpTime(rd.ReadSttpTime());
                case SttpValueTypeCode.Boolean:
                    return new SttpValueBoolean(rd.ReadBoolean());
                case SttpValueTypeCode.Guid:
                    return new SttpValueGuid(rd.ReadGuid());
                case SttpValueTypeCode.String:
                    return new SttpValueString(rd.ReadString());
                case SttpValueTypeCode.SttpBuffer:
                    return new SttpValueSttpBuffer(rd.ReadSttpBuffer());
                case SttpValueTypeCode.SttpMarkup:
                    return new SttpValueSttpMarkup(rd.ReadSttpMarkup());
                case SttpValueTypeCode.SttpBulkTransport:
                    return new SttpValueSttpBulkTransport(rd.ReadSttpBulkTransport());
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }



    }
}

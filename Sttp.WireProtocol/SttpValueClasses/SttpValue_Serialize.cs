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
        public void Save(ByteWriter wr)
        {
            var value = ValueTypeCode;
            wr.Write((byte)value);
            switch (value)
            {
                case SttpValueTypeCode.Null:
                    break;
                case SttpValueTypeCode.SByte:
                    wr.Write(AsSByte);
                    break;
                case SttpValueTypeCode.Int16:
                    wr.Write(AsInt16);
                    break;
                case SttpValueTypeCode.Int32:
                    wr.Write(AsInt32);
                    break;
                case SttpValueTypeCode.Int64:
                    wr.Write(AsInt64);
                    break;
                case SttpValueTypeCode.Byte:
                    wr.Write(AsByte);
                    break;
                case SttpValueTypeCode.UInt16:
                    wr.Write(AsUInt16);
                    break;
                case SttpValueTypeCode.UInt32:
                    wr.Write(AsUInt32);
                    break;
                case SttpValueTypeCode.UInt64:
                    wr.Write(AsUInt64);
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
                case SttpValueTypeCode.DateTime:
                    wr.Write(AsDateTime);
                    break;
                case SttpValueTypeCode.DateTimeOffset:
                    wr.Write(AsDateTimeOffset);
                    break;
                case SttpValueTypeCode.SttpTime:
                    wr.Write(AsSttpTime);
                    break;
                case SttpValueTypeCode.SttpTimeOffset:
                    wr.Write(AsSttpTimeOffset);
                    break;
                case SttpValueTypeCode.TimeSpan:
                    wr.Write(AsTimeSpan);
                    break;
                case SttpValueTypeCode.Boolean:
                    wr.Write(AsBoolean);
                    break;
                case SttpValueTypeCode.Char:
                    wr.Write(AsChar);
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
                case SttpValueTypeCode.SttpValueSet:
                    wr.Write(AsSttpValueSet);
                    break;
                case SttpValueTypeCode.SttpNamedSet:
                    wr.Write(AsSttpNamedSet);
                    break;
                case SttpValueTypeCode.SttpMarkup:
                    wr.Write(AsSttpMarkup);
                    break;
                case SttpValueTypeCode.BulkTransportGuid:
                    wr.Write(AsBulkTransportGuid);
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
                case SttpValueTypeCode.SByte:
                    return new SttpValueSByte(rd.ReadSByte());
                case SttpValueTypeCode.Int16:
                    return new SttpValueInt16(rd.ReadInt16());
                case SttpValueTypeCode.Int32:
                    return new SttpValueInt32(rd.ReadInt32());
                case SttpValueTypeCode.Int64:
                    return new SttpValueInt64(rd.ReadInt64());
                case SttpValueTypeCode.Byte:
                    return new SttpValueByte(rd.ReadByte());
                case SttpValueTypeCode.UInt16:
                    return new SttpValueUInt16(rd.ReadUInt16());
                case SttpValueTypeCode.UInt32:
                    return new SttpValueUInt32(rd.ReadUInt32());
                case SttpValueTypeCode.UInt64:
                    return new SttpValueUInt64(rd.ReadUInt64());
                case SttpValueTypeCode.Single:
                    return new SttpValueSingle(rd.ReadSingle());
                case SttpValueTypeCode.Double:
                    return new SttpValueDouble(rd.ReadDouble());
                case SttpValueTypeCode.Decimal:
                    return new SttpValueDecimal(rd.ReadDecimal());
                case SttpValueTypeCode.DateTime:
                    return new SttpValueDateTime(rd.ReadDateTime());
                case SttpValueTypeCode.DateTimeOffset:
                    return new SttpValueDateTimeOffset(rd.ReadDateTimeOffset());
                case SttpValueTypeCode.SttpTime:
                    return new SttpValueSttpTime(rd.ReadSttpTime());
                case SttpValueTypeCode.SttpTimeOffset:
                    return new SttpValueSttpTimeOffset(rd.ReadSttpTimeOffset());
                case SttpValueTypeCode.TimeSpan:
                    return new SttpValueTimeSpan(rd.ReadTimeSpan());
                case SttpValueTypeCode.Boolean:
                    return new SttpValueBoolean(rd.ReadBoolean());
                case SttpValueTypeCode.Char:
                    return new SttpValueChar(rd.ReadChar());
                case SttpValueTypeCode.Guid:
                    return new SttpValueGuid(rd.ReadGuid());
                case SttpValueTypeCode.String:
                    return new SttpValueString(rd.ReadString());
                case SttpValueTypeCode.SttpBuffer:
                    return new SttpValueSttpBuffer(rd.ReadSttpBuffer());
                case SttpValueTypeCode.SttpValueSet:
                    return new SttpValueSttpValueSet(rd.ReadSttpValueSet());
                case SttpValueTypeCode.SttpNamedSet:
                    return new SttpValueSttpNamedSet(rd.ReadSttpNamedSet());
                case SttpValueTypeCode.SttpMarkup:
                    return new SttpValueSttpMarkup(rd.ReadSttpMarkup());
                case SttpValueTypeCode.BulkTransportGuid:
                    return new SttpValueBulkTransportGuid(rd.ReadGuid());
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }



    }
}

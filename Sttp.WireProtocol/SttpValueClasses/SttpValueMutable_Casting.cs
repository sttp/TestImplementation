using System;
using Sttp.SttpValueClasses;

namespace Sttp
{
    /// <summary>
    /// This class contains the fundamental value for STTP.
    /// </summary>
    public partial class SttpValueMutable : SttpValue
    {
        public override string ToTypeString
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        return SttpValueNullMethods.ToTypeString();
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.ToTypeString(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.ToTypeString(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.ToTypeString(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.ToTypeString(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.ToTypeString(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.ToTypeString(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.ToTypeString(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.ToTypeString(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.ToTypeString(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.ToTypeString(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.ToTypeString(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.ToTypeString(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.ToTypeString(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.ToTypeString(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.ToTypeString(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.ToTypeString(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.ToTypeString(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.ToTypeString(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.ToTypeString(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.ToTypeString(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.ToTypeString(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.ToTypeString(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.ToTypeString(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.ToTypeString(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.ToTypeString(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        public override object ToNativeType
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        return SttpValueNullMethods.ToNativeType();
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.ToNativeType(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.ToNativeType(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.ToNativeType(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.ToNativeType(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.ToNativeType(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.ToNativeType(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.ToNativeType(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.ToNativeType(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.ToNativeType(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.ToNativeType(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.ToNativeType(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.ToNativeType(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.ToNativeType(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.ToNativeType(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.ToNativeType(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.ToNativeType(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.ToNativeType(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.ToNativeType(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.ToNativeType(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.ToNativeType(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.ToNativeType(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.ToNativeType(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.ToNativeType(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.ToNativeType(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.ToNativeType(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override sbyte AsSByte
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to SByte");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsSByte(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsSByte(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsSByte(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsSByte(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsSByte(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsSByte(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsSByte(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsSByte(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsSByte(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsSByte(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsSByte(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsSByte(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsSByte(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsSByte(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsSByte(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsSByte(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsSByte(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsSByte(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsSByte(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsSByte(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsSByte(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsSByte(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsSByte(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsSByte(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsSByte(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override short AsInt16
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to Int16");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsInt16(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsInt16(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsInt16(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsInt16(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsInt16(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsInt16(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsInt16(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsInt16(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsInt16(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsInt16(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsInt16(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsInt16(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsInt16(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsInt16(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsInt16(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsInt16(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsInt16(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsInt16(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsInt16(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsInt16(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsInt16(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsInt16(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsInt16(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsInt16(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsInt16(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public override int AsInt32
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to Int32");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsInt32(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsInt32(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsInt32(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsInt32(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsInt32(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsInt32(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsInt32(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsInt32(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsInt32(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsInt32(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsInt32(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsInt32(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsInt32(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsInt32(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsInt32(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsInt32(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsInt32(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsInt32(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsInt32(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsInt32(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsInt32(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsInt32(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsInt32(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsInt32(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsInt32(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public override long AsInt64
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to Int64");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsInt64(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsInt64(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsInt64(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsInt64(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsInt64(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsInt64(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsInt64(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsInt64(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsInt64(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsInt64(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsInt64(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsInt64(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsInt64(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsInt64(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsInt64(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsInt64(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsInt64(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsInt64(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsInt64(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsInt64(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsInt64(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsInt64(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsInt64(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsInt64(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsInt64(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public override byte AsByte
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to Byte");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsByte(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsByte(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsByte(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsByte(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsByte(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsByte(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsByte(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsByte(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsByte(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsByte(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsByte(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsByte(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsByte(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsByte(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsByte(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsByte(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsByte(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsByte(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsByte(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsByte(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsByte(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsByte(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsByte(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsByte(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsByte(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public override ushort AsUInt16
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to UInt16");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsUInt16(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsUInt16(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsUInt16(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsUInt16(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsUInt16(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsUInt16(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsUInt16(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsUInt16(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsUInt16(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsUInt16(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsUInt16(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsUInt16(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsUInt16(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsUInt16(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsUInt16(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsUInt16(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsUInt16(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsUInt16(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsUInt16(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsUInt16(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsUInt16(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsUInt16(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsUInt16(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsUInt16(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsUInt16(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public override uint AsUInt32
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to UInt32");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsUInt32(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsUInt32(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsUInt32(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsUInt32(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsUInt32(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsUInt32(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsUInt32(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsUInt32(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsUInt32(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsUInt32(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsUInt32(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsUInt32(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsUInt32(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsUInt32(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsUInt32(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsUInt32(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsUInt32(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsUInt32(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsUInt32(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsUInt32(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsUInt32(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsUInt32(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsUInt32(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsUInt32(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsUInt32(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public override ulong AsUInt64
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to UInt64");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsUInt64(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsUInt64(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsUInt64(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsUInt64(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsUInt64(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsUInt64(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsUInt64(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsUInt64(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsUInt64(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsUInt64(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsUInt64(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsUInt64(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsUInt64(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsUInt64(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsUInt64(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsUInt64(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsUInt64(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsUInt64(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsUInt64(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsUInt64(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsUInt64(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsUInt64(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsUInt64(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsUInt64(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsUInt64(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public override float AsSingle
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to Single");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsSingle(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsSingle(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsSingle(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsSingle(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsSingle(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsSingle(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsSingle(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsSingle(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsSingle(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsSingle(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsSingle(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsSingle(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsSingle(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsSingle(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsSingle(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsSingle(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsSingle(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsSingle(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsSingle(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsSingle(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsSingle(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsSingle(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsSingle(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsSingle(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsSingle(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public override double AsDouble
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to Double");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsDouble(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsDouble(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsDouble(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsDouble(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsDouble(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsDouble(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsDouble(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsDouble(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsDouble(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsDouble(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsDouble(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsDouble(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsDouble(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsDouble(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsDouble(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsDouble(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsDouble(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsDouble(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsDouble(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsDouble(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsDouble(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsDouble(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsDouble(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsDouble(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsDouble(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public override decimal AsDecimal
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to Decimal");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsDecimal(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsDecimal(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsDecimal(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsDecimal(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsDecimal(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsDecimal(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsDecimal(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsDecimal(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsDecimal(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsDecimal(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsDecimal(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsDecimal(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsDecimal(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsDecimal(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsDecimal(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsDecimal(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsDecimal(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsDecimal(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsDecimal(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsDecimal(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsDecimal(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsDecimal(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsDecimal(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsDecimal(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsDecimal(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public override DateTime AsDateTime
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to DateTime");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsDateTime(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsDateTime(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsDateTime(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsDateTime(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsDateTime(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsDateTime(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsDateTime(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsDateTime(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsDateTime(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsDateTime(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsDateTime(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsDateTime(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsDateTime(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsDateTime(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsDateTime(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsDateTime(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsDateTime(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsDateTime(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsDateTime(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsDateTime(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsDateTime(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsDateTime(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsDateTime(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsDateTime(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsDateTime(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public override DateTimeOffset AsDateTimeOffset
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to DateTimeOffset");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsDateTimeOffset(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsDateTimeOffset(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsDateTimeOffset(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsDateTimeOffset(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsDateTimeOffset(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsDateTimeOffset(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsDateTimeOffset(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsDateTimeOffset(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsDateTimeOffset(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsDateTimeOffset(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsDateTimeOffset(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsDateTimeOffset(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsDateTimeOffset(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsDateTimeOffset(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsDateTimeOffset(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsDateTimeOffset(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsDateTimeOffset(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsDateTimeOffset(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsDateTimeOffset(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsDateTimeOffset(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsDateTimeOffset(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsDateTimeOffset(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsDateTimeOffset(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsDateTimeOffset(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsDateTimeOffset(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public override SttpTime AsSttpTime
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to SttpTime");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsSttpTime(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsSttpTime(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsSttpTime(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsSttpTime(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsSttpTime(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsSttpTime(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsSttpTime(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsSttpTime(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsSttpTime(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsSttpTime(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsSttpTime(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsSttpTime(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsSttpTime(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsSttpTime(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsSttpTime(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsSttpTime(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsSttpTime(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsSttpTime(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsSttpTime(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsSttpTime(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsSttpTime(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsSttpTime(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsSttpTime(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsSttpTime(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsSttpTime(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public override SttpTimeOffset AsSttpTimeOffset
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to SttpTimeOffset");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsSttpTimeOffset(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsSttpTimeOffset(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsSttpTimeOffset(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsSttpTimeOffset(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsSttpTimeOffset(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsSttpTimeOffset(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsSttpTimeOffset(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsSttpTimeOffset(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsSttpTimeOffset(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsSttpTimeOffset(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsSttpTimeOffset(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsSttpTimeOffset(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsSttpTimeOffset(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsSttpTimeOffset(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsSttpTimeOffset(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsSttpTimeOffset(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsSttpTimeOffset(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsSttpTimeOffset(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsSttpTimeOffset(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsSttpTimeOffset(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsSttpTimeOffset(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsSttpTimeOffset(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsSttpTimeOffset(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsSttpTimeOffset(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsSttpTimeOffset(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public override TimeSpan AsTimeSpan
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to TimeSpan");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsTimeSpan(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsTimeSpan(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsTimeSpan(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsTimeSpan(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsTimeSpan(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsTimeSpan(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsTimeSpan(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsTimeSpan(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsTimeSpan(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsTimeSpan(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsTimeSpan(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsTimeSpan(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsTimeSpan(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsTimeSpan(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsTimeSpan(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsTimeSpan(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsTimeSpan(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsTimeSpan(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsTimeSpan(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsTimeSpan(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsTimeSpan(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsTimeSpan(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsTimeSpan(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsTimeSpan(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsTimeSpan(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public override bool AsBoolean
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to Boolean");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsBoolean(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsBoolean(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsBoolean(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsBoolean(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsBoolean(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsBoolean(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsBoolean(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsBoolean(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsBoolean(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsBoolean(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsBoolean(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsBoolean(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsBoolean(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsBoolean(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsBoolean(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsBoolean(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsBoolean(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsBoolean(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsBoolean(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsBoolean(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsBoolean(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsBoolean(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsBoolean(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsBoolean(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsBoolean(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override char AsChar
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to Char");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsChar(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsChar(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsChar(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsChar(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsChar(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsChar(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsChar(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsChar(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsChar(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsChar(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsChar(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsChar(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsChar(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsChar(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsChar(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsChar(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsChar(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsChar(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsChar(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsChar(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsChar(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsChar(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsChar(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsChar(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsChar(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public override Guid AsGuid
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to Guid");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsGuid(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsGuid(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsGuid(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsGuid(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsGuid(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsGuid(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsGuid(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsGuid(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsGuid(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsGuid(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsGuid(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsGuid(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsGuid(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsGuid(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsGuid(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsGuid(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsGuid(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsGuid(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsGuid(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsGuid(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsGuid(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsGuid(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsGuid(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsGuid(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsGuid(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public override string AsString
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        return null;
                        throw new InvalidOperationException("Cannot cast from Null to String");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsString(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsString(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsString(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsString(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsString(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsString(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsString(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsString(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsString(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsString(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsString(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsString(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsString(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsString(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsString(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsString(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsString(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsString(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsString(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsString(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsString(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsString(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsString(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsString(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsString(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public override SttpBuffer AsSttpBuffer
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to SttpBuffer");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsSttpBuffer(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsSttpBuffer(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsSttpBuffer(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsSttpBuffer(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsSttpBuffer(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsSttpBuffer(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsSttpBuffer(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsSttpBuffer(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsSttpBuffer(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsSttpBuffer(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsSttpBuffer(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsSttpBuffer(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsSttpBuffer(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsSttpBuffer(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsSttpBuffer(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsSttpBuffer(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsSttpBuffer(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsSttpBuffer(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsSttpBuffer(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsSttpBuffer(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsSttpBuffer(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsSttpBuffer(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsSttpBuffer(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsSttpBuffer(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsSttpBuffer(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public override SttpValueSet AsSttpValueSet
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to SttpValueSet");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsSttpValueSet(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsSttpValueSet(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsSttpValueSet(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsSttpValueSet(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsSttpValueSet(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsSttpValueSet(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsSttpValueSet(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsSttpValueSet(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsSttpValueSet(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsSttpValueSet(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsSttpValueSet(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsSttpValueSet(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsSttpValueSet(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsSttpValueSet(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsSttpValueSet(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsSttpValueSet(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsSttpValueSet(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsSttpValueSet(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsSttpValueSet(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsSttpValueSet(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsSttpValueSet(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsSttpValueSet(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsSttpValueSet(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsSttpValueSet(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsSttpValueSet(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public override SttpNamedSet AsSttpNamedSet
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to SttpNamedSet");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsSttpNamedSet(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsSttpNamedSet(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsSttpNamedSet(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsSttpNamedSet(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsSttpNamedSet(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsSttpNamedSet(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsSttpNamedSet(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsSttpNamedSet(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsSttpNamedSet(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsSttpNamedSet(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsSttpNamedSet(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsSttpNamedSet(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsSttpNamedSet(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsSttpNamedSet(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsSttpNamedSet(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsSttpNamedSet(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsSttpNamedSet(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsSttpNamedSet(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsSttpNamedSet(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsSttpNamedSet(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsSttpNamedSet(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsSttpNamedSet(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsSttpNamedSet(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsSttpNamedSet(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsSttpNamedSet(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override SttpMarkup AsSttpMarkup
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to SttpMarkup");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsSttpMarkup(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsSttpMarkup(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsSttpMarkup(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsSttpMarkup(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsSttpMarkup(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsSttpMarkup(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsSttpMarkup(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsSttpMarkup(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsSttpMarkup(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsSttpMarkup(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsSttpMarkup(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsSttpMarkup(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsSttpMarkup(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsSttpMarkup(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsSttpMarkup(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsSttpMarkup(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsSttpMarkup(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsSttpMarkup(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsSttpMarkup(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsSttpMarkup(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsSttpMarkup(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsSttpMarkup(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsSttpMarkup(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsSttpMarkup(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsSttpMarkup(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override Guid AsBulkTransportGuid
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to BulkTransportGuid");
                    case SttpValueTypeCode.SByte:
                        return SttpValueSByteMethods.AsBulkTransportGuid(m_valueSByte);
                    case SttpValueTypeCode.Int16:
                        return SttpValueInt16Methods.AsBulkTransportGuid(m_valueInt16);
                    case SttpValueTypeCode.Int32:
                        return SttpValueInt32Methods.AsBulkTransportGuid(m_valueInt32);
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsBulkTransportGuid(m_valueInt64);
                    case SttpValueTypeCode.Byte:
                        return SttpValueByteMethods.AsBulkTransportGuid(m_valueByte);
                    case SttpValueTypeCode.UInt16:
                        return SttpValueUInt16Methods.AsBulkTransportGuid(m_valueUInt16);
                    case SttpValueTypeCode.UInt32:
                        return SttpValueUInt32Methods.AsBulkTransportGuid(m_valueUInt32);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsBulkTransportGuid(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsBulkTransportGuid(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsBulkTransportGuid(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsBulkTransportGuid(m_valueDecimal);
                    case SttpValueTypeCode.DateTime:
                        return SttpValueDateTimeMethods.AsBulkTransportGuid(m_valueDateTime);
                    case SttpValueTypeCode.DateTimeOffset:
                        return SttpValueDateTimeOffsetMethods.AsBulkTransportGuid(m_valueDateTimeOffset);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsBulkTransportGuid(m_valueSttpTime);
                    case SttpValueTypeCode.SttpTimeOffset:
                        return SttpValueSttpTimeOffsetMethods.AsBulkTransportGuid(m_valueSttpTimeOffset);
                    case SttpValueTypeCode.TimeSpan:
                        return SttpValueTimeSpanMethods.AsBulkTransportGuid(m_valueTimeSpan);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsBulkTransportGuid(m_valueBoolean);
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsBulkTransportGuid(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsBulkTransportGuid(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsBulkTransportGuid(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsBulkTransportGuid(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpValueSet:
                        return SttpValueSttpValueSetMethods.AsBulkTransportGuid(m_valueObject as SttpValueSet);
                    case SttpValueTypeCode.SttpNamedSet:
                        return SttpValueSttpNamedSetMethods.AsBulkTransportGuid(m_valueObject as SttpNamedSet);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsBulkTransportGuid(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsBulkTransportGuid(m_valueGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}

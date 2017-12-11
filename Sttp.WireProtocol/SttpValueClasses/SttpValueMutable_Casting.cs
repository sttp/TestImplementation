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
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.ToTypeString(m_valueInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.ToTypeString(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.ToTypeString(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.ToTypeString(m_valueDecimal);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.ToTypeString(m_valueSttpTime);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.ToTypeString(m_valueBoolean);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.ToTypeString(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.ToTypeString(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.ToTypeString(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.ToTypeString(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.SttpBulkTransport:
                        return SttpValueSttpBulkTransportMethods.ToTypeString(m_valueObject as SttpBulkTransport);
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
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.ToNativeType(m_valueInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.ToNativeType(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.ToNativeType(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.ToNativeType(m_valueDecimal);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.ToNativeType(m_valueSttpTime);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.ToNativeType(m_valueBoolean);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.ToNativeType(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.ToNativeType(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.ToNativeType(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.ToNativeType(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.SttpBulkTransport:
                        return SttpValueSttpBulkTransportMethods.ToNativeType(m_valueObject as SttpBulkTransport);
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
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsInt64(m_valueInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsInt64(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsInt64(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsInt64(m_valueDecimal);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsInt64(m_valueSttpTime);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsInt64(m_valueBoolean);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsInt64(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsInt64(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsInt64(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsInt64(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.SttpBulkTransport:
                        return SttpValueSttpBulkTransportMethods.AsInt64(m_valueObject as SttpBulkTransport);
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
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsSingle(m_valueInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsSingle(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsSingle(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsSingle(m_valueDecimal);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsSingle(m_valueSttpTime);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsSingle(m_valueBoolean);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsSingle(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsSingle(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsSingle(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsSingle(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.SttpBulkTransport:
                        return SttpValueSttpBulkTransportMethods.AsSingle(m_valueObject as SttpBulkTransport);
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
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsDouble(m_valueInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsDouble(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsDouble(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsDouble(m_valueDecimal);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsDouble(m_valueSttpTime);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsDouble(m_valueBoolean);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsDouble(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsDouble(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsDouble(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsDouble(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.SttpBulkTransport:
                        return SttpValueSttpBulkTransportMethods.AsDouble(m_valueObject as SttpBulkTransport);
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
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsDecimal(m_valueInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsDecimal(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsDecimal(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsDecimal(m_valueDecimal);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsDecimal(m_valueSttpTime);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsDecimal(m_valueBoolean);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsDecimal(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsDecimal(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsDecimal(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsDecimal(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.SttpBulkTransport:
                        return SttpValueSttpBulkTransportMethods.AsDecimal(m_valueObject as SttpBulkTransport);
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
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsSttpTime(m_valueInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsSttpTime(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsSttpTime(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsSttpTime(m_valueDecimal);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsSttpTime(m_valueSttpTime);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsSttpTime(m_valueBoolean);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsSttpTime(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsSttpTime(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsSttpTime(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsSttpTime(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.SttpBulkTransport:
                        return SttpValueSttpBulkTransportMethods.AsSttpTime(m_valueObject as SttpBulkTransport);
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
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsBoolean(m_valueInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsBoolean(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsBoolean(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsBoolean(m_valueDecimal);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsBoolean(m_valueSttpTime);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsBoolean(m_valueBoolean);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsBoolean(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsBoolean(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsBoolean(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsBoolean(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.SttpBulkTransport:
                        return SttpValueSttpBulkTransportMethods.AsBoolean(m_valueObject as SttpBulkTransport);
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
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsGuid(m_valueInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsGuid(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsGuid(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsGuid(m_valueDecimal);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsGuid(m_valueSttpTime);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsGuid(m_valueBoolean);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsGuid(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsGuid(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsGuid(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsGuid(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.SttpBulkTransport:
                        return SttpValueSttpBulkTransportMethods.AsGuid(m_valueObject as SttpBulkTransport);
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
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsString(m_valueInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsString(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsString(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsString(m_valueDecimal);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsString(m_valueSttpTime);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsString(m_valueBoolean);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsString(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsString(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsString(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsString(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.SttpBulkTransport:
                        return SttpValueSttpBulkTransportMethods.AsString(m_valueObject as SttpBulkTransport);
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
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsSttpBuffer(m_valueInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsSttpBuffer(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsSttpBuffer(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsSttpBuffer(m_valueDecimal);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsSttpBuffer(m_valueSttpTime);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsSttpBuffer(m_valueBoolean);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsSttpBuffer(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsSttpBuffer(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsSttpBuffer(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsSttpBuffer(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.SttpBulkTransport:
                        return SttpValueSttpBulkTransportMethods.AsSttpBuffer(m_valueObject as SttpBulkTransport);
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
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsSttpMarkup(m_valueInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsSttpMarkup(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsSttpMarkup(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsSttpMarkup(m_valueDecimal);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsSttpMarkup(m_valueSttpTime);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsSttpMarkup(m_valueBoolean);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsSttpMarkup(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsSttpMarkup(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsSttpMarkup(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsSttpMarkup(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.SttpBulkTransport:
                        return SttpValueSttpBulkTransportMethods.AsSttpMarkup(m_valueObject as SttpBulkTransport);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override SttpBulkTransport AsSttpBulkTransport
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to BulkTransportGuid");
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsSttpBulkTransport(m_valueInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsSttpBulkTransport(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsSttpBulkTransport(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsSttpBulkTransport(m_valueDecimal);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsSttpBulkTransport(m_valueSttpTime);
                    case SttpValueTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsSttpBulkTransport(m_valueBoolean);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsSttpBulkTransport(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsSttpBulkTransport(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsSttpBulkTransport(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsSttpBulkTransport(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.SttpBulkTransport:
                        return SttpValueSttpBulkTransportMethods.AsSttpBulkTransport(m_valueObject as SttpBulkTransport);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}

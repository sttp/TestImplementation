using System;

namespace CTP
{
    /// <summary>
    /// This class contains the fundamental value for STTP.
    /// </summary>
    public partial class CtpValueMutable : CtpValue
    {
        public override string ToTypeString
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case CtpTypeCode.Null:
                        return SttpValueNullMethods.ToTypeString();
                    case CtpTypeCode.Int64:
                        return SttpValueInt64Methods.ToTypeString(m_valueInt64);
                    case CtpTypeCode.Single:
                        return SttpValueSingleMethods.ToTypeString(m_valueSingle);
                    case CtpTypeCode.Double:
                        return SttpValueDoubleMethods.ToTypeString(m_valueDouble);
                    case CtpTypeCode.CtpTime:
                        return SttpValueSttpTimeMethods.ToTypeString(m_valueCtpTime);
                    case CtpTypeCode.Boolean:
                        return SttpValueBooleanMethods.ToTypeString(m_valueBoolean);
                    case CtpTypeCode.Guid:
                        return SttpValueGuidMethods.ToTypeString(m_valueGuid);
                    case CtpTypeCode.String:
                        return SttpValueStringMethods.ToTypeString(m_valueObject as string);
                    case CtpTypeCode.CtpBuffer:
                        return SttpValueSttpBufferMethods.ToTypeString(m_valueObject as CtpBuffer);
                    case CtpTypeCode.CtpMarkup:
                        return SttpValueSttpMarkupMethods.ToTypeString(m_valueObject as CtpMarkup);
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
                    case CtpTypeCode.Null:
                        return SttpValueNullMethods.ToNativeType();
                    case CtpTypeCode.Int64:
                        return SttpValueInt64Methods.ToNativeType(m_valueInt64);
                    case CtpTypeCode.Single:
                        return SttpValueSingleMethods.ToNativeType(m_valueSingle);
                    case CtpTypeCode.Double:
                        return SttpValueDoubleMethods.ToNativeType(m_valueDouble);
                    case CtpTypeCode.CtpTime:
                        return SttpValueSttpTimeMethods.ToNativeType(m_valueCtpTime);
                    case CtpTypeCode.Boolean:
                        return SttpValueBooleanMethods.ToNativeType(m_valueBoolean);
                    case CtpTypeCode.Guid:
                        return SttpValueGuidMethods.ToNativeType(m_valueGuid);
                    case CtpTypeCode.String:
                        return SttpValueStringMethods.ToNativeType(m_valueObject as string);
                    case CtpTypeCode.CtpBuffer:
                        return SttpValueSttpBufferMethods.ToNativeType(m_valueObject as CtpBuffer);
                    case CtpTypeCode.CtpMarkup:
                        return SttpValueSttpMarkupMethods.ToNativeType(m_valueObject as CtpMarkup);
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
                    case CtpTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to Int64");
                    case CtpTypeCode.Int64:
                        return SttpValueInt64Methods.AsInt64(m_valueInt64);
                    case CtpTypeCode.Single:
                        return SttpValueSingleMethods.AsInt64(m_valueSingle);
                    case CtpTypeCode.Double:
                        return SttpValueDoubleMethods.AsInt64(m_valueDouble);
                    case CtpTypeCode.CtpTime:
                        return SttpValueSttpTimeMethods.AsInt64(m_valueCtpTime);
                    case CtpTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsInt64(m_valueBoolean);
                    case CtpTypeCode.Guid:
                        return SttpValueGuidMethods.AsInt64(m_valueGuid);
                    case CtpTypeCode.String:
                        return SttpValueStringMethods.AsInt64(m_valueObject as string);
                    case CtpTypeCode.CtpBuffer:
                        return SttpValueSttpBufferMethods.AsInt64(m_valueObject as CtpBuffer);
                    case CtpTypeCode.CtpMarkup:
                        return SttpValueSttpMarkupMethods.AsInt64(m_valueObject as CtpMarkup);
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
                    case CtpTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to Single");
                    case CtpTypeCode.Int64:
                        return SttpValueInt64Methods.AsSingle(m_valueInt64);
                    case CtpTypeCode.Single:
                        return SttpValueSingleMethods.AsSingle(m_valueSingle);
                    case CtpTypeCode.Double:
                        return SttpValueDoubleMethods.AsSingle(m_valueDouble);
                    case CtpTypeCode.CtpTime:
                        return SttpValueSttpTimeMethods.AsSingle(m_valueCtpTime);
                    case CtpTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsSingle(m_valueBoolean);
                    case CtpTypeCode.Guid:
                        return SttpValueGuidMethods.AsSingle(m_valueGuid);
                    case CtpTypeCode.String:
                        return SttpValueStringMethods.AsSingle(m_valueObject as string);
                    case CtpTypeCode.CtpBuffer:
                        return SttpValueSttpBufferMethods.AsSingle(m_valueObject as CtpBuffer);
                    case CtpTypeCode.CtpMarkup:
                        return SttpValueSttpMarkupMethods.AsSingle(m_valueObject as CtpMarkup);
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
                    case CtpTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to Double");
                    case CtpTypeCode.Int64:
                        return SttpValueInt64Methods.AsDouble(m_valueInt64);
                    case CtpTypeCode.Single:
                        return SttpValueSingleMethods.AsDouble(m_valueSingle);
                    case CtpTypeCode.Double:
                        return SttpValueDoubleMethods.AsDouble(m_valueDouble);
                    case CtpTypeCode.CtpTime:
                        return SttpValueSttpTimeMethods.AsDouble(m_valueCtpTime);
                    case CtpTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsDouble(m_valueBoolean);
                    case CtpTypeCode.Guid:
                        return SttpValueGuidMethods.AsDouble(m_valueGuid);
                    case CtpTypeCode.String:
                        return SttpValueStringMethods.AsDouble(m_valueObject as string);
                    case CtpTypeCode.CtpBuffer:
                        return SttpValueSttpBufferMethods.AsDouble(m_valueObject as CtpBuffer);
                    case CtpTypeCode.CtpMarkup:
                        return SttpValueSttpMarkupMethods.AsDouble(m_valueObject as CtpMarkup);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public override CtpTime AsCtpTime
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case CtpTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to SttpTime");
                    case CtpTypeCode.Int64:
                        return SttpValueInt64Methods.AsSttpTime(m_valueInt64);
                    case CtpTypeCode.Single:
                        return SttpValueSingleMethods.AsSttpTime(m_valueSingle);
                    case CtpTypeCode.Double:
                        return SttpValueDoubleMethods.AsSttpTime(m_valueDouble);
                    case CtpTypeCode.CtpTime:
                        return SttpValueSttpTimeMethods.AsSttpTime(m_valueCtpTime);
                    case CtpTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsSttpTime(m_valueBoolean);
                    case CtpTypeCode.Guid:
                        return SttpValueGuidMethods.AsSttpTime(m_valueGuid);
                    case CtpTypeCode.String:
                        return SttpValueStringMethods.AsSttpTime(m_valueObject as string);
                    case CtpTypeCode.CtpBuffer:
                        return SttpValueSttpBufferMethods.AsSttpTime(m_valueObject as CtpBuffer);
                    case CtpTypeCode.CtpMarkup:
                        return SttpValueSttpMarkupMethods.AsSttpTime(m_valueObject as CtpMarkup);
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
                    case CtpTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to Boolean");
                    case CtpTypeCode.Int64:
                        return SttpValueInt64Methods.AsBoolean(m_valueInt64);
                    case CtpTypeCode.Single:
                        return SttpValueSingleMethods.AsBoolean(m_valueSingle);
                    case CtpTypeCode.Double:
                        return SttpValueDoubleMethods.AsBoolean(m_valueDouble);
                    case CtpTypeCode.CtpTime:
                        return SttpValueSttpTimeMethods.AsBoolean(m_valueCtpTime);
                    case CtpTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsBoolean(m_valueBoolean);
                    case CtpTypeCode.Guid:
                        return SttpValueGuidMethods.AsBoolean(m_valueGuid);
                    case CtpTypeCode.String:
                        return SttpValueStringMethods.AsBoolean(m_valueObject as string);
                    case CtpTypeCode.CtpBuffer:
                        return SttpValueSttpBufferMethods.AsBoolean(m_valueObject as CtpBuffer);
                    case CtpTypeCode.CtpMarkup:
                        return SttpValueSttpMarkupMethods.AsBoolean(m_valueObject as CtpMarkup);
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
                    case CtpTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to Guid");
                    case CtpTypeCode.Int64:
                        return SttpValueInt64Methods.AsGuid(m_valueInt64);
                    case CtpTypeCode.Single:
                        return SttpValueSingleMethods.AsGuid(m_valueSingle);
                    case CtpTypeCode.Double:
                        return SttpValueDoubleMethods.AsGuid(m_valueDouble);
                    case CtpTypeCode.CtpTime:
                        return SttpValueSttpTimeMethods.AsGuid(m_valueCtpTime);
                    case CtpTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsGuid(m_valueBoolean);
                    case CtpTypeCode.Guid:
                        return SttpValueGuidMethods.AsGuid(m_valueGuid);
                    case CtpTypeCode.String:
                        return SttpValueStringMethods.AsGuid(m_valueObject as string);
                    case CtpTypeCode.CtpBuffer:
                        return SttpValueSttpBufferMethods.AsGuid(m_valueObject as CtpBuffer);
                    case CtpTypeCode.CtpMarkup:
                        return SttpValueSttpMarkupMethods.AsGuid(m_valueObject as CtpMarkup);
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
                    case CtpTypeCode.Null:
                        return null;
                        throw new InvalidOperationException("Cannot cast from Null to String");
                    case CtpTypeCode.Int64:
                        return SttpValueInt64Methods.AsString(m_valueInt64);
                    case CtpTypeCode.Single:
                        return SttpValueSingleMethods.AsString(m_valueSingle);
                    case CtpTypeCode.Double:
                        return SttpValueDoubleMethods.AsString(m_valueDouble);
                    case CtpTypeCode.CtpTime:
                        return SttpValueSttpTimeMethods.AsString(m_valueCtpTime);
                    case CtpTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsString(m_valueBoolean);
                    case CtpTypeCode.Guid:
                        return SttpValueGuidMethods.AsString(m_valueGuid);
                    case CtpTypeCode.String:
                        return SttpValueStringMethods.AsString(m_valueObject as string);
                    case CtpTypeCode.CtpBuffer:
                        return SttpValueSttpBufferMethods.AsString(m_valueObject as CtpBuffer);
                    case CtpTypeCode.CtpMarkup:
                        return SttpValueSttpMarkupMethods.AsString(m_valueObject as CtpMarkup);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public override CtpBuffer AsSttpBuffer
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case CtpTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to SttpBuffer");
                    case CtpTypeCode.Int64:
                        return SttpValueInt64Methods.AsSttpBuffer(m_valueInt64);
                    case CtpTypeCode.Single:
                        return SttpValueSingleMethods.AsSttpBuffer(m_valueSingle);
                    case CtpTypeCode.Double:
                        return SttpValueDoubleMethods.AsSttpBuffer(m_valueDouble);
                    case CtpTypeCode.CtpTime:
                        return SttpValueSttpTimeMethods.AsSttpBuffer(m_valueCtpTime);
                    case CtpTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsSttpBuffer(m_valueBoolean);
                    case CtpTypeCode.Guid:
                        return SttpValueGuidMethods.AsSttpBuffer(m_valueGuid);
                    case CtpTypeCode.String:
                        return SttpValueStringMethods.AsSttpBuffer(m_valueObject as string);
                    case CtpTypeCode.CtpBuffer:
                        return SttpValueSttpBufferMethods.AsSttpBuffer(m_valueObject as CtpBuffer);
                    case CtpTypeCode.CtpMarkup:
                        return SttpValueSttpMarkupMethods.AsSttpBuffer(m_valueObject as CtpMarkup);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public override CtpMarkup AsSttpMarkup
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case CtpTypeCode.Null:
                        throw new InvalidOperationException("Cannot cast from Null to SttpMarkup");
                    case CtpTypeCode.Int64:
                        return SttpValueInt64Methods.AsSttpMarkup(m_valueInt64);
                    case CtpTypeCode.Single:
                        return SttpValueSingleMethods.AsSttpMarkup(m_valueSingle);
                    case CtpTypeCode.Double:
                        return SttpValueDoubleMethods.AsSttpMarkup(m_valueDouble);
                    case CtpTypeCode.CtpTime:
                        return SttpValueSttpTimeMethods.AsSttpMarkup(m_valueCtpTime);
                    case CtpTypeCode.Boolean:
                        return SttpValueBooleanMethods.AsSttpMarkup(m_valueBoolean);
                    case CtpTypeCode.Guid:
                        return SttpValueGuidMethods.AsSttpMarkup(m_valueGuid);
                    case CtpTypeCode.String:
                        return SttpValueStringMethods.AsSttpMarkup(m_valueObject as string);
                    case CtpTypeCode.CtpBuffer:
                        return SttpValueSttpBufferMethods.AsSttpMarkup(m_valueObject as CtpBuffer);
                    case CtpTypeCode.CtpMarkup:
                        return SttpValueSttpMarkupMethods.AsSttpMarkup(m_valueObject as CtpMarkup);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

    }
}

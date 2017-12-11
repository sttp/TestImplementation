﻿using System;
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
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.ToTypeString(m_valueUInt64);
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
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.ToTypeString(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.ToTypeString(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.ToTypeString(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.ToTypeString(m_valueObject as SttpBuffer);
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
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.ToNativeType(m_valueInt64);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.ToNativeType(m_valueUInt64);
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
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.ToNativeType(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.ToNativeType(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.ToNativeType(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.ToNativeType(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.ToNativeType(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.ToNativeType(m_valueGuid);
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
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsInt64(m_valueUInt64);
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
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsInt64(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsInt64(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsInt64(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsInt64(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsInt64(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsInt64(m_valueGuid);
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
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsUInt64(m_valueInt64);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsUInt64(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsUInt64(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsUInt64(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsUInt64(m_valueDecimal);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsUInt64(m_valueSttpTime);
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
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsSingle(m_valueInt64);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsSingle(m_valueUInt64);
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
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsSingle(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsSingle(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsSingle(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsSingle(m_valueObject as SttpBuffer);
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
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsDouble(m_valueInt64);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsDouble(m_valueUInt64);
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
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsDouble(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsDouble(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsDouble(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsDouble(m_valueObject as SttpBuffer);
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
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsDecimal(m_valueInt64);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsDecimal(m_valueUInt64);
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
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsDecimal(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsDecimal(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsDecimal(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsDecimal(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsDecimal(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsDecimal(m_valueGuid);
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
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsSttpTime(m_valueUInt64);
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
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsSttpTime(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsSttpTime(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsSttpTime(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsSttpTime(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsSttpTime(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsSttpTime(m_valueGuid);
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
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsBoolean(m_valueUInt64);
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
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsBoolean(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsBoolean(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsBoolean(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsBoolean(m_valueObject as SttpBuffer);
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
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsChar(m_valueInt64);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsChar(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsChar(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsChar(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsChar(m_valueDecimal);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsChar(m_valueSttpTime);
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
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsGuid(m_valueInt64);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsGuid(m_valueUInt64);
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
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsGuid(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsGuid(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsGuid(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsGuid(m_valueObject as SttpBuffer);
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
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsString(m_valueInt64);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsString(m_valueUInt64);
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
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsString(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsString(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsString(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsString(m_valueObject as SttpBuffer);
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
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsSttpBuffer(m_valueInt64);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsSttpBuffer(m_valueUInt64);
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
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsSttpBuffer(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsSttpBuffer(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsSttpBuffer(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsSttpBuffer(m_valueObject as SttpBuffer);
                    case SttpValueTypeCode.SttpMarkup:
                        return SttpValueSttpMarkupMethods.AsSttpBuffer(m_valueObject as SttpMarkup);
                    case SttpValueTypeCode.BulkTransportGuid:
                        return SttpValueBulkTransportGuidMethods.AsSttpBuffer(m_valueGuid);
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
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsSttpMarkup(m_valueUInt64);
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
                    case SttpValueTypeCode.Char:
                        return SttpValueCharMethods.AsSttpMarkup(m_valueChar);
                    case SttpValueTypeCode.Guid:
                        return SttpValueGuidMethods.AsSttpMarkup(m_valueGuid);
                    case SttpValueTypeCode.String:
                        return SttpValueStringMethods.AsSttpMarkup(m_valueObject as string);
                    case SttpValueTypeCode.SttpBuffer:
                        return SttpValueSttpBufferMethods.AsSttpMarkup(m_valueObject as SttpBuffer);
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
                    case SttpValueTypeCode.Int64:
                        return SttpValueInt64Methods.AsBulkTransportGuid(m_valueInt64);
                    case SttpValueTypeCode.UInt64:
                        return SttpValueUInt64Methods.AsBulkTransportGuid(m_valueUInt64);
                    case SttpValueTypeCode.Single:
                        return SttpValueSingleMethods.AsBulkTransportGuid(m_valueSingle);
                    case SttpValueTypeCode.Double:
                        return SttpValueDoubleMethods.AsBulkTransportGuid(m_valueDouble);
                    case SttpValueTypeCode.Decimal:
                        return SttpValueDecimalMethods.AsBulkTransportGuid(m_valueDecimal);
                    case SttpValueTypeCode.SttpTime:
                        return SttpValueSttpTimeMethods.AsBulkTransportGuid(m_valueSttpTime);
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
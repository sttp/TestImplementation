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
    public partial class SttpValueMutable : SttpValue
    {
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
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return (short)m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return (short)m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return (short)m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return (short)m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return (short)m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return (short)m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return (short)m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return (short)m_valueDecimal;
                        case SttpValueTypeCode.Double:
                            return (short)m_valueDouble;
                        case SttpValueTypeCode.Single:
                            return (short)m_valueSingle;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            return (short)m_valueChar;
                        case SttpValueTypeCode.Boolean:
                            return (short)(m_valueBoolean ? 0 : 1);
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return short.Parse((string)m_valueObject);
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override int AsInt32
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return (int)m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return (int)m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return (int)m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return (int)m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return (int)m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return (int)m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return (int)m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return (int)m_valueDecimal;
                        case SttpValueTypeCode.Double:
                            return (int)m_valueDouble;
                        case SttpValueTypeCode.Single:
                            return (int)m_valueSingle;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            return (int)m_valueChar;
                        case SttpValueTypeCode.Boolean:
                            return (int)(m_valueBoolean ? 0 : 1);
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return int.Parse((string)m_valueObject);
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override long AsInt64
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return (long)m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return (long)m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return (long)m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return (long)m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return (long)m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return (long)m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return (long)m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return (long)m_valueDecimal;
                        case SttpValueTypeCode.Double:
                            return (long)m_valueDouble;
                        case SttpValueTypeCode.Single:
                            return (long)m_valueSingle;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            return (long)m_valueChar;
                        case SttpValueTypeCode.Boolean:
                            return (long)(m_valueBoolean ? 0 : 1);
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return long.Parse((string)m_valueObject);
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override byte AsByte
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return (byte)m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return (byte)m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return (byte)m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return (byte)m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return (byte)m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return (byte)m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return (byte)m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return (byte)m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return (byte)m_valueDecimal;
                        case SttpValueTypeCode.Double:
                            return (byte)m_valueDouble;
                        case SttpValueTypeCode.Single:
                            return (byte)m_valueSingle;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            return (byte)m_valueChar;
                        case SttpValueTypeCode.Boolean:
                            return (byte)(m_valueBoolean ? 0 : 1);
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return byte.Parse((string)m_valueObject);
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override ushort AsUInt16
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return (ushort)m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return (ushort)m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return (ushort)m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return (ushort)m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return (ushort)m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return (ushort)m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return (ushort)m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return (ushort)m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return (ushort)m_valueDecimal;
                        case SttpValueTypeCode.Double:
                            return (ushort)m_valueDouble;
                        case SttpValueTypeCode.Single:
                            return (ushort)m_valueSingle;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            return (ushort)m_valueChar;
                        case SttpValueTypeCode.Boolean:
                            return (ushort)(m_valueBoolean ? 0 : 1);
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return ushort.Parse((string)m_valueObject);
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override uint AsUInt32
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return (uint)m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return (uint)m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return (uint)m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return (uint)m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return (uint)m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return (uint)m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return (uint)m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return (uint)m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return (uint)m_valueDecimal;
                        case SttpValueTypeCode.Double:
                            return (uint)m_valueDouble;
                        case SttpValueTypeCode.Single:
                            return (uint)m_valueSingle;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            return (uint)m_valueChar;
                        case SttpValueTypeCode.Boolean:
                            return (uint)(m_valueBoolean ? 0 : 1);
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return uint.Parse((string)m_valueObject);
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override ulong AsUInt64
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return (ulong)m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return (ulong)m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return (ulong)m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return (ulong)m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return (ulong)m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return (ulong)m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return (ulong)m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return (ulong)m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return (ulong)m_valueDecimal;
                        case SttpValueTypeCode.Double:
                            return (ulong)m_valueDouble;
                        case SttpValueTypeCode.Single:
                            return (ulong)m_valueSingle;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            return (ulong)m_valueChar;
                        case SttpValueTypeCode.Boolean:
                            return (ulong)(m_valueBoolean ? 0 : 1);
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return ulong.Parse((string)m_valueObject);
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override decimal AsDecimal
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return (decimal)m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return (decimal)m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return (decimal)m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return (decimal)m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return (decimal)m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return (decimal)m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return (decimal)m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return (decimal)m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return (decimal)m_valueDecimal;
                        case SttpValueTypeCode.Double:
                            return (decimal)m_valueDouble;
                        case SttpValueTypeCode.Single:
                            return (decimal)m_valueSingle;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            return (decimal)m_valueChar;
                        case SttpValueTypeCode.Boolean:
                            return (decimal)(m_valueBoolean ? 0 : 1);
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return decimal.Parse((string)m_valueObject);
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override double AsDouble
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return (double)m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return (double)m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return (double)m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return (double)m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return (double)m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return (double)m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return (double)m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return (double)m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return (double)m_valueDecimal;
                        case SttpValueTypeCode.Double:
                            return (double)m_valueDouble;
                        case SttpValueTypeCode.Single:
                            return (double)m_valueSingle;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            return (double)m_valueChar;
                        case SttpValueTypeCode.Boolean:
                            return (double)(m_valueBoolean ? 0 : 1);
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return double.Parse((string)m_valueObject);
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override float AsSingle
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return (float)m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return (float)m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return (float)m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return (float)m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return (float)m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return (float)m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return (float)m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return (float)m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return (float)m_valueDecimal;
                        case SttpValueTypeCode.Double:
                            return (float)m_valueDouble;
                        case SttpValueTypeCode.Single:
                            return (float)m_valueSingle;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            return (float)m_valueChar;
                        case SttpValueTypeCode.Boolean:
                            return (float)(m_valueBoolean ? 0 : 1);
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return float.Parse((string)m_valueObject);
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override DateTime AsDateTime
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            throw new InvalidCastException("Cannot cast from SByte");
                        case SttpValueTypeCode.Int16:
                            throw new InvalidCastException("Cannot cast from Int16");
                        case SttpValueTypeCode.Int32:
                            throw new InvalidCastException("Cannot cast from Int32");
                        case SttpValueTypeCode.Int64:
                            throw new InvalidCastException("Cannot cast from Int64");
                        case SttpValueTypeCode.Byte:
                            throw new InvalidCastException("Cannot cast from Byte");
                        case SttpValueTypeCode.UInt16:
                            throw new InvalidCastException("Cannot cast from UInt16");
                        case SttpValueTypeCode.UInt32:
                            throw new InvalidCastException("Cannot cast from UInt32");
                        case SttpValueTypeCode.UInt64:
                            throw new InvalidCastException("Cannot cast from UInt64");
                        case SttpValueTypeCode.Decimal:
                            throw new InvalidCastException("Cannot cast from Decimal");
                        case SttpValueTypeCode.Double:
                            throw new InvalidCastException("Cannot cast from Double");
                        case SttpValueTypeCode.Single:
                            throw new InvalidCastException("Cannot cast from Single");
                        case SttpValueTypeCode.SttpTime:
                            return m_valueSttpTime.Ticks;
                        case SttpValueTypeCode.SttpTimeOffset:
                            return m_valueSttpTimeOffset.Ticks;
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Boolean:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return DateTime.Parse((string)m_valueObject);
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        case SttpValueTypeCode.DateTime:
                            return m_valueDateTime;
                        case SttpValueTypeCode.DateTimeOffset:
                            return m_valueDateTimeOffset.UtcDateTime;
                        case SttpValueTypeCode.SttpNamedSet:
                        case SttpValueTypeCode.SttpMarkup:
                        case SttpValueTypeCode.BulkTransportGuid:
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override DateTimeOffset AsDateTimeOffset
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            throw new InvalidCastException("Cannot cast from SByte");
                        case SttpValueTypeCode.Int16:
                            throw new InvalidCastException("Cannot cast from Int16");
                        case SttpValueTypeCode.Int32:
                            throw new InvalidCastException("Cannot cast from Int32");
                        case SttpValueTypeCode.Int64:
                            throw new InvalidCastException("Cannot cast from Int64");
                        case SttpValueTypeCode.Byte:
                            throw new InvalidCastException("Cannot cast from Byte");
                        case SttpValueTypeCode.UInt16:
                            throw new InvalidCastException("Cannot cast from UInt16");
                        case SttpValueTypeCode.UInt32:
                            throw new InvalidCastException("Cannot cast from UInt32");
                        case SttpValueTypeCode.UInt64:
                            throw new InvalidCastException("Cannot cast from UInt64");
                        case SttpValueTypeCode.Decimal:
                            throw new InvalidCastException("Cannot cast from Decimal");
                        case SttpValueTypeCode.Double:
                            throw new InvalidCastException("Cannot cast from Double");
                        case SttpValueTypeCode.Single:
                            throw new InvalidCastException("Cannot cast from Single");
                        case SttpValueTypeCode.SttpTime:
                            throw new NotImplementedException();
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new NotImplementedException();
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Boolean:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return DateTimeOffset.Parse((string)m_valueObject);
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        case SttpValueTypeCode.DateTime:
                            return new DateTimeOffset(m_valueDateTime);
                        case SttpValueTypeCode.DateTimeOffset:
                            return m_valueDateTimeOffset.UtcDateTime;
                        case SttpValueTypeCode.SttpNamedSet:
                        case SttpValueTypeCode.SttpMarkup:
                        case SttpValueTypeCode.BulkTransportGuid:
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override SttpTime AsSttpTime
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            throw new InvalidCastException("Cannot cast from SByte");
                        case SttpValueTypeCode.Int16:
                            throw new InvalidCastException("Cannot cast from Int16");
                        case SttpValueTypeCode.Int32:
                            throw new InvalidCastException("Cannot cast from Int32");
                        case SttpValueTypeCode.Int64:
                            throw new InvalidCastException("Cannot cast from Int64");
                        case SttpValueTypeCode.Byte:
                            throw new InvalidCastException("Cannot cast from Byte");
                        case SttpValueTypeCode.UInt16:
                            throw new InvalidCastException("Cannot cast from UInt16");
                        case SttpValueTypeCode.UInt32:
                            throw new InvalidCastException("Cannot cast from UInt32");
                        case SttpValueTypeCode.UInt64:
                            throw new InvalidCastException("Cannot cast from UInt64");
                        case SttpValueTypeCode.Decimal:
                            throw new InvalidCastException("Cannot cast from Decimal");
                        case SttpValueTypeCode.Double:
                            throw new InvalidCastException("Cannot cast from Double");
                        case SttpValueTypeCode.Single:
                            throw new InvalidCastException("Cannot cast from Single");
                        case SttpValueTypeCode.SttpTime:
                            return m_valueSttpTime;
                        case SttpValueTypeCode.SttpTimeOffset:
                            return m_valueSttpTimeOffset.ToSttpTimestamp();
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Boolean:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return SttpTime.Parse((string)m_valueObject);
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override SttpTimeOffset AsSttpTimeOffset
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            throw new InvalidCastException("Cannot cast from SByte");
                        case SttpValueTypeCode.Int16:
                            throw new InvalidCastException("Cannot cast from Int16");
                        case SttpValueTypeCode.Int32:
                            throw new InvalidCastException("Cannot cast from Int32");
                        case SttpValueTypeCode.Int64:
                            throw new InvalidCastException("Cannot cast from Int64");
                        case SttpValueTypeCode.Byte:
                            throw new InvalidCastException("Cannot cast from Byte");
                        case SttpValueTypeCode.UInt16:
                            throw new InvalidCastException("Cannot cast from UInt16");
                        case SttpValueTypeCode.UInt32:
                            throw new InvalidCastException("Cannot cast from UInt32");
                        case SttpValueTypeCode.UInt64:
                            throw new InvalidCastException("Cannot cast from UInt64");
                        case SttpValueTypeCode.Decimal:
                            throw new InvalidCastException("Cannot cast from Decimal");
                        case SttpValueTypeCode.Double:
                            throw new InvalidCastException("Cannot cast from Double");
                        case SttpValueTypeCode.Single:
                            throw new InvalidCastException("Cannot cast from Single");
                        case SttpValueTypeCode.SttpTime:
                            return m_valueSttpTime.ToSttpTimestampOffset();
                        case SttpValueTypeCode.SttpTimeOffset:
                            return m_valueSttpTimeOffset;
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Boolean:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return SttpTimeOffset.Parse((string)m_valueObject);
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override TimeSpan AsTimeSpan
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            throw new InvalidCastException("Cannot cast from SByte");
                        case SttpValueTypeCode.Int16:
                            throw new InvalidCastException("Cannot cast from Int16");
                        case SttpValueTypeCode.Int32:
                            throw new InvalidCastException("Cannot cast from Int32");
                        case SttpValueTypeCode.Int64:
                            throw new InvalidCastException("Cannot cast from Int64");
                        case SttpValueTypeCode.Byte:
                            throw new InvalidCastException("Cannot cast from Byte");
                        case SttpValueTypeCode.UInt16:
                            throw new InvalidCastException("Cannot cast from UInt16");
                        case SttpValueTypeCode.UInt32:
                            throw new InvalidCastException("Cannot cast from UInt32");
                        case SttpValueTypeCode.UInt64:
                            throw new InvalidCastException("Cannot cast from UInt64");
                        case SttpValueTypeCode.Decimal:
                            throw new InvalidCastException("Cannot cast from Decimal");
                        case SttpValueTypeCode.Double:
                            throw new InvalidCastException("Cannot cast from Double");
                        case SttpValueTypeCode.Single:
                            throw new InvalidCastException("Cannot cast from Single");
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            return m_valueTimeSpan;
                        case SttpValueTypeCode.Char:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Boolean:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return TimeSpan.Parse((string)m_valueObject);
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override char AsChar
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return (char)m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return (char)m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return (char)m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return (char)m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return (char)m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return (char)m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return (char)m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return (char)m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return (char)m_valueDecimal;
                        case SttpValueTypeCode.Double:
                            return (char)m_valueDouble;
                        case SttpValueTypeCode.Single:
                            return (char)m_valueSingle;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            return (char)m_valueChar;
                        case SttpValueTypeCode.Boolean:
                            return (char)(m_valueBoolean ? 'T' : 'F');
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return char.Parse((string)m_valueObject);
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override bool AsBoolean
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return 0 != m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return 0 != m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return 0 != m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return 0 != m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return 0 != m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return 0 != m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return 0 != m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return 0 != m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return Math.Abs(m_valueDecimal) > 0.75M;
                        case SttpValueTypeCode.Double:
                            return Math.Abs(m_valueDouble) > 0.75;
                        case SttpValueTypeCode.Single:
                            return Math.Abs(m_valueSingle) > 0.75;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            switch (m_valueChar)
                            {
                                case '1':
                                case 'T':
                                    return true;
                                case '0':
                                case 'F':
                                    return false;
                            }
                            throw new InvalidCastException("Cannot cast from Char");
                        case SttpValueTypeCode.Boolean:
                            return m_valueBoolean;
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return bool.Parse((string)m_valueObject);
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        public override Guid AsGuid
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            throw new InvalidCastException("Cannot cast from SByte");
                        case SttpValueTypeCode.Int16:
                            throw new InvalidCastException("Cannot cast from Int16");
                        case SttpValueTypeCode.Int32:
                            throw new InvalidCastException("Cannot cast from Int32");
                        case SttpValueTypeCode.Int64:
                            throw new InvalidCastException("Cannot cast from Int64");
                        case SttpValueTypeCode.Byte:
                            throw new InvalidCastException("Cannot cast from Byte");
                        case SttpValueTypeCode.UInt16:
                            throw new InvalidCastException("Cannot cast from UInt16");
                        case SttpValueTypeCode.UInt32:
                            throw new InvalidCastException("Cannot cast from UInt32");
                        case SttpValueTypeCode.UInt64:
                            throw new InvalidCastException("Cannot cast from UInt64");
                        case SttpValueTypeCode.Decimal:
                            throw new InvalidCastException("Cannot cast from Decimal");
                        case SttpValueTypeCode.Double:
                            throw new InvalidCastException("Cannot cast from Double");
                        case SttpValueTypeCode.Single:
                            throw new InvalidCastException("Cannot cast from Single");
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Boolean:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Guid:
                            return m_valueGuid;
                        case SttpValueTypeCode.String:
                            return Guid.Parse((string)m_valueObject);
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override string AsString
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            return null;
                        case SttpValueTypeCode.SByte:
                            return m_valueSByte.ToString();
                        case SttpValueTypeCode.Int16:
                            return m_valueInt16.ToString();
                        case SttpValueTypeCode.Int32:
                            return m_valueInt32.ToString();
                        case SttpValueTypeCode.Int64:
                            return m_valueInt64.ToString();
                        case SttpValueTypeCode.Byte:
                            return m_valueByte.ToString();
                        case SttpValueTypeCode.UInt16:
                            return m_valueUInt16.ToString();
                        case SttpValueTypeCode.UInt32:
                            return m_valueUInt32.ToString();
                        case SttpValueTypeCode.UInt64:
                            return m_valueUInt64.ToString();
                        case SttpValueTypeCode.Decimal:
                            return m_valueDecimal.ToString();
                        case SttpValueTypeCode.Double:
                            return m_valueDouble.ToString();
                        case SttpValueTypeCode.Single:
                            return m_valueSingle.ToString();
                        case SttpValueTypeCode.SttpTime:
                            return m_valueSttpTime.ToString();
                        case SttpValueTypeCode.SttpTimeOffset:
                            return m_valueSttpTimeOffset.ToString();
                        case SttpValueTypeCode.TimeSpan:
                            return m_valueTimeSpan.ToString();
                        case SttpValueTypeCode.Char:
                            return m_valueChar.ToString();
                        case SttpValueTypeCode.Boolean:
                            return m_valueBoolean.ToString();
                        case SttpValueTypeCode.Guid:
                            return m_valueGuid.ToString();
                        case SttpValueTypeCode.String:
                            return (string)m_valueObject;
                        case SttpValueTypeCode.SttpBuffer:
                            //ToDo: Return a byte string 0x292A78B402;
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override string ToTypeString
        {
            get
            {
                return $"({m_valueTypeCode}){AsString}";
            }
        }

        public override SttpBuffer AsBuffer
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            throw new InvalidCastException("Cannot cast from SByte");
                        case SttpValueTypeCode.Int16:
                            throw new InvalidCastException("Cannot cast from Int16");
                        case SttpValueTypeCode.Int32:
                            throw new InvalidCastException("Cannot cast from Int32");
                        case SttpValueTypeCode.Int64:
                            throw new InvalidCastException("Cannot cast from Int64");
                        case SttpValueTypeCode.Byte:
                            throw new InvalidCastException("Cannot cast from Byte");
                        case SttpValueTypeCode.UInt16:
                            throw new InvalidCastException("Cannot cast from UInt16");
                        case SttpValueTypeCode.UInt32:
                            throw new InvalidCastException("Cannot cast from UInt32");
                        case SttpValueTypeCode.UInt64:
                            throw new InvalidCastException("Cannot cast from UInt64");
                        case SttpValueTypeCode.Decimal:
                            throw new InvalidCastException("Cannot cast from Decimal");
                        case SttpValueTypeCode.Double:
                            throw new InvalidCastException("Cannot cast from Double");
                        case SttpValueTypeCode.Single:
                            throw new InvalidCastException("Cannot cast from Single");
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Boolean:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            throw new InvalidCastException("Cannot cast from String");
                        case SttpValueTypeCode.SttpBuffer:
                            return (SttpBuffer)m_valueObject;
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override SttpValueSet AsSttpValueSet
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            throw new InvalidCastException("Cannot cast from SByte");
                        case SttpValueTypeCode.Int16:
                            throw new InvalidCastException("Cannot cast from Int16");
                        case SttpValueTypeCode.Int32:
                            throw new InvalidCastException("Cannot cast from Int32");
                        case SttpValueTypeCode.Int64:
                            throw new InvalidCastException("Cannot cast from Int64");
                        case SttpValueTypeCode.Byte:
                            throw new InvalidCastException("Cannot cast from Byte");
                        case SttpValueTypeCode.UInt16:
                            throw new InvalidCastException("Cannot cast from UInt16");
                        case SttpValueTypeCode.UInt32:
                            throw new InvalidCastException("Cannot cast from UInt32");
                        case SttpValueTypeCode.UInt64:
                            throw new InvalidCastException("Cannot cast from UInt64");
                        case SttpValueTypeCode.Decimal:
                            throw new InvalidCastException("Cannot cast from Decimal");
                        case SttpValueTypeCode.Double:
                            throw new InvalidCastException("Cannot cast from Double");
                        case SttpValueTypeCode.Single:
                            throw new InvalidCastException("Cannot cast from Single");
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Boolean:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            throw new InvalidCastException("Cannot cast from String");
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            return (SttpValueSet)m_valueObject;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override SttpNamedSet AsSttpNamedSet
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            throw new InvalidCastException("Cannot cast from SByte");
                        case SttpValueTypeCode.Int16:
                            throw new InvalidCastException("Cannot cast from Int16");
                        case SttpValueTypeCode.Int32:
                            throw new InvalidCastException("Cannot cast from Int32");
                        case SttpValueTypeCode.Int64:
                            throw new InvalidCastException("Cannot cast from Int64");
                        case SttpValueTypeCode.Byte:
                            throw new InvalidCastException("Cannot cast from Byte");
                        case SttpValueTypeCode.UInt16:
                            throw new InvalidCastException("Cannot cast from UInt16");
                        case SttpValueTypeCode.UInt32:
                            throw new InvalidCastException("Cannot cast from UInt32");
                        case SttpValueTypeCode.UInt64:
                            throw new InvalidCastException("Cannot cast from UInt64");
                        case SttpValueTypeCode.Decimal:
                            throw new InvalidCastException("Cannot cast from Decimal");
                        case SttpValueTypeCode.Double:
                            throw new InvalidCastException("Cannot cast from Double");
                        case SttpValueTypeCode.Single:
                            throw new InvalidCastException("Cannot cast from Single");
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Boolean:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            throw new InvalidCastException("Cannot cast from String");
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        case SttpValueTypeCode.SttpNamedSet:
                            return (SttpNamedSet)m_valueObject;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        public override SttpMarkup AsSttpMarkup => null;

        public override Guid AsBulkTransportGuid => Guid.Empty;

        public override object ToNativeType
        {
            get
            {
                switch (ValueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        return null;
                    case SttpValueTypeCode.SByte:
                        return AsSByte;
                    case SttpValueTypeCode.Int16:
                        return AsInt16;
                    case SttpValueTypeCode.Int32:
                        return AsInt32;
                    case SttpValueTypeCode.Int64:
                        return AsInt64;
                    case SttpValueTypeCode.Byte:
                        return AsByte;
                    case SttpValueTypeCode.UInt16:
                        return AsUInt16;
                    case SttpValueTypeCode.UInt32:
                        return AsUInt32;
                    case SttpValueTypeCode.UInt64:
                        return AsUInt64;
                    case SttpValueTypeCode.Single:
                        return AsSingle;
                    case SttpValueTypeCode.Double:
                        return AsDouble;
                    case SttpValueTypeCode.Decimal:
                        return AsDecimal;
                    case SttpValueTypeCode.DateTime:
                        return AsDateTime;
                    case SttpValueTypeCode.DateTimeOffset:
                        return AsDateTimeOffset;
                    case SttpValueTypeCode.SttpTime:
                        return AsSttpTime;
                    case SttpValueTypeCode.SttpTimeOffset:
                        return AsSttpTimeOffset;
                    case SttpValueTypeCode.TimeSpan:
                        return AsTimeSpan.Ticks;
                    case SttpValueTypeCode.Boolean:
                        return AsBoolean;
                    case SttpValueTypeCode.Char:
                        return AsChar;
                    case SttpValueTypeCode.Guid:
                        return AsGuid;
                    case SttpValueTypeCode.String:
                        return AsString;
                    case SttpValueTypeCode.SttpBuffer:
                        return AsBuffer;
                    case SttpValueTypeCode.SttpValueSet:
                        return AsSttpValueSet;
                    case SttpValueTypeCode.SttpNamedSet:
                        return AsSttpNamedSet;
                    case SttpValueTypeCode.SttpMarkup:
                        throw new NotImplementedException();
                    //return AsConnectionString);
                    case SttpValueTypeCode.BulkTransportGuid:
                        throw new NotImplementedException();
                    //return AsBulkTransportGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

    }
}

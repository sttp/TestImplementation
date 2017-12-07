using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueBulkTransportGuid : SttpValue
    {
        public readonly Guid Value;

        public SttpValueBulkTransportGuid(Guid value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueBulkTransportGuidMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueBulkTransportGuidMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueBulkTransportGuidMethods.ToNativeType(Value);
        public override sbyte AsSByte => SttpValueBulkTransportGuidMethods.AsSByte(Value);
        public override short AsInt16 => SttpValueBulkTransportGuidMethods.AsInt16(Value);
        public override int AsInt32 => SttpValueBulkTransportGuidMethods.AsInt32(Value);
        public override long AsInt64 => SttpValueBulkTransportGuidMethods.AsInt64(Value);
        public override byte AsByte => SttpValueBulkTransportGuidMethods.AsByte(Value);
        public override ushort AsUInt16 => SttpValueBulkTransportGuidMethods.AsUInt16(Value);
        public override uint AsUInt32 => SttpValueBulkTransportGuidMethods.AsUInt32(Value);
        public override ulong AsUInt64 => SttpValueBulkTransportGuidMethods.AsUInt64(Value);
        public override float AsSingle => SttpValueBulkTransportGuidMethods.AsSingle(Value);
        public override double AsDouble => SttpValueBulkTransportGuidMethods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueBulkTransportGuidMethods.AsDecimal(Value);
        public override DateTime AsDateTime => SttpValueBulkTransportGuidMethods.AsDateTime(Value);
        public override DateTimeOffset AsDateTimeOffset => SttpValueBulkTransportGuidMethods.AsDateTimeOffset(Value);
        public override SttpTime AsSttpTime => SttpValueBulkTransportGuidMethods.AsSttpTime(Value);
        public override SttpTimeOffset AsSttpTimeOffset => SttpValueBulkTransportGuidMethods.AsSttpTimeOffset(Value);
        public override TimeSpan AsTimeSpan => SttpValueBulkTransportGuidMethods.AsTimeSpan(Value);
        public override bool AsBoolean => SttpValueBulkTransportGuidMethods.AsBoolean(Value);
        public override char AsChar => SttpValueBulkTransportGuidMethods.AsChar(Value);
        public override Guid AsGuid => SttpValueBulkTransportGuidMethods.AsGuid(Value);
        public override string AsString => SttpValueBulkTransportGuidMethods.AsString(Value);
        public override SttpBuffer AsBuffer => SttpValueBulkTransportGuidMethods.AsBuffer(Value);
        public override SttpValueSet AsSttpValueSet => SttpValueBulkTransportGuidMethods.AsValueSet(Value);
        public override SttpNamedSet AsSttpNamedSet => SttpValueBulkTransportGuidMethods.AsNamedSet(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueBulkTransportGuidMethods.AsSttpMarkup(Value);
        public override Guid AsBulkTransportGuid => SttpValueBulkTransportGuidMethods.AsBulkTransportGuid(Value);
    }

    internal static class SttpValueBulkTransportGuidMethods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.BulkTransportGuid;

        public static string ToTypeString(Guid value)
        {
            return $"(BulkTransportGuid){value.ToString()}";
        }

        public static object ToNativeType(Guid value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static sbyte AsSByte(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SByte");
        }
        public static short AsInt16(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int16");
        }

        public static int AsInt32(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int32");
        }

        public static long AsInt64(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int64");
        }

        public static byte AsByte(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Byte");
        }

        public static ushort AsUInt16(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt16");
        }

        public static uint AsUInt32(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt32");
        }

        public static ulong AsUInt64(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt64");
        }

        public static float AsSingle(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Single");
        }

        public static double AsDouble(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Double");
        }

        public static decimal AsDecimal(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Decimal");
        }

        public static DateTime AsDateTime(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTime");
        }

        public static DateTimeOffset AsDateTimeOffset(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTimeOffset");
        }

        public static SttpTime AsSttpTime(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static SttpTimeOffset AsSttpTimeOffset(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTimeOffset");
        }

        public static TimeSpan AsTimeSpan(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to TimeSpan");
        }

        public static bool AsBoolean(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static char AsChar(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Char");
        }

        public static Guid AsGuid(Guid value)
        {
            return value;
        }

        public static string AsString(Guid value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsBuffer(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpValueSet AsValueSet(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpValueSet");
        }

        public static SttpNamedSet AsNamedSet(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpNamedSet");
        }

        public static SttpMarkup AsSttpMarkup(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static Guid AsBulkTransportGuid(Guid value)
        {
            return value;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CTP
{
    public class CtpValueDocument : CtpValue
    {
        public readonly CtpDocument Value;

        public CtpValueDocument(CtpDocument value)
        {
            Value = value;
        }

        public override CtpTypeCode ValueTypeCode => SttpValueSttpMarkupMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueSttpMarkupMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueSttpMarkupMethods.ToNativeType(Value);
        public override long AsInt64 => SttpValueSttpMarkupMethods.AsInt64(Value);
        public override float AsSingle => SttpValueSttpMarkupMethods.AsSingle(Value);
        public override double AsDouble => SttpValueSttpMarkupMethods.AsDouble(Value);
        public override CtpTime AsCtpTime => SttpValueSttpMarkupMethods.AsSttpTime(Value);
        public override bool AsBoolean => SttpValueSttpMarkupMethods.AsBoolean(Value);
        public override Guid AsGuid => SttpValueSttpMarkupMethods.AsGuid(Value);
        public override string AsString => SttpValueSttpMarkupMethods.AsString(Value);
        public override CtpBuffer AsSttpBuffer => SttpValueSttpMarkupMethods.AsSttpBuffer(Value);
        public override CtpDocument AsDocument => SttpValueSttpMarkupMethods.AsSttpMarkup(Value);
    }

    internal static class SttpValueSttpMarkupMethods
    {
        public static CtpTypeCode ValueTypeCode => CtpTypeCode.CtpDocument;

        public static string ToTypeString(CtpDocument value)
        {
            return $"(CtpDocument){value.ToString()}";
        }

        public static object ToNativeType(CtpDocument value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static long AsInt64(CtpDocument value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int64");
        }

        public static float AsSingle(CtpDocument value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Single");
        }

        public static double AsDouble(CtpDocument value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Double");
        }

        public static CtpTime AsSttpTime(CtpDocument value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static bool AsBoolean(CtpDocument value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static Guid AsGuid(CtpDocument value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(CtpDocument value)
        {
            return value.ToString();
        }

        public static CtpBuffer AsSttpBuffer(CtpDocument value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static CtpDocument AsSttpMarkup(CtpDocument value)
        {
            return value;
        }

        #endregion
    }
}

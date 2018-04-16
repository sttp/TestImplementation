using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CTP
{
    public class CtpString : CtpValue
    {
        public static readonly CtpValue EmptyString = new CtpString(string.Empty);

        public readonly string Value;

        public CtpString(string value)
        {
            Value = value;
        }

        public override CtpTypeCode ValueTypeCode => SttpValueStringMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueStringMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueStringMethods.ToNativeType(Value);
        public override long AsInt64 => SttpValueStringMethods.AsInt64(Value);
        public override float AsSingle => SttpValueStringMethods.AsSingle(Value);
        public override double AsDouble => SttpValueStringMethods.AsDouble(Value);
        public override CtpTime AsCtpTime => SttpValueStringMethods.AsSttpTime(Value);
        public override bool AsBoolean => SttpValueStringMethods.AsBoolean(Value);
        public override Guid AsGuid => SttpValueStringMethods.AsGuid(Value);
        public override string AsString => SttpValueStringMethods.AsString(Value);
        public override CtpBuffer AsSttpBuffer => SttpValueStringMethods.AsSttpBuffer(Value);
        public override CtpDocument AsDocument => SttpValueStringMethods.AsSttpMarkup(Value);
    }

    internal static class SttpValueStringMethods
    {
        public static CtpTypeCode ValueTypeCode => CtpTypeCode.String;

        public static string ToTypeString(string value)
        {
            return $"(String){value.ToString()}";
        }

        public static object ToNativeType(string value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static long AsInt64(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int64");
        }

        public static float AsSingle(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Single");
        }

        public static double AsDouble(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Double");
        }

        public static CtpTime AsSttpTime(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static bool AsBoolean(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static Guid AsGuid(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(string value)
        {
            return value.ToString();
        }

        public static CtpBuffer AsSttpBuffer(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static CtpDocument AsSttpMarkup(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        #endregion
    }
}

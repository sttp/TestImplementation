using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp
{
    public class SttpValueSttpMarkup : SttpValue
    {
        public readonly SttpMarkup Value;

        public SttpValueSttpMarkup(SttpMarkup value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueSttpMarkupMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueSttpMarkupMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueSttpMarkupMethods.ToNativeType(Value);
        public override long AsInt64 => SttpValueSttpMarkupMethods.AsInt64(Value);
        public override float AsSingle => SttpValueSttpMarkupMethods.AsSingle(Value);
        public override double AsDouble => SttpValueSttpMarkupMethods.AsDouble(Value);
        public override SttpTime AsSttpTime => SttpValueSttpMarkupMethods.AsSttpTime(Value);
        public override bool AsBoolean => SttpValueSttpMarkupMethods.AsBoolean(Value);
        public override Guid AsGuid => SttpValueSttpMarkupMethods.AsGuid(Value);
        public override string AsString => SttpValueSttpMarkupMethods.AsString(Value);
        public override SttpBuffer AsSttpBuffer => SttpValueSttpMarkupMethods.AsSttpBuffer(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueSttpMarkupMethods.AsSttpMarkup(Value);
    }

    internal static class SttpValueSttpMarkupMethods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.SttpMarkup;

        public static string ToTypeString(SttpMarkup value)
        {
            return $"(SttpMarkup){value.ToString()}";
        }

        public static object ToNativeType(SttpMarkup value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static long AsInt64(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int64");
        }

        public static float AsSingle(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Single");
        }

        public static double AsDouble(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Double");
        }

        public static SttpTime AsSttpTime(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static bool AsBoolean(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static Guid AsGuid(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(SttpMarkup value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsSttpBuffer(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpMarkup AsSttpMarkup(SttpMarkup value)
        {
            return value;
        }

        #endregion
    }
}

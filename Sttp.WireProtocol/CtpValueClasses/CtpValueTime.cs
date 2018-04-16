using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CTP
{
    public class CtpValueTime : CtpValue
    {
        public readonly CtpTime Value;

        public CtpValueTime(CtpTime value)
        {
            Value = value;
        }

        public override CtpTypeCode ValueTypeCode => SttpValueSttpTimeMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueSttpTimeMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueSttpTimeMethods.ToNativeType(Value);
        public override long AsInt64 => SttpValueSttpTimeMethods.AsInt64(Value);
        public override float AsSingle => SttpValueSttpTimeMethods.AsSingle(Value);
        public override double AsDouble => SttpValueSttpTimeMethods.AsDouble(Value);
        public override CtpTime AsCtpTime => SttpValueSttpTimeMethods.AsSttpTime(Value);
        public override bool AsBoolean => SttpValueSttpTimeMethods.AsBoolean(Value);
        public override Guid AsGuid => SttpValueSttpTimeMethods.AsGuid(Value);
        public override string AsString => SttpValueSttpTimeMethods.AsString(Value);
        public override CtpBuffer AsSttpBuffer => SttpValueSttpTimeMethods.AsSttpBuffer(Value);
        public override CtpMarkup AsSttpMarkup => SttpValueSttpTimeMethods.AsSttpMarkup(Value);
    }

    internal static class SttpValueSttpTimeMethods
    {
        public static CtpTypeCode ValueTypeCode => CtpTypeCode.CtpTime;
       
        public static string ToTypeString(CtpTime value)
        {
            return $"(SttpTime){value.ToString()}";
        }

        public static object ToNativeType(CtpTime value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static long AsInt64(CtpTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int64");
        }

        public static float AsSingle(CtpTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Single");
        }

        public static double AsDouble(CtpTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Double");
        }

        public static CtpTime AsSttpTime(CtpTime value)
        {
            return value;
        }

        public static bool AsBoolean(CtpTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static Guid AsGuid(CtpTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(CtpTime value)
        {
            return value.ToString();
        }

        public static CtpBuffer AsSttpBuffer(CtpTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static CtpMarkup AsSttpMarkup(CtpTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        #endregion
    }
}

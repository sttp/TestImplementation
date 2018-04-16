using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CTP
{
    public class CtpGuid : CtpValue
    {
        public readonly Guid Value;

        public CtpGuid(Guid value)
        {
            Value = value;
        }

        public override CtpTypeCode ValueTypeCode => SttpValueGuidMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueGuidMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueGuidMethods.ToNativeType(Value);
        public override long AsInt64 => SttpValueGuidMethods.AsInt64(Value);
        public override float AsSingle => SttpValueGuidMethods.AsSingle(Value);
        public override double AsDouble => SttpValueGuidMethods.AsDouble(Value);
        public override CtpTime AsCtpTime => SttpValueGuidMethods.AsSttpTime(Value);
        public override bool AsBoolean => SttpValueGuidMethods.AsBoolean(Value);
        public override Guid AsGuid => SttpValueGuidMethods.AsGuid(Value);
        public override string AsString => SttpValueGuidMethods.AsString(Value);
        public override CtpBuffer AsSttpBuffer => SttpValueGuidMethods.AsSttpBuffer(Value);
        public override CtpDocument AsDocument => SttpValueGuidMethods.AsSttpMarkup(Value);
    }

    internal static class SttpValueGuidMethods
    {
        public static CtpTypeCode ValueTypeCode => CtpTypeCode.Guid;

        public static string ToTypeString(Guid value)
        {
            return $"(Guid){value.ToString()}";
        }

        public static object ToNativeType(Guid value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static long AsInt64(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int64");
        }

        public static float AsSingle(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Single");
        }

        public static double AsDouble(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Double");
        }

        public static CtpTime AsSttpTime(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static bool AsBoolean(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static Guid AsGuid(Guid value)
        {
            return value;
        }

        public static string AsString(Guid value)
        {
            return value.ToString();
        }

        public static CtpBuffer AsSttpBuffer(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static CtpDocument AsSttpMarkup(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        #endregion
    }
}

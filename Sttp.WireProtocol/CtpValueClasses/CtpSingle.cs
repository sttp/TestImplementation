using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CTP
{
    public class CtpSingle : CtpValue
    {
        public readonly float Value;

        public CtpSingle(float value)
        {
            Value = value;
        }

        public override CtpTypeCode ValueTypeCode => SttpValueSingleMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueSingleMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueSingleMethods.ToNativeType(Value);
        public override long AsInt64 => SttpValueSingleMethods.AsInt64(Value);
        public override float AsSingle => SttpValueSingleMethods.AsSingle(Value);
        public override double AsDouble => SttpValueSingleMethods.AsDouble(Value);
        public override CtpTime AsCtpTime => SttpValueSingleMethods.AsSttpTime(Value);
        public override bool AsBoolean => SttpValueSingleMethods.AsBoolean(Value);
        public override Guid AsGuid => SttpValueSingleMethods.AsGuid(Value);
        public override string AsString => SttpValueSingleMethods.AsString(Value);
        public override CtpBuffer AsSttpBuffer => SttpValueSingleMethods.AsSttpBuffer(Value);
        public override CtpDocument AsDocument => SttpValueSingleMethods.AsSttpMarkup(Value);
    }

    internal static class SttpValueSingleMethods
    {
        public static CtpTypeCode ValueTypeCode => CtpTypeCode.Single;
       
        public static string ToTypeString(float value)
        {
            return $"(float){value.ToString()}";
        }

        public static object ToNativeType(float value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static long AsInt64(float value)
        {
            checked
            {
                return (long)value;
            }
        }

        public static float AsSingle(float value)
        {
            checked
            {
                return (float)value;
            }
        }

        public static double AsDouble(float value)
        {
            checked
            {
                return (double)value;
            }
        }

        public static CtpTime AsSttpTime(float value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static bool AsBoolean(float value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static Guid AsGuid(float value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(float value)
        {
            return value.ToString();
        }

        public static CtpBuffer AsSttpBuffer(float value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static CtpDocument AsSttpMarkup(float value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        #endregion
    }
}

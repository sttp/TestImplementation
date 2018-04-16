using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CTP
{
    public class CtpBoolean : CtpValue
    {
        public static readonly CtpValue ValueTrue = new CtpBoolean(true);
        public static readonly CtpValue ValueFalse = new CtpBoolean(false);

        public readonly bool Value;

        private CtpBoolean(bool value)
        {
            Value = value;
        }

        public override CtpTypeCode ValueTypeCode => SttpValueBooleanMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueBooleanMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueBooleanMethods.ToNativeType(Value);
        public override long AsInt64 => SttpValueBooleanMethods.AsInt64(Value);
        public override float AsSingle => SttpValueBooleanMethods.AsSingle(Value);
        public override double AsDouble => SttpValueBooleanMethods.AsDouble(Value);
        public override CtpTime AsCtpTime => SttpValueBooleanMethods.AsSttpTime(Value);
        public override bool AsBoolean => SttpValueBooleanMethods.AsBoolean(Value);
        public override Guid AsGuid => SttpValueBooleanMethods.AsGuid(Value);
        public override string AsString => SttpValueBooleanMethods.AsString(Value);
        public override CtpBuffer AsSttpBuffer => SttpValueBooleanMethods.AsSttpBuffer(Value);
        public override CtpDocument AsDocument => SttpValueBooleanMethods.AsSttpMarkup(Value);
    }

    internal static class SttpValueBooleanMethods
    {
        public static CtpTypeCode ValueTypeCode => CtpTypeCode.Boolean;

        public static string ToTypeString(bool value)
        {
            return $"(bool){value.ToString()}";
        }

        public static object ToNativeType(bool value)
        {
            return value;
        }

        #region [ Type Casting ]


        public static long AsInt64(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int64");
        }

        public static float AsSingle(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Single");
        }

        public static double AsDouble(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Double");
        }

        public static CtpTime AsSttpTime(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static bool AsBoolean(bool value)
        {
            return value;
        }

        public static Guid AsGuid(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(bool value)
        {
            return value.ToString();
        }

        public static CtpBuffer AsSttpBuffer(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static CtpDocument AsSttpMarkup(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        #endregion
    }
}

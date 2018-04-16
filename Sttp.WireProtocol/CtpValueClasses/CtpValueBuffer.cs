using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CTP
{
    public class CtpValueBuffer : CtpValue
    {
        public readonly CtpBuffer Value;

        public CtpValueBuffer(CtpBuffer value)
        {
            Value = value;
        }

        public CtpValueBuffer(byte[] value)
        {
            Value = new CtpBuffer(value);
        }

        public override CtpTypeCode ValueTypeCode => SttpValueSttpBufferMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueSttpBufferMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueSttpBufferMethods.ToNativeType(Value);
        public override long AsInt64 => SttpValueSttpBufferMethods.AsInt64(Value);
        public override float AsSingle => SttpValueSttpBufferMethods.AsSingle(Value);
        public override double AsDouble => SttpValueSttpBufferMethods.AsDouble(Value);
        public override CtpTime AsCtpTime => SttpValueSttpBufferMethods.AsSttpTime(Value);
        public override bool AsBoolean => SttpValueSttpBufferMethods.AsBoolean(Value);
        public override Guid AsGuid => SttpValueSttpBufferMethods.AsGuid(Value);
        public override string AsString => SttpValueSttpBufferMethods.AsString(Value);
        public override CtpBuffer AsSttpBuffer => SttpValueSttpBufferMethods.AsSttpBuffer(Value);
        public override CtpMarkup AsSttpMarkup => SttpValueSttpBufferMethods.AsSttpMarkup(Value);
    }

    internal static class SttpValueSttpBufferMethods
    {
        public static CtpTypeCode ValueTypeCode => CtpTypeCode.CtpBuffer;

        public static string ToTypeString(CtpBuffer value)
        {
            return $"(SttpBuffer){value.ToString()}";
        }

        public static object ToNativeType(CtpBuffer value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static long AsInt64(CtpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int64");
        }

        public static float AsSingle(CtpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Single");
        }

        public static double AsDouble(CtpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Double");
        }

        public static CtpTime AsSttpTime(CtpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static bool AsBoolean(CtpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static Guid AsGuid(CtpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(CtpBuffer value)
        {
            return value.ToString();
        }

        public static CtpBuffer AsSttpBuffer(CtpBuffer value)
        {
            return value;
        }

        public static CtpMarkup AsSttpMarkup(CtpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        #endregion
    }
}

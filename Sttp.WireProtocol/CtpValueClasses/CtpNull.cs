using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CTP
{
    public class CtpNull : CtpValue
    {
        public static readonly CtpValue NullValue = new CtpNull();

        private CtpNull()
        {
        }

        public override CtpTypeCode ValueTypeCode => CtpTypeCode.Null;
        public override string ToTypeString => "(Null)";
        public override object ToNativeType => null;

        public override long AsInt64 => throw new InvalidCastException();
        public override double AsDouble => throw new InvalidCastException();
        public override float AsSingle => throw new InvalidCastException();
        public override CtpTime AsCtpTime => throw new InvalidCastException();
        public override bool AsBoolean => throw new InvalidCastException();
        public override Guid AsGuid => throw new InvalidCastException();
        public override string AsString => throw new InvalidCastException();
        public override CtpBuffer AsSttpBuffer => throw new InvalidCastException();
        public override CtpDocument AsDocument => throw new InvalidCastException();

       
    }


    internal static class SttpValueNullMethods
    {
        public static CtpTypeCode ValueTypeCode => CtpTypeCode.Null;

        public static string ToTypeString()
        {
            return $"(Null)";
        }

        public static object ToNativeType()
        {
            return null;
        }

    }
}

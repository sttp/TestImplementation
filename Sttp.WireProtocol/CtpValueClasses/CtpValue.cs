using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CTP
{
    /// <summary>
    /// This class contains the fundamental value for STTP.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public abstract partial class CtpValue : IEquatable<CtpValue>
    {
        public static readonly CtpValue Null = CtpNull.NullValue;

        protected CtpValue()
        {
        }

        public abstract long AsInt64 { get; }
        public abstract float AsSingle { get; }
        public abstract double AsDouble { get; }
        public abstract CtpTime AsCtpTime { get; }
        public abstract bool AsBoolean { get; }

        public abstract Guid AsGuid { get; }

        public abstract string AsString { get; }
        public abstract CtpBuffer AsSttpBuffer { get; }
        public abstract CtpDocument AsDocument { get; }
        public abstract object ToNativeType { get; }
        public abstract string ToTypeString { get; }

        public sbyte AsSByte
        {
            get
            {
                checked
                {
                    return (sbyte)AsInt32;
                }
            }
        }

        public ulong AsUInt64
        {
            get
            {
                checked
                {
                    return (ulong)AsInt64;
                }
            }
        }

        public int AsInt32
        {
            get
            {
                checked
                {
                    return (int)AsInt64;
                }
            }
        }

        public short AsInt16
        {
            get
            {
                checked
                {
                    return (short)AsInt32;
                }
            }
        }

        public byte AsByte
        {
            get
            {
                checked
                {
                    return (byte)AsInt32;
                }
            }
        }

        public ushort AsUInt16
        {
            get
            {
                checked
                {
                    return (ushort)AsInt32;
                }
            }
        }

        public uint AsUInt32
        {
            get
            {
                checked
                {
                    return (uint)AsInt64;
                }
            }
        }

        public DateTime AsDateTime => AsCtpTime.AsDateTime;

        /// <summary>
        /// Gets if this class has a value. Clear this by setting a value.
        /// </summary>
        public bool IsNull => ValueTypeCode == CtpTypeCode.Null;

        /// <summary>
        /// The type code of the raw value.
        /// </summary>
        public abstract CtpTypeCode ValueTypeCode { get; }

        public bool IsImmutable => !(this is CtpValueMutable);

        /// <summary>
        /// Clones the value
        /// </summary>
        /// <returns></returns>
        public CtpValue Clone()
        {
            if (IsImmutable)
                return this;
            return ((CtpValueMutable)this).CloneAsImmutable();
        }

        //public static bool operator ==(SttpValue a, SttpValue b)
        //{
        //    if (ReferenceEquals(a, b))
        //        return true;
        //    if (ReferenceEquals(a, null))
        //        return false;
        //    if (ReferenceEquals(b, null))
        //        return false;
        //    if (a.ValueTypeCode != b.ValueTypeCode)
        //        return false;
        //    return true;
        //}

        //public static bool operator !=(SttpValue a, SttpValue b)
        //{
        //    return !(a == b);
        //}

        //public object ToNativeType(SttpValueTypeCode typeCode)
        //{
        //    throw new NotImplementedException();
        //}



        public override string ToString()
        {
            return ToTypeString;
        }

        public bool Equals(CtpValue other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            if (ValueTypeCode == other.ValueTypeCode)
            {
                switch (ValueTypeCode)
                {
                    case CtpTypeCode.Null:
                        return true; //Null == Null, should that return false?
                    case CtpTypeCode.Int64:
                        return AsInt64 == other.AsInt64;
                    case CtpTypeCode.Single:
                        return AsSingle == other.AsSingle;
                    case CtpTypeCode.Double:
                        return AsDouble == other.AsDouble;
                    case CtpTypeCode.CtpTime:
                        return AsCtpTime == other.AsCtpTime;
                    case CtpTypeCode.Boolean:
                        return AsBoolean == other.AsBoolean;
                    case CtpTypeCode.Guid:
                        return AsGuid == other.AsGuid;
                    case CtpTypeCode.String:
                        return AsString == other.AsString;
                    case CtpTypeCode.CtpBuffer:
                        return AsSttpBuffer == other.AsSttpBuffer;
                    case CtpTypeCode.CtpDocument:
                        return AsDocument == other.AsDocument;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (ValueTypeCode == CtpTypeCode.Null || other.ValueTypeCode == CtpTypeCode.Null)
                return false;

            switch (ValueTypeCode)
            {
                case CtpTypeCode.Int64:
                    {
                        var value = AsInt64;
                        switch (other.ValueTypeCode)
                        {
                            case CtpTypeCode.Int64:
                                return value == other.AsInt64;
                            case CtpTypeCode.Single:
                                return value == other.AsSingle;
                            case CtpTypeCode.Double:
                                return value == other.AsDouble;
                        }
                        break;
                    }
                case CtpTypeCode.Single:
                    {
                        var value = AsSingle;
                        switch (other.ValueTypeCode)
                        {
                            case CtpTypeCode.Int64:
                                return value == other.AsInt64;
                            case CtpTypeCode.Single:
                                return value == other.AsSingle;
                            case CtpTypeCode.Double:
                                return value == other.AsDouble;
                        }
                        break;
                    }
                case CtpTypeCode.Double:
                    {
                        var value = AsDouble;
                        switch (other.ValueTypeCode)
                        {
                            case CtpTypeCode.Int64:
                                return value == other.AsInt64;
                            case CtpTypeCode.Single:
                                return value == other.AsSingle;
                            case CtpTypeCode.Double:
                                return value == other.AsDouble;
                        }
                        break;
                    }
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((CtpValue)obj);
        }

        //public abstract override int GetHashCode();

        //public void Save(PayloadWriter payloadWriter, bool includeTypeCode)
        //{
        //    throw new NotImplementedException();
        //}

        //public static SttpValue CreateBulkTransportGuid(Guid guid)
        //{
        //    return new SttpValueBulkTransportGuid(guid);
        //}

    }
}

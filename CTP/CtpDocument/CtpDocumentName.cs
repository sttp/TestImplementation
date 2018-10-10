using System;
using System.Collections.Generic;

namespace CTP
{
    public class CtpDocumentName : IEquatable<CtpDocumentName>
    {
        private static int s_runtimeID = 1;
        private static Dictionary<string, CtpDocumentName> s_names = new Dictionary<string, CtpDocumentName>();

        public static CtpDocumentName Lookup(byte[] buffer, int offset)
        {
            char[] data = new char[buffer[offset]];
            for (int x = 0; x < data.Length; x++)
            {
                data[x] = (char)buffer[offset + 1 + x];
                if (data[x] > 127)
                    throw new Exception("Not an ASCII string");
            }

            var str = new string(data);
            lock (s_names)
            {
                if (!s_names.TryGetValue(str, out var name))
                {
                    return new CtpDocumentName(str, -1);
                }
                return name;
            }
        }

        public static CtpDocumentName Create(string value)
        {
            lock (s_names)
            {
                if (!s_names.TryGetValue(value, out var name))
                {
                    name = new CtpDocumentName(value, s_runtimeID);
                    s_runtimeID++;
                    s_names[value] = name;
                }
                return name;
            }
        }

        internal readonly int RuntimeID;
        internal readonly byte[] TextWithPrefix;
        public readonly string Value;
        public readonly int HashCode;

        private CtpDocumentName(string value, int runtimeID)
        {
            RuntimeID = runtimeID;
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));
            if (value.Length > 255)
                throw new ArgumentException("Length cannot exceed 255 characters", nameof(value));
            TextWithPrefix = new byte[value.Length + 1];

            TextWithPrefix[0] = (byte)value.Length;
            for (var x = 0; x < value.Length; x++)
            {
                if ((ushort)value[x] > 127) //casting to ushort also takes care of negative numbers if they exist.
                    throw new ArgumentException("Not an ASCII string", nameof(value));
                TextWithPrefix[x + 1] = (byte)value[x];
            }
            Value = value;
            HashCode = value.GetHashCode();
        }

        public bool Equals(CtpDocumentName other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (RuntimeID >= 0 && other.RuntimeID >= 0)
                return RuntimeID == other.RuntimeID;
            if (HashCode != other.HashCode)
                return false;
            if (TextWithPrefix.Length != other.TextWithPrefix.Length)
                return false;
            for (int x = 0; x < TextWithPrefix.Length; x++)
            {
                if (TextWithPrefix[x] != other.TextWithPrefix[x])
                    return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((CtpDocumentName)obj);
        }

        public override int GetHashCode()
        {
            return HashCode;
        }

        public static bool operator ==(CtpDocumentName left, CtpDocumentName right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CtpDocumentName left, CtpDocumentName right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}

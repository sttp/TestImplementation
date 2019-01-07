using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace CTP
{
    /// <summary>
    /// This is a helper class to speed up the serialization of <see cref="CommandObject"/>. It does this by
    /// assigning runtime IDs to all KeyWords and pooling string objects.
    /// </summary>
    internal class CtpCommandKeyword : IEquatable<CtpCommandKeyword>
    {
        internal readonly int RuntimeID;
        internal readonly byte[] TextWithPrefix;
        public readonly string Value;
        public readonly int HashCode;

        private CtpCommandKeyword(string value, int runtimeID)
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


        private bool Equals(byte[] buffer, int offset)
        {
            if (buffer[offset] == TextWithPrefix[0])
            {
                for (int x = 1; x < TextWithPrefix.Length; x++)
                {
                    if (TextWithPrefix[x] != buffer[offset + x])
                        return false;
                }
                return true;
            }
            return false;
        }

        public bool Equals(CtpCommandKeyword other)
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
            return Equals((CtpCommandKeyword)obj);
        }

        public override int GetHashCode()
        {
            return HashCode;
        }

        public override string ToString()
        {
            return Value;
        }

        private static int s_runtimeID = 1;
        private static Dictionary<string, CtpCommandKeyword> s_names = new Dictionary<string, CtpCommandKeyword>();
        private static List<CtpCommandKeyword>[] m_names = new List<CtpCommandKeyword>[1024];

        private static byte[] m_hash1 = new byte[256];
        private static byte[] m_hash2 = new byte[256];
        private static byte[] m_hash3 = new byte[256];
        private static byte[] m_hash4 = new byte[256];

        static CtpCommandKeyword()
        {
            //This is used to effectively hash a name.
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(m_hash1);
            rng.GetBytes(m_hash2);
            rng.GetBytes(m_hash3);
            rng.GetBytes(m_hash4);
        }

        private static int ComputeHash(byte[] buffer, int offset)
        {
            int len = buffer[offset];
            int hash = m_hash1[len];
            if (len > 0)
                hash ^= m_hash1[buffer[offset + 1]] << 2;
            if (len > 1)
                hash ^= m_hash2[buffer[offset + 2]];
            if (len > 2)
                hash ^= m_hash2[buffer[offset + 3]] << 2;
            if (len > 3)
                hash ^= m_hash3[buffer[offset + 4]];
            if (len > 4)
                hash ^= m_hash3[buffer[offset + 5]] << 2;
            if (len > 5)
                hash ^= m_hash4[buffer[offset + 6]];
            if (len > 6)
                hash ^= m_hash4[buffer[offset + 7]] << 2;

            return hash;
        }

        private static int ComputeHash(string buffer)
        {
            int len = buffer.Length;
            int hash = m_hash1[(byte)len];
            if (len > 0)
                hash ^= m_hash1[(byte)buffer[0]] << 2;
            if (len > 1)
                hash ^= m_hash2[(byte)buffer[1]];
            if (len > 2)
                hash ^= m_hash2[(byte)buffer[2]] << 2;
            if (len > 3)
                hash ^= m_hash3[(byte)buffer[3]];
            if (len > 4)
                hash ^= m_hash3[(byte)buffer[4]] << 2;
            if (len > 5)
                hash ^= m_hash4[(byte)buffer[5]];
            if (len > 6)
                hash ^= m_hash4[(byte)buffer[6]] << 2;

            return hash;
        }

        public static bool operator ==(CtpCommandKeyword left, CtpCommandKeyword right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CtpCommandKeyword left, CtpCommandKeyword right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// Returns a <see cref="CtpCommandKeyword"/> for a ASCIIString from the local lookup table.
        /// If the string does not exist, one is created, but assigned a -1 runtime ID.
        /// </summary>
        /// <param name="buffer">The first index of the ASCIIString, This is the 1 byte length header</param>
        /// <param name="offset">The offset in <see cref="buffer"/></param>
        /// <returns></returns>
        public static CtpCommandKeyword Lookup(byte[] buffer, int offset)
        {
            lock (s_names)
            {
                var lists = m_names[ComputeHash(buffer, offset)];
                if (lists != null)
                {
                    foreach (var item in lists)
                    {
                        if (item.Equals(buffer, offset))
                        {
                            return item;
                        }
                    }
                }
            }

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
                    return new CtpCommandKeyword(str, -1);
                }
                return name;
            }
        }

        /// <summary>
        /// Creates a <see cref="CtpCommandKeyword"/> using the supplied string. First a lookup is attempted. If the lookup fails,
        /// a new record is added to the internal list of all <see cref="CtpCommandKeyword"/>s.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CtpCommandKeyword Create(string value)
        {
            lock (s_names)
            {
                if (!s_names.TryGetValue(value, out var name))
                {
                    name = new CtpCommandKeyword(value, s_runtimeID);
                    s_runtimeID++;
                    s_names[value] = name;
                    var sum = ComputeHash(value);
                    if (m_names[sum] == null)
                    {
                        m_names[sum] = new List<CtpCommandKeyword>();
                    }

                    m_names[sum].Add(name);

                }
                return name;
            }
        }
    }
}

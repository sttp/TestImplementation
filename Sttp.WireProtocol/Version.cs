using System;

namespace Sttp.WireProtocol
{
    public class Version : IEquatable<Version>
    {
        public readonly byte Major;
        public readonly byte Minor;

        public Version(byte major, byte minor)
        {
            Major = major;
            Minor = minor;
        }

        public byte[] Encode()
        {
            return new[] { Major, Minor };
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Version other)
        {
            return (object)other != null && Major == other.Major && Minor == other.Minor;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns><c>true</c> if the specified object  is equal to the current object; otherwise, <c>false</c>.</returns>
        /// <param name="obj">The object to compare with the current object.</param>
        public override bool Equals(object obj)
        {
            return Equals(obj as Version);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            int hashcode = 23;

            hashcode = hashcode * 37 + Major;
            hashcode = hashcode * 37 + Minor;

            return hashcode;
        }

        public static Version Decode(byte[] buffer, int startIndex, int length)
        {
            buffer.ValidateParameters(startIndex, length);

            // TODO: Do more buffer length validation!

            return new Version(buffer[startIndex], buffer[startIndex + 1]);
        }

        public static bool operator==(Version left, Version right)
        {
            if ((object)left == null && (object)right == null)
                return true;

            return left?.Equals(right) ?? false;
        }

        public static bool operator!=(Version left, Version right)
        {
            return !(left == right);
        }
    }
}

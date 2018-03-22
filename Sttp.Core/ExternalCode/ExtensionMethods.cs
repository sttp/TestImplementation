using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Sttp
{
    /// <summary>
    /// Defines extension functions related to <see cref="Array"/> manipulation.
    /// </summary>
    internal static unsafe class ExtensionMethods
    {
        /// <summary>
        /// Validates that the specified <paramref name="startIndex"/> and <paramref name="length"/> are valid within the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">Array to validate.</param>
        /// <param name="startIndex">0-based start index into the <paramref name="array"/>.</param>
        /// <param name="length">Valid number of items within <paramref name="array"/> from <paramref name="startIndex"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="startIndex"/> or <paramref name="length"/> is less than 0 -or- 
        /// <paramref name="startIndex"/> and <paramref name="length"/> will exceed <paramref name="array"/> length.
        /// </exception>
        public static void ValidateParameters<T>(this T[] array, int startIndex, int length)
        {
            if ((object)array == null || startIndex < 0 || length < 0 || startIndex + length > array.Length)
                RaiseValidationError(array, startIndex, length);
        }

        // This method will raise the actual error - this is needed since .NET will not inline anything that might throw an exception
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RaiseValidationError<T>(T[] array, int startIndex, int length)
        {
            if ((object)array == null)
                throw new ArgumentNullException(nameof(array));

            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), "cannot be negative");

            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), "cannot be negative");

            if (startIndex + length > array.Length)
                throw new ArgumentOutOfRangeException(nameof(length), $"startIndex of {startIndex} and length of {length} will exceed array size of {array.Length}");
        }

        #region [ Big Endian Based Encoding (ie RFC 4122) ]

        /// <summary>
        /// Encodes a <see cref="Guid"/> following RFC 4122.
        /// </summary>
        /// <param name="guid">The <see cref="Guid"/> to serialize.</param>
        /// <param name="buffer">Destination buffer to hold serialized <paramref name="guid"/>.</param>
        /// <param name="startingIndex">Starting index in <paramref name="buffer"/>.</param>
        public static int ToRfcBytes(this Guid guid, byte[] buffer, int startingIndex)
        {
            // Since Microsoft is not very clear how Guid.ToByteArray() performs on big endian processors
            // we are assuming that the internal structure of a Guid will always be the same. Reviewing
            // mono source code the internal structure is also the same.
            buffer.ValidateParameters(startingIndex, 16);

            byte* src = (byte*)&guid;

            fixed (byte* dst = &buffer[startingIndex])
            {
                if (BitConverter.IsLittleEndian)
                {
                    // Guid._a (int)
                    dst[0] = src[3];
                    dst[1] = src[2];
                    dst[2] = src[1];
                    dst[3] = src[0];

                    // Guid._b (short)
                    dst[4] = src[5];
                    dst[5] = src[4];

                    // Guid._c (short)
                    dst[6] = src[7];
                    dst[7] = src[6];

                    // Guid._d - Guid._k (8 bytes)
                    // Since already encoded as big endian, just copy the data
                    *(long*)(dst + 8) = *(long*)(src + 8);
                }
                else
                {
                    // All fields are encoded big-endian, just copy
                    *(long*)(dst + 0) = *(long*)(src + 0);
                    *(long*)(dst + 8) = *(long*)(src + 8);
                }
            }
            return 16;
        }

        /// <summary>
        /// Encodes a <see cref="Guid"/> following RFC 4122.
        /// </summary>
        /// <param name="guid"><see cref="Guid"/> to serialize.</param>
        /// <returns>A <see cref="byte"/> array that represents a big-endian encoded <see cref="Guid"/>.</returns>
        public static byte[] ToRfcBytes(this Guid guid)
        {
            byte[] rv = new byte[16];
            guid.ToRfcBytes(rv, 0);
            return rv;
        }

        /// <summary>
        /// Decodes a <see cref="Guid"/> following RFC 4122
        /// </summary>
        /// <param name="buffer">Buffer containing a serialized big-endian encoded <see cref="Guid"/>.</param>
        /// <param name="startingIndex">Starting index in <paramref name="buffer"/>.</param>
        /// <returns><see cref="Guid"/> deserialized from <paramref name="buffer"/>.</returns>
        public static Guid ToRfcGuid(this byte[] buffer, int startingIndex)
        {
            buffer.ValidateParameters(startingIndex, 16);

            // Since Microsoft is not very clear how Guid.ToByteArray() performs on big endian processors
            // we are assuming that the internal structure of a Guid will always be the same. Reviewing
            // mono source code the internal structure is also the same.
            Guid rv;
            byte* dst = (byte*)&rv;

            fixed (byte* src = &buffer[startingIndex])
            {
                if (BitConverter.IsLittleEndian)
                {
                    // Guid._a (int)
                    dst[0] = src[3];
                    dst[1] = src[2];
                    dst[2] = src[1];
                    dst[3] = src[0];

                    // Guid._b (short)
                    dst[4] = src[5];
                    dst[5] = src[4];

                    // Guid._c (short)
                    dst[6] = src[7];
                    dst[7] = src[6];

                    // Guid._d - Guid._k (8 bytes)
                    // Since already encoded as big endian, just copy the data
                    *(long*)(dst + 8) = *(long*)(src + 8);
                }
                else
                {
                    // All fields are encoded big-endian, just copy
                    *(long*)(dst + 0) = *(long*)(src + 0);
                    *(long*)(dst + 8) = *(long*)(src + 8);
                }

                return rv;
            }
        }

        /// <summary>
        /// Reads all of the provided bytes. Will not return prematurely, 
        /// but continue to execute a <see cref="Stream.Read"/> command until the entire
        /// <paramref name="length"/> has been read.
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <param name="buffer">The buffer to write to</param>
        /// <param name="position">the start position in the <paramref name="buffer"/></param>
        /// <param name="length">the number of bytes to read</param>
        /// <exception cref="EndOfStreamException">occurs if the end of the stream has been reached.</exception>
        public static void ReadAll(this Stream stream, byte[] buffer, int position, int length)
        {
            buffer.ValidateParameters(position, length);
            while (length > 0)
            {
                int bytesRead = stream.Read(buffer, position, length);
                if (bytesRead == 0)
                    throw new EndOfStreamException();
                length -= bytesRead;
                position += bytesRead;
            }
        }

        #endregion
    }
}

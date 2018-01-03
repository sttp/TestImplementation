using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Sttp
{
    /// <summary>
    /// Defines extension functions related to <see cref="Array"/> manipulation.
    /// </summary>
    internal static unsafe class ArrayExtensions
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

        
    }
}

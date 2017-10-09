using System;

namespace Sttp.WireProtocol
{
    public static class uint15
    {
        /// <summary>
        /// Reads a 15 bit encoded integer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="position"></param>
        /// <param name="messageLength"></param>
        /// <returns></returns>
        public static int Read(byte[] buffer, int position, out int messageLength)
        {
            if (position >= buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(position), "Exceeded array bounds.");

            if (buffer[position] < 128)
            {
                messageLength = 1;
                return buffer[position];
            }

            if (position + 1 >= buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(position), "Exceeded array bounds.");

            messageLength = 2;
            int value = buffer[position];
            value ^= (short)(buffer[position + 1] << 7);
            return (short)(value ^ 0x80);
        }

        /// <summary>
        /// Writes the specified <see cref="value"/> into the <see cref="buffer"/>, encoding it as an int15
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="position"></param>
        /// <param name="value">must be a positive 15 bit number</param>
        /// <returns>the number of bytes it took to encode this value. 1 or 2</returns>
        public static int Write(byte[] buffer, int position, int value)
        {
            if (position >= buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(position), "Exceeded array bounds.");

            if (value < 0 || value > short.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(value), "Must be between 0 and 32767");
            if (value < 128)
            {
                buffer[position] = (byte)value;
                return 1;
            }

            if (position + 1 >= buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(position), "Exceeded array bounds.");

            buffer[position] = (byte)(value | 128);
            buffer[position + 1] = (byte)(value >> 7);
            return 2;
        }
    }
}
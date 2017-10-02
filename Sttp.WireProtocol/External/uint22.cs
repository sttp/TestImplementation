using System;

namespace Sttp.WireProtocol
{
    public static class uint22
    {
        /// <summary>
        /// Reads a 15 bit encoded integer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="bufferPosition"></param>
        /// <param name="messageLength"></param>
        /// <returns></returns>
        public static int Read(byte[] buffer, int bufferPosition, out int messageLength)
        {
            if (buffer[0] < 128)
            {
                messageLength = 1;
                return buffer[bufferPosition];
            }
            messageLength = 2;
            return (buffer[bufferPosition] - 128) | (buffer[bufferPosition + 1] << 7);
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
            if (value < 0 || value > short.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(value), "Must be between 0 and 32767");
            if (value < 128)
            {
                buffer[position] = (byte)value;
                return 1;
            }
            buffer[position] = (byte)(value & 127);
            buffer[position + 1] = (byte)(value >> 7);
            return 2;
        }
    }
}
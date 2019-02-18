using System;
using System.Collections.Generic;
using GSF;

namespace CTP
{
    /// <summary>
    /// A user friendly means of writing a <see cref="CtpCommand"/>.
    /// </summary>
    internal class CtpCommandWriter
    {
        /// <summary>
        /// Where to write the data.
        /// </summary>
        private CtpObjectWriter m_stream;
        private CtpCommandSchema m_schema;

        public CtpCommandWriter(CtpCommandSchema schema)
        {
            m_schema = schema;
            m_stream = new CtpObjectWriter();
        }

        /// <summary>
        /// The size of the writer if all elements were closed and the data was serialized.
        /// </summary>
        public int Length
        {
            get
            {
                var innerLength = 2 + m_stream.Length;
                if (innerLength > 4093)
                    return innerLength + 2;
                return innerLength;
            }
        }

        public void WriteArray(int count)
        {
            m_stream.Write(count);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="value">the value</param>
        public void WriteValue(CtpObject value)
        {
            m_stream.Write(value);
        }

        /// <summary>
        /// Completes the writing to an <see cref="CtpCommand"/> and returns the completed buffer. This may be called multiple times.
        /// </summary>
        /// <returns></returns>
        public CtpCommand ToCtpCommand()
        {
            byte[] rv = new byte[Length];
            CopyTo(rv, 0);
            return CtpCommand.Load(rv, false, m_schema);
        }

        /// <summary>
        /// Copies the contest of the current document writer to the specified stream.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        public void CopyTo(byte[] buffer, int offset)
        {
            var length = Length;
            buffer.ValidateParameters(offset, length);

            if (length <= 4095)
            {
                //This is a 2 byte header;
                WriteSize(buffer, ref offset, (ushort)length);
            }
            else
            {
                //This is a 4 byte header;
                WriteSize(buffer, ref offset, (uint)(length + (1 << 28)));
            }
            m_stream.CopyTo(buffer, offset);
        }

        private void WriteSize(byte[] buffer, ref int length, ushort value)
        {
            buffer[length] = (byte)(value >> 8);
            length++;
            buffer[length] = (byte)(value);
            length++;
        }

        private void WriteSize(byte[] buffer, ref int length, uint value)
        {
            buffer[length] = (byte)(value >> 24);
            length++;
            buffer[length] = (byte)(value >> 16);
            length++;
            buffer[length] = (byte)(value >> 8);
            length++;
            buffer[length] = (byte)(value);
            length++;
        }

    }
}
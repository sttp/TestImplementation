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
            return new CtpCommand(m_schema, m_stream.ToArray());
        }

    }
}
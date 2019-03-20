using System;
using System.Threading;

namespace CTP
{
    internal class CommandSchemaWriter
    {
        private static int ProcessRuntimeID;

        private CtpObjectWriter m_stream;

        public CommandSchemaWriter()
        {
            m_stream = new CtpObjectWriter();
        }

        public void StartArray(string name)
        {
            m_stream.Write((byte)CommandSchemaSymbol.StartArray);
            m_stream.Write(name);
        }

        public void StartElement(string name)
        {
            m_stream.Write((byte)CommandSchemaSymbol.StartElement);
            m_stream.Write(name);
        }

        public void EndElement()
        {
            m_stream.Write((byte)CommandSchemaSymbol.EndElement);
        }

        public void EndArray()
        {
            m_stream.Write((byte)CommandSchemaSymbol.EndArray);
        }

        public void Value(string name)
        {
            m_stream.Write((byte)CommandSchemaSymbol.Value);
            m_stream.Write(name);
        }

        public CtpCommandSchema ToSchema()
        {
            if (m_stream == null)
                throw new Exception("Duplicate calls are not supported.");
            var data = m_stream.ToArray();
            m_stream = null;
            return new CtpCommandSchema(data, Interlocked.Increment(ref ProcessRuntimeID));
        }
    }
}
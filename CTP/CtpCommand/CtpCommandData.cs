using System;

namespace CTP
{
    public class CtpCommandData
    {
        private byte[] m_data;
        public readonly Guid Identifier;

        public CtpCommandData(Guid identifier, byte[] data)
        {
            Identifier = identifier;
            m_data = data;
        }
    }
}
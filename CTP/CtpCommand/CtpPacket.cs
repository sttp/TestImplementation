using System;

namespace CTP
{
    public class CtpPacket
    {
        public readonly PacketContents Contents;
        private byte[] m_data;
        private readonly Guid Identifier;
        private readonly byte RawID;

        internal CtpPacket(PacketContents contents, byte[] data, Guid identifier)
        {
            Contents = contents;
            m_data = data;
            Identifier = identifier;
            RawID = 0;
        }

        internal CtpPacket(PacketContents contents, byte[] data, byte rawID)
        {
            Contents = contents;
            m_data = data;
            RawID = rawID;
            Identifier = Guid.Empty;
        }

        public CtpRaw GetRaw()
        {
            checked
            {
                return new CtpRaw(m_data, (byte)RawID);
            }
        }

        public CtpCommandSchema GetSchema()
        {
            if (Contents != PacketContents.CommandSchema)
                throw new InvalidCastException();
            return new CtpCommandSchema(Identifier, m_data);
        }

        public CtpCommandData GetData()
        {
            if (Contents != PacketContents.CommandData)
                throw new InvalidCastException();
            return new CtpCommandData(Identifier, m_data);
        }

        public CtpCommand GetCommand(CtpCommandSchema schema)
        {
            if (Contents != PacketContents.CommandData)
                throw new InvalidCastException();
            return new CtpCommand(m_data, schema);
        }
    }
}
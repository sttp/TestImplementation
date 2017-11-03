using System;

namespace Sttp.WireProtocol
{
    public abstract class BaseEncoder
    {
        public abstract CommandCode Code { get; }

        protected PacketWriter Stream;

        protected Action<byte[], int, int> m_sendPacket;

        protected BaseEncoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
        {
            Stream = new PacketWriter(sessionDetails);
            m_sendPacket = sendPacket;
        }

        /// <summary>
        /// Begins a new metadata packet
        /// </summary>
        public void BeginCommand()
        {
            Stream.BeginCommand(Code);
        }

        /// <summary>
        /// Ends a metadata packet and requests the buffer block that was allocated.
        /// </summary>
        /// <returns></returns>
        public void EndCommand()
        {
            Stream.EndCommand(m_sendPacket);
        }
    }
}
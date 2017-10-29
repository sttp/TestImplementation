using System;

namespace Sttp.WireProtocol
{
    public abstract class BaseEncoder
    {
        public abstract CommandCode Code { get; }

        internal PacketWriter m_stream;

        protected Action<byte[], int, int> m_sendPacket;

        protected BaseEncoder(Action<byte[], int, int> sendPacket)
        {
            m_stream = new PacketWriter();
            m_sendPacket = sendPacket;
        }

        /// <summary>
        /// Begins a new metadata packet
        /// </summary>
        public void BeginCommand()
        {
            m_stream.BeginCommand(Code, false, 1500);
        }

        /// <summary>
        /// Ends a metadata packet and requests the buffer block that was allocated.
        /// </summary>
        /// <returns></returns>
        public void EndCommand()
        {
            m_stream.EndCommand(m_sendPacket);
        }
    }
}
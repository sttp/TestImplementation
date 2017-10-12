using System;

namespace Sttp.WireProtocol.Data.Raw
{
    public abstract class BaseEncoder
    {
        public abstract CommandCode Code { get; }

        internal StreamWriter m_stream;

        protected Action<byte[], int, int> m_sendPacket;

        protected BaseEncoder(Action<byte[], int, int> sendPacket, ushort streamSize = 512)
        {
            m_stream = new StreamWriter(streamSize);
            m_sendPacket = sendPacket;
        }

        /// <summary>
        /// Begins a new metadata packet
        /// </summary>
        public void BeginCommand()
        {
            m_stream.Clear();
            m_stream.Write(Code);
            m_stream.Write((ushort)0); //Packet Length
        }

        /// <summary>
        /// Ends a metadata packet and requests the buffer block that was allocated.
        /// </summary>
        /// <returns></returns>
        public void EndCommand()
        {
            if (m_stream.Position <= 3) //3 bytes means nothing will be sent.
                return;
            int length = m_stream.Length;
            m_stream.Position = 1;
            m_stream.Write((ushort)length);
            m_sendPacket(m_stream.Buffer, 0, length);
        }
    }
}
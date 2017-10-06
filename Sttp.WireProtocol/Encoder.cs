using System;
using Sttp.WireProtocol.Codec.DataPointPacket;
using Sttp.WireProtocol.Data.Raw;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Responsible for encoding each command into bytes
    /// </summary>
    public class Encoder
    {

        /// <summary>
        /// Once this size has been reached, the protocol will automatically call 
        /// </summary>
        private int m_autoFlushPacketSize;

        /// <summary>
        /// Occurs when a packet of data must be sent on the wire. This is called immediately
        /// after completing a Packet;
        /// </summary>
        public event Action<byte[], int, int> NewPacket;

        private MetadataEncoder m_metadata;

        private DataPointEncoder m_dataPoint;

        private SubscriptionEncoder m_subscription;

        private NegotiateSessionEncoder m_negotiateSession;

        private CommandCode m_lastCode;

        /// <summary>
        /// The desired number of bytes before data is automatically flushed via <see cref="NewPacket"/>
        /// </summary>
        /// <param name="autoflushPacketSize"></param>
        public Encoder(int autoflushPacketSize)
        {
            m_lastCode = CommandCode.Invalid;
            m_autoFlushPacketSize = autoflushPacketSize;
            //m_subscription = new SubscriptionEncoder(SendPacket);
            //m_dataPoint = new DataPointEncoder(SendPacket);
            //m_negotiateSession = new NegotiateSessionEncoder(SendPacket);
            m_metadata = new MetadataEncoder(SendNewPacket, m_autoFlushPacketSize);
        }

        private void SendNewPacket(byte[] arg1, int arg2, int arg3)
        {
            NewPacket?.Invoke(arg1, arg2, arg3);
        }

        public MetadataEncoder BeginMetadataPacket()
        {
            if (m_lastCode != CommandCode.Metadata)
            {
                EndPacket();
                m_lastCode = CommandCode.Metadata;
            }
            m_metadata.BeginCommand();
            return m_metadata;
        }

        public void Flush()
        {
            EndPacket();
        }

        public void EndPacket()
        {
            switch (m_lastCode)
            {
                case CommandCode.Invalid:
                    return;
                case CommandCode.Metadata:
                    m_metadata.EndCommand();
                    return;
                default:
                    throw new NotImplementedException("This command was not coded.");
            }
        }



    }
}

using System;
using Sttp.WireProtocol.Codec.DataPointPacket;
using Sttp.WireProtocol.Data;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Responsible for encoding each command into bytes
    /// </summary>
    public class WireEncoder
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

        private BulkTransportEncoder m_bulkEncoder;

        private CommandCode m_lastCode;

        private SessionDetails m_sessionDetails;

        /// <summary>
        /// The desired number of bytes before data is automatically flushed via <see cref="NewPacket"/>
        /// </summary>
        /// <param name="autoflushPacketSize"></param>
        public WireEncoder(int autoflushPacketSize)
        {
            m_sessionDetails = new SessionDetails();
            m_lastCode = CommandCode.Invalid;
            m_autoFlushPacketSize = autoflushPacketSize;
            //m_subscription = new SubscriptionEncoder(SendPacket);
            //m_dataPoint = new DataPointEncoder(SendPacket);
            //m_negotiateSession = new NegotiateSessionEncoder(SendPacket);
            m_metadata = new MetadataEncoder(SendNewPacket, m_sessionDetails);
            m_bulkEncoder = new BulkTransportEncoder(SendNewPacket, m_sessionDetails);
        }

        private void SendNewPacket(byte[] buffer, int position, int length)
        {
            NewPacket?.Invoke(buffer, position, length);
        }

        public BulkTransportEncoder BeginBulkTransferPacket()
        {
            if (m_lastCode != CommandCode.BulkTransport)
            {
                EndPacket();
                m_lastCode = CommandCode.BulkTransport;
            }

            return m_bulkEncoder;
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
                case CommandCode.BulkTransport:
                    return;
                default:
                    throw new NotImplementedException("This command was not coded.");
            }
        }



    }
}

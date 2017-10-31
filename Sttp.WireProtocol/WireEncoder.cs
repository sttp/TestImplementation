using System;
using Sttp.WireProtocol.SendDataPoints;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Responsible for encoding each command into bytes
    /// </summary>
    public class WireEncoder
    {
        /// <summary>
        /// Occurs when a packet of data must be sent on the wire. This is called immediately
        /// after completing a Packet;
        /// </summary>
        public event Action<byte[], int, int> NewPacket;

        //private DataPointEncoder m_dataPoint;

        private SubscriptionEncoder m_subscription;

        private NegotiateSessionEncoder m_negotiateSession;

        private BulkTransportEncoder m_bulkEncoder;

        private CommandCode m_lastCode;

        private SessionDetails m_sessionDetails;

        private GetMetadataSchema.Encoder m_getMetadataSchema;
        private GetMetadataSchemaResponse.Encoder m_getMetadataSchemaResponse;
        private GetMetadata.Encoder m_getMetadata;
        private GetMetadataResponse.Encoder m_getMetadataResponse;


        /// <summary>
        /// The desired number of bytes before data is automatically flushed via <see cref="NewPacket"/>
        /// </summary>
        public WireEncoder()
        {
            m_sessionDetails = new SessionDetails();
            m_lastCode = CommandCode.Invalid;
            m_getMetadataSchema = new GetMetadataSchema.Encoder(SendNewPacket, m_sessionDetails);
            m_getMetadataSchemaResponse = new GetMetadataSchemaResponse.Encoder(SendNewPacket, m_sessionDetails);
            m_getMetadata = new GetMetadata.Encoder(SendNewPacket, m_sessionDetails);
            m_getMetadataResponse = new GetMetadataResponse.Encoder(SendNewPacket, m_sessionDetails);

            //m_subscription = new SubscriptionEncoder(SendPacket);
            //m_dataPoint = new DataPointEncoder(SendPacket);
            //m_negotiateSession = new NegotiateSessionEncoder(SendPacket);
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
                m_lastCode = CommandCode.BulkTransport;
            }

            return m_bulkEncoder;
        }

        public GetMetadataSchema.Encoder GetMetadataSchema()
        {
            return m_getMetadataSchema;
        }
        public GetMetadataSchemaResponse.Encoder GetMetadataSchemaResponse()
        {
            return m_getMetadataSchemaResponse;
        }
        public GetMetadata.Encoder GetMetadata()
        {
            return m_getMetadata;
        }
        public GetMetadataResponse.Encoder GetMetadataResponse()
        {
            return m_getMetadataResponse;
        }

        public void Flush()
        {
        }


    }
}

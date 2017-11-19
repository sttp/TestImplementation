using System;

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

        private CommandCode m_lastCode;

        private SessionDetails m_sessionDetails;

        public MetadataSchema.Encoder GetMetadataSchemaResponse;
        public GetMetadata.Encoder GetMetadata;
        public Metadata.Encoder GetMetadataResponse;
        public Subscription.Encoder Subscribe;
        public SendDataPoints.Encoder SendDataPoints;
        public NegotiateSession.Encoder NegotiateSession;
        public NegotiateSessionResponse.Encoder NegotiateSessionResponse;
        public RequestFailed.Encoder RequestFailed;
        public RequestSucceeded.Encoder RequestSucceeded;
        public BulkTransport.Encoder BulkTransport;
        public RuntimeIDMapping.Encoder RegisterDataPointRuntimeIdentifier;
        public SendComplete.Encoder CompletedSendingDataPoints;

        /// <summary>
        /// The desired number of bytes before data is automatically flushed via <see cref="NewPacket"/>
        /// </summary>
        public WireEncoder()
        {
            m_sessionDetails = new SessionDetails();
            m_lastCode = CommandCode.Invalid;

            GetMetadataSchemaResponse = new MetadataSchema.Encoder(SendNewPacket, m_sessionDetails);
            GetMetadata = new GetMetadata.Encoder(SendNewPacket, m_sessionDetails);
            GetMetadataResponse = new Metadata.Encoder(SendNewPacket, m_sessionDetails);
            Subscribe = new Subscription.Encoder(SendNewPacket, m_sessionDetails);
            SendDataPoints = new SendDataPoints.Encoder(SendNewPacket, m_sessionDetails);
            NegotiateSession = new NegotiateSession.Encoder(SendNewPacket, m_sessionDetails);
            NegotiateSessionResponse = new NegotiateSessionResponse.Encoder(SendNewPacket, m_sessionDetails);
            RequestFailed = new RequestFailed.Encoder(SendNewPacket, m_sessionDetails);
            RequestSucceeded = new RequestSucceeded.Encoder(SendNewPacket, m_sessionDetails);
            BulkTransport = new BulkTransport.Encoder(SendNewPacket, m_sessionDetails);
            RegisterDataPointRuntimeIdentifier = new RuntimeIDMapping.Encoder(SendNewPacket, m_sessionDetails);
            CompletedSendingDataPoints = new SendComplete.Encoder(SendNewPacket, m_sessionDetails);
        }

        private void SendNewPacket(byte[] buffer, int position, int length)
        {
            NewPacket?.Invoke(buffer, position, length);
        }

       
        public void Flush()
        {
        }


    }
}

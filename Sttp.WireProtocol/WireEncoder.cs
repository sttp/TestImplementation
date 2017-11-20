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

        public GetMetadata.Encoder GetMetadata;
        public Metadata.Encoder GetMetadataResponse;
        public Subscription.Encoder Subscribe;
        public NegotiateSession.Encoder NegotiateSession;
        public RequestFailed.Encoder RequestFailed;
        public RequestSucceeded.Encoder RequestSucceeded;
        public BulkTransportBeginSend.Encoder BulkTransportBeginSend;
        public BulkTransportCancelSend.Encoder BulkTransportCancelSend;
        public BulkTransportSendFragment.Encoder BulkTransportSendFragment;
        public MapRuntimeIDs.Encoder RegisterDataPointRuntimeIdentifier;
        private CommandEncoder m_encoder;

        /// <summary>
        /// The desired number of bytes before data is automatically flushed via <see cref="NewPacket"/>
        /// </summary>
        public WireEncoder()
        {
            m_sessionDetails = new SessionDetails();
            m_encoder = new CommandEncoder(m_sessionDetails, SendNewPacket);
            m_lastCode = CommandCode.Invalid;

            GetMetadata = new GetMetadata.Encoder(m_encoder, m_sessionDetails);
            GetMetadataResponse = new Metadata.Encoder(m_encoder, m_sessionDetails);
            Subscribe = new Subscription.Encoder(m_encoder, m_sessionDetails);
            NegotiateSession = new NegotiateSession.Encoder(m_encoder, m_sessionDetails);
            RequestFailed = new RequestFailed.Encoder(m_encoder, m_sessionDetails);
            RequestSucceeded = new RequestSucceeded.Encoder(m_encoder, m_sessionDetails);
            BulkTransportBeginSend = new BulkTransportBeginSend.Encoder(m_encoder, m_sessionDetails);
            BulkTransportCancelSend = new BulkTransportCancelSend.Encoder(m_encoder, m_sessionDetails);
            BulkTransportSendFragment = new BulkTransportSendFragment.Encoder(m_encoder, m_sessionDetails);
            RegisterDataPointRuntimeIdentifier = new MapRuntimeIDs.Encoder(m_encoder, m_sessionDetails);
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

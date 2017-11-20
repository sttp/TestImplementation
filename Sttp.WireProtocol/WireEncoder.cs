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

        /// <summary>
        /// The desired number of bytes before data is automatically flushed via <see cref="NewPacket"/>
        /// </summary>
        public WireEncoder()
        {
            m_sessionDetails = new SessionDetails();
            m_lastCode = CommandCode.Invalid;

            GetMetadata = new GetMetadata.Encoder(SendNewPacket, m_sessionDetails);
            GetMetadataResponse = new Metadata.Encoder(SendNewPacket, m_sessionDetails);
            Subscribe = new Subscription.Encoder(SendNewPacket, m_sessionDetails);
            NegotiateSession = new NegotiateSession.Encoder(SendNewPacket, m_sessionDetails);
            RequestFailed = new RequestFailed.Encoder(SendNewPacket, m_sessionDetails);
            RequestSucceeded = new RequestSucceeded.Encoder(SendNewPacket, m_sessionDetails);
            BulkTransportBeginSend = new BulkTransportBeginSend.Encoder(SendNewPacket, m_sessionDetails);
            BulkTransportCancelSend = new BulkTransportCancelSend.Encoder(SendNewPacket, m_sessionDetails);
            BulkTransportSendFragment = new BulkTransportSendFragment.Encoder(SendNewPacket, m_sessionDetails);
            RegisterDataPointRuntimeIdentifier = new MapRuntimeIDs.Encoder(SendNewPacket, m_sessionDetails);
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

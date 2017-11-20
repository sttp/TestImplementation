namespace Sttp.WireProtocol
{
    public class CommandDecoder
    {
        private object m_decoder;
        public CommandCode CommandCode { get; }

        internal CommandDecoder(CommandCode code, object decoder)
        {
            m_decoder = decoder;
            CommandCode = code;
        }

        public GetMetadata.Decoder GetMetadata => m_decoder as GetMetadata.Decoder;
        public Metadata.Decoder Metadata => m_decoder as Metadata.Decoder;
        public Subscription.Decoder Subscription => m_decoder as Subscription.Decoder;
        public NegotiateSession.Decoder NegotiateSession => m_decoder as NegotiateSession.Decoder;
        public RequestFailed.Decoder RequestFailed => m_decoder as RequestFailed.Decoder;
        public RequestSucceeded.Decoder RequestSucceeded => m_decoder as RequestSucceeded.Decoder;
        public BulkTransport.Decoder BulkTransport => m_decoder as BulkTransport.Decoder;

    }
}
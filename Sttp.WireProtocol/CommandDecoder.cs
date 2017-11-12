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

        public GetMetadataSchema.Decoder GetMetadataSchema => m_decoder as GetMetadataSchema.Decoder;
        public GetMetadataSchemaResponse.Decoder GetMetadataSchemaResponse => m_decoder as GetMetadataSchemaResponse.Decoder;
        public GetMetadata.Decoder GetMetadata => m_decoder as GetMetadata.Decoder;
        public GetMetadataResponse.Decoder GetMetadataResponse => m_decoder as GetMetadataResponse.Decoder;
        public Subscribe.Decoder Subscribe => m_decoder as Subscribe.Decoder;
        public SendDataPoints.Decoder SendDataPoints => m_decoder as SendDataPoints.Decoder;
        public NegotiateSession.Decoder NegotiateSession => m_decoder as NegotiateSession.Decoder;
        public NegotiateSessionResponse.Decoder NegotiateSessionResponse => m_decoder as NegotiateSessionResponse.Decoder;
        public RequestFailed.Decoder RequestFailed => m_decoder as RequestFailed.Decoder;
        public RequestSucceeded.Decoder RequestSucceeded => m_decoder as RequestSucceeded.Decoder;
        public BulkTransport.Decoder BulkTransport => m_decoder as BulkTransport.Decoder;
        public CompletedSendingDataPoints.Decoder CompletedSendingDataPoints => m_decoder as CompletedSendingDataPoints.Decoder;

    }
}
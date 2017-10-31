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

    }
}
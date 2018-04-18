using System;
using CTP;

namespace Sttp.Codec
{
    public class CommandObjects
    {
        private object m_decoder;

        public CommandCode CommandCode { get; }
        public string CommandName { get; }
        private CtpDocument m_document;

        static CommandObjects()
        {
            DocumentCommandBase.Register("GetMetadataSchema", x => new CommandGetMetadataSchema(x));
            DocumentCommandBase.Register("MetadataSchema", x => new CommandMetadataSchema(x));
            DocumentCommandBase.Register("MetadataSchemaUpdate", x => new CommandMetadataSchemaUpdate(x));
            DocumentCommandBase.Register("MetadataSchemaVersion", x => new CommandMetadataSchemaVersion(x));

            DocumentCommandBase.Register("GetMetadata", x => new CommandGetMetadata(x));
            DocumentCommandBase.Register("MetadataRequestFailed", x => new CommandMetadataRequestFailed(x));
            DocumentCommandBase.Register("BeginMetadataResponse", x => new CommandBeginMetadataResponse(x));
            DocumentCommandBase.Register("EndMetadataResponse", x => new CommandEndMetadataResponse(x));
        }

        internal CommandObjects(CtpDecoder decoder)
        {
            CommandCode = decoder.CommandCode;

            switch (decoder.CommandCode)
            {
                case CommandCode.Invalid:
                    throw new ArgumentOutOfRangeException("Command code of 0 is not permitted");
                case CommandCode.Document:
                    m_document = decoder.DocumentPayload;
                    CommandName = m_document.MakeReader().RootElement;
                    m_decoder = DocumentCommandBase.Create(decoder.DocumentPayload);
                    break;
                case CommandCode.Binary:
                    m_document = null;
                    CommandName = "Raw";
                    m_decoder = new CommandRaw(decoder.BinaryChannelID, decoder.BinaryPayload);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public CommandGetMetadataSchema GetMetadataSchema => m_decoder as CommandGetMetadataSchema;
        public CommandMetadataSchema MetadataSchema => m_decoder as CommandMetadataSchema;
        public CommandMetadataSchemaUpdate MetadataSchemaUpdate => m_decoder as CommandMetadataSchemaUpdate;
        public CommandMetadataSchemaVersion MetadataSchemaVersion => m_decoder as CommandMetadataSchemaVersion;

        public CommandGetMetadata GetMetadata => m_decoder as CommandGetMetadata;
        public CommandMetadataRequestFailed MetadataRequestFailed => m_decoder as CommandMetadataRequestFailed;
        public CommandBeginMetadataResponse BeginMetadataResponse => m_decoder as CommandBeginMetadataResponse;
        public CommandEndMetadataResponse EndMetadataResponse => m_decoder as CommandEndMetadataResponse;

        public CommandRaw Raw => m_decoder as CommandRaw;
        public CommandUnknown Unknown => m_decoder as CommandUnknown;

        public string ToXMLString()
        {
            if (m_document == null)
                return "Null";

            return m_document.ToXML();
        }

        public CtpDocument Document => m_document;


    }
}
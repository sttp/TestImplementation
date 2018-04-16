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
            CommandBase.Register("GetMetadataSchema", x => new CommandGetMetadataSchema(x));
            CommandBase.Register("MetadataSchema", x => new CommandMetadataSchema(x));
            CommandBase.Register("MetadataSchemaUpdate", x => new CommandMetadataSchemaUpdate(x));
            CommandBase.Register("MetadataSchemaVersion", x => new CommandMetadataSchemaVersion(x));

            CommandBase.Register("GetMetadata", x => new CommandGetMetadata(x));
            CommandBase.Register("MetadataRequestFailed", x => new CommandMetadataRequestFailed(x));
            CommandBase.Register("BeginMetadataResponse", x => new CommandBeginMetadataResponse(x));
            CommandBase.Register("EndMetadataResponse", x => new CommandEndMetadataResponse(x));
        }

        internal CommandObjects(CommandDecoder decoder)
        {
            CommandCode = decoder.Command;

            switch (decoder.Command)
            {
                case CommandCode.Invalid:
                    throw new ArgumentOutOfRangeException("Command code of 0 is not permitted");
                case CommandCode.MarkupCommand:
                    m_document = decoder.DocumentPayload;
                    CommandName = m_document.MakeReader().RootElement;
                    m_decoder = CommandBase.Create(decoder.DocumentPayload);
                    break;
                case CommandCode.Raw:
                    m_document = null;
                    CommandName = "Raw";
                    m_decoder = new CommandRaw(decoder.RawChannelID, decoder.RawPayload);
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
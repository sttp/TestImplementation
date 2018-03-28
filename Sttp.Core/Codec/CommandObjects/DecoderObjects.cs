using System;

namespace Sttp.Codec
{
    public class CommandObjects
    {
        private object m_decoder;

        public CommandCode CommandCode { get; }
        public string CommandName { get; }
        private SttpMarkup m_markup;

        static CommandObjects()
        {
            CommandBase.Register("DataPointRequest", x => new CommandDataPointRequest(x));
            CommandBase.Register("DataPointResponseCompleted", x => new CommandDataPointResponseCompleted(x));
            CommandBase.Register("DataPointResponse", x => new CommandDataPointResponse(x));
            CommandBase.Register("GetMetadataBasic", x => new CommandGetMetadataBasic(x));
            CommandBase.Register("GetMetadataAdvance", x => new CommandGetMetadataAdvance(x));
            CommandBase.Register("GetMetadataProcedure", x => new CommandGetMetadataProcedure(x));
            CommandBase.Register("GetMetadataSchema", x => new CommandGetMetadataSchema(x));
            CommandBase.Register("Metadata", x => new CommandMetadata(x));
            CommandBase.Register("MetadataSchema", x => new CommandMetadataSchema(x));
            CommandBase.Register("MetadataSchemaVersion", x => new CommandMetadataSchemaVersion(x));
            CommandBase.Register("MetadataSchemaUpdate", x => new CommandMetadataSchemaUpdate(x));
            CommandBase.Register("MetadataVersionNotCompatible", x => new CommandMetadataVersionNotCompatible(x));
            CommandBase.Register("RequestFailed", x => new CommandRequestFailed(x));
            CommandBase.Register("RequestSucceeded", x => new CommandRequestSucceeded(x));
            CommandBase.Register("KeepAlive", x => new CommandKeepAlive(x));
            CommandBase.Register("Subscribe", x => new CommandConfigureSubscription(x));
        }

        internal CommandObjects(CommandDecoder decoder)
        {
            CommandCode = decoder.Command;

            switch (decoder.Command)
            {
                case CommandCode.Invalid:
                    throw new ArgumentOutOfRangeException("Command code of 0 is not permitted");
                case CommandCode.MarkupCommand:
                    m_markup = decoder.MarkupPayload;
                    CommandName = m_markup.MakeReader().RootElement;
                    m_decoder = CommandBase.Create(decoder.MarkupPayload);
                    break;
                case CommandCode.Raw:
                    m_markup = null;
                    CommandName = "Raw";
                    m_decoder = new CommandRaw(decoder.RawCommandCode, decoder.RawPayload);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public CommandKeepAlive KeepAlive => m_decoder as CommandKeepAlive;
        public CommandDataPointResponseCompleted DataPointResponseCompleted => m_decoder as CommandDataPointResponseCompleted;
        public CommandDataPointResponse DataPointResponse => m_decoder as CommandDataPointResponse;
        public CommandDataPointRequest DataPointRequest => m_decoder as CommandDataPointRequest;
        public CommandGetMetadataProcedure GetMetadataProcedure => m_decoder as CommandGetMetadataProcedure;
        public CommandGetMetadataBasic GetMetadataBasic => m_decoder as CommandGetMetadataBasic;
        public CommandGetMetadataAdvance GetMetadataAdvance => m_decoder as CommandGetMetadataAdvance;
        public CommandGetMetadataSchema GetMetadataSchema => m_decoder as CommandGetMetadataSchema;
        //public CommandMapRuntimeIDs MapRuntimeIDs => m_decoder as CommandMapRuntimeIDs;
        public CommandMetadata Metadata => m_decoder as CommandMetadata;
        public CommandMetadataSchema MetadataSchema => m_decoder as CommandMetadataSchema;
        public CommandMetadataSchemaVersion MetadataSchemaVersion => m_decoder as CommandMetadataSchemaVersion;
        public CommandMetadataSchemaUpdate MetadataSchemaUpdate => m_decoder as CommandMetadataSchemaUpdate;
        public CommandMetadataVersionNotCompatible MetadataVersionNotCompatible => m_decoder as CommandMetadataVersionNotCompatible;
        //public CommandNegotiateSession NegotiateSession => m_decoder as CommandNegotiateSession;
        //public CommandNoOp NoOp => m_decoder as CommandNoOp;
        public CommandRequestFailed RequestFailed => m_decoder as CommandRequestFailed;
        public CommandRequestSucceeded RequestSucceeded => m_decoder as CommandRequestSucceeded;
        public CommandConfigureSubscription ConfigureSubscription => m_decoder as CommandConfigureSubscription;
        public CommandRaw Raw => m_decoder as CommandRaw;
        public CommandUnknown Unknown => m_decoder as CommandUnknown;

        public string ToXMLString()
        {
            if (m_markup == null)
                return "Null";

            return m_markup.ToXML();
        }

        public SttpMarkup Markup => m_markup;


    }
}
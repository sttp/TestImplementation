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
            CommandBase.Register("BulkTransportRequest", x => new CommandBulkTransportRequest(x));
            CommandBase.Register("BulkTransportReply", x => new CommandBulkTransportReply(x));
            CommandBase.Register("DataPointRequest", x => new CommandDataPointRequest(x));
            CommandBase.Register("DataPointRequestCompleted", x => new CommandDataPointRequestCompleted(x));
            CommandBase.Register("GetMetadataSimple", x => new CommandGetMetadataSimple(x));
            CommandBase.Register("GetMetadataStatement", x => new CommandGetMetadataStatement(x));
            CommandBase.Register("GetMetadataSchema", x => new CommandGetMetadataSchema(x));
            CommandBase.Register("Metadata", x => new CommandMetadata(x));
            CommandBase.Register("MetadataSchema", x => new CommandMetadataSchema(x));
            CommandBase.Register("MetadataSchemaUpdate", x => new CommandMetadataSchemaUpdate(x));
            CommandBase.Register("MetadataVersionNotCompatible", x => new CommandMetadataVersionNotCompatible(x));
            CommandBase.Register("RequestFailed", x => new CommandRequestFailed(x));
            CommandBase.Register("RequestSucceeded", x => new CommandRequestSucceeded(x));
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
                case CommandCode.RawCommand:
                    m_markup = null;
                    CommandName = "SubscriptionStream";
                    m_decoder = new CommandSubscriptionStream(decoder.RawCommandCode, decoder.RawCommandPayload);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public CommandBulkTransportRequest BulkTransportRequest => m_decoder as CommandBulkTransportRequest;
        public CommandBulkTransportReply BulkTransportReply => m_decoder as CommandBulkTransportReply;
        public CommandDataPointRequestCompleted DataPointRequestCompleted => m_decoder as CommandDataPointRequestCompleted;
        public CommandDataPointRequest DataPointRequest => m_decoder as CommandDataPointRequest;
        public CommandGetMetadataSimple GetMetadataSimple => m_decoder as CommandGetMetadataSimple;
        public CommandGetMetadataStatement GetMetadataStatement => m_decoder as CommandGetMetadataStatement;
        public CommandGetMetadataSchema GetMetadataSchema => m_decoder as CommandGetMetadataSchema;
        //public CommandMapRuntimeIDs MapRuntimeIDs => m_decoder as CommandMapRuntimeIDs;
        public CommandMetadata Metadata => m_decoder as CommandMetadata;
        public CommandMetadataSchema MetadataSchema => m_decoder as CommandMetadataSchema;
        public CommandMetadataSchemaUpdate MetadataSchemaUpdate => m_decoder as CommandMetadataSchemaUpdate;
        public CommandMetadataVersionNotCompatible MetadataVersionNotCompatible => m_decoder as CommandMetadataVersionNotCompatible;
        //public CommandNegotiateSession NegotiateSession => m_decoder as CommandNegotiateSession;
        //public CommandNoOp NoOp => m_decoder as CommandNoOp;
        public CommandRequestFailed RequestFailed => m_decoder as CommandRequestFailed;
        public CommandRequestSucceeded RequestSucceeded => m_decoder as CommandRequestSucceeded;
        //public CommandSubscription Subscription => m_decoder as CommandSubscription;
        public CommandSubscriptionStream SubscriptionStream => m_decoder as CommandSubscriptionStream;
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
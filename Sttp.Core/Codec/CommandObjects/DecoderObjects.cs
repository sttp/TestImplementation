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
            CommandBase.Register("BulkTransportBeginSend", x => new CommandBulkTransportBeginSend(x));
            CommandBase.Register("BulkTransportCancelSend", x => new CommandBulkTransportCancelSend(x));
            CommandBase.Register("BulkTransportRequest", x => new CommandBulkTransportRequest(x));
            CommandBase.Register("BulkTransportSendFragment", x => new CommandBulkTransportSendFragment(x));
            CommandBase.Register("DataPointReply", x => new CommandDataPointReply(x));
            CommandBase.Register("GetMetadata", x => new CommandGetMetadata(x));
            CommandBase.Register("GetMetadataSchema", x => new CommandGetMetadataSchema(x));
            CommandBase.Register("Metadata", x => new CommandMetadata(x));
            CommandBase.Register("MetadataSchema", x => new CommandMetadataSchema(x));
            CommandBase.Register("MetadataSchemaUpdate", x => new CommandMetadataSchemaUpdate(x));
            CommandBase.Register("MetadataVersionNotCompatible", x => new CommandMetadataVersionNotCompatible(x));
            CommandBase.Register("RequestFailed", x => new CommandRequestFailed(x));
            CommandBase.Register("RequestSucceeded", x => new CommandRequestSucceeded(x));

            SttpQueryBase.Register("SttpQuery", x => new SttpQueryStatement(x));
            SttpQueryBase.Register("SttpQuerySimple", x => new SttpQuerySimple(x));
        }


        internal CommandObjects(CommandDecoder decoder)
        {
            CommandCode = decoder.Command;
            CommandName = decoder.CommandName;

            switch (decoder.Command)
            {
                case CommandCode.Invalid:
                    throw new ArgumentOutOfRangeException("Command code of 0 is not permitted");
                case CommandCode.BeginFragment:
                    throw new ArgumentOutOfRangeException("BeginFragment is not valid at this level");
                case CommandCode.NextFragment:
                    throw new ArgumentOutOfRangeException("NextFragment is not valid at this level");
                case CommandCode.MarkupCommand:
                    m_markup = decoder.MarkupPayload;
                    m_decoder = CommandBase.Create(decoder.CommandName, decoder.MarkupPayload);
                    break;
                case CommandCode.SubscriptionStream:
                    m_markup = null;
                    m_decoder = new CommandSubscriptionStream(decoder.SubscriptionEncoding, decoder.SubscriptionPayload);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public CommandBulkTransportBeginSend BulkTransportBeginSend => m_decoder as CommandBulkTransportBeginSend;
        public CommandBulkTransportCancelSend BulkTransportCancelSend => m_decoder as CommandBulkTransportCancelSend;
        public CommandBulkTransportRequest BulkTransportRequest => m_decoder as CommandBulkTransportRequest;
        public CommandBulkTransportSendFragment BulkTransportSendFragment => m_decoder as CommandBulkTransportSendFragment;
        public CommandDataPointReply DataPointReply => m_decoder as CommandDataPointReply;
        //public CommandDataPointRequest DataPointRequest => m_decoder as CommandDataPointRequest;
        public CommandGetMetadata GetMetadata => m_decoder as CommandGetMetadata;
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
        //public CommandUnknown Unknown => m_decoder as CommandUnknown;
        public string ToXMLString()
        {
            if (m_markup == null)
                return "Null";

            return m_markup.ToXML();
        }

        public SttpMarkup Markup => m_markup;


    }
}
using System;

namespace Sttp.Codec
{
    public class CommandObjects
    {
        private object m_decoder;
        public CommandCode CommandCode { get; }

        internal CommandObjects(PayloadReader reader)
        {
            CommandCode = reader.Command;
            switch (CommandCode)
            {
                case CommandCode.Invalid:
                    throw new ArgumentOutOfRangeException("Command code of 0 is not permitted");
                case CommandCode.BeginFragment:
                    throw new ArgumentOutOfRangeException("BeginFragment is not valid at this level");
                case CommandCode.NextFragment:
                    throw new ArgumentOutOfRangeException("NextFragment is not valid at this level");
                case CommandCode.GetMetadata:
                    m_decoder = new CommandGetMetadata(reader);
                    break;
                case CommandCode.GetMetadataSchema:
                    m_decoder = new CommandGetMetadataSchema(reader);
                    break;
                case CommandCode.Metadata:
                    m_decoder = new CommandMetadata();
                    Metadata.Fill(reader);
                    break;
                case CommandCode.MetadataSchema:
                    m_decoder = new CommandMetadataSchema(reader);
                    break;
                case CommandCode.MetadataVersionNotCompatible:
                    m_decoder = new CommandMetadataVersionNotCompatible(reader);
                    break;
                case CommandCode.Subscription:
                    m_decoder = new CommandSubscription(reader);
                    break;
                case CommandCode.SubscriptionStream:
                    m_decoder = new CommandSubscriptionStream(reader);
                    break;
                case CommandCode.DataPointRequest:
                    m_decoder = new CommandDataPointRequest(reader);
                    break;
                case CommandCode.DataPointReply:
                    m_decoder = new CommandDataPointReply(reader);
                    break;
                case CommandCode.MapRuntimeIDs:
                    m_decoder = new CommandMapRuntimeIDs(reader);
                    break;
                case CommandCode.NegotiateSession:
                    m_decoder = new CommandNegotiateSession(reader);
                    break;
                case CommandCode.RequestFailed:
                    m_decoder = new CommandRequestFailed(reader);
                    break;
                case CommandCode.RequestSucceeded:
                    m_decoder = new CommandRequestSucceeded(reader);
                    break;
                case CommandCode.BulkTransportBeginSend:
                    m_decoder = new CommandBulkTransportBeginSend(reader);
                    break;
                case CommandCode.BulkTransportSendFragment:
                    m_decoder = new CommandBulkTransportSendFragment(reader);
                    break;
                case CommandCode.BulkTransportCancelSend:
                    m_decoder = new CommandBulkTransportCancelSend(reader);
                    break;
                case CommandCode.NoOp:
                    m_decoder = new CommandNoOp(reader);
                    break;
                default:
                    m_decoder = new CommandUnknown(reader);
                    break;
            }
        }

        public CommandBulkTransportBeginSend BulkTransportBeginSend => m_decoder as CommandBulkTransportBeginSend;
        public CommandBulkTransportCancelSend BulkTransportCancelSend => m_decoder as CommandBulkTransportCancelSend;
        public CommandBulkTransportSendFragment BulkTransportSendFragment => m_decoder as CommandBulkTransportSendFragment;
        public CommandDataPointReply DataPointReply => m_decoder as CommandDataPointReply;
        public CommandDataPointRequest DataPointRequest => m_decoder as CommandDataPointRequest;
        public CommandGetMetadata GetMetadata => m_decoder as CommandGetMetadata;
        public CommandGetMetadataSchema GetMetadataSchema => m_decoder as CommandGetMetadataSchema;
        public CommandMapRuntimeIDs MapRuntimeIDs => m_decoder as CommandMapRuntimeIDs;
        public CommandMetadata Metadata => m_decoder as CommandMetadata;
        public CommandMetadataSchema MetadataSchema => m_decoder as CommandMetadataSchema;
        public CommandMetadataVersionNotCompatible MetadataVersionNotCompatible => m_decoder as CommandMetadataVersionNotCompatible;
        public CommandNegotiateSession NegotiateSession => m_decoder as CommandNegotiateSession;
        public CommandNoOp NoOp => m_decoder as CommandNoOp;
        public CommandRequestFailed RequestFailed => m_decoder as CommandRequestFailed;
        public CommandRequestSucceeded RequestSucceeded => m_decoder as CommandRequestSucceeded;
        public CommandSubscription Subscription => m_decoder as CommandSubscription;
        public CommandSubscriptionStream SubscriptionStream => m_decoder as CommandSubscriptionStream;
        public CommandUnknown Unknown => m_decoder as CommandUnknown;


    }
}
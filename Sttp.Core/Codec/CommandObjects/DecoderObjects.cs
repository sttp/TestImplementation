using System;

namespace Sttp.Codec
{
    public class CommandObjects
    {
        private object m_decoder;

        public CommandCode CommandCode { get; }
        public string CommandName { get; }

        internal CommandObjects(CommandDecoder decoder)
        {
            CommandCode = decoder.Command;

            switch (decoder.Command)
            {
                case CommandCode.Invalid:
                    throw new ArgumentOutOfRangeException("Command code of 0 is not permitted");
                case CommandCode.BeginFragment:
                    throw new ArgumentOutOfRangeException("BeginFragment is not valid at this level");
                case CommandCode.NextFragment:
                    throw new ArgumentOutOfRangeException("NextFragment is not valid at this level");
                case CommandCode.Metadata:
                case CommandCode.Subscription:
                case CommandCode.DataPoints:
                case CommandCode.NegotiateSession:
                case CommandCode.Message:
                case CommandCode.LargeObject:
                case CommandCode.Heartbeat:
                    LoadSttpMarkupBasedCommand(decoder);
                    break;
                case CommandCode.SubscriptionStream:
                    m_decoder = new CommandSubscriptionStream(decoder.SubscriptionEncoding, decoder.SubscriptionPayload);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void LoadSttpMarkupBasedCommand(CommandDecoder decoder)
        {
            var markup = decoder.MarkupPayload.MakeReader();
            markup.Read();
            switch (decoder.Command)
            {
                case CommandCode.Metadata:
                    switch (markup.ElementName)
                    {
                        case "GetMetadata":
                            m_decoder = new CommandGetMetadata(markup);
                            break;
                        case "GetMetadataSchema":
                            m_decoder = new CommandGetMetadata(markup);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(markup.ElementName);
                    }
                    break;
                case CommandCode.Subscription:

                    break;
                case CommandCode.DataPoints:

                    break;
                case CommandCode.NegotiateSession:

                    break;
                case CommandCode.Message:

                    break;
                case CommandCode.LargeObject:

                    break;
                case CommandCode.Heartbeat:

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
        public CommandDataPointRequest DataPointRequest => m_decoder as CommandDataPointRequest;
        public CommandGetMetadata GetMetadata => m_decoder as CommandGetMetadata;
        public CommandGetMetadataSchema GetMetadataSchema => m_decoder as CommandGetMetadataSchema;
        public CommandMapRuntimeIDs MapRuntimeIDs => m_decoder as CommandMapRuntimeIDs;
        public CommandMetadata Metadata => m_decoder as CommandMetadata;
        public CommandMetadataSchema MetadataSchema => m_decoder as CommandMetadataSchema;
        public CommandMetadataSchemaUpdate MetadataSchemaUpdate => m_decoder as CommandMetadataSchemaUpdate;
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
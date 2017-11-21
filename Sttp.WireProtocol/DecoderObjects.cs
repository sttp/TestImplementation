namespace Sttp.WireProtocol
{
    public class DecoderObjects
    {
        private object m_decoder;
        public CommandCode CommandCode { get; }

        internal DecoderObjects(CommandCode code, object decoder)
        {
            m_decoder = decoder;
            CommandCode = code;
        }

        public Metadata.Decoder Metadata => m_decoder as Metadata.Decoder;
        public CommandSubscription Subscription => m_decoder as CommandSubscription;
        public CommandNegotiateSession NegotiateSession => m_decoder as CommandNegotiateSession;
        public CommandRequestFailed RequestFailed => m_decoder as CommandRequestFailed;
        public CommandRequestSucceeded RequestSucceeded => m_decoder as CommandRequestSucceeded;
        public CommandBulkTransportBeginSend BulkTransportBeginSend => m_decoder as CommandBulkTransportBeginSend;
        public CommandBulkTransportCancelSend BulkTransportCancelSend => m_decoder as CommandBulkTransportCancelSend;
        public CommandBulkTransportSendFragment BulkTransportSendFragment => m_decoder as CommandBulkTransportSendFragment;

    }
}
using System;
using Sttp.WireProtocol;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Responsible for decoding each packet into commands.
    /// </summary>
    public class WireDecoder
    {
        //private DataPointDecoder m_dataPointDecoder;
        private CommandDecoder m_packetDecoder;
        private SessionDetails m_sessionDetails;

        private Metadata.Decoder m_metadata;
        private CommandSubscription m_subscription;
        private CommandMapRuntimeIDs m_runtimeIDMapping;
        private CommandNegotiateSession m_negotiateSession;
        private CommandRequestFailed m_requestFailed;
        private CommandRequestSucceeded m_requestSucceeded;
        private CommandBulkTransportBeginSend m_bulkTransportBeginSend;
        private CommandBulkTransportCancelSend m_bulkTransportCancelSend;
        private CommandBulkTransportSendFragment m_bulkTransportSendFragment;

        public WireDecoder()
        {
            // m_dataPointDecoder = new DataPointDecoder();
            m_negotiateSession = new CommandNegotiateSession();
            m_sessionDetails = new SessionDetails();
            m_packetDecoder = new CommandDecoder(m_sessionDetails);
            m_metadata = new Metadata.Decoder();
            m_subscription = new CommandSubscription();
            m_runtimeIDMapping = new CommandMapRuntimeIDs();
            m_negotiateSession = new CommandNegotiateSession();
            m_requestFailed = new CommandRequestFailed();
            m_requestSucceeded = new CommandRequestSucceeded();
            m_bulkTransportBeginSend = new CommandBulkTransportBeginSend();
            m_bulkTransportCancelSend = new CommandBulkTransportCancelSend();
            m_bulkTransportSendFragment = new CommandBulkTransportSendFragment();
        }

        /// <summary>
        /// Writes the wire protocol data to the decoder.
        /// </summary>
        /// <param name="data">the data to write</param>
        /// <param name="position">the starting position</param>
        /// <param name="length">the length</param>
        public void WriteData(byte[] data, int position, int length)
        {
            m_packetDecoder.WriteData(data, position, length);
        }

        /// <summary>
        /// Gets the next data packet. This method should be in a while loop, decoding all
        /// messages before the next block of data is added to the decoder via <see cref="WriteData"/>
        /// </summary>
        /// <returns>The decoder for this segment of data, null if there are no pending data packets. </returns>
        public DecoderObjects NextCommand()
        {
            PayloadReader reader = m_packetDecoder.NextPacket();
            if (reader == null)
                return null;

            switch (reader.Command)
            {
                case CommandCode.NegotiateSession:
                    m_negotiateSession.Fill(reader);
                    return new DecoderObjects(reader.Command, m_negotiateSession);
                case CommandCode.Subscription:
                    m_subscription.Fill(reader);
                    return new DecoderObjects(reader.Command, m_subscription);
                case CommandCode.MapRuntimeIDs:
                    break;
                //m_dataPointDecoder.Fill(reader);
                //return new CommandDecoder(reader.Command, m_dataPointDecoder);
                case CommandCode.NoOp:
                    break;
                case CommandCode.Invalid:
                    break;
                case CommandCode.BeginFragment:
                    break;
                case CommandCode.NextFragment:
                    break;
                case CommandCode.Metadata:
                    m_metadata.Fill(reader);
                    return new DecoderObjects(reader.Command, m_metadata);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return null;
        }



    }
}

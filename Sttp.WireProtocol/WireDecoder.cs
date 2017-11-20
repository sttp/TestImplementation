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
        private PacketDecoder m_packetDecoder = new PacketDecoder(new SessionDetails());
        private SessionDetails m_sessionDetails = new SessionDetails();

        private GetMetadata.Decoder m_getMetadata;
        private Metadata.Decoder m_metadata;
        private Subscription.Decoder m_subscription;
        private MapRuntimeIDs.Decoder m_runtimeIDMapping;
        private NegotiateSession.Decoder m_negotiateSession;
        private RequestFailed.Decoder m_requestFailed;
        private RequestSucceeded.Decoder m_requestSucceeded;
        private BulkTransport.Decoder m_bulkTransport;

        public WireDecoder()
        {
            // m_dataPointDecoder = new DataPointDecoder();
            m_negotiateSession = new NegotiateSession.Decoder();

            m_getMetadata = new GetMetadata.Decoder();
            m_metadata = new Metadata.Decoder();
            m_subscription = new Subscription.Decoder();
            m_runtimeIDMapping = new MapRuntimeIDs.Decoder();
            m_negotiateSession = new NegotiateSession.Decoder();
            m_requestFailed = new RequestFailed.Decoder();
            m_requestSucceeded = new RequestSucceeded.Decoder();
            m_bulkTransport = new BulkTransport.Decoder();
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
        public CommandDecoder NextCommand()
        {
            PacketReader reader = m_packetDecoder.NextPacket();
            if (reader == null)
                return null;

            switch (reader.Command)
            {
                case CommandCode.NegotiateSession:
                    m_negotiateSession.Fill(reader);
                    return new CommandDecoder(reader.Command, m_negotiateSession);
                case CommandCode.BulkTransport:
                    m_bulkTransport.Fill(reader);
                    return new CommandDecoder(reader.Command, m_bulkTransport);
                case CommandCode.Subscription:
                    m_subscription.Fill(reader);
                    return new CommandDecoder(reader.Command, m_subscription);
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
                case CommandCode.GetMetadata:
                    m_getMetadata.Fill(reader);
                    return new CommandDecoder(reader.Command, m_getMetadata);
                case CommandCode.Metadata:
                    m_metadata.Fill(reader);
                    return new CommandDecoder(reader.Command, m_metadata);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return null;
        }

       

    }
}

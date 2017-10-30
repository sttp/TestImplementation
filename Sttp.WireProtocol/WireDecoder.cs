using System;
using Sttp.WireProtocol.Codec.DataPointPacket;
using Sttp.WireProtocol.Data;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Responsible for decoding each packet into commands.
    /// </summary>
    public class WireDecoder
    {
        private MetadataDecoder m_metadataDecoder;
        private DataPointDecoder m_dataPointDecoder;
        private NegotiateSessionDecoder m_negotiateSessionDecoder;
        private SubscriptionDecoder m_subscriptionDecoder;
        private BulkTransportDecoder m_bulkDecoder;
        private PacketDecoder m_packetDecoder = new PacketDecoder(new SessionDetails());
        private SessionDetails m_sessionDetails = new SessionDetails();

        public WireDecoder()
        {
            m_metadataDecoder = new MetadataDecoder();
            m_dataPointDecoder = new DataPointDecoder();
            m_negotiateSessionDecoder = new NegotiateSessionDecoder();
            m_subscriptionDecoder = new SubscriptionDecoder();
            m_bulkDecoder = new BulkTransportDecoder();
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
        public IPacketDecoder NextPacket()
        {
            PacketReader reader = m_packetDecoder.NextPacket();
            if (reader == null)
                return null;

            switch (reader.Command)
            {
                case CommandCode.NegotiateSession:
                    m_negotiateSessionDecoder.Fill(reader);
                    return m_negotiateSessionDecoder;
                    m_metadataDecoder.Fill(reader);
                    return m_metadataDecoder;
                case CommandCode.BulkTransport:
                    m_bulkDecoder.Fill(reader);
                    return m_bulkDecoder;
                case CommandCode.Subscribe:
                    m_subscriptionDecoder.Fill(reader);
                    return m_subscriptionDecoder;
                case CommandCode.SecureDataChannel:
                    break;
                case CommandCode.RuntimeIDMapping:
                    m_dataPointDecoder.Fill(reader);
                    return m_dataPointDecoder;
                case CommandCode.DataPointPacket:
                    m_dataPointDecoder.Fill(reader);
                    return m_dataPointDecoder;
                case CommandCode.NoOp:
                    break;
                case CommandCode.Invalid:
                    break;
                case CommandCode.BeginFragment:
                    break;
                case CommandCode.NextFragment:
                    break;
                case CommandCode.CompressedPacket:
                    break;
                case CommandCode.MetadataGetSchema:
                    break;
                case CommandCode.MetadataGetData:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return null;
        }

       

    }
}

using System;
using Sttp.WireProtocol.Codec.DataPointPacket;
using Sttp.WireProtocol.Data;
using Sttp.WireProtocol.Data.Raw;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Responsible for decoding each packet into commands.
    /// </summary>
    public class Decoder
    {
        private IMetadataDecoder m_metadataDecoder;
        private DataPointDecoder m_dataPointDecoder;
        private NegotiateSessionDecoder m_negotiateSessionDecoder;
        private SubscriptionDecoder m_subscriptionDecoder;

        private StreamReader m_buffer = new StreamReader();

        public Decoder()
        {
            m_metadataDecoder = new MetadataDecoder();
            m_dataPointDecoder = new DataPointDecoder();
            m_negotiateSessionDecoder = new NegotiateSessionDecoder();
            m_subscriptionDecoder = new SubscriptionDecoder();
        }

        /// <summary>
        /// Writes the wire protocol data to the decoder.
        /// </summary>
        /// <param name="data">the data to write</param>
        /// <param name="position">the starting position</param>
        /// <param name="length">the length</param>
        public void WriteData(byte[] data, int position, int length)
        {
            m_buffer.Compact();
            m_buffer.Fill(data, position, length);
        }

        /// <summary>
        /// Gets the next data packet. This method should be in a while loop, decoding all
        /// messages before the next block of data is added to the decoder via <see cref="WriteData"/>
        /// </summary>
        /// <returns>The decoder for this segment of data, null if there are no pending data packets. </returns>
        public IPacketDecoder NextPacket()
        {
            //A message fewer than 2 bytes are not valid.
            if (m_buffer.PendingBytes < 2)
                return null;

            int origPosition = m_buffer.Position;
            int messageLength = m_buffer.ReadByte();

            if (messageLength > m_buffer.PendingBytes)
            {
                m_buffer.Position = origPosition;
                return null;
            }

            switch (m_buffer.ReadCommandCode())
            {
                case CommandCode.NegotiateSession:
                    m_negotiateSessionDecoder.Fill(m_buffer);
                    return m_negotiateSessionDecoder;
                case CommandCode.Metadata:
                    m_metadataDecoder.Fill(m_buffer);
                    return m_metadataDecoder;
                case CommandCode.Subscribe:
                    m_subscriptionDecoder.Fill(m_buffer);
                    return m_subscriptionDecoder;
                case CommandCode.SecureDataChannel:
                    break;
                case CommandCode.RuntimeIDMapping:
                    m_dataPointDecoder.Fill(m_buffer);
                    return m_dataPointDecoder;
                case CommandCode.DataPointPacket:
                    m_dataPointDecoder.Fill(m_buffer);
                    return m_dataPointDecoder;
                case CommandCode.NoOp:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return null;
        }

        public void NegotiateSessionStep1(out ProtocolVersions protocolVersionNumber)
        {
            throw new NotImplementedException();
        }
        public void NegotiateSessionStep1Reply(out OperationalModes modes)
        {
            throw new NotImplementedException();
        }
        public void NegotiateSessionStep2(out OperationalModes modes)
        {
            throw new NotImplementedException();
        }
        public void CommandSuccess(out CommandCode command, out string response)
        {
            throw new NotImplementedException();
        }
        public void CommandFailed(out CommandCode command, out string response)
        {
            throw new NotImplementedException();
        }
        public void RequestMetadataTables()
        {
            //Do nothing, just here to be complete.
        }
        //public void RequestMetadataTablesReply(out MetadataTableSource[] tableSourceDefinitions)
        //{
        //    throw new NotImplementedException();
        //}

        public void RequestMetadata(out string tableName, out Guid cachedBaseVersionNumber, out int versionNumber, out string filterString)
        {
            throw new NotImplementedException();
        }

        //public void RequestMetadataReply(out Guid cachedBaseVersionNumber, out int versionNumber, out MetadataRow[] rows)
        //{
        //    throw new NotImplementedException();
        //}

        public void Subscribe(out string subscriptionString, out bool augment)
        {
            throw new NotImplementedException();
        }

    }
}

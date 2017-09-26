using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sttp.Data;

namespace Sttp.WireProtocol
{
    public enum DecoderCallback
    {
        NegotiateSessionStep1,
        NegotiateSessionStep1Reply,
        NegotiateSessionStep2,
        CommandSuccess,
        CommandFailed,
        RequestMetadataTables,
        RequestMetadataTablesReply,
        RequestMetadata,
        RequestMetadataReply,
        Subscribe,
        /// <summary>
        /// Indicates that all full messages have been parsed. There might still be a split fragment
        /// since TCP turns messages into streams, but after this point, more data needs to be pushed to the decoder.
        /// </summary>
        EndOfMessages
    }

    /// <summary>
    /// Responsible for decoding each packet into commands.
    /// </summary>
    public class DecoderTCP
    {
        private byte[] m_buffer = new byte[128];
        private int m_bufferPosition;
        private int m_bufferLength;

        /// <summary>
        /// The number of bytes that can be written to the buffer.
        /// </summary>
        private int FreeSpace => m_buffer.Length - m_bufferPosition;

        /// <summary>
        /// The number of bytes that are still being used.
        /// </summary>
        private int UsedSpace => m_bufferLength - m_bufferPosition;

        /// <summary>
        /// Writes the wire protocol data to the decoder.
        /// </summary>
        /// <param name="data">the data to write</param>
        /// <param name="position">the starting position</param>
        /// <param name="length">the length</param>
        public void WriteData(byte[] data, int position, int length)
        {
            int bufferLength = UsedSpace;
            if (m_bufferPosition > 0 && UsedSpace != 0)
            {
                //Compact
                Array.Copy(m_buffer, m_bufferPosition, m_buffer, 0, bufferLength);
            }
            m_bufferLength = bufferLength;
            m_bufferPosition = 0;

            while (FreeSpace < length)
            {
                //Grow the buffer
                byte[] newBuffer = new byte[m_buffer.Length * 2];
                Array.Copy(m_buffer, 0, newBuffer, 0, bufferLength);
                m_buffer = newBuffer;
            }

            Array.Copy(data, position, m_buffer, m_bufferLength, length);
            m_bufferLength += length;
        }

        /// <summary>
        /// Gets the next decoded message. This method should be in a while loop, decoding all
        /// messages before the next block of data is added to the decoder via <see cref="WriteData"/>
        /// </summary>
        /// <returns></returns>
        public DecoderCallback NextMessage()
        {
            //A message fewer than 2 bytes are not valid.
            if (UsedSpace < 2)
                return DecoderCallback.EndOfMessages;

            int position;
            int messageLength = uint15.Read(m_buffer, m_bufferPosition, out position);
            if (messageLength > UsedSpace)
                return DecoderCallback.EndOfMessages;

            switch ((CommandCode)m_buffer[m_bufferPosition + position])
            {
                case CommandCode.NegotiateSession:
                    return DecoderCallback.NegotiateSessionStep1;
                    return DecoderCallback.NegotiateSessionStep2;
                case CommandCode.MetadataRefresh:
                    return DecoderCallback.RequestMetadata;
                case CommandCode.Subscribe:
                    return DecoderCallback.Subscribe;
                case CommandCode.Unsubscribe:
                    break;
                case CommandCode.SecureDataChannel:
                    break;
                case CommandCode.RuntimeIDMapping:
                    break;
                case CommandCode.DataPointPacket:
                    break;
                case CommandCode.NoOp:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //Decode the next message. Return the message type so it can be properly handled.
            return DecoderCallback.EndOfMessages;
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
        public void RequestMetadataTablesReply(out MetadataTableSource[] tableSourceDefinitions)
        {
            throw new NotImplementedException();
        }

        public void RequestMetadata(out string tableName, out Guid cachedBaseVersionNumber, out int versionNumber, out string filterString)
        {
            throw new NotImplementedException();
        }

        public void RequestMetadataReply(out Guid cachedBaseVersionNumber, out int versionNumber, out MetadataRow[] rows)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(out string subscriptionString, out bool augment)
        {
            throw new NotImplementedException();
        }

    }
}

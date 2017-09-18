using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    /// Responsible with decoding each packet into commands.
    /// </summary>
    public class Decoder
    {
        private byte[] m_buffer;
        private int m_bufferPosition;
        private int m_bufferLength;

        public void WriteData(byte[] data, int position, int length)
        {
            //ToDo: Compact and resize if needed. 
            Array.Copy(data, position, m_buffer, m_bufferLength, length);
            m_bufferLength += length;
        }

        public DecoderCallback NextMessage()
        {
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
        public void RequestMetadataTablesReply(out MetadataTable[] tableDefinitions)
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

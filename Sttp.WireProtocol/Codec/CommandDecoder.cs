using System;
using System.IO;
using System.Text;
using Ionic.Zlib;

namespace Sttp.Codec
{
    /// <summary>
    /// Decodes an incoming byte stream into a series of command objects. This class will align packets, reassemble fragments, and decompress packets. 
    /// </summary>
    public class CommandDecoder
    {
        /// <summary>
        /// raw unprocessed data received from the client
        /// </summary>
        private byte[] m_inboundBuffer;
        /// <summary>
        /// The active position of the inbound buffer.
        /// </summary>
        private int m_inboundBufferCurrentPosition;
        /// <summary>
        /// The number of unconsumed bytes in the inbound buffer.
        /// </summary>
        private int m_inboundBufferLength;

        /// <summary>
        /// A buffer for building fragmented packets.
        /// </summary>
        private byte[] m_fragmentBuffer;

        /// <summary>
        /// A buffer for extracting a compressed packet.
        /// </summary>
        private byte[] m_compressionBuffer;

        /// <summary>
        /// Constraints on the inbound data.
        /// </summary>
        private SessionDetails m_sessionDetails;

        /// <summary>
        /// A state variable to ensure that fragmented packets are properly sequenced.
        /// </summary>
        private bool m_isProcessingFragments;
        /// <summary>
        /// The compressed size of a fragmented packet.
        /// </summary>
        private int m_fragmentTotalSize;
        /// <summary>
        /// The uncompressed size of a fragmented packet.
        /// </summary>
        private int m_fragmentTotalRawSize;
        /// <summary>
        /// The CommandCode that is contained within a fragmented or compressed packet
        /// </summary>
        private CommandCode m_fragmentCommandCode;
        /// <summary>
        /// The compression mode for the data being sent.
        /// </summary>
        private byte m_fragmentCompressionMode;
        /// <summary>
        /// The number of bytes of the fragment that have been received thus far.
        /// </summary>
        private int m_fragmentBytesReceived;
        private CommandCode m_command;
        private SttpMarkup m_markupPayload;
        private byte[] m_subscriptionPayload;
        private byte m_subscriptionEncoding;

        /// <summary>
        /// Indicates if a command has successfully been decoded. 
        /// This is equal to the return value of the most recent 
        /// <see cref="NextCommand"/> method call.
        /// </summary>
        public bool IsValid => m_command != CommandCode.Invalid;

        /// <summary>
        /// If valid, This is the command that was decoded.
        /// </summary>
        public CommandCode Command
        {
            get
            {
                if (!IsValid)
                    throw new InvalidOperationException("IsValid is false.");
                return m_command;
            }
        }

        /// <summary>
        /// Valid if <see cref="NextCommand"/> returned true. And the command 
        /// This is the command that was decoded.
        /// </summary>
        public SttpMarkup MarkupPayload
        {
            get
            {
                if (!IsValid)
                    throw new InvalidOperationException("IsValid is false.");
                if (m_command != CommandCode.MarkupCommand)
                    throw new InvalidOperationException("Command is not a MarkupCommand.");
                return m_markupPayload;
            }
        }

        /// <summary>
        /// Valid if <see cref="NextCommand"/> returned true. 
        /// This is the command that was decoded.
        /// </summary>
        public byte[] SubscriptionPayload
        {
            get
            {
                if (!IsValid)
                    throw new InvalidOperationException("IsValid is false.");
                if (m_command != CommandCode.SubscriptionStream)
                    throw new InvalidOperationException("Command is not a SubscriptionStream.");
                return m_subscriptionPayload;
            }
        }

        /// <summary>
        /// Valid if <see cref="NextCommand"/> returned true. 
        /// This is the command that was decoded.
        /// </summary>
        public byte SubscriptionEncoding
        {
            get
            {
                if (!IsValid)
                    throw new InvalidOperationException("IsValid is false.");
                if (m_command != CommandCode.SubscriptionStream)
                    throw new InvalidOperationException("Command is not a SubscriptionStream.");
                return m_subscriptionEncoding;
            }
        }

        public CommandDecoder(SessionDetails sessionDetails)
        {
            m_fragmentBuffer = new byte[512];
            m_compressionBuffer = new byte[512];
            m_sessionDetails = sessionDetails;
            m_inboundBuffer = new byte[512];
        }

        /// <summary>
        /// Writes the wire protocol data to the decoder. This data does not have to be packet aligned.
        /// </summary>
        /// <param name="data">the data to write</param>
        /// <param name="position">the starting position</param>
        /// <param name="length">the length</param>
        public void FillBuffer(byte[] data, int position, int length)
        {
            data.ValidateParameters(position, length);

            if (m_inboundBufferCurrentPosition > 0 && m_inboundBufferLength != 0)
            {
                // Compact - trims all data before current position if position is in middle of stream
                Array.Copy(m_inboundBuffer, m_inboundBufferCurrentPosition, m_inboundBuffer, 0, m_inboundBufferLength);
            }
            m_inboundBufferCurrentPosition = 0;

            int growSize = m_inboundBufferLength + length;
            if (m_inboundBuffer.Length < growSize)
            {
                //12% larger than the requested buffer size.
                byte[] newBuffer = new byte[growSize + (growSize >> 3)];
                m_inboundBuffer.CopyTo(newBuffer, 0);
                m_inboundBuffer = newBuffer;
            }

            Array.Copy(data, position, m_inboundBuffer, m_inboundBufferLength, length);
            m_inboundBufferLength += length;
        }

        /// <summary>
        /// Gets the next data packet. This method should be in a while loop, decoding all
        /// messages before the next block of data is added to the decoder via <see cref="FillBuffer"/>
        /// 
        /// Reads the inbound raw buffer for the next full command. 
        /// Automatically decompresses and combines fragments and waits for the entire packet before
        /// responding as True.
        /// </summary>
        public bool NextCommand()
        {
            m_command = CommandCode.Invalid;
            m_markupPayload = null;
            m_subscriptionPayload = null;
            m_subscriptionEncoding = 0;

            TryAgain:

            if (m_inboundBufferLength < 3)
                return false;

            CommandCode code = (CommandCode)m_inboundBuffer[m_inboundBufferCurrentPosition];
            int payloadLength = ToInt16(m_inboundBuffer, m_inboundBufferCurrentPosition + 1);
            if (m_inboundBufferLength < payloadLength + 3)
            {
                return false;
            }
            if (code == CommandCode.BeginFragment)
            {
                if (m_isProcessingFragments)
                    throw new Exception("A previous fragment has not finished.");

                m_isProcessingFragments = true;
                m_fragmentTotalSize = ToInt32(m_inboundBuffer, m_inboundBufferCurrentPosition + 3);
                m_fragmentTotalRawSize = ToInt32(m_inboundBuffer, m_inboundBufferCurrentPosition + 7);
                m_fragmentCommandCode = (CommandCode)m_inboundBuffer[m_inboundBufferCurrentPosition + 11];
                m_fragmentCompressionMode = m_inboundBuffer[m_inboundBufferCurrentPosition + 12];

                if (m_fragmentBuffer.Length < m_fragmentTotalSize)
                {
                    m_fragmentBuffer = new byte[m_fragmentTotalSize];
                }

                Array.Copy(m_inboundBuffer, m_inboundBufferCurrentPosition + 13, m_fragmentBuffer, 0, payloadLength - 10);
                m_fragmentBytesReceived = payloadLength - 10;
                m_inboundBufferCurrentPosition += payloadLength + 3;
                m_inboundBufferLength -= payloadLength + 3;

                if (m_fragmentBytesReceived == m_fragmentTotalSize)
                {
                    SendFragmentedPacket();
                    return true;
                }
                goto TryAgain;
            }
            else if (code == CommandCode.NextFragment)
            {
                if (!m_isProcessingFragments)
                    throw new Exception("A fragment has not been defined.");

                Array.Copy(m_inboundBuffer, m_inboundBufferCurrentPosition + 3, m_fragmentBuffer, m_fragmentBytesReceived, payloadLength);
                m_fragmentBytesReceived += payloadLength;
                m_inboundBufferCurrentPosition += payloadLength + 3;
                m_inboundBufferLength -= payloadLength + 3;

                if (m_fragmentBytesReceived == m_fragmentTotalSize)
                {
                    SendFragmentedPacket();
                    return true;
                }
                goto TryAgain;
            }
            else
            {
                if (m_isProcessingFragments)
                    throw new Exception("Expecting the next fragment");
                ProcessNextCommand(code, m_inboundBuffer, m_inboundBufferCurrentPosition + 3, payloadLength);
                m_inboundBufferCurrentPosition += payloadLength + 3;
                m_inboundBufferLength -= payloadLength + 3;
                return true;
            }
        }

        /// <summary>
        /// Combined and decompresses a fragmented packet.
        /// </summary>
        private void SendFragmentedPacket()
        {
            m_isProcessingFragments = false;
            if (m_fragmentCompressionMode == 0)
            {
                ProcessNextCommand(m_fragmentCommandCode, m_fragmentBuffer, 0, m_fragmentTotalRawSize);
                return;
            }
            if (m_compressionBuffer.Length < m_fragmentTotalRawSize)
            {
                m_compressionBuffer = new byte[m_fragmentTotalRawSize];
            }
            var ms = new MemoryStream(m_fragmentBuffer);
            using (var inflate = new DeflateStream(ms, CompressionMode.Decompress, true))
            {
                inflate.ReadAll(m_compressionBuffer, 0, m_fragmentTotalRawSize);
            }
            ProcessNextCommand(m_fragmentCommandCode, m_compressionBuffer, 0, m_fragmentTotalRawSize);
        }

        /// <summary>
        /// Decodes a packet and assigns the member variables of this class with the results.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <param name="position"></param>
        /// <param name="length"></param>
        private void ProcessNextCommand(CommandCode code, byte[] data, int position, int length)
        {
            data.ValidateParameters(position, length);

            byte[] results;
            switch (code)
            {
                case CommandCode.Invalid:
                    throw new ArgumentOutOfRangeException("Command code of 0 is not permitted");
                case CommandCode.BeginFragment:
                    throw new ArgumentOutOfRangeException("BeginFragment is not valid at this level");
                case CommandCode.NextFragment:
                    throw new ArgumentOutOfRangeException("NextFragment is not valid at this level");
                case CommandCode.MarkupCommand:
                    m_command = code;
                    int markupLength = length;
                    int markupStart = position;
                    results = new byte[markupLength];
                    Array.Copy(data, markupStart, results, 0, markupLength);
                    m_markupPayload = new SttpMarkup(results);
                    break;
                case CommandCode.SubscriptionStream:
                    m_command = code;
                    results = new byte[length - 1];
                    Array.Copy(data, position + 1, results, 0, length - 1);
                    m_subscriptionEncoding = data[position];
                    m_subscriptionPayload = results;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Unknown Command");
            }
        }

        private static short ToInt16(byte[] buffer, int startIndex)
        {
            return (short)((int)buffer[startIndex] << 8 | (int)buffer[startIndex + 1]);
        }

        private static int ToInt32(byte[] buffer, int startIndex)
        {
            return (int)buffer[startIndex + 0] << 24 |
                   (int)buffer[startIndex + 1] << 16 |
                   (int)buffer[startIndex + 2] << 8 |
                   (int)buffer[startIndex + 3];
        }
    }
}

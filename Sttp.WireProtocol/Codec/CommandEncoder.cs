using System;
using System.IO;
using System.IO.Compression;
using Sttp.IO.Checksums;

namespace Sttp.Codec
{
    /// <summary>
    /// Responsible for encoding each command into bytes that can be sent on the socket. 
    /// This class will fragment and compress packets to ensure that all packets fit inside the desired maximum fragment size.
    /// </summary>
    public class CommandEncoder
    {
        /// <summary>
        /// Occurs when a packet of data must be sent on the wire. This is called immediately
        /// after completing each packet or fragment;
        /// </summary>
        public event Action<byte[], int, int> NewPacket;

        private SessionDetails m_sessionDetails;

        /// <summary>
        /// A buffer to use to for all of the packets.
        /// </summary>
        private byte[] m_buffer;

        private const int BufferOffset = 25;

        /// <summary>
        /// The desired number of bytes before data is automatically flushed via <see cref="NewPacket"/>
        /// </summary>
        /// <param name="sessionDetails">Details concerning the maximum command size and the maximum packet size.</param>
        public CommandEncoder(SessionDetails sessionDetails = null)
        {
            m_sessionDetails = sessionDetails ?? new SessionDetails();
            m_buffer = new byte[64];
        }

        private void SendNewPacket(byte[] buffer, int position, int length)
        {
            NewPacket?.Invoke(buffer, position, length);
        }

        /// <summary>
        /// Sends a raw command.
        /// </summary>
        /// <param name="rawCode">a user code for this raw stream</param>
        /// <param name="payload">the byte payload to send.</param>
        /// <param name="position">the offset in <see cref="payload"/></param>
        /// <param name="length">the length of the payload.</param>
        public void SendRawCommand(byte rawCode, byte[] payload, int position, int length)
        {
            payload.ValidateParameters(position, length);
            EnsureCapacity(BufferOffset + 1 + length);
            Array.Copy(payload, position, m_buffer, BufferOffset, length);

            //Check the special case for encoding this packet with a 2 bytes of header.
            if (rawCode < 32 && length < 1024 && length <= m_sessionDetails.MaximumCommandSize && length + 2 <= m_sessionDetails.MaximumPacketSize)
            {
                bool shouldAttemptCompression = m_sessionDetails.SupportsDeflate && length >= m_sessionDetails.DeflateThreshold;
                if (!shouldAttemptCompression)
                {
                    ushort header = (ushort)(rawCode << 10);
                    header |= (ushort)length;

                    payload[BufferOffset - 2 + 0] = (byte)(header >> 8);
                    payload[BufferOffset - 2 + 1] = (byte)(header);

                    SendNewPacket(payload, BufferOffset - 2, length + 2);
                    return;
                }
            }

            m_buffer[BufferOffset - 1] = rawCode;
            EncodeAndSend(false, m_buffer, BufferOffset - 1, BufferOffset, length);
        }

        /// <summary>
        /// Encodes and sends the supplied command to the client.
        /// </summary>
        /// <param name="command">The command to send.</param>
        public void SendMarkupCommand(CommandBase command)
        {
            var writer = new SttpMarkupWriter(command.CommandName);
            command.Save(writer);
            SendMarkupCommand(writer);
        }

        /// <summary>
        /// Encodes and sends the data specified in <see cref="markup"/>. It's recommended to use
        /// the other overload that contains <see cref="CommandBase"/> if one exists.
        /// </summary>
        /// <param name="markup">The data to send.</param>
        public void SendMarkupCommand(SttpMarkupWriter markup)
        {
            //ToDo: Consider changing the MarkupCommand to split the RootElement and the other payload.
            SttpMarkup data = markup.ToSttpMarkup();
            //The data at this point includes:
            // byte Length of Root Element
            // ASCII root Element
            // byte[] payload.

            EnsureCapacity(BufferOffset + data.Length);
            data.CopyTo(m_buffer, BufferOffset);

            int headerLength = m_buffer[BufferOffset] + 1;
            //Data encoded here includes:
            // byte Length of Root Element
            // ASCII root Element
            // byte[] payload.
            EncodeAndSend(true, m_buffer, BufferOffset, BufferOffset + headerLength, data.Length - headerLength);
        }

        /// <summary>
        /// Ensures that <see cref="m_buffer"/> has at least the supplied number of bytes
        /// before returning.
        /// </summary>
        /// <param name="bufferSize"></param>
        private void EnsureCapacity(int bufferSize)
        {
            if (m_buffer.Length < bufferSize)
            {
                //12% larger than the requested buffer size.
                byte[] newBuffer = new byte[bufferSize + (bufferSize >> 3)];
                m_buffer.CopyTo(newBuffer, 0);
                m_buffer = newBuffer;
            }
        }

        /// <summary>
        /// Sends a command of data over the wire. 
        /// </summary>
        /// <param name="isMarkup">indicates if this command is a markup command</param>
        /// <param name="buffer"></param>
        /// <param name="headerOffset">the offset of the buffer that is the first byte of the header</param>
        /// <param name="payloadOffset">the offset for the first byte of the payload</param>
        /// <param name="payloadLength">The length of the payload segment.</param>
        private void EncodeAndSend(bool isMarkup, byte[] buffer, int headerOffset, int payloadOffset, int payloadLength)
        {
            if (payloadLength > m_sessionDetails.MaximumCommandSize)
            {
                throw new Exception("This packet is too large to send, if this is a legitimate size, increase the MaxPacketSize.");
            }

            int packetLength;
            bool isCompressed = false;

            if (m_sessionDetails.SupportsDeflate && payloadLength >= m_sessionDetails.DeflateThreshold)
            {
                if (TryCompressPayload(buffer, payloadOffset, payloadLength, out int newSize, out uint checksum))
                {
                    isCompressed = true;
                    headerOffset -= 8;
                    buffer[headerOffset + 0] = (byte)(payloadLength >> 24);
                    buffer[headerOffset + 1] = (byte)(payloadLength >> 16);
                    buffer[headerOffset + 2] = (byte)(payloadLength >> 8);
                    buffer[headerOffset + 3] = (byte)(payloadLength >> 0);

                    buffer[headerOffset + 4] = (byte)(checksum >> 24);
                    buffer[headerOffset + 5] = (byte)(checksum >> 16);
                    buffer[headerOffset + 6] = (byte)(checksum >> 8);
                    buffer[headerOffset + 7] = (byte)(checksum >> 0);
                    payloadLength = newSize;
                }
            }

            if (3 + (payloadOffset - headerOffset) + payloadLength < m_sessionDetails.MaximumPacketSize)
            {
                //This packet doesn't have to be fragmented.
                headerOffset -= 3;
                packetLength = (payloadOffset - headerOffset) + payloadLength;
                buffer[headerOffset + 0] = (byte)MakeHeader(isMarkup, isCompressed, false);
                buffer[headerOffset + 1] = (byte)(packetLength >> 8);
                buffer[headerOffset + 2] = (byte)(packetLength);
                SendNewPacket(buffer, headerOffset, packetLength);
            }
            else
            {

                int fragmentLength = (payloadOffset - headerOffset) + payloadLength;

                uint checksum = Crc32.Compute(buffer, headerOffset, fragmentLength);

                headerOffset -= 8;
                buffer[headerOffset + 0] = (byte)(fragmentLength >> 24);
                buffer[headerOffset + 1] = (byte)(fragmentLength >> 16);
                buffer[headerOffset + 2] = (byte)(fragmentLength >> 8);
                buffer[headerOffset + 3] = (byte)(fragmentLength >> 0);

                buffer[headerOffset + 4] = (byte)(checksum >> 24);
                buffer[headerOffset + 5] = (byte)(checksum >> 16);
                buffer[headerOffset + 6] = (byte)(checksum >> 8);
                buffer[headerOffset + 7] = (byte)(checksum >> 0);

                DataPacketHeader header = MakeHeader(isMarkup, isCompressed, true);
                SendFragment(header, buffer, headerOffset, fragmentLength + 8);
            }
        }

        private void SendFragment(DataPacketHeader header, byte[] buffer, int offset, int length)
        {
            int bytesSentPerFragment = m_sessionDetails.MaximumPacketSize - 7;

            int bytesToSendThisFragment = Math.Min(bytesSentPerFragment, length);
            int packetLength = bytesToSendThisFragment + 7;

            int totalFragments = (length + bytesSentPerFragment - 1) / bytesSentPerFragment; //Adding (bytesSentPerFragment - 1) with integer division means round up.
            int currentFragment = 1;

            if (totalFragments > ushort.MaxValue)
                throw new OverflowException();

            //The size of the payload.
            buffer[offset - 7] = (byte)header;
            buffer[offset - 6] = (byte)(packetLength >> 8);
            buffer[offset - 5] = (byte)(packetLength);
            buffer[offset - 4] = (byte)(currentFragment >> 8);
            buffer[offset - 3] = (byte)(currentFragment);
            buffer[offset - 2] = (byte)(totalFragments >> 8);
            buffer[offset - 1] = (byte)(totalFragments);


            SendNewPacket(buffer, offset - 7, packetLength);

            offset += bytesToSendThisFragment;
            length -= bytesToSendThisFragment;

            while (length > 0)
            {
                bytesToSendThisFragment = Math.Min(bytesSentPerFragment, length);
                packetLength = bytesToSendThisFragment + 7;
                currentFragment++;

                buffer[offset - 7] = (byte)DataPacketHeader.NextFragment;
                buffer[offset - 6] = (byte)(packetLength >> 8);
                buffer[offset - 5] = (byte)(packetLength);
                buffer[offset - 4] = (byte)(currentFragment >> 8);
                buffer[offset - 3] = (byte)(currentFragment);
                buffer[offset - 2] = (byte)(totalFragments >> 8);
                buffer[offset - 1] = (byte)(totalFragments);

                SendNewPacket(buffer, offset - 7, packetLength);
                offset += bytesToSendThisFragment;
                length -= bytesToSendThisFragment;
            }
        }

        /// <summary>
        /// Creates a header byte for a packet with the provided classifications. Note: This method is not valid for 
        /// headers that are part of fragmented packets.
        /// </summary>
        /// <param name="isMarkup"></param>
        /// <param name="isCompressed"></param>
        /// <param name="isFragmented"></param>
        /// <returns></returns>
        private DataPacketHeader MakeHeader(bool isMarkup, bool isCompressed, bool isFragmented)
        {
            DataPacketHeader header = DataPacketHeader.None;
            if (isMarkup)
            {
                header |= DataPacketHeader.IsMarkupCommand;
            }
            if (isCompressed)
            {
                header |= DataPacketHeader.IsCompressed;
            }
            if (isFragmented)
            {
                header |= DataPacketHeader.BeginFragment;
            }
            else
            {
                header |= DataPacketHeader.NotFragmented;
            }
            return header;
        }

        /// <summary>
        /// Attempts to compress the payload. If the compression is smaller than the original, the compressed
        /// data is copied over the original and <see cref="newLength"/> contains the length of the compressed data.
        /// Otherwise, this method returns false.
        /// </summary>
        /// <param name="payload">the data to compress</param>
        /// <param name="payloadOffset">the offset</param>
        /// <param name="payloadLength">the length of the payload</param>
        /// <param name="newLength">the length of the compressed data if successful, -1 if the compression failed.</param>
        /// <param name="checksum">A computed CRC32 of compressed data if compression is successful.</param>
        /// <returns></returns>
        private bool TryCompressPayload(byte[] payload, int payloadOffset, int payloadLength, out int newLength, out uint checksum)
        {
            using (var ms = new MemoryStream())
            {
                using (var deflate = new DeflateStream(ms, CompressionMode.Compress, true))
                {
                    deflate.Write(payload, payloadOffset, payloadLength);
                }

                //Verifies that there was a size reduction with compression.
                if (ms.Position + 8 >= payloadLength)
                {
                    newLength = -1;
                    checksum = 0;
                    return false;
                }

                checksum = Crc32.Compute(payload, payloadOffset, payloadLength);
                newLength = (int)ms.Position;
                ms.Position = 0;
                ms.Read(payload, payloadOffset, newLength);
                return true;
            }
        }
    }
}

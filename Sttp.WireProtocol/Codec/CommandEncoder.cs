using System;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace CTP
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
        private int m_fragmentID = 0;

        /// <summary>
        /// A buffer to use to for all of the packets.
        /// </summary>
        private byte[] m_buffer;

        private const int BufferOffset = 35;

        /// <summary>
        /// The desired number of bytes before data is automatically flushed via <see cref="NewPacket"/>
        /// </summary>
        public CommandEncoder()
        {
            m_sessionDetails = new SessionDetails();
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
        public void SendBinaryCommand(int rawCode, byte[] payload, int position, int length)
        {
            payload.ValidateParameters(position, length);
            EnsureCapacity(BufferOffset + 1 + length);
            Array.Copy(payload, position, m_buffer, BufferOffset, length);

            CtpHeader header;
            int headerOffset = BufferOffset;
            if (rawCode == 0)
            {
                header = CtpHeader.CommandBinary0;
            }
            else if (rawCode == 1)
            {
                header = CtpHeader.CommandBinary1;
            }
            else
            {
                header = CtpHeader.CommandRawInt32;
                m_buffer[BufferOffset - 4] = (byte)(rawCode >> 24);
                m_buffer[BufferOffset - 3] = (byte)(rawCode >> 16);
                m_buffer[BufferOffset - 2] = (byte)(rawCode >> 8);
                m_buffer[BufferOffset - 1] = (byte)(rawCode);
                headerOffset -= 4;
                length += 4;
            }
            EncodeAndSend(header, m_buffer, headerOffset, length);
        }

        /// <summary>
        /// Encodes and sends the supplied command to the client.
        /// </summary>
        /// <param name="command">The command to send.</param>
        public void SendDocumentCommands(DocumentCommandBase command)
        {
            var writer = new CtpDocumentWriter(command.CommandName);
            command.Save(writer);
            SendDocumentCommands(writer);
        }

        /// <summary>
        /// Encodes and sends the data specified in <see cref="writer"/>. It's recommended to use
        /// the other overload that contains <see cref="DocumentCommandBase"/> if one exists.
        /// </summary>
        /// <param name="writer">The data to send.</param>
        public void SendDocumentCommands(CtpDocumentWriter writer)
        {
            CtpDocument data = writer.ToCtpDocument();
            EnsureCapacity(BufferOffset + data.Length);
            data.CopyTo(m_buffer, BufferOffset);

            EncodeAndSend(CtpHeader.CommandDocument, m_buffer, BufferOffset, data.Length);
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
        /// <param name="header">The existing header word</param>
        /// <param name="buffer"></param>
        /// <param name="offset">the offset of the buffer</param>
        /// <param name="length">The length of the data to send</param>
        private void EncodeAndSend(CtpHeader header, byte[] buffer, int offset, int length)
        {
            if (length > m_sessionDetails.MaximumCommandSize)
            {
                throw new Exception("This packet is too large to send, if this is a legitimate size, increase the MaxPacketSize.");
            }


            if (m_sessionDetails.SupportsDeflate && length >= m_sessionDetails.DeflateThreshold)
            {
                if (TryCompressPayload(buffer, offset, length, out int newSize, out uint checksum))
                {
                    header |= CtpHeader.IsCompressed;
                    offset -= 8;
                    buffer[offset + 0] = (byte)(length >> 24);
                    buffer[offset + 1] = (byte)(length >> 16);
                    buffer[offset + 2] = (byte)(length >> 8);
                    buffer[offset + 3] = (byte)(length >> 0);

                    buffer[offset + 4] = (byte)(checksum >> 24);
                    buffer[offset + 5] = (byte)(checksum >> 16);
                    buffer[offset + 6] = (byte)(checksum >> 8);
                    buffer[offset + 7] = (byte)(checksum >> 0);
                    length = newSize + 8;
                }
            }

            int packetLength = 2 + length;

            if (packetLength <= m_sessionDetails.MaximumPacketSize)
            {
                //This packet doesn't have to be fragmented.
                offset -= 2;
                header |= (CtpHeader)packetLength;
                buffer[offset + 0] = (byte)((ushort)header >> 8);
                buffer[offset + 1] = (byte)((ushort)header);
                SendNewPacket(buffer, offset, packetLength);
            }
            else
            {
                header |= CtpHeader.IsFragmented;

                uint checksum = Crc32.Compute(buffer, offset, length);

                offset -= 8;
                buffer[offset + 0] = (byte)(length >> 24);
                buffer[offset + 1] = (byte)(length >> 16);
                buffer[offset + 2] = (byte)(length >> 8);
                buffer[offset + 3] = (byte)(length >> 0);

                buffer[offset + 4] = (byte)(checksum >> 24);
                buffer[offset + 5] = (byte)(checksum >> 16);
                buffer[offset + 6] = (byte)(checksum >> 8);
                buffer[offset + 7] = (byte)(checksum >> 0);

                SendFragment(header, buffer, offset, length + 8);
            }
        }

        private void SendFragment(CtpHeader header, byte[] buffer, int offset, int length)
        {
            int bytesSentPerFragment = m_sessionDetails.MaximumPacketSize - 10;
            int bytesToSendThisFragment = Math.Min(bytesSentPerFragment, length);
            int packetLength = bytesToSendThisFragment + 10;

            int totalFragments = (length + bytesSentPerFragment - 1) / bytesSentPerFragment; //Adding (bytesSentPerFragment - 1) with integer division means round up.
            int currentFragment = 0;

            if (totalFragments > ushort.MaxValue)
                throw new OverflowException();

            int fragmentID = Interlocked.Increment(ref m_fragmentID);

            CtpHeader headerToSend = header | (CtpHeader)packetLength;

            buffer[offset - 10] = (byte)((ushort)headerToSend >> 8);
            buffer[offset - 9] = (byte)((ushort)headerToSend);
            buffer[offset - 8] = (byte)(fragmentID >> 24);
            buffer[offset - 7] = (byte)(fragmentID >> 16);
            buffer[offset - 6] = (byte)(fragmentID >> 8);
            buffer[offset - 5] = (byte)(fragmentID >> 0);
            buffer[offset - 4] = (byte)(currentFragment >> 8);
            buffer[offset - 3] = (byte)(currentFragment);
            buffer[offset - 2] = (byte)(totalFragments >> 8);
            buffer[offset - 1] = (byte)(totalFragments);

            SendNewPacket(buffer, offset - 10, packetLength);

            offset += bytesToSendThisFragment;
            length -= bytesToSendThisFragment;

            while (length > 0)
            {
                bytesToSendThisFragment = Math.Min(bytesSentPerFragment, length);
                packetLength = bytesToSendThisFragment + 10;
                currentFragment++;

                headerToSend = header | (CtpHeader)packetLength;

                buffer[offset - 10] = (byte)((ushort)headerToSend >> 8);
                buffer[offset - 9] = (byte)((ushort)headerToSend);
                buffer[offset - 8] = (byte)(fragmentID >> 24);
                buffer[offset - 7] = (byte)(fragmentID >> 16);
                buffer[offset - 6] = (byte)(fragmentID >> 8);
                buffer[offset - 5] = (byte)(fragmentID >> 0);
                buffer[offset - 4] = (byte)(currentFragment >> 8);
                buffer[offset - 3] = (byte)(currentFragment);
                buffer[offset - 2] = (byte)(totalFragments >> 8);
                buffer[offset - 1] = (byte)(totalFragments);

                SendNewPacket(buffer, offset - 10, packetLength);
                offset += bytesToSendThisFragment;
                length -= bytesToSendThisFragment;
            }
        }

        /// <summary>
        /// Attempts to compress the payload. If the compression is smaller than the original, the compressed
        /// data is copied over the original and <see cref="newLength"/> contains the length of the compressed data.
        /// Otherwise, this method returns false.
        /// </summary>
        /// <param name="buffer">the data to compress</param>
        /// <param name="offset">the offset</param>
        /// <param name="length">the length of the payload</param>
        /// <param name="newLength">the length of the compressed data if successful, -1 if the compression failed.</param>
        /// <param name="checksum">A computed CRC32 of compressed data if compression is successful.</param>
        /// <returns></returns>
        private bool TryCompressPayload(byte[] buffer, int offset, int length, out int newLength, out uint checksum)
        {
            using (var ms = new MemoryStream())
            {
                using (var deflate = new DeflateStream(ms, CompressionMode.Compress, true))
                {
                    deflate.Write(buffer, offset, length);
                }

                //Verifies that there was a size reduction with compression.
                if (ms.Position + 8 >= length)
                {
                    newLength = -1;
                    checksum = 0;
                    return false;
                }

                checksum = Crc32.Compute(buffer, offset, length);
                newLength = (int)ms.Position;
                ms.Position = 0;
                ms.Read(buffer, offset, newLength);
                return true;
            }
        }
    }
}

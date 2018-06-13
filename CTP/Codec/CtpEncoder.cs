using System;
using System.IO;
using System.IO.Compression;
using GSF;
using GSF.IO;
using GSF.IO.Checksums;

namespace CTP
{
    /// <summary>
    /// Responsible for encoding each command into bytes that can be sent on the socket. 
    /// This class will compress packets if needed.
    /// </summary>
    public class CtpEncoder
    {
        /// <summary>
        /// Occurs when a packet of data must be sent on the wire. This is called immediately
        /// after completing each packet.
        /// </summary>
        public event Action<byte[], int, int> NewPacket;

        /// <summary>
        /// Options for how commands will be compressed.
        /// </summary>
        private EncoderOptions m_encoderOptions;

        /// <summary>
        /// A buffer to use for all of the packets.
        /// </summary>
        private byte[] m_buffer;

        /// <summary>
        /// A stream for compressing data.
        /// </summary>
        private MemoryStream m_compressionStream = new MemoryStream();

        /// <summary>
        /// A reserved amount of packet overhead
        /// </summary>
        private const int BufferOffset = 1 + 3 + 1 + 20; //20, extra space for compression overhead.

        /// <summary>
        /// The desired number of bytes before data is automatically flushed via <see cref="NewPacket"/>
        /// </summary>
        public CtpEncoder()
        {
            m_encoderOptions = new EncoderOptions();
            m_buffer = new byte[128];
        }

        /// <summary>
        /// Modify certain serialization options.
        /// </summary>
        public EncoderOptions Options => m_encoderOptions;

        private void SendNewPacket(byte[] buffer, int position, int length)
        {
            NewPacket?.Invoke(buffer, position, length);
        }

        /// <summary>
        /// Sends a command.
        /// </summary>
        /// <param name="channelCode">indicates the content type for this data.</param>
        /// <param name="payload"></param>
        public void Send(CtpChannelCode channelCode, CtpDocument payload)
        {
            Send(channelCode, payload.ToArray());
        }

        /// <summary>
        /// Sends a command.
        /// </summary>
        /// <param name="channelCode">indicates the content type for this data.</param>
        /// <param name="payload"></param>
        public void Send(CtpChannelCode channelCode, byte[] payload)
        {
            Send(channelCode, payload, 0, payload.Length);
        }

        /// <summary>
        /// Sends data.
        /// </summary>
        /// <param name="channelCode">indicates the content type for this data.</param>
        /// <param name="payload">the byte payload to send.</param>
        /// <param name="offset">the offset in <see cref="payload"/></param>
        /// <param name="length">the length of the payload.</param>
        public void Send(CtpChannelCode channelCode, byte[] payload, int offset, int length)
        {
            payload.ValidateParameters(offset, length);

            //In case of an overflow exception.
            if (length > m_encoderOptions.MaximumCommandSize ||
                1 + 3 + 1 + length > m_encoderOptions.MaximumCommandSize)
                throw new Exception("This packet is too large to send, if this is a legitimate size, increase the MaxPacketSize.");

            EnsureCapacity(BufferOffset + length);
            Array.Copy(payload, offset, m_buffer, BufferOffset, length);

            CtpHeader header = CtpHeader.None;
            header = header.SetChannelCode(channelCode);

            int headerOffset = BufferOffset;
            if (m_encoderOptions.SupportsDeflate && length >= m_encoderOptions.DeflateThreshold)
            {
                if (TryCompressPayload(m_buffer, headerOffset, length, out int newSize, out uint uncompressedChecksum))
                {
                    header |= CtpHeader.IsCompressed;
                    headerOffset -= 8;
                    m_buffer[headerOffset + 0] = (byte)(length >> 24);
                    m_buffer[headerOffset + 1] = (byte)(length >> 16);
                    m_buffer[headerOffset + 2] = (byte)(length >> 8);
                    m_buffer[headerOffset + 3] = (byte)(length >> 0);
                    m_buffer[headerOffset + 4] = (byte)(uncompressedChecksum >> 24);
                    m_buffer[headerOffset + 5] = (byte)(uncompressedChecksum >> 16);
                    m_buffer[headerOffset + 6] = (byte)(uncompressedChecksum >> 8);
                    m_buffer[headerOffset + 7] = (byte)(uncompressedChecksum >> 0);
                    length = newSize + 8;
                }
            }

            headerOffset--;
            length++;
            m_buffer[headerOffset] = (byte)header;

            if (length + 1 <= 254)
            {
                headerOffset--;
                length++;
                m_buffer[headerOffset] = (byte)length;
            }
            else
            {
                length += 4;
                headerOffset -= 4;
                if ((length >> 24) != 0)
                    throw new Exception("The compressed size of this packet exceeds the maximum command size.");

                m_buffer[headerOffset + 0] = 255;
                m_buffer[headerOffset + 1] = (byte)(length >> 16);
                m_buffer[headerOffset + 2] = (byte)(length >> 8);
                m_buffer[headerOffset + 3] = (byte)(length);
            }
            SendNewPacket(m_buffer, headerOffset, length);
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
            m_compressionStream.SetLength(0);
            using (var deflate = new DeflateStream(m_compressionStream, CompressionMode.Compress, true))
            {
                deflate.Write(buffer, offset, length);
            }

            //Verifies that there was a size reduction with compression.
            if (m_compressionStream.Position + 8 >= length)
            {
                newLength = -1;
                checksum = 0;
                return false;
            }

            checksum = Crc32.Compute(buffer, offset, length);
            newLength = (int)m_compressionStream.Position;
            m_compressionStream.Position = 0;
            m_compressionStream.ReadAll(buffer, offset, newLength);
            return true;
        }
    }
}

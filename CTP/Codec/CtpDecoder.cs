using System;
using System.IO;
using System.IO.Compression;
using GSF;
using GSF.IO;
using GSF.IO.Checksums;

namespace CTP
{
    /// <summary>
    /// Decodes an incoming byte stream into a series of command objects. This class will align packets, reassemble fragments, and decompress packets. 
    /// </summary>
    internal class CtpDecoder
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
        /// A buffer for extracting a compressed packet.
        /// </summary>
        private byte[] m_compressionBuffer;

        public CtpDecoderResults Results = new CtpDecoderResults();

        private EncoderOptions m_encoderOptions;

        public CtpDecoder()
        {
            m_encoderOptions = new EncoderOptions();
            m_compressionBuffer = new byte[128];
            m_inboundBuffer = new byte[128];
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
        public bool ReadCommand()
        {
            Results.SetInvalid();

            if (m_inboundBufferLength < 1)
                return false;

            bool isCompressed;
            int packetLength;
            int position = m_inboundBufferCurrentPosition;
            byte[] buffer = m_inboundBuffer;

            packetLength = buffer[position];
            position++;
            if (packetLength == 255)
            {
                if (m_inboundBufferLength < 4)
                    return false;
                packetLength = buffer[position] << 16 | buffer[position + 1] << 8 | buffer[position + 2];
                position += 3;
            }

            if (packetLength > m_encoderOptions.MaximumCommandSize)
                throw new Exception("Command size is too large");

            if (m_inboundBufferLength < packetLength)
                return false;

            CtpHeader header = (CtpHeader)buffer[position];
            position++;
            isCompressed = (header & CtpHeader.IsCompressed) != 0;

            uint requestID = (uint)ToInt32(buffer, position);
            position += 4;

            int length = packetLength - (position - m_inboundBufferCurrentPosition);
            if (isCompressed)
            {
                //Decompresses the data.
                int inflatedSize = ToInt32(buffer, position);
                uint checksum = (uint)ToInt32(buffer, position + 4);

                //Only the payload is compressed, but some payload has to be copied to the decompressed block.
                if (m_compressionBuffer.Length < inflatedSize)
                {
                    m_compressionBuffer = new byte[inflatedSize];
                }

                var ms = new MemoryStream(buffer);
                ms.Position = position + 8;
                using (var inflate = new DeflateStream(ms, CompressionMode.Decompress, true))
                {
                    inflate.ReadAll(m_compressionBuffer, 0, inflatedSize);
                }

                if (checksum != Crc32.Compute(m_compressionBuffer, 0, inflatedSize))
                {
                    throw new InvalidOperationException("Decompression checksum failed.");
                }

                buffer = m_compressionBuffer;
                position = 0;
                length = inflatedSize;
            }
            buffer.ValidateParameters(position, length);

            byte[] results;
            results = new byte[length];
            Array.Copy(buffer, position, results, 0, length);
            Results.SetRaw(results);
            m_inboundBufferCurrentPosition += packetLength;
            m_inboundBufferLength -= packetLength;
            return true;
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

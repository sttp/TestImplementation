using System;
using System.IO;
using GSF;

namespace CTP.Net
{
    public class CtpStream
    {
        /// <summary>
        /// A buffer to use for all of the packets.
        /// </summary>
        private byte[] m_buffer;

        /// <summary>
        /// A reserved amount of packet overhead
        /// </summary>
        private const int BufferOffset = 1 + 3 + 1 + 20; //20, extra space for compression overhead.

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

        private Stream m_stream;

        /// <summary>
        /// A buffer used to read data from the underlying stream.
        /// </summary>
        private byte[] m_streamReadBuffer = new byte[3000];

        public CtpStream(Stream stream)
        {
            m_stream = stream;
            m_buffer = new byte[128];
            m_inboundBuffer = new byte[128];
        }

        /// <summary>
        /// Commands larger than this will cause protocol exceptions. 
        /// </summary>
        public int MaximumPayloadSize { get; set; } = 1_000_000;

        /// <summary>
        /// Writes a payload to the underlying stream.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="payload"></param>
        public void Write(byte channel, byte[] payload)
        {
            Write(channel, payload, 0, payload.Length);
        }

        /// <summary>
        /// Writes a payload to the underlying stream.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="payload">the byte payload to send.</param>
        /// <param name="offset">the offset in <see cref="payload"/></param>
        /// <param name="length">the length of the payload.</param>
        public void Write(byte channel, byte[] payload, int offset, int length)
        {
            payload.ValidateParameters(offset, length);
            if (channel > 127)
                throw new ArgumentOutOfRangeException(nameof(channel), "Cannot be greater than 127");

            //In case of an overflow exception.
            if (length > MaximumPayloadSize)
                throw new Exception("This packet is too large to send, if this is a legitimate size, increase the MaxPacketSize.");

            EnsureCapacity(BufferOffset + length);

            Array.Copy(payload, offset, m_buffer, BufferOffset, length);

            byte header = channel;
            int headerOffset = BufferOffset;
            headerOffset--;
            length++;
            m_buffer[headerOffset] = (byte)header;

            if (length + 1 <= 254)
            {
                length++;
                headerOffset--;
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
                m_buffer[headerOffset + 3] = (byte)length;
            }
            m_stream.Write(m_buffer, headerOffset, length);
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


        public CtpPacket Read()
        {
            CtpPacket packet;
            while (!InternalRead(out packet))
            {
                int length = m_stream.Read(m_streamReadBuffer, 0, m_streamReadBuffer.Length);
                if (length == 0)
                    throw new EndOfStreamException("The stream has been shutdown");

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

                Array.Copy(m_streamReadBuffer, 0, m_inboundBuffer, m_inboundBufferLength, length);
                m_inboundBufferLength += length;
            }
            return packet;
        }

        /// <summary>
        /// Gets the next data packet. This method should be in a while loop, decoding all
        /// messages before the next block of data is added to the decoder via <see cref="FillBuffer"/>
        /// 
        /// Reads the inbound raw buffer for the next full command. 
        /// Automatically decompresses and combines fragments and waits for the entire packet before
        /// responding as True.
        /// </summary>
        private bool InternalRead(out CtpPacket packet)
        {
            packet = null;
            if (m_inboundBufferLength < 1)
                return false;

            int packetLength;
            int position = m_inboundBufferCurrentPosition;
            packetLength = m_inboundBuffer[position];
            position++;
            if (packetLength == 255)
            {
                if (m_inboundBufferLength < 4)
                    return false;
                packetLength = m_inboundBuffer[position] << 16 | m_inboundBuffer[position + 1] << 8 | m_inboundBuffer[position + 2];
                position += 3;
            }

            if (packetLength > MaximumPayloadSize + 5)
                throw new Exception("Command size is too large");

            if (m_inboundBufferLength < packetLength)
                return false;

            byte channel = m_inboundBuffer[position];
            position++;

            int length = packetLength - (position - m_inboundBufferCurrentPosition);
            if (length > MaximumPayloadSize)
                throw new Exception("Command size is too large");

            m_inboundBuffer.ValidateParameters(position, length);

            byte[] results;
            results = new byte[length];
            Array.Copy(m_inboundBuffer, position, results, 0, length);

            packet = new CtpPacket((byte)(channel & 127), results);
            m_inboundBufferCurrentPosition += packetLength;
            m_inboundBufferLength -= packetLength;
            return true;
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTP.Net;
using GSF;

namespace CTP.IO
{
    /// <summary>
    /// Reads and Writes to a FileStream
    /// </summary>
    public class CtpFileStream : IDisposable
    {
        private Stream m_stream;
        private bool m_ownsStream;
        private byte[] m_tempBuffer;
        private bool m_disposed;

        /// <summary>
        /// Raw unprocessed data received from the client.
        /// </summary>
        private byte[] m_inboundBuffer;
        /// <summary>
        /// The current position of the inbound buffer.
        /// </summary>
        private int m_inboundBufferCurrentPosition;
        /// <summary>
        /// The number of unconsumed bytes in the inbound buffer.
        /// </summary>
        private int m_inboundBufferLength;

        public CtpFileStream(Stream stream, bool ownsStream)
        {
            m_stream = stream;
            m_ownsStream = ownsStream;
            m_tempBuffer = new byte[3000];
            m_inboundBuffer = new byte[128];
        }

        /// <summary>
        /// Reads the next command from the stream. 
        /// </summary>
        /// <returns></returns>
        public CtpCommand Read()
        {
            if (m_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            CtpCommand command;
            while (!ReadFromBuffer(out command))
            {
                if (m_disposed)
                    throw new ObjectDisposedException(GetType().FullName);
                int length = m_stream.Read(m_tempBuffer, 0, m_tempBuffer.Length);
                AppendToBuffer(m_tempBuffer, length);
            }
            return command;
        }

        /// <summary>
        /// The maximum packet size before a protocol parsing exceptions are raised. This defaults to 1MB.
        /// </summary>
        public int MaximumPacketSize { get; set; } = 1_000_000;

        private void AppendToBuffer(byte[] buffer, int length)
        {
            if (length == 0)
                throw new EndOfStreamException("The stream has been closed");

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

            Array.Copy(buffer, 0, m_inboundBuffer, m_inboundBufferLength, length);
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
        private bool ReadFromBuffer(out CtpCommand command)
        {
            //Note: The code in these two modules are identical:  
            command = null;
            if (m_disposed)
                return false;
            if (m_inboundBufferLength < 2)
                return false;

            byte header = m_inboundBuffer[m_inboundBufferCurrentPosition];
            if (header > 63)
                throw new Exception("Unknown Packet Header Version");

            bool longPayload = (header & 16) > 0;
            int length = 0;

            if (!longPayload)
            {
                length = BigEndian.ToInt16(m_inboundBuffer, m_inboundBufferCurrentPosition) & ((1 << 12) - 1);
            }
            else
            {
                if (m_inboundBufferLength < 4)
                    return false;
                length = BigEndian.ToInt32(m_inboundBuffer, m_inboundBufferCurrentPosition) & ((1 << 28) - 1);
            }

            if (length > MaximumPacketSize)
                throw new Exception("Command size is too large");

            if (m_inboundBufferLength < length)
                return false;

            byte[] payload;
            payload = new byte[length];
            Array.Copy(m_inboundBuffer, m_inboundBufferCurrentPosition, payload, 0, length);

            //ToDo: Consider not killing the connection on a command type casting error.
            command = CtpCommand.Load(payload, false);
            m_inboundBufferCurrentPosition += length;
            m_inboundBufferLength -= length;
            return true;
        }

        /// <summary>
        /// Writes a command to the underlying stream. Note: this method blocks until a packet has successfully been sent.
        /// </summary>
        public void Write(CtpCommand command)
        {
            if ((object)command == null)
                throw new ArgumentNullException(nameof(command));

            if (command.Length > MaximumPacketSize)
                throw new Exception("This command is too large to send, if this is a legitimate size, increase the MaxPacketSize.");

            if (m_disposed)
                throw new ObjectDisposedException("Stream has been closed");

            command.CopyTo(m_stream);
        }

        public void Dispose()
        {
            if (!m_disposed)
            {
                m_disposed = true;
                if (m_ownsStream)
                {
                    m_stream?.Dispose();
                }
            }
            
        }

    }
}

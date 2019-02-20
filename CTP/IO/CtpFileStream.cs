using System;
using System.Collections.Generic;
using System.IO;
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
        private CtpReadParser m_readParser;
        private CtpWriteParser m_write;

        public CtpFileStream(Stream stream, CtpCompressionMode mode, bool ownsStream)
        {
            m_write = new CtpWriteParser(mode, WriteInternal);
            m_readParser = new CtpReadParser();
            m_stream = stream;
            m_ownsStream = ownsStream;
            m_tempBuffer = new byte[3000];
        }

        /// <summary>
        /// Reads the next command from the stream. 
        /// </summary>
        /// <returns></returns>
        public CtpCommand Read()
        {
            if (m_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            CtpCommand packet;

            while (!m_readParser.ReadFromBuffer(out packet))
            {
                if (m_disposed)
                    throw new ObjectDisposedException(GetType().FullName);
                int length = m_stream.Read(m_tempBuffer, 0, m_tempBuffer.Length);
                if (length == 0)
                    return null;
                m_readParser.AppendToBuffer(m_tempBuffer, length);
            }

            return packet;
        }

        /// <summary>
        /// The maximum packet size before a protocol parsing exceptions are raised. This defaults to 1MB.
        /// </summary>
        public int MaximumPacketSize
        {
            get => m_readParser.MaximumPacketSize;
            set => m_write.MaximumPacketSize = m_readParser.MaximumPacketSize = value;
        }

        /// <summary>
        /// The maximum Number of schemas before the protocol will quite because a misuse is detected.
        /// </summary>
        public int MaximumSchemaCount
        {
            get => m_readParser.MaximumSchemeCount;
            set => m_write.MaximumSchemeCount = m_readParser.MaximumSchemeCount = value;
        }

        /// <summary>
        /// Writes a command to the underlying stream. Note: this method blocks until a packet has successfully been sent.
        /// </summary>
        public void Write(CtpCommand command)
        {
            m_write.Send(command);
        }

        private void WriteInternal(byte[] packet)
        {
            if ((object)packet == null)
                throw new ArgumentNullException(nameof(packet));

            if (packet.Length > MaximumPacketSize)
                throw new Exception("This command is too large to send, if this is a legitimate size, increase the MaxPacketSize.");

            if (m_disposed)
                throw new ObjectDisposedException("Stream has been closed");

            m_stream.Write(packet, 0, packet.Length);
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
                m_readParser.Dispose();
            }
        }

    }
}

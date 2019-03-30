using System;
using System.Collections.Generic;
using System.IO;
using GSF.IO;
using Ionic.Zlib;

namespace CTP.IO
{
    internal class CtpReadDecoder : IDisposable
    {
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

        /// <summary>
        /// The maximum packet size before a protocol parsing exceptions are raised. This defaults to 1MB.
        /// </summary>
        public int MaximumPacketSize { get; set; } = 1_000_000;

        /// <summary>
        /// The maximum Number of schemas before the protocol will quite because a misuse is detected.
        /// </summary>
        public int MaximumSchemeCount { get; set; } = 1_000;

        /// <summary>
        /// A local cache of all schemas
        /// </summary>
        private Dictionary<int, CtpCommandSchema> m_inboundSchemes;

        private MemoryStream m_stream;
        private DeflateStream m_inflate;

        public CtpReadDecoder()
        {
            m_inboundSchemes = new Dictionary<int, CtpCommandSchema>();
            m_inboundBuffer = new byte[128];
        }

        public void AppendToBuffer(byte[] buffer, int length)
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
        /// messages before the next block of data is added to the decoder via <see cref="AppendToBuffer"/>
        /// 
        /// Reads the inbound raw buffer for the next full command. 
        /// Automatically decompresses and combines fragments and waits for the entire packet before
        /// responding as True.
        /// </summary>
        public bool ReadFromBuffer(out CtpCommand packet)
        {
            TryAgain:
            int length = ReadFromBuffer(out packet, m_inboundBuffer, m_inboundBufferCurrentPosition, m_inboundBufferLength);
            if (length == 0)
                return false;
            m_inboundBufferLength -= length;
            m_inboundBufferCurrentPosition += length;
            if ((object)packet == null) //I read a packet that was a schema and has no data to return to the user.
                goto TryAgain;
            return true;
        }

        public int ReadFromBuffer(out CtpCommand packet, byte[] buffer, int current, int length)
        {
            //Note: The code in these two modules are identical:  
            packet = null;
            if (m_disposed)
                return 0;
            if (length < 2)
                return 0;

            if (!PacketMethods.TryReadPacket(buffer, current, length, MaximumPacketSize, out PacketContents payloadType, out int payloadFlags, out byte[] payloadBuffer, out int consumedLength))
                return 0;

            switch (payloadType)
            {
                case PacketContents.CommandSchema:
                    var scheme = new CtpCommandSchema(payloadBuffer);
                    m_inboundSchemes[payloadFlags] = scheme;
                    if (m_inboundSchemes.Count > MaximumSchemeCount)
                        throw new Exception("Too many schemes have been defined.");
                    break;
                case PacketContents.CommandSchemaWithData:
                        packet = new CtpCommand(payloadBuffer);
                    break;
                case PacketContents.CommandData:
                    if (m_inboundSchemes.TryGetValue(payloadFlags, out var commandSchema))
                        packet = new CtpCommand(commandSchema, payloadBuffer);
                    break;
                case PacketContents.CompressedDeflate:
                case PacketContents.CompressedZlib:
                    packet = Inflate(payloadType, payloadFlags, payloadBuffer);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return consumedLength;
        }

        public void Dispose()
        {
            m_disposed = true;
        }

        private CtpCommand Inflate(PacketContents mode, long inflatedSize, byte[] payload)
        {
            var rv = new byte[inflatedSize];

            switch (mode)
            {
                case PacketContents.CompressedDeflate:
                    using (var ms = new MemoryStream(payload))
                    using (var comp = new DeflateStream(ms, CompressionMode.Decompress, true))
                    {
                        comp.ReadAll(rv, 0, rv.Length);
                    }
                    break;
                case PacketContents.CompressedZlib:
                    if (m_stream == null)
                    {
                        m_stream = new MemoryStream();
                        m_inflate = new DeflateStream(m_stream, CompressionMode.Decompress);
                        m_inflate.FlushMode = FlushType.Sync;
                    }
                    m_stream.Write(payload, 0, payload.Length);
                    m_stream.Position = 0;
                    m_inflate.Read(rv, 0, rv.Length);
                    m_stream.SetLength(0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }

            ReadFromBuffer(out var packet, rv, 0, rv.Length);
            return packet;
        }

    }
}

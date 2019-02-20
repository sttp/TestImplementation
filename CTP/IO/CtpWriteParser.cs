using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF;
using Ionic.Zlib;
using CompressionMode = System.IO.Compression.CompressionMode;
using DeflateStream = System.IO.Compression.DeflateStream;

namespace CTP.IO
{
    internal class CtpWriteParser
    {
        private Dictionary<Guid, int> m_knownSchemas;
        private CtpCompressionMode m_compressionMode;
        private readonly Action<byte[]> m_send;
        private MemoryStream m_stream;
        private Ionic.Zlib.DeflateStream m_deflate;

        public CtpWriteParser(CtpCompressionMode mode, Action<byte[]> send)
        {
            m_knownSchemas = new Dictionary<Guid, int>();
            m_compressionMode = mode;
            m_send = send;
        }

        /// <summary>
        /// The maximum packet size before a protocol parsing exceptions are raised. This defaults to 1MB.
        /// </summary>
        public int MaximumPacketSize { get; set; } = 1_000_000;

        /// <summary>
        /// The maximum Number of schemas before the protocol will quite because a misuse is detected.
        /// </summary>
        public int MaximumSchemeCount { get; set; } = 1_000;

        public void Send(CtpCommand command)
        {
            int schemeRuntimeID;
            lock (m_knownSchemas)
            {
                if (!m_knownSchemas.TryGetValue(command.SchemaID, out schemeRuntimeID))
                {
                    schemeRuntimeID = m_knownSchemas.Count;
                    m_knownSchemas.Add(command.SchemaID, schemeRuntimeID);
                    Send(command.ToCommandSchema(schemeRuntimeID));
                }
            }
            Send(command.ToCommandData(schemeRuntimeID));
        }

        public void Send(byte[] packet)
        {
            switch (m_compressionMode)
            {
                case CtpCompressionMode.None:
                    m_send(packet);
                    break;
                case CtpCompressionMode.Deflate:
                    using (var ms = new MemoryStream())
                    {
                        using (var comp = new DeflateStream(ms, CompressionMode.Compress, true))
                        {
                            comp.Write(packet,0,packet.Length);
                        }
                        
                        m_send(CtpObjectWriter.CreatePacket(PacketContents.CompressedDeflate, packet.Length, ms.ToArray()));
                    }
                    break;
                case CtpCompressionMode.Zlib:
                    if (m_stream == null)
                    {
                        m_stream = new MemoryStream();
                        m_deflate = new Ionic.Zlib.DeflateStream(m_stream, Ionic.Zlib.CompressionMode.Compress);
                        m_deflate.FlushMode = FlushType.Sync;
                    }

                    m_stream.Write(BigEndian.GetBytes(packet.Length), 0, 4);
                    m_deflate.Write(packet,0,packet.Length);
                    m_deflate.Flush();
                    byte[] rv = m_stream.ToArray();
                    m_stream.SetLength(0);
                    m_send(CtpObjectWriter.CreatePacket(PacketContents.CompressedZlib, packet.Length, rv));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}

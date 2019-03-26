using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTP.Collection;
using GSF;
using Ionic.Zlib;
using CompressionMode = System.IO.Compression.CompressionMode;
using DeflateStream = System.IO.Compression.DeflateStream;

namespace CTP.IO
{
    /// <summary>
    /// This class will write all <see cref="CommandObject"/>s, ensure that a schema has been previously negotiated
    /// and it will also compress the data if configured to do so.
    /// </summary>
    internal class CtpWriteEncoder
    {
        private Dictionary<int, int> m_knownSchemas;
        private CtpCompressionMode m_compressionMode;
        private readonly Action<PooledBuffer> m_send;
        private MemoryStream m_stream;
        private Ionic.Zlib.DeflateStream m_deflate;

        public CtpWriteEncoder(CtpCompressionMode mode, Action<PooledBuffer> send)
        {
            m_knownSchemas = new Dictionary<int, int>();
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

        public void Send(CommandObject command)
        {
            if (!command.Schema.ProcessRuntimeID.HasValue)
            {
                Send(PacketMethods.CreatePacket(PacketContents.CommandSchemaWithData, 0, command.ToCommand().ToArray()));
            }
            else
            {
                int schemeRuntimeID;
                lock (m_knownSchemas)
                {
                    if (!m_knownSchemas.TryGetValue(command.Schema.ProcessRuntimeID.Value, out schemeRuntimeID))
                    {
                        if (command.Schema.ProcessRuntimeID == -1)
                            throw new Exception("Cannot serialize a schema that has not been defined in this process.");
                        schemeRuntimeID = m_knownSchemas.Count;
                        m_knownSchemas.Add(command.Schema.ProcessRuntimeID.Value, schemeRuntimeID);
                        Send(command.Schema.ToCommand(schemeRuntimeID));
                    }
                }
                Send(command.ToDataCommandPacket(schemeRuntimeID));
            }

        }

        private void Send(PooledBuffer packet)
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
                            packet.CopyTo(comp);
                        }
                        m_send(PacketMethods.CreatePacket(PacketContents.CompressedDeflate, packet.Length, ms.ToArray()));
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
                    packet.CopyTo(m_deflate);
                    m_deflate.Flush();
                    byte[] rv = m_stream.ToArray();
                    m_stream.SetLength(0);
                    m_send(PacketMethods.CreatePacket(PacketContents.CompressedZlib, packet.Length, rv));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}

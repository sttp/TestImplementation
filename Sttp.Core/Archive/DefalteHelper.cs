using System;
using System.IO;
using CTP;
using GSF.IO;
using Ionic.Zlib;
using Sttp.Codec;
using CompressionMode = System.IO.Compression.CompressionMode;
using DeflateStream = System.IO.Compression.DeflateStream;

namespace Sttp
{
    internal class DefalteHelper
    {
        private readonly CommandBeginCompressionStream m_compression;

        private readonly MemoryStream m_stream;
        private readonly Ionic.Zlib.DeflateStream m_deflate;
        private readonly Ionic.Zlib.DeflateStream m_inflate;

        public DefalteHelper(CommandBeginCompressionStream compression)
        {
            m_compression = compression;
            if (compression.EncodingMechanism == "zlib")
            {
                m_stream = new MemoryStream();
                m_deflate = new Ionic.Zlib.DeflateStream(m_stream, Ionic.Zlib.CompressionMode.Compress);
                m_deflate.FlushMode = FlushType.Sync;
                m_inflate = new Ionic.Zlib.DeflateStream(m_stream, Ionic.Zlib.CompressionMode.Decompress);
                m_inflate.FlushMode = FlushType.Sync;
            }
        }

        public CtpRaw Deflate(CtpCommand command)
        {
            if (m_stream == null)
            {
                using (var ms = new MemoryStream())
                {
                    using (var comp = new DeflateStream(ms, CompressionMode.Compress, true))
                    {
                        command.CopyTo(comp);
                    }
                    return new CtpRaw(ms.ToArray(), m_compression.ChannelCode);
                }
            }

            m_stream.Write(command.Length);
            command.CopyTo(m_deflate);
            m_deflate.Flush();
            byte[] rv = m_stream.ToArray();
            m_stream.SetLength(0);
            return new CtpRaw(rv, m_compression.ChannelCode);
        }

        public CtpCommand Inflate(CtpRaw raw)
        {
            if (raw.Channel != m_compression.ChannelCode)
                throw new Exception("Wrong channel code");
            if (m_stream == null)
            {
                using (var ms = new MemoryStream(raw.Payload))
                using (var ms2 = new MemoryStream())
                {
                    using (var comp = new DeflateStream(ms, CompressionMode.Decompress, true))
                    {
                        comp.CopyTo(ms2);
                    }

                    return CtpCommand.Load(ms2.ToArray(), false, null);
                }
            }


            m_stream.Write(raw.Payload, 0, raw.Payload.Length);
            m_stream.Position = 0;
            int length = m_stream.ReadInt32();
            byte[] rv = new byte[length];
            m_inflate.Read(rv, 0, rv.Length);
            m_stream.SetLength(0);
            return CtpCommand.Load(rv, false, null);
        }


    }
}
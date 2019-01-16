using System;
using System.IO;
using System.IO.Compression;
using CTP;
using Sttp.Codec;

namespace Sttp.Archive
{
    public class DefalteHelper
    {
        private readonly CommandBeginCompressionStream m_compression;

        public DefalteHelper(CommandBeginCompressionStream compression)
        {
            m_compression = compression;
        }
        public CtpRaw Deflate(CtpCommand command)
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

        public CtpCommand Inflate(CtpRaw raw)
        {
            if (raw.Channel != m_compression.ChannelCode)
                throw new Exception("Wrong channel code");
            using (var ms = new MemoryStream(raw.Payload))
            using (var ms2 = new MemoryStream())
            {
                using (var comp = new DeflateStream(ms, CompressionMode.Decompress, true))
                {
                    comp.CopyTo(ms2);
                }

                return CtpCommand.Load(ms2.ToArray(), false);
            }
        }


    }
}
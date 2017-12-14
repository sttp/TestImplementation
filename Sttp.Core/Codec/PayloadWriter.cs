using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Sttp.IO;

namespace Sttp.Codec
{
    public class PayloadWriter : ByteWriter
    {
        private CommandEncoder m_encoder;

        public PayloadWriter(CommandEncoder encoder)
            : base(15)
        {
            m_encoder = encoder;
        }

        public void Send(CommandCode command)
        {
            GetBuffer(out byte[] data, out int offset, out int length);

            m_encoder.EncodeAndSend(command, data, offset, length);
            Clear();
        }

    }
}


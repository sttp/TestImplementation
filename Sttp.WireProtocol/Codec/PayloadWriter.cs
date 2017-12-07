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
        {
            m_encoder = encoder;
        }

        public void Send(CommandCode command)
        {
            int length = UserData;
            int offset = UserDataPosition;

            m_encoder.EncodeAndSend(command, m_buffer, offset, length);
            Clear();
        }

    }
}


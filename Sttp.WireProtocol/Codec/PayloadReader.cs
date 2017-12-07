using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sttp.Codec
{
    public class PayloadReader : ByteReader
    {
        private SessionDetails m_sessionDetails;
        public CommandCode Command { get; private set; }
        public int Length { get; private set; }

        public PayloadReader(SessionDetails sessionDetails)
        {
            m_sessionDetails = sessionDetails;
        }

        internal void SetBuffer(CommandCode code, byte[] data, int position, int length)
        {
            base.SetBuffer(data, position, length);
            Command = code;
        }
    }
}

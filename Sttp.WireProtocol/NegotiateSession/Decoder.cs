using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.NegotiateSession
{
    public class Decoder
    {
        private SttpNamedSet ConnectionString;

        public CommandCode CommandCode => CommandCode.NegotiateSession;

        public void Fill(PayloadReader reader)
        {
            ConnectionString = reader.Read<SttpNamedSet>();
        }
    }
}

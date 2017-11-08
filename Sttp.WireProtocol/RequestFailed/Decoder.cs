using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.RequestFailed
{
    public class Decoder
    {
        public CommandCode FailedCommand;
        public string Reason;
        public string Details;

        public CommandCode CommandCode => CommandCode.RequestFailed;

        public void Fill(PacketReader reader)
        {
            FailedCommand = reader.Read<CommandCode>();
            Reason = reader.ReadString();
            Details = reader.ReadString();
        }
    }
}

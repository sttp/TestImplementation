using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    public class CommandRequestFailed
    {
        public CommandCode FailedCommand;
        public string Reason;
        public string Details;

        public CommandCode CommandCode => CommandCode.RequestFailed;

        public void Fill(PayloadReader reader)
        {
            FailedCommand = reader.Read<CommandCode>();
            Reason = reader.ReadString();
            Details = reader.ReadString();
        }
    }
}

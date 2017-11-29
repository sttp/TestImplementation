using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    public class CommandRequestFailed
    {
        public readonly CommandCode FailedCommand;
        public readonly string Reason;
        public readonly string Details;

        public CommandCode CommandCode => CommandCode.RequestFailed;

        public CommandRequestFailed(PayloadReader reader)
        {
            FailedCommand = (CommandCode)reader.ReadByte();
            Reason = reader.ReadString();
            Details = reader.ReadString();
        }
    }
}

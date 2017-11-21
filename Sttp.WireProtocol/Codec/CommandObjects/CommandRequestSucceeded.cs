using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    public class CommandRequestSucceeded
    {
        public readonly CommandCode CommandSucceeded;
        public readonly string Reason;
        public readonly string Details;

        public CommandCode CommandCode => CommandCode.RequestSucceeded;

        public CommandRequestSucceeded(PayloadReader reader)
        {
            CommandSucceeded = reader.Read<CommandCode>();
            Reason = reader.ReadString();
            Details = reader.ReadString();
        }
    }
}

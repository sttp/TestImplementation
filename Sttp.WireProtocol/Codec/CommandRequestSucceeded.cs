using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    public class CommandRequestSucceeded
    {
        public CommandCode CommandSucceeded;
        public string Reason;
        public string Details;

        public CommandCode CommandCode => CommandCode.RequestSucceeded;

        public void Fill(PayloadReader reader)
        {
            CommandSucceeded = reader.Read<CommandCode>();
            Reason = reader.ReadString();
            Details = reader.ReadString();
        }
    }
}

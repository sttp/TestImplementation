using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sttp.Codec;
using Sttp.Core;

namespace Sttp.Services
{
    public class SttpKeepAlive : ISttpCommandHandler
    {
        public List<string> CommandsHandled()
        {
            var lst = new List<string>();
            lst.Add("KeepAlive");
            return lst;
        }

        public void HandleCommand(CommandObjects command, WireCodec encoder)
        {
            if (command.CommandName != "KeepAlive")
                throw new Exception("This command is not supported");

            throw new NotImplementedException();
            //encoder.KeepAlive();
        }
    }
}

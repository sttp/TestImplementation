using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol.NegotiateSessionResponse
{
    public class CmdInstanceList : ICmd
    {
        public SubCommand SubCommand => SubCommand.InstanceList;
        public List<Tuple<string, string>> Instances;

        public void Load(PacketReader reader)
        {
            Instances = reader.ReadList<string, string>();
        }

    }
}
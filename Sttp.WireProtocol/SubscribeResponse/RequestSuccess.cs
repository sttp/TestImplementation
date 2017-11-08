using System;

namespace Sttp.WireProtocol.SubscribeResponse
{
    public class CmdRequestSuccess : ICmd
    {
        public SubCommand SubCommand => SubCommand.RequestSuccess;
        public string Reason;
        public string Details;

        public void Load(PacketReader reader)
        {
            Reason = reader.ReadString();
            Details = reader.ReadString();
        }

    }
}
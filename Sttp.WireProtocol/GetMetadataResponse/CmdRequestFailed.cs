using System;

namespace Sttp.WireProtocol.GetMetadataResponse
{
    public class CmdRequestFailed : ICmd
    {
        public SubCommand SubCommand => SubCommand.RequestFailed;
        public string Reason;
        public string Details;

        public void Load(PacketReader reader)
        {
            Reason = reader.ReadString();
            Details = reader.ReadString();
        }

    }
}
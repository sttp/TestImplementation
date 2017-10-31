using System;

namespace Sttp.WireProtocol.GetMetadataResponse
{
    public class CmdClear : ICmd
    {
        public SubCommand SubCommand => SubCommand.Clear;

        public void Load(PacketReader reader)
        {
        }

        CmdDatabaseVersion ICmd.DatabaseVersion => null;
        CmdAddColumn ICmd.AddColumn => null;
        CmdAddRow ICmd.AddRow => null;
        CmdAddTable ICmd.AddTable => null;
        CmdAddValue ICmd.AddValue => null;
        CmdClear ICmd.Clear => this;
        CmdDeleteRow ICmd.DeleteRow => null;

    }
}
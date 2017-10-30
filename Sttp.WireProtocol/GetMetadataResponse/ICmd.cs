namespace Sttp.WireProtocol.GetMetadataResponse
{
    public interface ICmd
    {
        SubCommand SubCommand { get; }

        void Load(PacketReader reader);

        CmdDatabaseVersion DatabaseVersion { get; }
        CmdAddColumn AddColumn { get; }
        CmdAddRow AddRow { get; }
        CmdAddTable AddTable { get; }
        CmdAddValue AddValue { get; }
        CmdClear Clear { get; }
        CmdDeleteRow DeleteRow { get; }
    }
}
namespace Sttp.WireProtocol.GetMetadata
{
    public interface ICmd
    {
        SubCommand SubCommand { get; }

        void Load(PacketReader reader);

        CmdDatabaseVersion DatabaseVersion { get; }
        CmdJoin Join { get; }
        CmdSelect Select { get; }
        CmdWhereCompare WhereCompare { get; }
        CmdWhereInString WhereInString { get; }
        CmdWhereInValue WhereInValue { get; }
        CmdWhereOperator WhereOperator { get; }
    }
}
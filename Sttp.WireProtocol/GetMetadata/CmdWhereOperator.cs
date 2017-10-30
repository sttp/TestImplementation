namespace Sttp.WireProtocol.GetMetadata
{
    public class CmdWhereOperator : ICmd
    {
        public SubCommand SubCommand => SubCommand.WhereOperator;
        public OperatorMethod OperatorCode;

        public void Load(PacketReader reader)
        {
            OperatorCode = reader.Read<OperatorMethod>();
        }

        CmdDatabaseVersion ICmd.DatabaseVersion => null;
        CmdJoin ICmd.Join => null;
        CmdSelect ICmd.Select => null;
        CmdWhereCompare ICmd.WhereCompare => null;
        CmdWhereInString ICmd.WhereInString => null;
        CmdWhereInValue ICmd.WhereInValue => null;
        CmdWhereOperator ICmd.WhereOperator => this;
    }
}
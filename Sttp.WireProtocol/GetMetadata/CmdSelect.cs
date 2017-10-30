namespace Sttp.WireProtocol.GetMetadata
{
    public class CmdSelect : ICmd
    {
        public SubCommand SubCommand => GetMetadata.SubCommand.Select;
        public short TableIndex;
        public short ColumnIndex;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            ColumnIndex = reader.ReadInt16();
        }

        CmdDatabaseVersion ICmd.DatabaseVersion => null;
        CmdJoin ICmd.Join => null;
        CmdSelect ICmd.Select => this;
        CmdWhereCompare ICmd.WhereCompare => null;
        CmdWhereInString ICmd.WhereInString => null;
        CmdWhereInValue ICmd.WhereInValue => null;
        CmdWhereOperator ICmd.WhereOperator => null;

    }
}
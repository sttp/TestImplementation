namespace Sttp.WireProtocol.GetMetadata
{
    public class CmdJoin : ICmd
    {
        public SubCommand SubCommand => GetMetadata.SubCommand.Join;
        public short TableIndex;
        public short ColumnIndex;
        public short ForeignTableIndex;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            ColumnIndex = reader.ReadInt16();
            ForeignTableIndex = reader.ReadInt16();
        }

        CmdDatabaseVersion ICmd.DatabaseVersion => null;
        CmdJoin ICmd.Join => this;
        CmdSelect ICmd.Select => null;
        CmdWhereCompare ICmd.WhereCompare => null;
        CmdWhereInString ICmd.WhereInString => null;
        CmdWhereInValue ICmd.WhereInValue => null;
        CmdWhereOperator ICmd.WhereOperator => null;
    }
}
namespace Sttp.WireProtocol.GetMetadata
{
    public class CmdWhereInString : ICmd
    {
        public SubCommand SubCommand => SubCommand.WhereInString;
        public short TableIndex;
        public short ColumnIndex;
        public string[] Items;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            ColumnIndex = reader.ReadInt16();
            Items = reader.ReadArray<string>();
        }

        CmdDatabaseVersion ICmd.DatabaseVersion => null;
        CmdJoin ICmd.Join => null;
        CmdSelect ICmd.Select => null;
        CmdWhereCompare ICmd.WhereCompare => null;
        CmdWhereInString ICmd.WhereInString => this;
        CmdWhereInValue ICmd.WhereInValue => null;
        CmdWhereOperator ICmd.WhereOperator => null;
    }
}
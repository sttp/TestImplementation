namespace Sttp.WireProtocol.GetMetadata
{
    public class CmdWhereInValue : ICmd
    {
        public SubCommand SubCommand => SubCommand.WhereInValue;
        public short TableIndex;
        public short ColumnIndex;
        public SttpValue[] Items;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            ColumnIndex = reader.ReadInt16();
            Items = reader.ReadArray<SttpValue>();
        }

        CmdDatabaseVersion ICmd.DatabaseVersion => null;
        CmdJoin ICmd.Join => null;
        CmdSelect ICmd.Select => null;
        CmdWhereCompare ICmd.WhereCompare => null;
        CmdWhereInString ICmd.WhereInString => null; 
        CmdWhereInValue ICmd.WhereInValue => this;
        CmdWhereOperator ICmd.WhereOperator => null;
    }
}
namespace Sttp.WireProtocol.GetMetadata
{
    public class CmdWhereCompare : ICmd
    {
        public SubCommand SubCommand => SubCommand.WhereCompare;
        public short TableIndex;
        public short ColumnIndex;
        public CompareMethod CompareMethod;
        public byte[] Item;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            ColumnIndex = reader.ReadInt16();
            CompareMethod = reader.Read<CompareMethod>();
            Item = reader.ReadBytes();
        }

        CmdDatabaseVersion ICmd.DatabaseVersion => null;
        CmdJoin ICmd.Join => null;
        CmdSelect ICmd.Select => null;
        CmdWhereCompare ICmd.WhereCompare => this;
        CmdWhereInString ICmd.WhereInString => null;
        CmdWhereInValue ICmd.WhereInValue => null;
        CmdWhereOperator ICmd.WhereOperator => null;

    }
}
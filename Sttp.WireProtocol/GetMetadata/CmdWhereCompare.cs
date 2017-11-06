namespace Sttp.WireProtocol.GetMetadata
{
    public class CmdWhereCompare : ICmd
    {
        public SubCommand SubCommand => SubCommand.WhereCompare;
        public string TableName;
        public string ColumnName;
        public CompareMethod CompareMethod;
        public SttpValue Item;

        public void Load(PacketReader reader)
        {
            TableName = reader.ReadString();
            ColumnName = reader.ReadString();
            CompareMethod = reader.Read<CompareMethod>();
            Item = reader.Read<SttpValue>();
        }

    }
}
namespace Sttp.WireProtocol.GetMetadata
{
    public class CmdWhereInValue : ICmd
    {
        public SubCommand SubCommand => SubCommand.WhereInValue;
        public string TableName;
        public string ColumnName;
        public SttpValueSet Items;

        public void Load(PacketReader reader)
        {
            TableName = reader.ReadString();
            ColumnName = reader.ReadString();
            Items = reader.Read<SttpValueSet>();
        }
    }
}
namespace Sttp.WireProtocol.GetMetadata
{
    public class CmdWhereInString : ICmd
    {
        public SubCommand SubCommand => SubCommand.WhereInString;
        public string TableName;
        public string ColumnName;
        public bool AreRegularExpressions;
        public string[] Items;

        public void Load(PacketReader reader)
        {
            TableName = reader.ReadString();
            ColumnName = reader.ReadString();
            AreRegularExpressions = reader.ReadBoolean();
            Items = reader.ReadArray<string>();
        }
    }
}
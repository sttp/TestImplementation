namespace Sttp.WireProtocol.GetMetadata
{
    public class CmdSelect : ICmd
    {
        public SubCommand SubCommand => GetMetadata.SubCommand.Select;
        public string TableName;
        public string ColumnName;
        public string AliasName;

        public void Load(PacketReader reader)
        {
            TableName = reader.ReadString();
            ColumnName = reader.ReadString();
            AliasName = reader.ReadString();
        }

    }
}
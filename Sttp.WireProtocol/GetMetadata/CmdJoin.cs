namespace Sttp.WireProtocol.GetMetadata
{
    public class CmdJoin : ICmd
    {
        public SubCommand SubCommand => GetMetadata.SubCommand.Join;
        public string TableName;
        public string ColumnName;
        public string ForeignTableName;
        public string TableAlias;
        public bool  IsLeftJoin;

        public void Load(PacketReader reader)
        {
            TableName = reader.ReadString();
            ColumnName = reader.ReadString();
            ForeignTableName = reader.ReadString();
            TableAlias = reader.ReadString();
            IsLeftJoin = reader.ReadBoolean();
        }
    }
}
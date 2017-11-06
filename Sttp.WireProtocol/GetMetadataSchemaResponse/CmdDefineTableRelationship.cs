namespace Sttp.WireProtocol.GetMetadataSchemaResponse
{
    public class CmdDefineTableRelationship : ICmd
    {
        public SubCommand SubCommand => GetMetadataSchemaResponse.SubCommand.DefineTableRelationship;

        /// <summary>
        /// The table that has the column with the foreign key.
        /// </summary>
        public string TableName;
        /// <summary>
        /// The name of the column with the foreign key.
        /// </summary>
        public string ColumnName;
        /// <summary>
        /// The foreign table that has the key. 
        /// It could be itself of course.
        /// </summary>
        public string ForeignTableName;

        public void Load(PacketReader reader)
        {
            TableName = reader.ReadString();
            ColumnName = reader.ReadString();
            ForeignTableName = reader.ReadString();
        }
    }
}
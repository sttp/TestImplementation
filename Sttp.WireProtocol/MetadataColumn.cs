namespace Sttp.WireProtocol
{
    public class MetadataColumn
    {
        public MetadataTable Table;
        public string ColumnName;
        public ValueType ColumnType; //The type the column fields are encoded with. 

        public byte[] Encode(object value)
        {
            return null;
        }
    }
}
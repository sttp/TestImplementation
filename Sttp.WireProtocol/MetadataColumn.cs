namespace Sttp.WireProtocol
{
    public class MetadataColumn
    {
        public string ColumnName;
        public ValueType ColumnType; //The type the column fields are encoded with. 

        public MetadataColumn(string columnName, ValueType columnType)
        {
            ColumnName = columnName;
            ColumnType = columnType;
        }

        public byte[] Encode(object value)
        {
            return null;
        }
    }
}
namespace Sttp.WireProtocol
{
    public class MetadataColumn
    {
        public readonly int ColumnID;
        public readonly string ColumnName;
        public readonly ValueType ColumnType; //The type the column fields are encoded with. 

        public MetadataColumn(int columnID, string columnName, ValueType columnType)
        {
            ColumnID = columnID;
            ColumnName = columnName;
            ColumnType = columnType;
        }

        public byte[] Encode(object value)
        {
            return null;
        }
    }
}
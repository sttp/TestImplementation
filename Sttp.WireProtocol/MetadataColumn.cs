namespace Sttp.WireProtocol
{
    public class MetadataColumn
    {
        public readonly int ColumnID;
        public readonly ValueType ColumnType; //The type the column fields are encoded with. 

        public MetadataColumn(int columnID, ValueType columnType)
        {
            ColumnID = columnID;
            ColumnType = columnType;
        }

        public byte[] Encode(object value)
        {
            return null;
        }
    }
}
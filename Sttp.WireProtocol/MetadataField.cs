namespace Sttp.WireProtocol
{
    public class MetadataField
    {
        public MetadataColumn Column;
        public byte[] Value;
        public bool IsDeleted;

        public MetadataField(MetadataColumn column, object value)
        {
            Column = column;
            Value = Column.Encode(value);
        }
    }
}
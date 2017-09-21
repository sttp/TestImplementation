using ValueType = Sttp.WireProtocol.ValueType;
namespace Sttp.Data.Publisher
{
    public class MetadataColumn
    {
        /// <summary>
        /// The 0 based index of this column in the DataSet
        /// </summary>
        public readonly int DataSetColumnIndex;
        /// <summary>
        /// The 0 based index of this column in the data table.
        /// </summary>
        public readonly int ColumnIndex; 

        public readonly ValueType ColumnType; //The type the column fields are encoded with. 

        public MetadataColumn(int dataSetColumnIndex, int columnIndex, ValueType columnType)
        {
            DataSetColumnIndex = dataSetColumnIndex;
            ColumnIndex = columnIndex;
            ColumnType = columnType;
        }

        public byte[] Encode(object value)
        {
            return null;
        }
    }
}
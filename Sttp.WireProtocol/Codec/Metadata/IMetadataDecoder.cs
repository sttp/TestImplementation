using System;

namespace Sttp.WireProtocol.Data
{
    public interface IMetadataDecoder
    {
        void AddColumn(out int tableIndex, out int columnIndex, out string columnName, out ValueType columnType);
        void AddTable(out Guid instanceID, out long transactionID, out string tableName, out int tableIndex, out bool isMappedToDataPoint);
        void AddValue(out int tableIndex, out int columnIndex, out int rowIndex, out byte[] value);
        void BeginCommand(byte[] buffer, int position, int length);
        void Clear();
        void DeleteColumn(out int tableIndex, out int columnIndex);
        void DeleteRow(out int tableIndex, out int rowIndex);
        void DeleteTable(out int tableIndex);
        MetadataCommand NextCommand();
        void UpdateTable(out int tableIndex, out long transactionID);
    }
}
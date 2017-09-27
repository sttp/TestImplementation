using System;

namespace Sttp.WireProtocol.Data
{
    public interface IMetadataEncoder
    {
        void AddColumn(int tableIndex, int columnIndex, string columnName, ValueType columnType);
        void AddTable(Guid instanceID, long transactionID, string tableName, int tableIndex, bool isMappedToDataPoint);
        void AddValue(int tableIndex, int columnIndex, int rowIndex, byte[] value);
        void BeginCommand();
        void Clear();
        void DeleteColumn(int tableIndex, int columnIndex);
        void DeleteRow(int tableIndex, int rowIndex);
        void DeleteTable(int tableIndex);
        byte[] EndCommand();
        void ResyncTable(int tableIndex, Guid cachedInstanceId, long transactionId);
        void SelectAllTablesWithSchema();
        void UpdateTable(int tableIndex, long transactionID);
    }
}
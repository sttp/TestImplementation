using System;

namespace Sttp.WireProtocol.Data
{
    public interface IMetadataEncoder
    {
        void BeginCommand();
        byte[] EndCommand();

        void MetadataChanged(int tableIndex, Guid instanceID, long transactionID);

        void UseTable(int tableIndex);
        void AddTable(Guid instanceID, long transactionID, string tableName, bool isMappedToDataPoint);
        void UpdateTable(long transactionID);
        void AddColumn(int columnIndex, string columnName, ValueType type);
        void AddValue(int columnIndex, int rowIndex, byte[] value);
        void DeleteRow(int rowIndex);


        void GetTable(int tableIndex, int[] columnList, string[] filterExpression);
        void SyncTable(int tableIndex, Guid cachedInstanceId, long transactionId, int[] columnList);
        void SelectAllTablesWithSchema();
    }
}
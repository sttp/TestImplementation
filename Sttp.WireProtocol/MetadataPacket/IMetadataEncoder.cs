using System;
using System.Collections.Generic;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.WireProtocol.Data
{
    public interface IMetadataEncoder
    {
        void BeginCommand();
        void EndCommand();

        #region [ Response Publisher to Subscriber ]

        void Clear();
        void AddTable(int tableIndex, string tableName, TableFlags tableFlags);
        void AddColumn(int tableIndex, int columnIndex, string columnName, ValueType columnType);
        void AddRow(int tableIndex, int rowIndex);
        void AddValue(int tableIndex, int columnIndex, int rowIndex, byte[] value);
        void DeleteRow(int tableIndex, int rowIndex);
        void DatabaseVersion(Guid majorVersion, long minorVersion);

        #endregion

        #region [ Request Subscriber to Publisher ]

        void GetTable(int tableIndex, int[] columnList, List<Tuple<int, string>> filterExpression);
        void GetQuery(List<Tuple<int, int>> columnList, List<Tuple<int, int, int>> joinFields, List<Tuple<int, int, string>> filterExpression);

        void SyncDatabase(Guid majorVersion, long minorVersion, List<Tuple<int, int>> columnList);
        void SyncTableOrQuery(Guid majorVersion, long minorVersion, List<Tuple<int, int>> columnList, List<Tuple<int, int>> criticalColumnList);

        void GetDatabaseSchema();
        void GetDatabaseVersion();

        #endregion

    }
}
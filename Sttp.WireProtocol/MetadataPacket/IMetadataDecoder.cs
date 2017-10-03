using System;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.WireProtocol.Data
{
    public interface IMetadataDecoder
    {
        void BeginCommand(byte[] buffer, int position, int length);
        MetadataCommand NextCommand();

        #region [ Response Publisher to Subscriber ]

        void UseTable(out int tableIndex);
        void AddTable(out Guid majorVersion, out long minorVersion, out string tableName, out TableFlags tableFlags);
        void AddColumn(out int columnIndex, out string columnName, out ValueType columnType);
        void AddValue(out int columnIndex, out int rowIndex, out byte[] value);
        void DeleteRow(out int rowIndex);
        void TableVersion(out int tableIndex, out Guid majorVersion, out long minorVersion);
        void AddRelationship(out int tableIndex, out int columnIndex, out int foreignTableIndex);


        #endregion

        #region [ Request Subscriber to Publisher ]

        void GetTable(out int tableIndex, out int[] columnList, out string[] filterExpression);
        void SyncTable(out int tableIndex, out Guid majorVersion, out long minorVersion, out int[] columnList);
        void SelectAllTablesWithSchema();
        void GetAllTableVersions();

        #endregion

    }
}
using System;
using System.Collections.Generic;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.WireProtocol.Data
{
    /// <summary>
    /// Encodes a metadata packet.
    /// </summary>
    public class MetadataEncoder : BaseEncoder
    {
        public override CommandCode Code => CommandCode.Metadata;
        private SessionDetails m_sessionDetails;

        public MetadataEncoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails) 
            : base(sendPacket,sessionDetails)
        {
        }

        #region [ Response Publisher to Subscriber ]

        public void Clear()
        {
            m_stream.Write(MetadataCommand.Clear);
        }

        public void AddTable(int tableIndex, string tableName, TableFlags tableFlags)
        {
            m_stream.Write(MetadataCommand.AddTable);
            m_stream.Write(tableIndex);
            m_stream.Write(tableName);
            m_stream.Write(tableFlags);
        }

        public void AddColumn(int tableIndex, int columnIndex, string columnName, ValueType columnType)
        {
            m_stream.Write(MetadataCommand.AddColumn);
            m_stream.Write(tableIndex);
            m_stream.Write(columnIndex);
            m_stream.Write(columnName);
            m_stream.Write(columnType);
        }

        public void AddRow(int tableIndex, int rowIndex)
        {
            m_stream.Write(MetadataCommand.AddRow);
            m_stream.Write(tableIndex);
            m_stream.Write(rowIndex);
        }

        public void AddValue(int tableIndex, int columnIndex, int rowIndex, byte[] value)
        {
            m_stream.Write(MetadataCommand.AddValue);
            m_stream.Write(tableIndex);
            m_stream.Write(columnIndex);
            m_stream.Write(rowIndex);
            m_stream.Write(value);
        }

        public void DeleteRow(int tableIndex, int rowIndex)
        {
            m_stream.Write(MetadataCommand.DeleteRow);
            m_stream.Write(tableIndex);
            m_stream.Write(rowIndex);
        }

        public void DatabaseVersion(Guid majorVersion, long minorVersion)
        {
            m_stream.Write(MetadataCommand.DatabaseVersion);
            m_stream.Write(majorVersion);
            m_stream.Write(minorVersion);
        }

        #endregion

        #region [ Request Subscriber to Publisher ]

        public void GetTable(int tableIndex, int[] columnList, List<Tuple<int, string>> filterExpression)
        {
            m_stream.Write(MetadataCommand.GetTable);
            m_stream.Write(tableIndex);
            m_stream.WriteArray(columnList);
            m_stream.WriteList(filterExpression);
        }

        public void GetQuery(List<Tuple<int, int>> columnList, List<Tuple<int, int, int>> joinFields, List<Tuple<int, int, string>> filterExpression)
        {
            m_stream.Write(MetadataCommand.GetQuery);
            m_stream.WriteList(columnList);
            m_stream.WriteList(joinFields);
            m_stream.WriteList(filterExpression);
        }

        public void SyncDatabase(Guid majorVersion, long minorVersion, List<Tuple<int, int>> columnList)
        {
            m_stream.Write(MetadataCommand.SyncDatabase);
            m_stream.Write(majorVersion);
            m_stream.Write(minorVersion);
            m_stream.WriteList(columnList);
        }

        public void SyncTableOrQuery(Guid majorVersion, long minorVersion, List<Tuple<int, int>> columnList, List<Tuple<int, int>> criticalColumnList)
        {
            m_stream.Write(MetadataCommand.SyncTableOrQuery);
            m_stream.Write(majorVersion);
            m_stream.Write(minorVersion);
            m_stream.WriteList(columnList);
            m_stream.WriteList(criticalColumnList);
        }

        public void GetDatabaseSchema()
        {
            m_stream.Write((byte)MetadataCommand.GetDatabaseSchema);
        }

        public void GetDatabaseVersion()
        {
            m_stream.Write((byte)MetadataCommand.GetDatabaseVersion);
        }

        #endregion

    }
}

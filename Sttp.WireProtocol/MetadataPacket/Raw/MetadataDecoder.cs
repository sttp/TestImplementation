using System;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.WireProtocol.Data.Raw
{
    public class MetadataDecoder : IMetadataDecoder
    {
        private MetadataClearParams m_clear = new MetadataClearParams();
        private MetadataAddTableParams m_addTable = new MetadataAddTableParams();
        private MetadataAddColumnParams m_addColumn = new MetadataAddColumnParams();
        private MetadataAddRowParams m_addRow = new MetadataAddRowParams();
        private MetadataAddValueParams m_addValue = new MetadataAddValueParams();
        private MetadataDeleteRowParams m_deleteRow = new MetadataDeleteRowParams();
        private MetadataDatabaseVersionParams m_databaseVersion = new MetadataDatabaseVersionParams();
        private MetadataGetTableParams m_getTable = new MetadataGetTableParams();
        private MetadataGetQueryParams m_getQuery = new MetadataGetQueryParams();
        private MetadataSyncDatabaseParams m_syncDatabase = new MetadataSyncDatabaseParams();
        private MetadataSyncTableOrQueryParams m_syncTableOrQuery = new MetadataSyncTableOrQueryParams();
        private MetadataGetDatabaseSchemaParams m_getDatabaseSchema = new MetadataGetDatabaseSchemaParams();
        private MetadataGetDatabaseVersionParams m_getDatabaseVersion = new MetadataGetDatabaseVersionParams();

        private StreamReader m_stream = new StreamReader();

        public CommandCode CommandCode => CommandCode.Metadata;

        public void Fill(StreamReader buffer)
        {
            m_stream = buffer;
        }

        public IMetadataParams NextCommand()
        {
            if (m_stream.Position == m_stream.Length)
                return null;

            MetadataCommand command = m_stream.Read<MetadataCommand>();
            switch (command)
            {
                case MetadataCommand.Clear:
                    return m_clear;
                case MetadataCommand.AddTable:
                    m_addTable.TableIndex = m_stream.ReadInt15();
                    m_addTable.TableName = m_stream.ReadString();
                    m_addTable.TableFlags = m_stream.Read<TableFlags>();
                    return m_addTable;
                case MetadataCommand.AddColumn:
                    m_addColumn.TableIndex = m_stream.ReadInt15();
                    m_addColumn.ColumnIndex = m_stream.ReadInt15();
                    m_addColumn.ColumnName = m_stream.ReadString();
                    m_addColumn.ColumnType = m_stream.Read<ValueType>();
                    return m_addColumn;
                case MetadataCommand.AddRow:
                    m_addRow.TableIndex = m_stream.ReadInt15();
                    m_addRow.RowIndex = m_stream.ReadInt32();
                    return m_addRow;
                case MetadataCommand.AddValue:
                    m_addValue.TableIndex = m_stream.ReadInt15();
                    m_addValue.ColumnIndex = m_stream.ReadInt15();
                    m_addValue.RowIndex = m_stream.ReadInt32();
                    m_addValue.Value = m_stream.ReadBytes();
                    return m_addValue;
                case MetadataCommand.DeleteRow:
                    m_deleteRow.TableIndex = m_stream.ReadInt15();
                    m_deleteRow.RowIndex = m_stream.ReadInt32();
                    return m_deleteRow;
                case MetadataCommand.DatabaseVersion:
                    m_databaseVersion.MajorVersion = m_stream.ReadGuid();
                    m_databaseVersion.MinorVersion = m_stream.ReadInt64();
                    return m_databaseVersion;
                case MetadataCommand.GetTable:
                    m_getTable.TableIndex = m_stream.ReadInt15();
                    m_getTable.ColumnList = m_stream.ReadArray<int>();
                    m_getTable.FilterExpression = m_stream.ReadList<int, string>();
                    return m_getTable;
                case MetadataCommand.GetQuery:
                    m_getQuery.ColumnList = m_stream.ReadList<int, int>();
                    m_getQuery.JoinFields = m_stream.ReadList<int, int, int>();
                    m_getQuery.FilterExpression = m_stream.ReadList<int, int, string>();
                    return m_getQuery;
                case MetadataCommand.SyncDatabase:
                    m_syncDatabase.MajorVersion = m_stream.ReadGuid();
                    m_syncDatabase.MinorVersion = m_stream.ReadInt64();
                    m_syncDatabase.ColumnList = m_stream.ReadList<int, int>();
                    return m_syncDatabase;
                case MetadataCommand.SyncTableOrQuery:
                    m_syncTableOrQuery.MajorVersion = m_stream.ReadGuid();
                    m_syncTableOrQuery.MinorVersion = m_stream.ReadInt64();
                    m_syncTableOrQuery.ColumnList = m_stream.ReadList<int, int>();
                    m_syncTableOrQuery.CriticalColumnList = m_stream.ReadList<int, int>();
                    return m_syncTableOrQuery;
                case MetadataCommand.GetDatabaseSchema:
                    return m_getDatabaseSchema;
                case MetadataCommand.GetDatabaseVersion:
                    return m_getDatabaseVersion;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

    }
}
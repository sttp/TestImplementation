using System;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.WireProtocol.Data
{
    public class MetadataDecoder : IPacketDecoder
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

        private PacketReader m_packet = new PacketReader(new SessionDetails());
        private SessionDetails m_details;

        public MetadataDecoder(SessionDetails details)
        {
            m_details = details;
        }

        public CommandCode CommandCode => CommandCode.Metadata;

        public void Fill(PacketReader buffer)
        {
            m_packet = buffer;
        }

        public IMetadataParams NextCommand()
        {
            if (m_packet.Position == m_packet.Length)
                return null;

            MetadataCommand command = m_packet.Read<MetadataCommand>();
            switch (command)
            {
                case MetadataCommand.Clear:
                    return m_clear;
                case MetadataCommand.AddTable:
                    m_addTable.TableIndex = m_packet.ReadInt32();
                    m_addTable.TableName = m_packet.ReadString();
                    m_addTable.TableFlags = m_packet.Read<TableFlags>();
                    return m_addTable;
                case MetadataCommand.AddColumn:
                    m_addColumn.TableIndex = m_packet.ReadInt32();
                    m_addColumn.ColumnIndex = m_packet.ReadInt32();
                    m_addColumn.ColumnName = m_packet.ReadString();
                    m_addColumn.ColumnType = m_packet.Read<ValueType>();
                    return m_addColumn;
                case MetadataCommand.AddRow:
                    m_addRow.TableIndex = m_packet.ReadInt32();
                    m_addRow.RowIndex = m_packet.ReadInt32();
                    return m_addRow;
                case MetadataCommand.AddValue:
                    m_addValue.TableIndex = m_packet.ReadInt32();
                    m_addValue.ColumnIndex = m_packet.ReadInt32();
                    m_addValue.RowIndex = m_packet.ReadInt32();
                    m_addValue.Value = m_packet.ReadBytes();
                    return m_addValue;
                case MetadataCommand.DeleteRow:
                    m_deleteRow.TableIndex = m_packet.ReadInt32();
                    m_deleteRow.RowIndex = m_packet.ReadInt32();
                    return m_deleteRow;
                case MetadataCommand.DatabaseVersion:
                    m_databaseVersion.MajorVersion = m_packet.ReadGuid();
                    m_databaseVersion.MinorVersion = m_packet.ReadInt64();
                    return m_databaseVersion;
                case MetadataCommand.GetTable:
                    m_getTable.TableIndex = m_packet.ReadInt32();
                    m_getTable.ColumnList = m_packet.ReadArray<int>();
                    m_getTable.FilterExpression = m_packet.ReadList<int, string>();
                    return m_getTable;
                case MetadataCommand.GetQuery:
                    m_getQuery.ColumnList = m_packet.ReadList<int, int>();
                    m_getQuery.JoinFields = m_packet.ReadList<int, int, int>();
                    m_getQuery.FilterExpression = m_packet.ReadList<int, int, string>();
                    return m_getQuery;
                case MetadataCommand.SyncDatabase:
                    m_syncDatabase.MajorVersion = m_packet.ReadGuid();
                    m_syncDatabase.MinorVersion = m_packet.ReadInt64();
                    m_syncDatabase.ColumnList = m_packet.ReadList<int, int>();
                    return m_syncDatabase;
                case MetadataCommand.SyncTableOrQuery:
                    m_syncTableOrQuery.MajorVersion = m_packet.ReadGuid();
                    m_syncTableOrQuery.MinorVersion = m_packet.ReadInt64();
                    m_syncTableOrQuery.ColumnList = m_packet.ReadList<int, int>();
                    m_syncTableOrQuery.CriticalColumnList = m_packet.ReadList<int, int>();
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
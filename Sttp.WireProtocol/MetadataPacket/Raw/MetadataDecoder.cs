using System;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.WireProtocol.Data.Raw
{
    public class MetadataDecoder : IMetadataDecoder
    {
        private MetadataUseTableParams m_useTable = new MetadataUseTableParams();
        private MetadataAddTableParams m_addTable = new MetadataAddTableParams();
        private MetadataAddColumnParams m_addColumn = new MetadataAddColumnParams();
        private MetadataAddValueParams m_addValue = new MetadataAddValueParams();
        private MetadataDeleteRowParams m_deleteRow = new MetadataDeleteRowParams();
        private MetadataTableVersionParams m_tableVersion = new MetadataTableVersionParams();
        private MetadataAddRelationshipParams m_addRelationship = new MetadataAddRelationshipParams();
        private MetadataGetTableParams m_getTable = new MetadataGetTableParams();
        private MetadataSyncTableParams m_syncTable = new MetadataSyncTableParams();
        private MetadataSelectAllTablesWithSchemaParams m_selectAllTablesWithSchema = new MetadataSelectAllTablesWithSchemaParams();
        private MetadataGetAllTableVersionsParams m_getAllTableVersions = new MetadataGetAllTableVersionsParams();

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
                case MetadataCommand.UseTable:
                    m_useTable.TableIndex = m_stream.ReadInt15();
                    return m_useTable;
                case MetadataCommand.AddTable:
                    m_addTable.TableName = m_stream.ReadString();
                    m_addTable.TableFlags = (TableFlags)m_stream.ReadByte();
                    m_addTable.MajorVersion = m_stream.ReadGuid();
                    m_addTable.MinorVersion = m_stream.ReadInt64();
                    return m_addTable;
                case MetadataCommand.AddColumn:
                    m_addColumn.ColumnIndex = m_stream.ReadInt15();
                    m_addColumn.ColumnName = m_stream.ReadString();
                    m_addColumn.ColumnType = m_stream.Read<ValueType>();
                    return m_addColumn;
                case MetadataCommand.AddValue:
                    m_addValue.ColumnIndex = m_stream.ReadInt15();
                    m_addValue.RowIndex = m_stream.ReadInt32();
                    m_addValue.Value = m_stream.ReadBytes();
                    return m_addValue;
                case MetadataCommand.DeleteRow:
                    m_deleteRow.RowIndex = m_stream.ReadInt32();
                    return m_deleteRow;
                case MetadataCommand.TableVersion:
                    m_tableVersion.TableIndex = m_stream.ReadInt15();
                    m_tableVersion.MajorVersion = m_stream.ReadGuid();
                    m_tableVersion.MinorVersion = m_stream.ReadInt64();
                    return m_tableVersion;
                case MetadataCommand.AddRelationship:
                    m_addRelationship.TableIndex = m_stream.ReadInt15();
                    m_addRelationship.ColumnIndex = m_stream.ReadInt15();
                    m_addRelationship.ForeignTableIndex = m_stream.ReadInt15();
                    return m_addRelationship;
                case MetadataCommand.GetTable:
                    m_getTable.TableIndex = m_stream.ReadInt15();
                    m_getTable.ColumnList = m_stream.ReadArray<int>();
                    m_getTable.FilterExpression = m_stream.ReadArray<string>();
                    return m_getTable;
                case MetadataCommand.SyncTable:
                    m_syncTable.TableIndex = m_stream.ReadInt15();
                    m_syncTable.MajorVersion = m_stream.ReadGuid();
                    m_syncTable.MinorVersion = m_stream.ReadInt64();
                    m_syncTable.ColumnList = m_stream.ReadArray<int>();
                    return m_syncTable;
                case MetadataCommand.SelectAllTablesWithSchema:
                    return m_selectAllTablesWithSchema;
                case MetadataCommand.GetAllTableVersions:
                    return m_getAllTableVersions;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

    }
}
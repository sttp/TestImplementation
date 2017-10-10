using System;
using System.Collections.Generic;
using Sttp.WireProtocol.Data;
using Sttp.WireProtocol.MetadataPacket;
using ValueType = Sttp.WireProtocol.ValueType;

namespace Sttp.Data
{
    public class MetadataTableSource
    {
        private MetadataChangeLog m_changeLog;
        /// <summary>
        /// The name of the table
        /// </summary>
        public string TableName;

        /// <summary>
        /// Indicates that this table has 1 record for each measurement and can be used
        /// in filtering. False means this is a ancillary table that won't be understood by the
        /// API, but is used for the application layer.
        /// </summary>
        public TableFlags TableFlags;

        /// <summary>
        /// All possible columns that are defined for the table.
        /// </summary>
        public List<MetadataColumn> Columns;

        /// <summary>
        /// lookup columns by their name
        /// </summary>
        private Dictionary<string, int> m_columnLookup;

        /// <summary>
        /// All possible rows.
        /// </summary>
        public List<MetadataRow> Rows;

        public int TableIndex;

        public MetadataTableSource(MetadataChangeLog changeLog, int tableIndex, string tableName, TableFlags tableFlags)
        {
            m_changeLog = changeLog;
            TableIndex = tableIndex;
            TableName = tableName;
            TableFlags = tableFlags;
            m_columnLookup = new Dictionary<string, int>();
            Columns = new List<MetadataColumn>();
            Rows = new List<MetadataRow>();
        }

        public void AddColumn(string columnName, ValueType columnType)
        {
            int columnId;
            MetadataColumn column;
            if (!m_columnLookup.TryGetValue(columnName, out columnId))
            {
                column = new MetadataColumn(Columns.Count, columnName, columnType);
                Columns.Add(column);
                m_changeLog.AddColumn(TableIndex, column);
                m_columnLookup[columnName] = column.Index;
            }
        }

        public void AddOrUpdateValue(string columnName, int rowIndex, object value)
        {
            while (Rows.Count <= rowIndex)
            {
                Rows.Add(null);
            }

            MetadataRow row = Rows[rowIndex];

            if (row == null)
            {
                row = new MetadataRow(rowIndex);
                Rows[rowIndex] = row;
            }
            row.FillData(TableIndex, m_changeLog, Columns[m_columnLookup[columnName]], value);
        }

        public void DeleteRow(int rowIndex)
        {
            Rows[rowIndex] = null;
            m_changeLog.DeleteRow(TableIndex, rowIndex);
        }

        public void RequestTableData(IMetadataEncoder encoder, MetadataTableFilter permissionFilter)
        {
            encoder.AddTable(TableIndex, TableName, TableFlags);
            foreach (var column in Columns)
            {
                encoder.AddColumn(TableIndex, column.Index, column.Name, column.Type);
            }
            foreach (var row in Rows)
            {
                if (permissionFilter == null || permissionFilter.PermitRow(row))
                {
                    for (var columnIndex = 0; columnIndex < row.Fields.Count; columnIndex++)
                    {
                        var field = row.Fields[columnIndex];
                        if (field != null && (permissionFilter == null || permissionFilter.PermitField(row.RowIndex, columnIndex, row.Fields[columnIndex].Value)))
                        {
                            encoder.AddValue(TableIndex, columnIndex, row.RowIndex, field.Value);
                        }
                    }
                }
            }
        }
    }
}

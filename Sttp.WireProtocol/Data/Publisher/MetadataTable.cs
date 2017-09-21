using System;
using System.Collections.Generic;
using ValueType = Sttp.WireProtocol.ValueType;

namespace Sttp.Data.Publisher
{
    public class MetadataTable
    {
        /// <summary>
        /// The 0 based index of this table in the DataSet.
        /// </summary>
        public readonly int TableIndex;
        /// <summary>
        /// The name of the table
        /// </summary>
        public string TableName;

        /// <summary>
        /// All possible columns that are defined for the table.
        /// </summary>
        public List<MetadataColumn> Columns;

        private Dictionary<string, int> m_columnLookup;

        /// <summary>
        /// All possible rows.
        /// </summary>
        public Dictionary<int, MetadataRow> Rows;

        public MetadataTable(string tableName, int tableIndex)
        {
            TableName = tableName;
            m_columnLookup = new Dictionary<string, int>();
            TableIndex = tableIndex;
            Columns = new List<MetadataColumn>();
            Rows = new Dictionary<int, MetadataRow>();
        }

        public void FillSchema(List<MetadataColumn> columns, MetadataChangeLog changeLog, string columnName, ValueType columnType)
        {
            int columnId;
            MetadataColumn column;
            if (!m_columnLookup.TryGetValue(columnName, out columnId))
            {
                column = new MetadataColumn(columns.Count, Columns.Count, columnType);
                Columns.Add(column);
                columns.Add(column);
                changeLog.AddColumn(TableIndex, column);
                m_columnLookup[columnName] = column.DataSetColumnIndex;
            }
        }

        public void FillData(MetadataChangeLog changeLog, string columnName, int recordID, object fieldValue)
        {
            MetadataRow row;
            if (!Rows.TryGetValue(recordID, out row))
            {
                row = new MetadataRow(recordID);
                Rows[recordID] = row;
                changeLog.AddRow(TableIndex, row);
            }
            row.FillData(TableIndex, changeLog, Columns[m_columnLookup[columnName]], fieldValue);

        }
    }
}

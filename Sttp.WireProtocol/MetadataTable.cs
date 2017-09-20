using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol
{
    public class MetadataTable
    {
        /// <summary>
        /// The name of the table.
        /// </summary>
        public readonly string TableName;

        public readonly int TableId;

        private int m_nextColumnSequenceNumber;

        /// <summary>
        /// All possible columns that are defined for the table.
        /// </summary>
        public Dictionary<string, MetadataColumn> Columns;

        /// <summary>
        /// All possible rows.
        /// </summary>
        public Dictionary<int, MetadataRow> Rows;

        public MetadataTable(string tableName, int tableId)
        {
            TableName = tableName;
            TableId = tableId;
            Columns = new Dictionary<string, MetadataColumn>();
            Rows = new Dictionary<int, MetadataRow>();
        }

        public void FillSchema(MetadataChangeLog changeLog, string columnName, ValueType columnType)
        {
            MetadataColumn column;
            if (!Columns.TryGetValue(columnName, out column))
            {
                column = new MetadataColumn(m_nextColumnSequenceNumber, columnName, columnType);
                m_nextColumnSequenceNumber++;
                Columns[columnName] = column;
                changeLog.AddColumn(TableId, column);
            }
        }

        public void FillData(MetadataChangeLog changeLog, string columnName, int recordID, object fieldValue)
        {
            MetadataRow row;
            if (!Rows.TryGetValue(recordID, out row))
            {
                row = new MetadataRow(recordID);
                Rows[recordID] = row;
                changeLog.AddRow(TableId, row);
            }
            row.FillData(TableId, changeLog, Columns[columnName], fieldValue);

        }
    }
}

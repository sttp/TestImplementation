using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol
{
    public class MetadataTable
    {
        /// <summary>
        /// The name of the table.
        /// </summary>
        public string TableName;

        /// <summary>
        /// All possible columns that are defined for the table.
        /// </summary>
        public Dictionary<string, MetadataColumn> Columns;

        /// <summary>
        /// All possible rows.
        /// </summary>
        public Dictionary<int, MetadataRow> Rows;

        public MetadataTable(string tableName)
        {
            TableName = tableName;
            Columns = new Dictionary<string, MetadataColumn>();
            Rows = new Dictionary<int, MetadataRow>();
        }

        public void FillSchema(string columnName, ValueType columnType)
        {
            MetadataColumn column;
            if (!Columns.TryGetValue(columnName, out column))
            {
                column = new MetadataColumn(columnName, columnType);
                Columns[columnName] = column;
            }
        }

        public void FillData(string columnName, int recordID, object fieldValue)
        {
            MetadataRow row;
            if (!Rows.TryGetValue(recordID, out row))
            {
                row = new MetadataRow(recordID);
                Rows[recordID] = row;
            }
            row.FillData(Columns[columnName], fieldValue);

        }
    }
}

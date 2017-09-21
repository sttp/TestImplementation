using System;
using System.Collections.Generic;
using ValueType = Sttp.WireProtocol.ValueType;

namespace Sttp.Data.Subscriber
{
    public class MetadataTable
    {
        public readonly int TableId;

        /// <summary>
        /// All possible columns that are defined for the table.
        /// </summary>
        public Dictionary<int, MetadataColumn> Columns;

        /// <summary>
        /// All possible rows.
        /// </summary>
        public Dictionary<int, MetadataRow> Rows;

        public MetadataTable(int tableId)
        {
            TableId = tableId;
            Columns = new Dictionary<int, MetadataColumn>();
            Rows = new Dictionary<int, MetadataRow>();
        }

        public void ApplyPatch(MetadataPatchDetails patch)
        {
            switch (patch.ChangeType)
            {
                case MetadataChangeType.AddColumn:
                    Columns[patch.ColumnID] = new MetadataColumn(patch.ColumnID, patch.ColumnType);
                    break;
                case MetadataChangeType.AddRow:
                    Rows[patch.RowID] = new MetadataRow(patch.RowID);
                    break;
                case MetadataChangeType.AddField:
                case MetadataChangeType.AddFieldValue:
                    Rows[patch.RowID].ApplyPatch(patch);
                    break;
                default:
                    throw new NotSupportedException("Invalid patch type:");
            }
        }
    }
}

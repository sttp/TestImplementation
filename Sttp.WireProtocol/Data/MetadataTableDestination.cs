using System;
using System.Collections.Generic;
using ValueType = Sttp.WireProtocol.ValueType;

namespace Sttp.Data.Subscriber
{
    public class MetadataTableDestination
    {
        /// <summary>
        /// If logging is enabled, this is the ID for the transaction log.
        /// </summary>
        public Guid InstanceID;

        /// <summary>
        /// This identifies the transaction number of the supplied log. 
        /// </summary>
        public long TransactionID;

        /// <summary>
        /// The name of the table
        /// </summary>
        public string TableName;

        /// <summary>
        /// Indicates that this table has 1 record for each measurement and can be used
        /// in filtering. False means this is a ancillary table that won't be understood by the
        /// API, but is used for the application layer.
        /// </summary>
        public bool IsMappedToDataPoint;

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

        public MetadataTableDestination(string tableName, bool isMappedToDataPoint)
        {
            TableName = tableName;
            IsMappedToDataPoint = isMappedToDataPoint;
            m_columnLookup = new Dictionary<string, int>();
            Columns = new List<MetadataColumn>();
            Rows = new List<MetadataRow>();
        }

        public void ApplyPatch(MetadataPatchDetails patch)
        {
            switch (patch.ChangeType)
            {
                case MetadataChangeType.AddColumn:
                    while (Columns.Count <= patch.ColumnIndex)
                    {
                        Columns.Add(null);
                    }
                    Columns[patch.ColumnIndex] = new MetadataColumn(patch.ColumnIndex, patch.ColumnName, patch.ColumnType);
                    break;
                case MetadataChangeType.AddValue:
                    while (Rows.Count <= patch.RowIndex)
                    {
                        Rows.Add(null);
                    }
                    if (Rows[patch.RowIndex] == null)
                    {
                        Rows[patch.RowIndex] = new MetadataRow(patch.RowIndex);
                    }
                    Rows[patch.RowIndex].ApplyPatch(patch);
                    break;
                case MetadataChangeType.DeleteRow:
                    Rows[patch.RowIndex] = null;
                    break;
                default:
                    throw new NotSupportedException("Invalid patch type:");
            }
        }
    }
}

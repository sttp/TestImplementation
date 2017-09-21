using System;
using System.Collections.Generic;
using ValueType = Sttp.WireProtocol.ValueType;

namespace Sttp.Data.Publisher
{
    public class MetadataTable
    {
        private MetadataChangeLog m_changeLog = new MetadataChangeLog();

        /// <summary>
        /// If logging is enabled, this is the ID for the transaction log.
        /// </summary>
        public Guid InstanceID => m_changeLog.InstanceID;

        /// <summary>
        /// This identifies the transaction number of the supplied log. 
        /// </summary>
        public long TransactionID => m_changeLog.TransactionID;

        /// <summary>
        /// Gets/Sets if transaction logging is supported for changes made to this data set.
        /// It's recommended that this be turned off if large changes will occur to the set. 
        /// </summary>
        public bool LogRevisions
        {
            get => m_changeLog.LogRevisions;
            set => m_changeLog.LogRevisions = value;
        }

        /// <summary>
        /// The name of the table
        /// </summary>
        public string TableName;

        /// <summary>
        /// Indicates that this table has 1 record for each measurement and can be used
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

        public MetadataTable(string tableName, bool isMappedToDataPoint)
        {
            TableName = tableName;
            IsMappedToDataPoint = isMappedToDataPoint;
            m_columnLookup = new Dictionary<string, int>();
            Columns = new List<MetadataColumn>();
            Rows = new List<MetadataRow>();
        }

        public void FillSchema(string columnName, ValueType columnType)
        {
            int columnId;
            MetadataColumn column;
            if (!m_columnLookup.TryGetValue(columnName, out columnId))
            {
                column = new MetadataColumn(Columns.Count, columnName, columnType);
                Columns.Add(column);
                m_changeLog.AddColumn(column);
                m_columnLookup[columnName] = column.Index;
            }
        }

        public void FillData(string columnName, int rowIndex, object fieldValue)
        {
            while (Rows.Count < rowIndex)
            {
                Rows.Add(null);
            }

            MetadataRow row = Rows[rowIndex];

            if (row == null)
            {
                row = new MetadataRow(rowIndex);
                Rows[rowIndex] = row;
            }
            row.FillData(m_changeLog, Columns[m_columnLookup[columnName]], fieldValue);
        }

        public byte[] SendToClient(Guid instanceId, long cachedRuntimeID, dynamic permissionsFilter)
        {
            if (m_changeLog.TryBuildPatchData(instanceId, cachedRuntimeID, out List<MetadataPatchDetails> data))
            {
                foreach (var record in data)
                {
                    if (permissionsFilter.Permit(record))
                    {
                        //Serialize Record
                    }
                }
                return null;
            }

            //Serialize all metadata
            throw new NotImplementedException();
        }
    }
}

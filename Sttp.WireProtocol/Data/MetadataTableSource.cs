using Sttp.IO;
using System;
using System.Collections.Generic;
using System.IO;
using ValueType = Sttp.WireProtocol.ValueType;

namespace Sttp.Data.Publisher
{
    public class MetadataTableSource
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

        public int TableIndex;

        public MetadataTableSource(int tableIndex, string tableName, bool isMappedToDataPoint)
        {
            TableIndex = tableIndex;
            TableName = tableName;
            IsMappedToDataPoint = isMappedToDataPoint;
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
                m_changeLog.AddColumn(column);
                m_columnLookup[columnName] = column.Index;
            }
        }

        public void AddOrUpdateValue(string columnName, int rowIndex, object value)
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
            row.FillData(m_changeLog, Columns[m_columnLookup[columnName]], value);
        }

        public void DeleteRow(int rowIndex)
        {
            Rows[rowIndex] = null;
            m_changeLog.DeleteRow(rowIndex);
        }

        public byte[] RequestTableData(Guid cachedInstanceID, long transaction, MetadataTableFilter permissionFilter)
        {
            MemoryStream stream = new MemoryStream();

            if (m_changeLog.TryBuildPatchData(cachedInstanceID, transaction, out List<MetadataPatchDetails> data))
            {
                stream.Write((byte)0); //PatchMetadata
                stream.Write((byte)1); //Version
                stream.Write(InstanceID);
                stream.Write(TransactionID);
                foreach (var record in data)
                {
                    if (permissionFilter == null || permissionFilter.Permit(record))
                    {
                        stream.Write(true);
                        record.Save(stream);
                    }
                }
                stream.Write(false);
                return stream.ToArray();
            }

            stream.Write((byte)1); //New Table. Patch not available.
            stream.Write((byte)1); //Version Number
            stream.Write(InstanceID);
            stream.Write(TransactionID);
            //Columns are already serialized by this point only send the rows.
            foreach (var row in Rows)
            {
                if (permissionFilter == null || permissionFilter.PermitRow(row))
                {
                    stream.Write(true);
                    stream.Write(row.RowIndex);
                    for (var columnIndex = 0; columnIndex < row.Fields.Count; columnIndex++)
                    {
                        var field = row.Fields[columnIndex];
                        if (field != null && (permissionFilter == null || permissionFilter.PermitField(row.RowIndex, columnIndex, row.Fields[columnIndex].Value)))
                        {
                            stream.Write(true);
                            stream.Write(columnIndex);
                            stream.Write(field.Value != null);
                            if (field.Value != null)
                                stream.WriteWithLength(field.Value);
                        }
                    }
                    stream.Write(false);
                }
            }
            stream.Write(false);
            return stream.ToArray();
        }
    }


    public class MetadataTableFilter
    {
        public bool Permit(MetadataPatchDetails record)
        {
            throw new NotImplementedException();
        }

        public bool PermitRow(MetadataRow row)
        {
            throw new NotImplementedException();
        }

        public bool PermitField(int rowRowIndex, int columnIndex, byte[] value)
        {
            throw new NotImplementedException();
        }
    }
}

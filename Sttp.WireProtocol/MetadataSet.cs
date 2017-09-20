using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// The set of all metadata used by the protocol.
    /// </summary>
    public class MetadataSet
    {
        private int m_nextTableSequenceNumber = 0;
        private Dictionary<string, MetadataTable> m_tables;
        private Dictionary<int, MetadataTable> m_tablesById;
        private MetadataChangeLog m_changeLog = new MetadataChangeLog();

        public MetadataSet()
        {
            m_tables = new Dictionary<string, MetadataTable>();
            m_tablesById = new Dictionary<int, MetadataTable>();
        }

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

        public void InitializeDefaults()
        {
            FillSchema("Measurement", "DeviceID", ValueType.Guid);
            FillSchema("Measurement", "PointTag", ValueType.String);
            FillSchema("Measurement", "SignalReference", ValueType.String);
            FillSchema("Measurement", "SignalTypeID", ValueType.Int32);
            FillSchema("Measurement", "Adder", ValueType.Decimal);
            FillSchema("Measurement", "Multiplier", ValueType.Decimal);
            FillSchema("Measurement", "Description", ValueType.String);
            FillSchema("Measurement", "ChannelName", ValueType.String);
            FillSchema("Measurement", "SignalType", ValueType.String);
            FillSchema("Measurement", "PositionIndex", ValueType.Int32);
            FillSchema("Measurement", "PhaseDesignation", ValueType.String);
            FillSchema("Measurement", "EngineeringUnits", ValueType.String);
            FillSchema("Measurement", "EngineeringScale", ValueType.Decimal);
            //ToDo: Add More
        }

        public void FillSchema(string tableName, string columnName, ValueType columnType)
        {
            MetadataTable table;
            if (!m_tables.TryGetValue(tableName, out table))
            {
                table = new MetadataTable(tableName, m_nextTableSequenceNumber);
                m_nextTableSequenceNumber++;
                m_tables[tableName] = table;
                m_tablesById[table.TableId] = table;
                m_changeLog.AddTable(table);
            }
            table.FillSchema(m_changeLog, columnName, columnType);
        }

        public void FillData(string tableName, string columnName, int recordID, object fieldValue)
        {
            m_tables[tableName].FillData(m_changeLog, columnName, recordID, fieldValue);
        }

        public void ApplyPatch(List<MetadataPatchDetails> patchDetails)
        {
            if (LogRevisions)
                throw new NotSupportedException("Not supported now, but might do it later.");

            foreach (var patch in patchDetails)
            {
                switch (patch.ChangeType)
                {
                    case MetadataChangeType.AddTable:
                        ApplyPatch(patch);
                        break;
                    case MetadataChangeType.AddColumn:
                    case MetadataChangeType.AddRow:
                    case MetadataChangeType.AddField:
                    case MetadataChangeType.AddFieldValue:
                        m_tablesById[patch.TableID].ApplyPatch(patch);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void ApplyPatch(MetadataPatchDetails patch)
        {
            switch (patch.ChangeType)
            {
                case MetadataChangeType.AddTable:
                    patch.OutAddTable(out int tableID, out string tableName);
                    var table = new MetadataTable(tableName, tableID);
                    m_tables[tableName] = table;
                    break;
                default:
                    throw new NotSupportedException("Invalid patch type:");
            }
        }

    }
}

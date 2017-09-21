using System;
using System.Collections.Generic;
using System.IO;
using ValueType = Sttp.WireProtocol.ValueType;

namespace Sttp.Data.Publisher
{
    /// <summary>
    /// The set of all metadata used by the protocol.
    /// </summary>
    public class MetadataSet
    {
        private MetadataChangeLog m_changeLog = new MetadataChangeLog();

        private Dictionary<string, int> m_tableLookup;

        private List<MetadataTable> m_tables;
        private List<MetadataColumn> m_columns;

        public MetadataSet()
        {
            m_tables = new List<MetadataTable>();
            m_columns = new List<MetadataColumn>();
            m_tableLookup = new Dictionary<string, int>();
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
            //Note: once the schema has been filled, removing the schema details are not supported. This is 
            //      because the schema is index mapped to improve execution speed with arrays. Removing columns or tables
            //      would mess this up.
            MetadataTable table;
            int tableID;
            if (!m_tableLookup.TryGetValue(tableName, out tableID))
            {
                tableID = m_tables.Count;
                table = new MetadataTable(tableName, tableID);
                m_tables.Add(table);
                m_changeLog.AddTable(table);
                m_tableLookup[tableName] = tableID;
            }
            m_tables[tableID].FillSchema(m_columns, m_changeLog, columnName, columnType);
        }

        public void FillData(string tableName, string columnName, int recordID, object fieldValue)
        {
            int tableID = m_tableLookup[tableName];

            m_tables[tableID].FillData(m_changeLog, columnName, recordID, fieldValue);
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

using System;
using System.Collections.Generic;
using ValueType = Sttp.WireProtocol.ValueType;

namespace Sttp.Data
{
    /// <summary>
    /// The set of all metadata used by the protocol.
    /// </summary>
    public class MetadataSet
    {
        private Dictionary<int, MetadataTable> m_tables;
        private MetadataChangeLog m_changeLog = new MetadataChangeLog();
        private Dictionary<string, int> m_keywordMapping;
        private List<string> m_keywords;

        public MetadataSet()
        {
            m_keywordMapping = new Dictionary<string, int>();
            m_keywords = new List<string>();
            m_tables = new Dictionary<int, MetadataTable>();
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
            int tableID = NameToId(tableName);
            int columnID = NameToId(columnName);

            MetadataTable table;
            if (!m_tables.TryGetValue(tableID, out table))
            {
                table = new MetadataTable(tableID);
                m_tables[table.TableId] = table;
                m_changeLog.AddTable(table);
            }
            table.FillSchema(m_changeLog, columnID, columnType);
        }


        public void FillData(string tableName, string columnName, int recordID, object fieldValue)
        {
            int tableID = NameToId(tableName);
            int columnID = NameToId(columnName);

            m_tables[tableID].FillData(m_changeLog, columnID, recordID, fieldValue);
        }

        public void ApplyPatch(List<MetadataPatchDetails> patchDetails)
        {
            if (LogRevisions)
                throw new NotSupportedException("Not supported now, but might do it later.");

            foreach (var patch in patchDetails)
            {
                ApplyPatch(patch);
            }
        }

        private void ApplyPatch(MetadataPatchDetails patch)
        {
            switch (patch.ChangeType)
            {
                case MetadataChangeType.AddKeyword:
                    if (patch.KeywordID != NameToId(patch.Keyword))
                    {
                        throw new Exception("Names out of sequence");
                    }
                    break;
                case MetadataChangeType.AddTable:
                    m_tables[patch.TableID] = new MetadataTable(patch.TableID);
                    break;
                case MetadataChangeType.AddColumn:
                case MetadataChangeType.AddRow:
                case MetadataChangeType.AddField:
                case MetadataChangeType.AddFieldValue:
                    m_tables[patch.TableID].ApplyPatch(patch);
                    break;
                default:
                    throw new NotSupportedException("Invalid patch type:");
            }
        }

        private int NameToId(string name)
        {
            int id;
            if (m_keywordMapping.TryGetValue(name, out id))
            {
                id = m_keywords.Count;
                m_keywordMapping[name] = id;
                m_keywords.Add(name);
            }
            return id;
        }


    }
}

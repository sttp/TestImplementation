using System;
using System.Collections.Generic;
using System.Data;
using ValueType = Sttp.WireProtocol.ValueType;

namespace Sttp.Data.Subscriber
{
    /// <summary>
    /// The set of all metadata used by the protocol.
    /// </summary>
    public class MetadataSet
    {
        private Dictionary<int, MetadataTable> m_tables;
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
        public Guid InstanceID { get; private set; }

        /// <summary>
        /// This identifies the transaction number of the supplied log. 
        /// </summary>
        public long TransactionID { get; private set; }

        public void ApplyPatch(List<MetadataPatchDetails> patchDetails)
        {
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

        public DataSet CreateDataSet()
        {
            //takes the current metadata and makes a dataset from it.
            return null;
        }

        public DataTable CreateDataTable(string tableName)
        {
            //takes the current metadata and makes a table from it.
            return null;
        }

    }
}

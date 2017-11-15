using System;
using System.Collections.Generic;
using System.Linq;
using Sttp.WireProtocol;

namespace Sttp.Data
{
    public class MetadataTable
    {
        /// <summary>
        /// The name of the table
        /// </summary>
        public string TableName;

        /// <summary>
        /// Indicates that this table has 1 record for each measurement and can be used
        /// in filtering. False means this is a ancillary table that won't be understood by the
        /// API, but is used for the application layer.
        /// </summary>
        public TableFlags TableFlags;

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
        public Dictionary<SttpValue, MetadataRow> Rows;

        public long LastModifiedRevision;

        public MetadataTable(string tableName, TableFlags tableFlags, List<Tuple<string, SttpValueTypeCode>> columns)
        {
            TableName = tableName;
            TableFlags = tableFlags;
            m_columnLookup = new Dictionary<string, int>();
            Columns = new List<MetadataColumn>();
            Rows = new Dictionary<SttpValue, MetadataRow>();
            for (var index = 0; index < columns.Count; index++)
            {
                var item = columns[index];
                m_columnLookup.Add(item.Item1, index);
                Columns.Add(new MetadataColumn(item.Item1, item.Item2));
            }
        }

        public void AddOrUpdateRow(SttpValue key, SttpValueSet fields)
        {
            if (!Rows.ContainsKey(key))
            {
                Rows[key] = new MetadataRow(key, fields);
            }
            else
            {
                Rows[key].Update(fields); 
            }
        }

        public void DeleteRow(SttpValue primaryKey)
        {
            Rows.Remove(primaryKey);
        }

      
    }
}

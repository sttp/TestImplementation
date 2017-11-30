using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Sttp.Codec;

namespace Sttp.Data
{
    public class MetadataTable
    {
        /// <summary>
        /// The name of the table
        /// </summary>
        public readonly string TableName;

        /// <summary>
        /// All possible columns that are defined for the table.
        /// </summary>
        public readonly List<MetadataColumn> Columns;

        public readonly List<MetadataForeignKey2> ForeignKeys;

        public readonly long LastModifiedRevision;

        private bool m_isReadOnly;

        /// <summary>
        /// This is concurrent since it will be shared with all modified instances. To check 
        /// if the value actually exists, make sure that the index returned is not outside the bounds of 
        /// m_rows.
        /// </summary>
        private readonly ConcurrentDictionary<SttpValue, int> m_rowLookup;

        public IEnumerable<MetadataRow> Rows => m_rows;

        private readonly List<MetadataRow> m_rows;

        public MetadataTable(string tableName, List<MetadataColumn> columns, List<MetadataForeignKey> tableRelationships)
        {
            TableName = tableName;
            Columns = columns.ToList();
            ForeignKeys = tableRelationships.Select(x => new MetadataForeignKey2(x)).ToList();

            foreach (var fk in ForeignKeys)
            {
                fk.LocalColumnIndex = Columns.FindIndex(y => y.Name == fk.ColumnName);
            }
            LastModifiedRevision = 0;
            m_rowLookup = new ConcurrentDictionary<SttpValue, int>();
            m_rows = new List<MetadataRow>();
            m_isReadOnly = false;
        }

        public MetadataTable(MetadataTable other)
        {
            m_rowLookup = other.m_rowLookup;
            m_rows = other.m_rows.ToList();
            ForeignKeys = other.ForeignKeys;
            Columns = other.Columns;
            TableName = other.TableName;
            LastModifiedRevision = other.LastModifiedRevision;
            m_isReadOnly = false;
        }

        /// <summary>
        /// Gets/Sets the readonly nature of this class.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return m_isReadOnly;
            }
        }

        public void SetIsReadOnly(long revision, Func<string, int> tableLookup, Func<int, SttpValue, int> lookupForeignKey)
        {
            if (IsReadOnly)
                throw new Exception("This class is immutable");
            int x = 0;
            if (revision == 0)
            {
                for (x = 0; x < m_rows.Count; x++)
                {
                    if (m_rows[x].Revision != 0)
                    {
                        m_rows[x] = new MetadataRow(m_rows[x].Key, m_rows[x].Fields, ForeignKeys.Count, 0);
                    }
                }
                //For a schema change, all foreign keys must be reassigned
                for (x = 0; x < ForeignKeys.Count; x++)
                {
                    if (ForeignKeys[x].TableIndex < 0)
                    {
                        ForeignKeys[x].TableIndex = tableLookup(TableName);
                    }
                    if (ForeignKeys[x].TableIndex >= 0 && ForeignKeys[x].LocalColumnIndex >= 0)
                    {
                        int tableIndex = ForeignKeys[x].TableIndex;
                        int columnIndex = ForeignKeys[x].LocalColumnIndex;

                        foreach (var row in m_rows)
                        {
                            if (row.Fields != null)
                            {
                                row.ForeignKeys[x] = lookupForeignKey(tableIndex, row.Fields.Values[columnIndex]);
                            }
                        }
                    }
                }

            }
            else
            {
                //All the foreign key tree must be searched 
                //even if there is not a schema update to check if a foreign table has been updated.
                for (x = 0; x < ForeignKeys.Count; x++)
                {
                    if (ForeignKeys[x].TableIndex < 0)
                    {
                        ForeignKeys[x].TableIndex = tableLookup(TableName);
                    }
                    if (ForeignKeys[x].TableIndex >= 0 && ForeignKeys[x].LocalColumnIndex >= 0)
                    {
                        int tableIndex = ForeignKeys[x].TableIndex;
                        int columnIndex = ForeignKeys[x].LocalColumnIndex;

                        foreach (var row in m_rows)
                        {
                            if (row.Fields != null && row.ForeignKeys[x] < 0)
                            {
                                row.ForeignKeys[x] = lookupForeignKey(tableIndex, row.Fields.Values[columnIndex]);
                            }
                        }
                    }
                }
            }
            m_isReadOnly = true;
        }

        public void AddOrUpdateRow(SttpValue key, SttpValueSet fields, long revision)
        {
            if (IsReadOnly)
                throw new Exception("This class is immutable");

            if (m_rowLookup.TryGetValue(key, out int rowIndex))
            {
                if (m_rows[rowIndex].Fields != fields)
                {
                    m_rows[rowIndex] = new MetadataRow(key, fields, ForeignKeys.Count, revision);
                }
            }
            else
            {
                m_rowLookup.TryAdd(key, m_rows.Count);
                m_rows.Add(new MetadataRow(key, fields, ForeignKeys.Count, revision));
            }
        }

        public MetadataTable CloneEditable()
        {
            return new MetadataTable(this);
        }

        public void DeleteRow(SttpValue key, long revision)
        {
            if (IsReadOnly)
                throw new Exception("This class is immutable");

            if (m_rowLookup.TryGetValue(key, out int rowIndex))
            {
                m_rows[rowIndex] = new MetadataRow(key, null, 0, revision);
            }
        }

        public int LookupRowIndex(SttpValue sttpValue)
        {
            if (m_rowLookup.TryGetValue(sttpValue, out int index))
            {
                if (index < m_rows.Count)
                {
                    return index;
                }
                return -1;
            }
            return -1;
        }

        public override string ToString()
        {
            return $"{TableName} Columns: {Columns.Count} Rows: {m_rows.Count} ForeignKeys: {ForeignKeys.Count}";
        }

        public MetadataRow LookupRow(int nextRowIndex)
        {
            return m_rows[nextRowIndex];
        }
    }
}

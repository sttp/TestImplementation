using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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

        public readonly string PrimaryKey;

        /// <summary>
        /// All possible columns that are defined for the table.
        /// </summary>
        public readonly List<MetadataColumn> Columns;

        public readonly List<MetadataForeignKeyMapping> ForeignKeys;

        public readonly long LastModifiedSequenceNumber;

        /// <summary>
        /// This is concurrent since it will be shared with all modified instances. To check 
        /// if the value actually exists, make sure that the index returned is not outside the bounds of 
        /// m_rows.
        /// </summary>
        private readonly ConcurrentDictionary<SttpValue, int> m_rowLookup;

        public IEnumerable<MetadataRow> Rows => m_rows;

        private readonly MetadataRow[] m_rows;

        public MetadataTable(string tableName, string primaryKey, List<MetadataColumn> columns, List<MetadataForeignKeyMapping> tableRelationships)
        {
            TableName = tableName;
            PrimaryKey = primaryKey ?? string.Empty;
            Columns = columns.ToList();
            ForeignKeys = tableRelationships;
            LastModifiedSequenceNumber = 0;
            m_rowLookup = new ConcurrentDictionary<SttpValue, int>();
            m_rows = new MetadataRow[0];
            foreach (var fk in ForeignKeys)
            {
            
            }
        }

        private MetadataTable(MetadataTable other, MetadataRow[] copiedRows, List<MetadataRow> newRows, long newSequenceNumber)
        {
            ForeignKeys = other.ForeignKeys;
            Columns = other.Columns;
            TableName = other.TableName;
            m_rowLookup = other.m_rowLookup;
            m_rows = new MetadataRow[copiedRows.Length + newRows.Count];
            copiedRows.CopyTo(m_rows, 0);
            newRows.CopyTo(m_rows, copiedRows.Length);
            LastModifiedSequenceNumber = newSequenceNumber;
        }

        public MetadataTable MergeDataSets(DataTable table, long newSequenceNumber)
        {
            using (var rdr = table.CreateDataReader())
            {
                return MergeDataSets(rdr, newSequenceNumber);
            }
        }

        public MetadataTable MergeDataSets(DbDataReader table, long newSequenceNumber)
        {
            List<MetadataRow> newRows = new List<MetadataRow>();
            MetadataRow[] copiedRows = new MetadataRow[m_rows.Length];
            bool hasChanges = false;
            int fieldCount = table.FieldCount;

            if (Columns.Count != fieldCount)
            {
                throw new Exception("Schema has changed, Cannot fill data set.");
            }
            for (var x = 0; x < fieldCount; x++)
            {
                if (Columns[x].Name != table.GetName(x))
                    throw new Exception("Schema has changed, Cannot fill data set.");
                if (Columns[x].TypeCode != SttpValueTypeCodec.FromType(table.GetFieldType(x)))
                    throw new Exception("Schema has changed, Cannot fill data set.");
            }

            int indexOfKey = table.GetOrdinal(PrimaryKey);

            object[] row = new object[fieldCount];
            while (table.Read())
            {
                if (table.GetValues(row) != fieldCount)
                    throw new Exception("Field count did not match");

                SttpValueMutable key = new SttpValueMutable();
                List<SttpValue> values = new List<SttpValue>();
                values.AddRange(row.Select(SttpValue.FromObject));
                key.SetValue(values[indexOfKey]);

                if (m_rowLookup.TryGetValue(key, out int rowIndex))
                {
                    if (m_rows[rowIndex].Fields == null)
                    {
                        //Row was previously deleted
                        copiedRows[rowIndex] = new MetadataRow(key, values, ForeignKeys.Count, newSequenceNumber);
                        hasChanges = true;
                    }
                    else if (!m_rows[rowIndex].Fields.SequenceEqual(values))
                    {
                        //Row has changed
                        copiedRows[rowIndex] = new MetadataRow(key, values, ForeignKeys.Count, newSequenceNumber);
                        hasChanges = true;
                    }
                    else
                    {
                        copiedRows[rowIndex] = m_rows[rowIndex];
                    }
                }
                else
                {
                    m_rowLookup.TryAdd(key, m_rows.Length + newRows.Count);
                    newRows.Add(new MetadataRow(key, values, ForeignKeys.Count, newSequenceNumber));
                    hasChanges = true;
                }
            }


            for (int x = 0; x < copiedRows.Length; x++)
            {
                if (copiedRows[x] == null)
                {
                    if (m_rows[x].Fields == null)
                    {
                        copiedRows[x] = m_rows[x];
                    }
                    else
                    {
                        copiedRows[x] = new MetadataRow(m_rows[x].Key, null, ForeignKeys.Count, newSequenceNumber);
                        hasChanges = true;
                    }
                }
            }

            if (!hasChanges)
            {
                return this;
            }

            return new MetadataTable(this, copiedRows, newRows, newSequenceNumber);
        }

        public void LookupForeignKeys(Func<int, SttpValue, int> lookupForeignKey)
        {
            //All the foreign key tree must be searched 
            //even if there is not a schema update to check if a foreign table has been updated.
            for (int x = 0; x < ForeignKeys.Count; x++)
            {
                if (ForeignKeys[x].TableIndex >= 0 && ForeignKeys[x].LocalColumnIndex >= 0)
                {
                    int tableIndex = ForeignKeys[x].TableIndex;
                    int columnIndex = ForeignKeys[x].LocalColumnIndex;

                    foreach (var row in m_rows)
                    {
                        if (row.Fields != null && row.ForeignKeys[x] < 0)
                        {
                            row.ForeignKeys[x] = lookupForeignKey(tableIndex, row.Fields[columnIndex]);
                        }
                    }
                }
            }
        }

        public int LookupRowIndex(SttpValue sttpValue)
        {
            if (m_rowLookup.TryGetValue(sttpValue, out int index))
            {
                if (index < m_rows.Length)
                {
                    return index;
                }
                return -1;
            }
            return -1;
        }

        public override string ToString()
        {
            return $"{TableName} Columns: {Columns.Count} Rows: {m_rows.Length} ForeignKeys: {ForeignKeys.Count}";
        }

        public MetadataRow LookupRow(int nextRowIndex)
        {
            return m_rows[nextRowIndex];
        }
    }
}

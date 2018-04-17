using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using CTP;
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

        public readonly long LastModifiedVersionNumber;

        public IEnumerable<MetadataRow> Rows => m_rows;

        private readonly MetadataRow[] m_rows;

        public MetadataTable(string tableName, List<MetadataColumn> columns)
        {
            TableName = tableName;
            Columns = columns.ToList();
            LastModifiedVersionNumber = 0;
            m_rows = new MetadataRow[0];
        }

        private MetadataTable(MetadataTable other, List<MetadataRow> newRows, long lastModifiedVersionNumber)
        {
            Columns = other.Columns;
            TableName = other.TableName;
            m_rows = new MetadataRow[newRows.Count];
            newRows.CopyTo(m_rows, 0);
            LastModifiedVersionNumber = lastModifiedVersionNumber;
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

            object[] row = new object[fieldCount];
            while (table.Read())
            {
                if (table.GetValues(row) != fieldCount)
                    throw new Exception("Field count did not match");

                List<CtpObject> values = new List<CtpObject>();
                values.AddRange(row.Select(CtpObject.FromObject));

                newRows.Add(new MetadataRow(values));
            }

            return new MetadataTable(this, newRows, newSequenceNumber);
        }
     
        public override string ToString()
        {
            return $"{TableName} Columns: {Columns.Count} Rows: {m_rows.Length}";
        }

    }
}

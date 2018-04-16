using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using CTP;
using Sttp.Codec;

namespace Sttp.Data
{
    /// <summary>
    /// A response to a GetMeadata command.
    /// </summary>
    public class MetadataQueryTable
    {
        /// <summary>
        /// The name of the table
        /// </summary>
        public string TableName;

        /// <summary>
        /// All possible columns that are defined for the table.
        /// </summary>
        public List<MetadataColumn> Columns;

        /// <summary>
        /// All possible rows.
        /// </summary>
        public List<CtpValue[]> Rows;
        public Guid RuntimeID;
        public long VersionNumber;

        public MetadataQueryTable(CommandBeginMetadataResponse command)
        {
            ProcessCommand(command);
        }

        public DataTable ToTable()
        {
            DataTable tbl = new DataTable();
            foreach (var column in Columns)
            {
                tbl.Columns.Add(column.Name, SttpValueTypeCodec.ToType(column.TypeCode));
            }
            object[] list = new object[Columns.Count];
            foreach (var row in Rows)
            {
                for (int x = 0; x < list.Length; x++)
                {
                    list[x] = row[x].ToNativeType;
                }
                tbl.Rows.Add(list);
            }
            return tbl;
        }

        private void ProcessCommand(CommandBeginMetadataResponse command)
        {
            RuntimeID = command.RuntimeID;
            VersionNumber = command.VersionNumber;
            TableName = command.TableName;
            Columns = new List<MetadataColumn>(command.Columns);
            Rows = new List<CtpValue[]>();
        }

        public void AddRow(CtpValueMutable[] values)
        {
            CtpValue[] item = new CtpValue[values.Length];
            for (int x = 0; x < values.Length; x++)
            {
                item[x] = values[x].CloneAsImmutable();
            }
            Rows.Add(item);
        }

    }
}

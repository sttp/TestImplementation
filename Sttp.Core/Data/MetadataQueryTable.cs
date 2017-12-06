using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Sttp.Codec;
using Sttp.Codec.Metadata;
using Sttp.IO;

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
        public Dictionary<SttpValue, SttpValueSet> Rows;

        public Guid SchemaVersion;
        public long Revision;

        public MetadataQueryTable(CmdDefineResponse command)
        {
            if (command.IsUpdateQuery)
                throw new Exception("Update responses must path an existing table.");

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
                    list[x] = row.Value.Values[x].ToNativeType;
                }
                tbl.Rows.Add(list);
            }
            return tbl;
        }

        public void ProcessCommand(CmdDefineResponse command)
        {
            if (command.IsUpdateQuery)
            {
                if (SchemaVersion != command.SchemaVersion)
                    throw new Exception("Schema Version Mismatch");

                if (Revision < command.UpdatedFromRevision)
                    throw new Exception("The version cannot be updated");

                if (TableName != command.TableName)
                    throw new Exception("There was a schema change");

                if (Columns.Count != command.Columns.Count)
                    throw new Exception("There was a schema change");

                for (int x = 0; x < Columns.Count; x++)
                {
                    if (Columns[x].Name != command.Columns[x].Name)
                        throw new Exception("There was a schema change");

                    if (Columns[x].TypeCode != command.Columns[x].TypeCode)
                        throw new Exception("There was a schema change");
                }

                Revision = command.Revision;
            }
            else
            {
                SchemaVersion = command.SchemaVersion;
                Revision = command.Revision;
                TableName = command.TableName;
                Columns = new List<MetadataColumn>(command.Columns);
                Rows = new Dictionary<SttpValue, SttpValueSet>();
            }

        }

        public void ProcessCommand(CmdDefineRow command)
        {
            Rows[command.PrimaryKey] = command.Values;
        }

        public void ProcessCommand(CmdUndefineRow command)
        {
            Rows.Remove(command.PrimaryKey);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Sttp.IO;
using Sttp.WireProtocol;
using Sttp.WireProtocol.Data;
using Sttp.WireProtocol.MetadataPacket;
using ValueType = Sttp.WireProtocol.ValueType;

namespace Sttp.Data
{
    public class MetadataTableDestination
    {
        /// <summary>
        /// If logging is enabled, this is the ID for the transaction log.
        /// </summary>
        public Guid MajorVersion;

        /// <summary>
        /// This identifies the transaction number of the supplied log. 
        /// </summary>
        public long MinorVersion;

        /// <summary>
        /// The name of the table
        /// </summary>
        public string TableName;

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
        public List<MetadataRow> Rows;
        /// <summary>
        /// the index of the table
        /// </summary>
        public int TableIndex;

        public MetadataTableDestination(Guid majorVersion, long minorVersion, string tableName, int tableIndex, TableFlags tableFlags)
        {
            MajorVersion = majorVersion;
            MinorVersion = minorVersion;
            TableName = tableName;
            TableIndex = tableIndex;
            TableFlags = tableFlags;
            m_columnLookup = new Dictionary<string, int>();
            Columns = new List<MetadataColumn>();
            Rows = new List<MetadataRow>();
        }

        public void ApplyPatch(MetadataChangeLogRecord patch)
        {
            switch (patch.ChangeType)
            {
                case MetadataChangeType.AddColumn:

                    break;
                case MetadataChangeType.AddValue:

                    break;
                case MetadataChangeType.DeleteRow:
                    Rows[patch.RowIndex] = null;
                    break;
                default:
                    throw new NotSupportedException("Invalid patch type:");
            }
        }

        public void AddColumn(int index, string name, ValueType type)
        {
            while (Columns.Count <= index)
            {
                Columns.Add(null);
            }
            Columns[index] = new MetadataColumn(index, name, type);
        }

        public void ProcessCommand(IMetadataParams command)
        {
            switch (command.Command)
            {
                case MetadataCommand.AddColumn:
                    var addC = command as MetadataAddColumnParams;
                    while (Columns.Count <= addC.ColumnIndex)
                    {
                        Columns.Add(null);
                    }
                    Columns[addC.ColumnIndex] = new MetadataColumn(addC.ColumnIndex, addC.ColumnName, addC.ColumnType);
                    break;
                case MetadataCommand.AddValue:
                    var addV = command as MetadataAddValueParams;

                    while (Rows.Count <= addV.RowIndex)
                    {
                        Rows.Add(null);
                    }
                    if (Rows[addV.RowIndex] == null)
                    {
                        Rows[addV.RowIndex] = new MetadataRow(addV.RowIndex);
                    }
                    Rows[addV.RowIndex].ProcessCommand(addV);

                    break;
                case MetadataCommand.DeleteRow:
                    var delRow = command as MetadataDeleteRowParams;
                    Rows[delRow.RowIndex] = null;
                    break;
                case MetadataCommand.TableVersion:
                    var tbl = command as MetadataTableVersionParams;
                    MajorVersion = tbl.MajorVersion;
                    MinorVersion = tbl.MinorVersion;
                    break;
                case MetadataCommand.AddRelationship:
                case MetadataCommand.AddTable:
                case MetadataCommand.UseTable:
                case MetadataCommand.GetTable:
                case MetadataCommand.SyncTable:
                case MetadataCommand.SelectAllTablesWithSchema:
                case MetadataCommand.GetAllTableVersions:
                    throw new NotSupportedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        public object GetValue(int rowIndex, int columnIndex)
        {
            if (Rows.Count >= rowIndex || Rows[rowIndex] == null)
                return null;

            var row = Rows[rowIndex];
            if (row.Fields.Count >= columnIndex || row.Fields[columnIndex] == null)
                return null;
            return Columns[columnIndex].Decode(row.Fields[columnIndex].Value);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Sttp.IO;
using Sttp.WireProtocol;
using Sttp.WireProtocol.GetMetadataResponse;

namespace Sttp.Data
{
    public class MetadataTableDestination
    {
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
        private Dictionary<string, short> m_columnLookup;

        /// <summary>
        /// All possible rows.
        /// </summary>
        public List<MetadataRow> Rows;
        /// <summary>
        /// the index of the table
        /// </summary>
        public int TableIndex;

        public MetadataTableDestination(string tableName, short tableIndex, TableFlags tableFlags)
        {
            TableName = tableName;
            TableIndex = tableIndex;
            TableFlags = tableFlags;
            m_columnLookup = new Dictionary<string, short>();
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

        public void AddColumn(short index, string name, SttpValueTypeCode typeCode)
        {
            while (Columns.Count <= index)
            {
                Columns.Add(null);
            }
            Columns[index] = new MetadataColumn(index, name, typeCode);
        }

        public void ProcessCommand(Sttp.WireProtocol.GetMetadataResponse.Cmd command)
        {
            switch (command.SubCommand)
            {
                case SubCommand.AddColumn:
                    var addC = command.AddColumn;
                    while (Columns.Count <= addC.ColumnIndex)
                    {
                        Columns.Add(null);
                    }
                    Columns[addC.ColumnIndex] = new MetadataColumn(addC.ColumnIndex, addC.ColumnName, (SttpValueTypeCode)addC.ColumnTypeCode);
                    break;
                case SubCommand.AddRow:
                    var addR = command.AddRow;
                    while (Rows.Count <= addR.RowIndex)
                    {
                        Rows.Add(null);
                    }
                    if (Rows[addR.RowIndex] == null)
                    {
                        Rows[addR.RowIndex] = new MetadataRow(addR.RowIndex);
                    }
                    break;
                case SubCommand.AddValue:
                    var addV = command.AddValue;
                    Rows[addV.RowIndex].ProcessCommand(addV);
                    break;
                case SubCommand.DeleteRow:
                    var delRow = command.DeleteRow;
                    Rows[delRow.RowIndex] = null;
                    break;
                case SubCommand.AddTable:
                case SubCommand.Clear:
                case SubCommand.DatabaseVersion:
                case SubCommand.Invalid:
                    throw new NotSupportedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        public object GetValue(int rowIndex, short columnIndex)
        {
            if (Rows.Count <= rowIndex || Rows[rowIndex] == null)
                return null;

            var row = Rows[rowIndex];
            if (row.Fields.Count <= columnIndex || row.Fields[columnIndex] == null)
                return null;
            return Columns[columnIndex].Decode(row.Fields[columnIndex].Value);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Sttp.IO;
using ValueType = Sttp.WireProtocol.ValueType;

namespace Sttp.Data
{
    public class MetadataTableDestination
    {
        /// <summary>
        /// If logging is enabled, this is the ID for the transaction log.
        /// </summary>
        public Guid InstanceID;

        /// <summary>
        /// This identifies the transaction number of the supplied log. 
        /// </summary>
        public long TransactionID;

        /// <summary>
        /// The name of the table
        /// </summary>
        public string TableName;

        /// <summary>
        /// Indicates that this table has 1 record for each measurement and can be used
        /// in filtering. False means this is a ancillary table that won't be understood by the
        /// API, but is used for the application layer.
        /// </summary>
        public bool IsMappedToDataPoint;

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

        public MetadataTableDestination(Guid instanceId, long transactionId, string tableName, int tableIndex, bool isMappedToDataPoint)
        {
            InstanceID = instanceId;
            TransactionID = transactionId;
            TableName = tableName;
            TableIndex = tableIndex;
            IsMappedToDataPoint = isMappedToDataPoint;
            m_columnLookup = new Dictionary<string, int>();
            Columns = new List<MetadataColumn>();
            Rows = new List<MetadataRow>();
        }

        public void ApplyPatch(MetadataPatchDetails patch)
        {
            switch (patch.ChangeType)
            {
                case MetadataChangeType.AddColumn:
                    while (Columns.Count <= patch.ColumnIndex)
                    {
                        Columns.Add(null);
                    }
                    Columns[patch.ColumnIndex] = new MetadataColumn(patch.ColumnIndex, patch.ColumnName, patch.ColumnType);
                    break;
                case MetadataChangeType.AddValue:
                    while (Rows.Count <= patch.RowIndex)
                    {
                        Rows.Add(null);
                    }
                    if (Rows[patch.RowIndex] == null)
                    {
                        Rows[patch.RowIndex] = new MetadataRow(patch.RowIndex);
                    }
                    Rows[patch.RowIndex].ApplyPatch(patch);
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

        public void FillTable(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            byte method = stream.ReadNextByte();

            switch (method)
            {
                case 0:
                    PatchTable(stream);
                    break;
                case 1:
                    RebuildTable(stream);
                    break;
                default:
                    throw new Exception();

            }
        }

        private void PatchTable(MemoryStream stream)
        {
            byte version = stream.ReadNextByte();

            switch (version)
            {
                case 1:
                    InstanceID = stream.ReadGuid();
                    TransactionID = stream.ReadInt64();
                    while (stream.ReadBoolean())
                    {
                        var record = new MetadataPatchDetails(stream);
                        ApplyPatch(record);
                    }
                    break;
                default:
                    throw new VersionNotFoundException();
            }
        }

        private void RebuildTable(MemoryStream stream)
        {
            byte version = stream.ReadNextByte();

            switch (version)
            {
                case 1:
                    InstanceID = stream.ReadGuid();
                    TransactionID = stream.ReadInt64();
                    Rows.Clear();
                    while (stream.ReadBoolean())
                    {
                        int rowIndex = stream.ReadInt32();
                        while (Rows.Count <= rowIndex)
                        {
                            Rows.Add(null);
                        }
                        var row = new MetadataRow(rowIndex);
                        Rows[rowIndex] = row;
                        while (stream.ReadBoolean())
                        {

                            int columnIndex = stream.ReadInt32();
                            byte[] data = null;
                            if (stream.ReadBoolean())
                                data = stream.ReadBytes();

                            while (row.Fields.Count <= columnIndex)
                            {
                                row.Fields.Add(null);
                            }
                            row.Fields[columnIndex] = new MetadataField();
                            row.Fields[columnIndex].Value = data;
                        }
                        var record = new MetadataPatchDetails(stream);
                        ApplyPatch(record);
                    }
                    break;
                default:
                    throw new VersionNotFoundException();
            }

        }
    }
}

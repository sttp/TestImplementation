using System;

namespace Sttp.WireProtocol
{
    public class MetadataPatchDetails
    {
        public MetadataChangeType ChangeType;
        private int m_int1;
        private int m_int2;
        private int m_int3;
        private string m_string1;
        private byte[] m_data;

        public int TableID
        {
            get
            {
                switch (ChangeType)
                {
                    case MetadataChangeType.AddTable:
                    case MetadataChangeType.AddColumn:
                    case MetadataChangeType.AddRow:
                    case MetadataChangeType.AddField:
                    case MetadataChangeType.AddFieldValue:
                        return m_int1;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public int RowID
        {
            get
            {
                switch (ChangeType)
                {
                    case MetadataChangeType.AddRow:
                        return m_int2;
                    case MetadataChangeType.AddField:
                    case MetadataChangeType.AddFieldValue:
                        return m_int3;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public int ColumnID
        {
            get
            {
                switch (ChangeType)
                {
                    case MetadataChangeType.AddColumn:
                    case MetadataChangeType.AddField:
                    case MetadataChangeType.AddFieldValue:
                        return m_int2;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public byte[] Data
        {
            get
            {
                switch (ChangeType)
                {
                    case MetadataChangeType.AddFieldValue:
                        return m_data;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }


        public static MetadataPatchDetails AddTable(int tableID, string tableName)
        {
            return new MetadataPatchDetails()
            {
                ChangeType = MetadataChangeType.AddTable,
                m_int1 = tableID,
                m_string1 = tableName
            };
        }

        public static MetadataPatchDetails AddColumn(int tableID, int columnId, string columnName, ValueType columnType)
        {
            return new MetadataPatchDetails()
            {
                ChangeType = MetadataChangeType.AddColumn,
                m_int1 = tableID,
                m_int2 = columnId,
                m_int3 = (int)columnType,
                m_string1 = columnName
            };
        }

        public static MetadataPatchDetails AddRow(int tableID, int rowRecordID)
        {
            return new MetadataPatchDetails()
            {
                ChangeType = MetadataChangeType.AddRow,
                m_int1 = tableID,
                m_int2 = rowRecordID
            };
        }

        public static MetadataPatchDetails AddField(int tableID, int columnColumnID, int recordID)
        {
            return new MetadataPatchDetails()
            {
                ChangeType = MetadataChangeType.AddField,
                m_int1 = tableID,
                m_int2 = columnColumnID,
                m_int3 = recordID
            };
        }

        public static MetadataPatchDetails AddFieldValue(int tableID, int columnColumnID, int recordID, byte[] encoding)
        {
            return new MetadataPatchDetails()
            {
                ChangeType = MetadataChangeType.AddField,
                m_int1 = tableID,
                m_int2 = columnColumnID,
                m_int3 = recordID,
                m_data = encoding
            };
        }

        public void OutAddTable(out int tableID, out string tableName)
        {
            if (ChangeType == MetadataChangeType.AddTable)
                throw new InvalidOperationException("The change type is invalid");
            tableID = m_int1;
            tableName = m_string1;
        }

        public void OutAddColumn(out int tableID, out int columnId, out string columnName, out ValueType columnType)
        {
            if (ChangeType == MetadataChangeType.AddColumn)
                throw new InvalidOperationException("The change type is invalid");
            tableID = m_int1;
            columnId = m_int2;
            columnType = (ValueType)m_int3;
            columnName = m_string1;
        }

        public void OutAddRow(out int tableID, out int rowRecordID)
        {
            if (ChangeType == MetadataChangeType.AddRow)
                throw new InvalidOperationException("The change type is invalid");
            tableID = m_int1;
            rowRecordID = m_int2;
        }

        public void OutAddField(out int tableID, out int columnColumnID, out int recordID)
        {
            if (ChangeType == MetadataChangeType.AddField)
                throw new InvalidOperationException("The change type is invalid");
            tableID = m_int1;
            columnColumnID = m_int2;
            recordID = m_int3;
        }

        public void OutAddFieldValue(out int tableID, out int columnColumnID, out int recordID, out byte[] encoding)
        {
            if (ChangeType != MetadataChangeType.AddField)
                throw new InvalidOperationException("The change type is invalid");
            tableID = m_int1;
            columnColumnID = m_int2;
            recordID = m_int3;
            encoding = m_data;
        }
    }
}
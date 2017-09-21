using System;
using ValueType = Sttp.WireProtocol.ValueType;

namespace Sttp.Data.Publisher
{
    public class MetadataPatchDetails
    {
        public MetadataChangeType ChangeType;
        private int m_tableId;
        private int m_columnId;
        private int m_recordId;
        private string m_keyword;
        private byte[] m_data;

        public int KeywordID
        {
            get
            {
                switch (ChangeType)
                {
                    case MetadataChangeType.AddKeyword:
                        return m_tableId;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public string Keyword
        {
            get
            {
                switch (ChangeType)
                {
                    case MetadataChangeType.AddKeyword:
                        return m_keyword;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

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
                        return m_tableId;
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
                    case MetadataChangeType.AddField:
                    case MetadataChangeType.AddFieldValue:
                        return m_recordId;
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
                        return m_columnId;
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

        public ValueType ColumnType
        {
            get
            {
                switch (ChangeType)
                {
                    case MetadataChangeType.AddColumn:
                        return (ValueType)m_recordId;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public static MetadataPatchDetails AddKeyword(int id, string name)
        {
            return new MetadataPatchDetails()
            {
                ChangeType = MetadataChangeType.AddKeyword,
                m_tableId = id,
                m_keyword = name
            };
        }

        public static MetadataPatchDetails AddTable(int tableID)
        {
            return new MetadataPatchDetails()
            {
                ChangeType = MetadataChangeType.AddTable,
                m_tableId = tableID
            };
        }

        public static MetadataPatchDetails AddColumn(int tableID, int columnId, ValueType columnType)
        {
            return new MetadataPatchDetails()
            {
                ChangeType = MetadataChangeType.AddColumn,
                m_tableId = tableID,
                m_columnId = columnId,
                m_recordId = (int)columnType
            };
        }

        public static MetadataPatchDetails AddRow(int tableID, int rowRecordID)
        {
            return new MetadataPatchDetails()
            {
                ChangeType = MetadataChangeType.AddRow,
                m_tableId = tableID,
                m_recordId = rowRecordID
            };
        }

        public static MetadataPatchDetails AddField(int tableID, int columnColumnID, int recordID)
        {
            return new MetadataPatchDetails()
            {
                ChangeType = MetadataChangeType.AddField,
                m_tableId = tableID,
                m_columnId = columnColumnID,
                m_recordId = recordID
            };
        }

        public static MetadataPatchDetails AddFieldValue(int tableID, int columnColumnID, int recordID, byte[] encoding)
        {
            return new MetadataPatchDetails()
            {
                ChangeType = MetadataChangeType.AddField,
                m_tableId = tableID,
                m_columnId = columnColumnID,
                m_recordId = recordID,
                m_data = encoding
            };
        }

        
    }
}
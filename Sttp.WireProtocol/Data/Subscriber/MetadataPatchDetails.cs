using System;
using ValueType = Sttp.WireProtocol.ValueType;

namespace Sttp.Data.Subscriber
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
        
    }
}
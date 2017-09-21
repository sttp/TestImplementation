using System;
using ValueType = Sttp.WireProtocol.ValueType;

namespace Sttp.Data.Publisher
{
    public class MetadataPatchDetails
    {
        public MetadataChangeType ChangeType;
        private ValueType m_columnType;
        private int m_columnIndex;
        private int m_rowIndex;
        private string m_columnName;
        private byte[] m_value;

        public string ColumnName
        {
            get
            {
                switch (ChangeType)
                {
                    case MetadataChangeType.AddColumn:
                        return m_columnName;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public int RowIndex
        {
            get
            {
                switch (ChangeType)
                {
                    case MetadataChangeType.DeleteRow:
                    case MetadataChangeType.AddValue:
                        return m_rowIndex;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public int ColumnIndex
        {
            get
            {
                switch (ChangeType)
                {
                    case MetadataChangeType.AddColumn:
                    case MetadataChangeType.AddValue:
                        return m_columnIndex;
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
                    case MetadataChangeType.AddValue:
                        return m_value;
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
                        return m_columnType;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public static MetadataPatchDetails AddColumn(int columnIndex, string columnName, ValueType columnType)
        {
            return new MetadataPatchDetails()
            {
                ChangeType = MetadataChangeType.AddColumn,
                m_columnName = columnName,
                m_columnIndex = columnIndex,
                m_columnType = columnType
            };
        }

        public static MetadataPatchDetails AddValue(int columnIndex, int rowIndex, byte[] value)
        {
            return new MetadataPatchDetails()
            {
                ChangeType = MetadataChangeType.AddValue,
                m_columnIndex = columnIndex,
                m_rowIndex = rowIndex,
                m_value = value
            };
        }

        public static MetadataPatchDetails DeleteRow(int rowIndex)
        {
            return new MetadataPatchDetails()
            {
                ChangeType = MetadataChangeType.DeleteRow,
                m_rowIndex = rowIndex
            };
        }
    }
}
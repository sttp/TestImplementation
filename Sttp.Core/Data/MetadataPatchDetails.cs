using Sttp.IO;
using System;
using System.IO;
using Sttp.WireProtocol;

namespace Sttp.Data
{
    public class MetadataChangeLogRecord
    {
        public MetadataChangeType ChangeType;
        private SttpValueTypeCode m_columnTypeCode;
        private short m_tableIndex;
        private short m_columnIndex;
        private int m_rowIndex;
        private string m_columnName;
        private SttpValue m_value;

        private MetadataChangeLogRecord()
        {

        }

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

        public short ColumnIndex
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

        public SttpValue Data
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

        public SttpValueTypeCode ColumnTypeCode
        {
            get
            {
                switch (ChangeType)
                {
                    case MetadataChangeType.AddColumn:
                        return m_columnTypeCode;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public static MetadataChangeLogRecord AddColumn(short tableIndex, short columnIndex, string columnName, SttpValueTypeCode columnTypeCode)
        {
            return new MetadataChangeLogRecord()
            {
                m_tableIndex = tableIndex,
                ChangeType = MetadataChangeType.AddColumn,
                m_columnName = columnName,
                m_columnIndex = columnIndex,
                m_columnTypeCode = columnTypeCode
            };
        }

        public static MetadataChangeLogRecord AddValue(short tableIndex, short columnIndex, int rowIndex, SttpValue value)
        {
            return new MetadataChangeLogRecord()
            {
                m_tableIndex = tableIndex,
                ChangeType = MetadataChangeType.AddValue,
                m_columnIndex = columnIndex,
                m_rowIndex = rowIndex,
                m_value = value
            };
        }

        public static MetadataChangeLogRecord DeleteRow(short tableIndex, int rowIndex)
        {
            return new MetadataChangeLogRecord()
            {
                m_tableIndex = tableIndex,
                ChangeType = MetadataChangeType.DeleteRow,
                m_rowIndex = rowIndex
            };
        }

        public static MetadataChangeLogRecord AddTable(short tableIndex, string tableName, TableFlags flags)
        {
            throw new NotImplementedException();
        }

        public void Save(Sttp.WireProtocol.GetMetadataResponse.Encoder encoder)
        {
            switch (ChangeType)
            {
                case MetadataChangeType.AddColumn:
                    encoder.DefineColumn(m_tableIndex, m_columnIndex, m_columnName, (byte)m_columnTypeCode);
                    break;
                case MetadataChangeType.AddValue:
                    encoder.DefineValue(m_tableIndex, m_columnIndex, m_rowIndex, m_value);
                    break;
                case MetadataChangeType.DeleteRow:
                    encoder.RemoveRow(m_tableIndex, m_rowIndex);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
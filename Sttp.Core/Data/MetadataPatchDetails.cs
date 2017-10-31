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
        private byte[] m_value;

        private MetadataChangeLogRecord()
        {

        }
        public MetadataChangeLogRecord(Stream stream)
        {
            ChangeType = (MetadataChangeType)stream.ReadNextByte();
            switch (ChangeType)
            {
                case MetadataChangeType.AddColumn:
                    m_columnIndex = stream.ReadInt16();
                    m_columnName = stream.ReadString();
                    m_columnTypeCode = (SttpValueTypeCode)stream.ReadNextByte();
                    break;
                case MetadataChangeType.AddValue:
                    m_columnIndex = stream.ReadInt16();
                    m_rowIndex = stream.ReadInt32();
                    if (stream.ReadBoolean())
                        m_value = stream.ReadBytes();
                    break;
                case MetadataChangeType.DeleteRow:
                    m_rowIndex = stream.ReadInt32();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

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

        public static MetadataChangeLogRecord AddValue(short tableIndex, short columnIndex, int rowIndex, byte[] value)
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
                    encoder.AddColumn(m_tableIndex, m_columnIndex, m_columnName, m_columnTypeCode);
                    break;
                case MetadataChangeType.AddValue:
                    encoder.AddValue(m_tableIndex, m_columnIndex, m_rowIndex, m_value);
                    break;
                case MetadataChangeType.DeleteRow:
                    encoder.DeleteRow(m_tableIndex, m_rowIndex);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
using Sttp.IO;
using System;
using System.IO;
using ValueType = Sttp.WireProtocol.ValueType;

namespace Sttp.Data
{
    public class MetadataPatchDetails
    {
        public MetadataChangeType ChangeType;
        private ValueType m_columnType;
        private int m_columnIndex;
        private int m_rowIndex;
        private string m_columnName;
        private byte[] m_value;

        private MetadataPatchDetails()
        {
            
        }
        public MetadataPatchDetails(Stream stream)
        {
            ChangeType = (MetadataChangeType)stream.ReadNextByte();
            switch (ChangeType)
            {
                case MetadataChangeType.AddColumn:
                    m_columnIndex = stream.ReadInt32();
                    m_columnName = stream.ReadString();
                    m_columnType = (ValueType)stream.ReadNextByte();
                    break;
                case MetadataChangeType.AddValue:
                    m_columnIndex = stream.ReadInt32();
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

        public void Save(Stream stream)
        {
            switch (ChangeType)
            {
                case MetadataChangeType.AddColumn:
                    stream.Write((byte)ChangeType);
                    stream.Write(ColumnIndex);
                    stream.Write(ColumnName);
                    stream.Write((byte)m_columnType);
                    break;
                case MetadataChangeType.AddValue:
                    stream.Write((byte)ChangeType);
                    stream.Write(ColumnIndex);
                    stream.Write(RowIndex);
                    stream.Write(m_value != null);
                    if (m_value != null)
                        stream.WriteWithLength(m_value);
                    break;
                case MetadataChangeType.DeleteRow:
                    stream.Write((byte)ChangeType);
                    stream.Write(RowIndex);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
    }
}
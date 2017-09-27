//using Sttp.IO;
//using System;
//using System.IO;

//namespace Sttp.WireProtocol.Data.Condensed
//{
//    /// <summary>
//    /// Encodes a metadata packet.
//    /// </summary>
//    public class MetadataEncoder : IMetadataEncoder
//    {
//        [Flags]
//        public enum EncodingHeader : byte
//        {
//            /// <summary>
//            /// If set, Command is AddValue, otherwise, command is specified. 
//            /// </summary>
//            IsAddValueCommand = 128,
//            /// <summary>
//            /// If set, TableIndex is the same as it was last time. Otherwise, a TableIndex is specified.
//            /// </summary>
//            IsTableIndexSameAsPrevious = 64,
//            /// <summary>
//            /// If set, RowIndex is he same as it was last time. Otherwise, a RowIndex is specified.
//            /// </summary>
//            IsRowIndexSameAsPrevious = 32,

//            /// <summary>
//            /// If Length = 0; Length is specified.
//            /// if Length = 1; Value is null,
//            /// if Length = 2-31; value is 0-30
//            /// </summary>
//            LengthMask = 31,

//            None = 0


//        }

//        private MemoryStream m_stream = new MemoryStream();
//        private int m_lastTableIndex;
//        private int m_lastRowIndex;

//        /// <summary>
//        /// Begins a new metadata packet
//        /// </summary>
//        public void BeginCommand()
//        {
//            m_stream.Position = 0;
//            m_stream.SetLength(0);
//            m_lastTableIndex = 0;
//            m_lastRowIndex = 0;
//        }

//        /// <summary>
//        /// Ends a metadata packet and requests the buffer block that was allocated.
//        /// </summary>
//        /// <returns></returns>
//        public byte[] EndCommand()
//        {
//            return m_stream.ToArray();
//        }


//        #region Defined sub-commands

//        public void Clear()
//        {
//            EncodingHeader header = EncodingHeader.None;
//            m_stream.Write((byte)header);
//            m_stream.Write((byte)MetadataCommand.Clear);
//        }

//        public void DeleteTable(int tableIndex)
//        {
//            EncodingHeader header = EncodingHeader.None;
//            if (m_lastTableIndex == tableIndex)
//                header |= EncodingHeader.IsTableIndexSameAsPrevious;

//            m_stream.Write((byte)header);
//            m_stream.Write((byte)MetadataCommand.DeleteTable);
//            if ((header & EncodingHeader.IsTableIndexSameAsPrevious) == 0)
//                m_stream.Write(tableIndex);

//            m_lastTableIndex = tableIndex;
//        }

//        public void UpdateTable(int tableIndex, long transactionID)
//        {
//            EncodingHeader header = EncodingHeader.None;
//            if (m_lastTableIndex == tableIndex)
//                header |= EncodingHeader.IsTableIndexSameAsPrevious;

//            m_stream.Write((byte)header);
//            m_stream.Write((byte)MetadataCommand.UpdateTable);
//            if ((header & EncodingHeader.IsTableIndexSameAsPrevious) == 0)
//                m_stream.Write(tableIndex);
//            m_stream.Write(transactionID);
//            m_lastTableIndex = tableIndex;
//        }

//        public void AddTable(Guid instanceID, long transactionID, string tableName, int tableIndex, bool isMappedToDataPoint)
//        {
//            EncodingHeader header = EncodingHeader.None;
//            if (m_lastTableIndex == tableIndex)
//                header |= EncodingHeader.IsTableIndexSameAsPrevious;

//            m_stream.Write((byte)header);
//            m_stream.Write((byte)MetadataCommand.AddTable);
//            m_stream.Write(instanceID);
//            m_stream.Write(transactionID);
//            m_stream.Write(tableName);
//            if ((header & EncodingHeader.IsTableIndexSameAsPrevious) == 0)
//                m_stream.Write(tableIndex);
//            m_stream.Write(isMappedToDataPoint);

//            m_lastTableIndex = tableIndex;
//        }

//        public void AddColumn(int tableIndex, int columnIndex, string columnName, ValueType columnType)
//        {
//            EncodingHeader header = EncodingHeader.None;
//            if (m_lastTableIndex == tableIndex)
//                header |= EncodingHeader.IsTableIndexSameAsPrevious;

//            m_stream.Write((byte)header);
//            m_stream.Write((byte)MetadataCommand.AddColumn);
//            if ((header & EncodingHeader.IsTableIndexSameAsPrevious) == 0)
//                m_stream.Write(tableIndex);
//            m_stream.Write(columnIndex);
//            m_stream.Write(columnName);
//            m_stream.Write((byte)columnType);
//            m_lastTableIndex = tableIndex;
//        }

//        public void DeleteColumn(int tableIndex, int columnIndex)
//        {
//            EncodingHeader header = EncodingHeader.None;
//            if (m_lastTableIndex == tableIndex)
//                header |= EncodingHeader.IsTableIndexSameAsPrevious;

//            m_stream.Write((byte)header);
//            m_stream.Write((byte)MetadataCommand.DeleteColumn);
//            if ((header & EncodingHeader.IsTableIndexSameAsPrevious) == 0)
//                m_stream.Write(tableIndex);
//            m_stream.Write(columnIndex);
//            m_lastTableIndex = tableIndex;
//        }

//        public void AddValue(int tableIndex, int columnIndex, int rowIndex, byte[] value)
//        {
//            EncodingHeader header = EncodingHeader.IsAddValueCommand;
//            if (m_lastTableIndex == tableIndex)
//                header |= EncodingHeader.IsTableIndexSameAsPrevious;
//            if (m_lastRowIndex == rowIndex)
//                header |= EncodingHeader.IsRowIndexSameAsPrevious;

//            if (value == null)
//            {
//                header |= (EncodingHeader)1;
//            }
//            else if (value.Length <= 30)
//            {
//                header |= (EncodingHeader)value.Length + 2;
//            }

//            m_stream.Write((byte)header);
//            if ((header & EncodingHeader.IsTableIndexSameAsPrevious) == 0)
//                m_stream.Write(tableIndex);
//            m_stream.Write(columnIndex);
//            if ((header & EncodingHeader.IsRowIndexSameAsPrevious) == 0)
//                m_stream.Write(rowIndex);
//            if ((header & EncodingHeader.LengthMask) != 0)
//            {
//                if (value != null && value.Length > 0)
//                {
//                    m_stream.Write(value);
//                }
//            }
//            else
//            {
//                m_stream.Write(value == null);
//                if (value != null)
//                {
//                    m_stream.Write(value.Length);
//                    m_stream.Write(value);
//                }
//            }

//            m_lastRowIndex = rowIndex;
//            m_lastTableIndex = tableIndex;
//        }

//        public void DeleteRow(int tableIndex, int rowIndex)
//        {
//            EncodingHeader header = EncodingHeader.None;
//            if (m_lastTableIndex == tableIndex)
//                header |= EncodingHeader.IsTableIndexSameAsPrevious;
//            if (m_lastRowIndex == rowIndex)
//                header |= EncodingHeader.IsRowIndexSameAsPrevious;

//            m_stream.Write((byte)header);
//            m_stream.Write((byte)MetadataCommand.DeleteRow);
//            if ((header & EncodingHeader.IsTableIndexSameAsPrevious) == 0)
//                m_stream.Write(tableIndex);
//            if ((header & EncodingHeader.IsRowIndexSameAsPrevious) == 0)
//                m_stream.Write(rowIndex);

//            m_lastRowIndex = rowIndex;
//            m_lastTableIndex = tableIndex;
//        }

//        public void SelectAllTablesWithSchema()
//        {
//            EncodingHeader header = EncodingHeader.None;
//            m_stream.Write((byte)header);
//            m_stream.Write((byte)MetadataCommand.SelectAllTablesWithSchema);
//        }

//        public void ResyncTable(int tableIndex, Guid cachedInstanceId, long transactionId)
//        {
//            EncodingHeader header = EncodingHeader.None;
//            if (m_lastTableIndex == tableIndex)
//                header |= EncodingHeader.IsTableIndexSameAsPrevious;
//            m_stream.Write((byte)header);
//            m_stream.Write((byte)MetadataCommand.ResyncTable);
//            if ((header & EncodingHeader.IsTableIndexSameAsPrevious) == 0)
//                m_stream.Write(tableIndex);
//            m_stream.Write(cachedInstanceId);
//            m_stream.Write(transactionId);
//            m_lastTableIndex = tableIndex;
//        }

//        #endregion



//    }
//}

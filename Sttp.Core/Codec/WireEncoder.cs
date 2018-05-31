//using System;
//using System.Collections.Generic;
//using System.Threading;
//using CTP;

//namespace Sttp.Codec
//{
//    /// <summary>
//    /// Responsible for encoding each command into bytes
//    /// </summary>
//    public class WireEncoder
//    {
//        /// <summary>
//        /// Occurs when a packet of data must be sent on the wire. This is called immediately
//        /// after completing a Packet;
//        /// </summary>
//        public event Action<byte[], int, int> NewPacket;

//        //private DataPointEncoder m_dataPoint;
//        private CtpEncoder m_encoder;
//        private int m_rawChannelID;

//        /// <summary>
//        /// The desired number of bytes before data is automatically flushed via <see cref="NewPacket"/>
//        /// </summary>
//        public WireEncoder()
//        {
//            m_encoder = new CtpEncoder();
//            m_encoder.NewPacket += EncoderOnNewPacket;
//        }


//        public int GetNextRawChannelID()
//        {
//            int id = 0;
//            while (id == 0 || id == 1)
//            {
//                id = Interlocked.Increment(ref m_rawChannelID);
//            }
//            return id;
//        }

//        private void EncoderOnNewPacket(byte[] data, int position, int length)
//        {
//            NewPacket?.Invoke(data, position, length);
//        }

//        public void GetMetadataSchema(Guid? lastKnownRuntimeID = null, long? lastKnownVersionNumber = null)
//        {
//            m_encoder.SendDocumentCommands(new CommandGetMetadataSchema(lastKnownRuntimeID, lastKnownVersionNumber), "GetMetadataSchema");
//        }

//        public void MetadataSchema(Guid runtimeID, long versionNumber, List<MetadataSchemaTable> tables)
//        {
//            m_encoder.SendDocumentCommands(new CommandMetadataSchema(runtimeID, versionNumber, tables), "MetadataSchema");
//        }

//        public void MetadataSchemaUpdate(Guid runtimeID, long versionNumber, List<MetadataSchemaTableUpdate> tables)
//        {
//            m_encoder.SendDocumentCommands(new CommandMetadataSchemaUpdate(runtimeID, versionNumber, tables));
//        }

//        public void MetadataSchemaVersion(Guid runtimeID, long versionNumber)
//        {
//            m_encoder.SendDocumentCommands(new CommandMetadataSchemaVersion(runtimeID, versionNumber));
//        }

//        public void GetMetadata(string table, IEnumerable<string> columns)
//        {
//            m_encoder.SendDocumentCommands(new CommandGetMetadata(table, columns));
//        }

//        public void MetadataRequestFailed(string reason, string details)
//        {
//            m_encoder.SendDocumentCommands(new CommandMetadataRequestFailed(reason, details));
//        }

//        public void BeginMetadataResponse(int rawChannelID, Guid encodingMethod, Guid runtimeID, long versionNumber, string tableName, List<MetadataColumn> columns)
//        {
//            m_encoder.SendDocumentCommands(new CommandBeginMetadataResponse(rawChannelID, encodingMethod, runtimeID, versionNumber, tableName, columns));
//        }

//        public void EndMetadataResponse(int rawChannelID, int rowCount)
//        {
//            m_encoder.SendDocumentCommands(new CommandEndMetadataResponse(rawChannelID, rowCount));
//        }

//        public void SendCustomCommand(DocumentCommandBase command)
//        {
//            m_encoder.SendDocumentCommands(command);
//        }

//        public void Raw(int rawCommandCode, byte[] payload)
//        {
//            m_encoder.SendBinaryCommand(rawCommandCode, payload, 0, payload.Length);
//        }

//    }
//}

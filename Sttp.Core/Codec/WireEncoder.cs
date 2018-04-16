using System;
using System.Collections.Generic;
using System.Threading;
using CTP;
using CTP.Codec;

namespace Sttp.Codec
{
    /// <summary>
    /// Responsible for encoding each command into bytes
    /// </summary>
    public class WireEncoder
    {
        /// <summary>
        /// Occurs when a packet of data must be sent on the wire. This is called immediately
        /// after completing a Packet;
        /// </summary>
        public event Action<byte[], int, int> NewPacket;

        //private DataPointEncoder m_dataPoint;
        private SessionDetails m_sessionDetails;
        private CommandEncoder m_encoder;
        private int m_rawChannelID;

        /// <summary>
        /// The desired number of bytes before data is automatically flushed via <see cref="NewPacket"/>
        /// </summary>
        public WireEncoder()
        {
            m_sessionDetails = new SessionDetails();
            m_encoder = new CommandEncoder();
            m_encoder.NewPacket += EncoderOnNewPacket;
        }


        public int GetNextRawChannelID()
        {
            int id = 0;
            while (id == 0 || id == 1)
            {
                id = Interlocked.Increment(ref m_rawChannelID);
            }
            return id;
        }

        private void EncoderOnNewPacket(byte[] data, int position, int length)
        {
            NewPacket?.Invoke(data, position, length);
        }

        public void GetMetadataSchema(Guid? lastKnownRuntimeID = null, long? lastKnownVersionNumber = null)
        {
            m_encoder.SendMarkupCommand(new CommandGetMetadataSchema(lastKnownRuntimeID, lastKnownVersionNumber));
        }

        public void MetadataSchema(Guid runtimeID, long versionNumber, List<MetadataSchemaTable> tables)
        {
            m_encoder.SendMarkupCommand(new CommandMetadataSchema(runtimeID, versionNumber, tables));
        }

        public void MetadataSchemaUpdate(Guid runtimeID, long versionNumber, List<MetadataSchemaTableUpdate> tables)
        {
            m_encoder.SendMarkupCommand(new CommandMetadataSchemaUpdate(runtimeID, versionNumber, tables));
        }

        public void MetadataSchemaVersion(Guid runtimeID, long versionNumber)
        {
            m_encoder.SendMarkupCommand(new CommandMetadataSchemaVersion(runtimeID, versionNumber));
        }

        public void GetMetadata(string table, IEnumerable<string> columns)
        {
            m_encoder.SendMarkupCommand(new CommandGetMetadata(table, columns));
        }

        public void MetadataRequestFailed(string reason, string details)
        {
            m_encoder.SendMarkupCommand(new CommandMetadataRequestFailed(reason, details));
        }

        public void BeginMetadataResponse(int rawChannelID, Guid encodingMethod, Guid runtimeID, long versionNumber, string tableName, List<MetadataColumn> columns)
        {
            m_encoder.SendMarkupCommand(new CommandBeginMetadataResponse(rawChannelID, encodingMethod, runtimeID, versionNumber, tableName, columns));
        }

        public void EndMetadataResponse(int rawChannelID, int rowCount)
        {
            m_encoder.SendMarkupCommand(new CommandEndMetadataResponse(rawChannelID, rowCount));
        }

        public void SendCustomCommand(CommandBase command)
        {
            m_encoder.SendMarkupCommand(command);
        }

        public void SendCustomCommand(CtpMarkupWriter command)
        {
            m_encoder.SendMarkupCommand(command);
        }

        public void Raw(int rawCommandCode, byte[] payload)
        {
            m_encoder.SendRawCommand(rawCommandCode, payload, 0, payload.Length);
        }

    }
}

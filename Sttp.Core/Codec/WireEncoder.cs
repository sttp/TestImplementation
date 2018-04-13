using System;
using System.Collections.Generic;
using System.Threading;

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

        //private void SendNewPacket(byte[] buffer, int position, int length)
        //{
        //    NewPacket?.Invoke(buffer, position, length);
        //}

        //public void KeepAlive()
        //{
        //    m_encoder.SendMarkupCommand(new CommandKeepAlive());
        //}

        //public void GetMetadataProcedure(string procedureName, SttpMarkup options)
        //{
        //    m_encoder.SendMarkupCommand(new CommandGetMetadataProcedure(procedureName, options));
        //}

        //public void DataPointRequest(string instanceName, SttpTime startTime, SttpTime stopTime, SttpValue[] dataPointIDs, double? samplesPerSecond)
        //{
        //    m_encoder.SendMarkupCommand(new CommandDataPointRequest(instanceName, startTime, stopTime, dataPointIDs, samplesPerSecond));
        //}

        //public void DataPointRequestCompleted()
        //{
        //    m_encoder.SendMarkupCommand(new CommandDataPointResponseCompleted());
        //}

        //public void MapRuntimeIDs(List<SttpDataPointID> points)
        //{
        //    throw new NotImplementedException();
        //    //Need some more work here. there's probably some kind of request/reply or sent with data point stream.

        //    //m_stream.Clear();
        //    //m_stream.Write(points.Count);
        //    //foreach (var point in points)
        //    //{
        //    //    m_stream.Write(point.RuntimeID);
        //    //    m_stream.Write((byte)point.ValueTypeCode);
        //    //    switch (point.ValueTypeCode)
        //    //    {
        //    //        case SttpDataPointIDTypeCode.Null:
        //    //            throw new InvalidOperationException("A registered pointID cannot be null");
        //    //        case SttpDataPointIDTypeCode.Guid:
        //    //            m_stream.Write(point.AsGuid);
        //    //            break;
        //    //        case SttpDataPointIDTypeCode.String:
        //    //            m_stream.Write(point.AsString);
        //    //            break;
        //    //        case SttpDataPointIDTypeCode.SttpMarkup:
        //    //            m_stream.Write(point.AsSttpMarkup);
        //    //            break;
        //    //        default:
        //    //            throw new ArgumentOutOfRangeException();
        //    //    }
        //    //}
        //    //m_stream.Send(CommandCode.MapRuntimeIDs);
        //}


        public void SendCustomCommand(CommandBase command)
        {
            m_encoder.SendMarkupCommand(command);
        }

        public void SendCustomCommand(SttpMarkupWriter command)
        {
            m_encoder.SendMarkupCommand(command);
        }

        //public void NegotiateSession(SttpMarkup config)
        //{
        //    m_encoder.NegotiateSession(config);
        //}

        //public void NoOp(bool shouldEcho)
        //{
        //    var sml = new SttpMarkupWriter();
        //    using (sml.StartElement("NoOp"))
        //    {
        //        sml.WriteValue("ShouldEcho", shouldEcho);

        //    }
        //    m_encoder.Heartbeat(sml.ToSttpMarkup());
        //}

        //public void RequestFailed(string failedCommand, bool terminateConnection, string reason, string details)
        //{
        //    m_encoder.SendMarkupCommand(new CommandRequestFailed(failedCommand, terminateConnection, reason, details));
        //}

        //public void RequestSucceeded(string commandSucceeded, string reason, string details)
        //{
        //    m_encoder.SendMarkupCommand(new CommandRequestSucceeded(commandSucceeded, reason, details));
        //}

        //public void Subscribe(string instanceName, SttpValue[] dataPointIDs, double? samplesPerSecond)
        //{
        //    m_encoder.SendMarkupCommand(new CommandConfigureSubscription(instanceName, dataPointIDs, samplesPerSecond));
        //}

        //public void Subscription(SubscriptionAppendMode mode, SttpMarkup options, List<SttpDataPointID> dataPoints)
        //{
        //    var sml = new SttpMarkupWriter();
        //    using (sml.StartElement("RequestSucceeded"))
        //    {
        //        sml.WriteValue("SubscriptionAppendMode", mode.ToString());
        //        sml.WriteValue("Options", options);
        //        //ToDo: rework this command.
        //        //sml.WriteValue("Reason", reason);
        //        //sml.WriteValue("Details", details);
        //    }
        //    m_encoder.Message(sml.ToSttpMarkup());
        //}

        public void Raw(int rawCommandCode, byte[] payload)
        {
            m_encoder.SendRawCommand(rawCommandCode, payload, 0, payload.Length);
        }

    }
}

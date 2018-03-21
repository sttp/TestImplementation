using System;
using System.Collections.Generic;

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
        private Metadata.MetadataCommandBuilder m_metadata;
        private CommandEncoder m_encoder;

        /// <summary>
        /// The desired number of bytes before data is automatically flushed via <see cref="NewPacket"/>
        /// </summary>
        public WireEncoder()
        {
            m_sessionDetails = new SessionDetails();
            m_encoder = new CommandEncoder();
            m_metadata = new Metadata.MetadataCommandBuilder(m_encoder, m_sessionDetails);
            m_encoder.NewPacket += EncoderOnNewPacket;
        }

        private void EncoderOnNewPacket(byte[] data, int position, int length)
        {
            NewPacket?.Invoke(data, position, length);
        }

        /// <summary>
        /// Builds a metadata response message. Be sure to call the SendCommand periodically.
        /// </summary>
        /// <returns></returns>
        public Metadata.MetadataCommandBuilder MetadataCommandBuilder()
        {
            m_metadata.BeginCommand();
            return m_metadata;
        }

        //private void SendNewPacket(byte[] buffer, int position, int length)
        //{
        //    NewPacket?.Invoke(buffer, position, length);
        //}

        public void BulkTransportRequest(Guid id, long offset, int length)
        {
            m_encoder.SendMarkupCommand(new CommandBulkTransportRequest(id, offset, length));
        }

        public void BulkTransportReply(Guid id, long bytesRemaining, byte[] data)
        {
            m_encoder.SendMarkupCommand(new CommandBulkTransportReply(id, bytesRemaining, data));
        }

        public void DataPointRequest(SttpTime startTime, SttpTime stopTime, SttpValue[] dataPointIDs, double? samplesPerSecond)
        {
            m_encoder.SendMarkupCommand(new CommandDataPointRequest(startTime, stopTime, dataPointIDs, samplesPerSecond));
        }

        public void DataPointRequestCompleted()
        {
            m_encoder.SendMarkupCommand(new CommandDataPointRequestCompleted());
        }

        public void GetMetadata(Guid requestID, Guid schemaVersion, long revision, bool areUpdateQueries, List<SttpQueryBase> queries)
        {
            m_encoder.SendMarkupCommand(new CommandGetMetadata(requestID, schemaVersion, revision, areUpdateQueries, queries));
        }

        public void GetMetadataSchema(Guid schemaVersion, long revision)
        {
            m_encoder.SendMarkupCommand(new CommandGetMetadataSchema(schemaVersion, revision));
        }

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

        public void SendCustomCommand(SttpMarkupWriter command)
        {
            m_encoder.SendMarkupCommand(command);
        }

        public void MetadataSchema(Guid schemaVersion, long revision, List<MetadataSchemaTables> tables)
        {
            m_encoder.SendMarkupCommand(new CommandMetadataSchema(schemaVersion, revision, tables));
        }

        public void MetadataSchemaUpdate(Guid schemaVersion, long revision, long updatedFromRevision, List<MetadataSchemaTableUpdate> tables)
        {
            m_encoder.SendMarkupCommand(new CommandMetadataSchemaUpdate(schemaVersion, revision, updatedFromRevision, tables));
        }

        public void MetadataVersionNotCompatible()
        {
            m_encoder.SendMarkupCommand(new CommandMetadataVersionNotCompatible());
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

        public void RequestFailed(string failedCommand, bool terminateConnection, string reason, string details)
        {
            m_encoder.SendMarkupCommand(new CommandRequestFailed(failedCommand, terminateConnection, reason, details));
        }

        public void RequestSucceeded(string commandSucceeded, string reason, string details)
        {
            m_encoder.SendMarkupCommand(new CommandRequestSucceeded(commandSucceeded, reason, details));
        }

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

        public void SubscriptionStream(byte encodingMethod, byte[] buffer)
        {
            m_encoder.SendRawCommand(encodingMethod, buffer, 0, buffer.Length);
        }

    }
}

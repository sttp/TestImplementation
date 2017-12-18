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
        //private Metadata.MetadataCommandBuilder m_metadata;
        private CommandEncoder m_encoder;

        /// <summary>
        /// The desired number of bytes before data is automatically flushed via <see cref="NewPacket"/>
        /// </summary>
        public WireEncoder()
        {
            m_sessionDetails = new SessionDetails();
            m_encoder = new CommandEncoder();
            //m_metadata = new Metadata.MetadataCommandBuilder(m_encoder, m_sessionDetails);
        }

        ///// <summary>
        ///// Builds a metadata response message. Be sure to call the SendCommand periodically.
        ///// </summary>
        ///// <returns></returns>
        //public Metadata.MetadataCommandBuilder MetadataCommandBuilder()
        //{
        //    m_metadata.BeginCommand();
        //    return m_metadata;
        //}

        //private void SendNewPacket(byte[] buffer, int position, int length)
        //{
        //    NewPacket?.Invoke(buffer, position, length);
        //}

        //public void BulkTransportRequest(Guid id, long startingPosition, long length)
        //{
        //    var sml = new SttpMarkupWriter();
        //    using (sml.StartElement("BulkTransportRequest"))
        //    {
        //        sml.WriteValue("ID", id);
        //        sml.WriteValue("StartingPosition", startingPosition);
        //        sml.WriteValue("Length", length);
        //    }
        //    m_encoder.GetLargeObject(sml.ToSttpMarkup());
        //}

        //public void BulkTransportBeginSend(Guid id, BulkTransportMode mode, BulkTransportCompression compression, long originalSize, byte[] source, int position, int length)
        //{
        //    var sml = new SttpMarkupWriter();
        //    using (sml.StartElement("BulkTransportBeginSend"))
        //    {
        //        sml.WriteValue("ID", id);
        //        sml.WriteValue("BulkTransportMode", mode.ToString());
        //        sml.WriteValue("BulkTransportCompression", compression.ToString());
        //        sml.WriteValue("OriginalSize", originalSize);
        //        sml.WriteValue("Data", source, position, length);
        //    }
        //    m_encoder.LargeObject(sml.ToSttpMarkup());
        //}

        //public void BulkTransportCancelSend(Guid id)
        //{
        //    var sml = new SttpMarkupWriter();
        //    using (sml.StartElement("BulkTransportCancelSend"))
        //    {
        //        sml.WriteValue("ID", id);
        //    }
        //    m_encoder.GetLargeObject(sml.ToSttpMarkup());
        //}

        //public void BulkTransportSendFragment(Guid id, long bytesRemaining, byte[] content, int position, int length)
        //{
        //    var sml = new SttpMarkupWriter();
        //    using (sml.StartElement("BulkTransportSendFragment"))
        //    {
        //        sml.WriteValue("ID", id);
        //        sml.WriteValue("bytesRemaining", bytesRemaining);
        //        sml.WriteValue("Data", content, position, length);
        //    }
        //    m_encoder.LargeObject(sml.ToSttpMarkup());
        //}

        //public void DataPointReply(Guid requestID, bool isEndOfResponse, byte encodingMethod, byte[] buffer)
        //{
        //    var sml = new SttpMarkupWriter();
        //    using (sml.StartElement("DataPointReply"))
        //    {
        //        sml.WriteValue("RequestID", requestID);
        //        sml.WriteValue("IsEndOfResponse", isEndOfResponse);
        //        sml.WriteValue("EncodingMethod", encodingMethod);
        //        sml.WriteValue("Data", buffer);
        //    }
        //    m_encoder.LargeObject(sml.ToSttpMarkup());
        //}

        //public void DataPointRequest(SttpMarkup request)
        //{
        //    m_encoder.LargeObject(request);
        //}

        //public void GetMetadata(Guid requestID, Guid schemaVersion, long revision, bool areUpdateQueries, SttpMarkup queries)
        //{

        //    var sml = new SttpMarkupWriter();
        //    using (sml.StartElement("GetMetadata"))
        //    {
        //        sml.WriteValue("RequestID", requestID);
        //        sml.WriteValue("SchemaVersion", schemaVersion);
        //        sml.WriteValue("Revision", revision);
        //        sml.WriteValue("AreUpdateQueries", areUpdateQueries);

        //        using (sml.StartElement("Queries"))
        //        {
        //            sml.UnionWith(queries);
        //        }
        //    }
        //    m_encoder.GetMetadata(sml.ToSttpMarkup());
        //}

        //public void GetMetadataSchema(Guid schemaVersion, long revision)
        //{
        //    var metadataSchema = new CommandGetMetadataSchema(schemaVersion, revision);
        //    m_encoder.SendCommand(metadataSchema);
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

        //public void MetadataSchema(Guid schemaVersion, long revision, List<MetadataSchemaTables> tables)
        //{
        //    var sml = new SttpMarkupWriter();
        //    using (sml.StartElement("MetadataSchema"))
        //    {
        //        sml.WriteValue("SchemaVersion", schemaVersion);
        //        sml.WriteValue("Revision", revision);
        //        using (sml.StartElement("Tables"))
        //        {
        //            foreach (var table in tables)
        //            {
        //                table.Save(sml);
        //            }
        //        }
        //    }
        //    m_encoder.Metadata(sml.ToSttpMarkup());
        //}

        //public void MetadataSchemaUpdate(Guid schemaVersion, long revision, long updatedFromRevision, List<MetadataSchemaTableUpdate> tables)
        //{
        //    var sml = new SttpMarkupWriter();
        //    using (sml.StartElement("MetadataSchemaUpdate"))
        //    {
        //        sml.WriteValue("SchemaVersion", schemaVersion);
        //        sml.WriteValue("Revision", revision);
        //        sml.WriteValue("UpdatedFromRevision", updatedFromRevision);
        //        using (sml.StartElement("Tables"))
        //        {
        //            foreach (var table in tables)
        //            {
        //                table.Save(sml);
        //            }
        //        }
        //    }
        //    m_encoder.Metadata(sml.ToSttpMarkup());
        //}

        //public void MetadataVersionNotCompatible()
        //{
        //    var sml = new SttpMarkupWriter();
        //    using (sml.StartElement("MetadataVersionNotCompatible"))
        //    {
        //    }
        //    m_encoder.Metadata(sml.ToSttpMarkup());
        //}

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

        //public void RequestFailed(CommandCode failedCommand, bool terminateConnection, string reason, string details)
        //{
        //    var sml = new SttpMarkupWriter();
        //    using (sml.StartElement("RequestFailed"))
        //    {
        //        sml.WriteValue("CommandCode", failedCommand.ToString());
        //        sml.WriteValue("TerminateConnection", terminateConnection);
        //        sml.WriteValue("Reason", reason);
        //        sml.WriteValue("Details", details);
        //    }
        //    m_encoder.Message(sml.ToSttpMarkup());
        //}

        //public void RequestSucceeded(CommandCode commandSucceeded, string reason, string details)
        //{
        //    var sml = new SttpMarkupWriter();
        //    using (sml.StartElement("RequestSucceeded"))
        //    {
        //        sml.WriteValue("CommandCode", commandSucceeded.ToString());
        //        sml.WriteValue("Reason", reason);
        //        sml.WriteValue("Details", details);
        //    }
        //    m_encoder.Message(sml.ToSttpMarkup());
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

        public void SubscriptionStream(byte encodingMethod, byte[] buffer)
        {
            m_encoder.SubscriptionStream(encodingMethod, buffer, 0, buffer.Length);
        }

    }
}

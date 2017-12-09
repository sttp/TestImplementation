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
        private PayloadWriter m_stream;

        /// <summary>
        /// The desired number of bytes before data is automatically flushed via <see cref="NewPacket"/>
        /// </summary>
        public WireEncoder()
        {
            m_sessionDetails = new SessionDetails();
            m_encoder = new CommandEncoder(m_sessionDetails, SendNewPacket);
            m_stream = new PayloadWriter(m_encoder);
            m_metadata = new Metadata.MetadataCommandBuilder(m_encoder, m_sessionDetails);
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

        private void SendNewPacket(byte[] buffer, int position, int length)
        {
            NewPacket?.Invoke(buffer, position, length);
        }

        public void BulkTransportBeginSend(Guid id, BulkTransportMode mode, BulkTransportCompression compression, long originalSize, byte[] source, long position, int length)
        {
            m_stream.Clear();
            m_stream.Write(id);
            m_stream.Write((byte)mode);
            m_stream.Write((byte)compression);
            m_stream.Write(originalSize);
            m_stream.Write(source, position, length);
            m_stream.Send(CommandCode.BulkTransportBeginSend);
        }

        public void BulkTransportCancelSend(Guid id)
        {
            m_stream.Clear();
            m_stream.Write(id);
            m_stream.Send(CommandCode.BulkTransportCancelSend);
        }

        public void BulkTransportSendFragment(Guid id, long bytesRemaining, byte[] content, long position, int length)
        {
            m_stream.Clear();
            m_stream.Write(id);
            m_stream.Write(bytesRemaining);
            m_stream.Write(content, position, length);
            m_stream.Send(CommandCode.BulkTransportSendFragment);
        }

        public void DataPointReply(Guid requestID, bool isEndOfResponse, byte encodingMethod, byte[] buffer)
        {
            m_stream.Clear();
            m_stream.Write(requestID);
            m_stream.Write(isEndOfResponse);
            m_stream.Write(encodingMethod);
            m_stream.Write(buffer);
            m_stream.Send(CommandCode.DataPointReply);
        }

        public void DataPointRequest(SttpMarkup options, List<SttpDataPointID> dataPoints)
        {
            m_stream.Clear();
            m_stream.Write(options);
            m_stream.Write(dataPoints);
            m_stream.Send(CommandCode.DataPointRequest);
        }

        public void GetMetadata(Guid requestID, Guid schemaVersion, long revision, bool areUpdateQueries, SttpMarkup queries)
        {
            m_stream.Clear();
            m_stream.Write(requestID);
            m_stream.Write(schemaVersion);
            m_stream.Write(revision);
            m_stream.Write(areUpdateQueries);
            m_stream.Write(queries);
            m_stream.Send(CommandCode.GetMetadata);
        }

        public void GetMetadataSchema(Guid schemaVersion, long revision)
        {
            m_stream.Clear();
            m_stream.Write(schemaVersion);
            m_stream.Write(revision);
            m_stream.Send(CommandCode.GetMetadataSchema);
        }

        public void MapRuntimeIDs(List<SttpDataPointID> points)
        {
            m_stream.Clear();
            m_stream.Write(points.Count);
            foreach (var point in points)
            {
                m_stream.Write(point.RuntimeID);
                m_stream.Write((byte)point.ValueTypeCode);
                switch (point.ValueTypeCode)
                {
                    case SttpDataPointIDTypeCode.Null:
                        throw new InvalidOperationException("A registered pointID cannot be null");
                    case SttpDataPointIDTypeCode.Guid:
                        m_stream.Write(point.AsGuid);
                        break;
                    case SttpDataPointIDTypeCode.String:
                        m_stream.Write(point.AsString);
                        break;
                    case SttpDataPointIDTypeCode.NamedSet:
                        m_stream.Write(point.AsNamedSet);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            m_stream.Send(CommandCode.MapRuntimeIDs);
        }

        public void MetadataSchema(Guid schemaVersion, long revision, List<MetadataSchemaTables> tables)
        {
            m_stream.Clear();
            m_stream.Write(schemaVersion);
            m_stream.Write(revision);
            m_stream.Write(tables);
            m_stream.Send(CommandCode.MetadataSchema);
        }

        public void MetadataSchemaUpdate(Guid schemaVersion, long revision, long updatedFromRevision, List<MetadataSchemaTableUpdate> tables)
        {
            m_stream.Clear();
            m_stream.Write(schemaVersion);
            m_stream.Write(revision);
            m_stream.Write(updatedFromRevision);
            m_stream.Write(tables);
            m_stream.Send(CommandCode.MetadataSchemaUpdate);
        }

        public void MetadataVersionNotCompatible()
        {
            m_stream.Clear();
            m_stream.Send(CommandCode.MetadataVersionNotCompatible);
        }

        public void NegotiateSession(SttpMarkup config)
        {
            m_stream.Clear();
            m_stream.Write(config);
            m_stream.Send(CommandCode.NegotiateSession);
        }

        public void NoOp(bool shouldEcho)
        {
            m_stream.Clear();
            m_stream.Write(shouldEcho);
            m_stream.Send(CommandCode.NoOp);
        }

        public void RequestFailed(CommandCode failedCommand, bool terminateConnection, string reason, string details)
        {
            m_stream.Clear();
            m_stream.Write((byte)failedCommand);
            m_stream.Write(terminateConnection);
            m_stream.Write(reason);
            m_stream.Write(details);
            m_stream.Send(CommandCode.RequestFailed);
        }

        public void RequestSucceeded(CommandCode commandSucceeded, string reason, string details)
        {
            m_stream.Clear();
            m_stream.Write((byte)commandSucceeded);
            m_stream.Write(reason);
            m_stream.Write(details);
            m_stream.Send(CommandCode.RequestSucceeded);
        }

        public void Subscription(SubscriptionAppendMode mode, SttpMarkup options, List<SttpDataPointID> dataPoints)
        {
            m_stream.Clear();
            m_stream.Write((byte)mode);
            m_stream.Write(options);
            m_stream.Write(dataPoints);
            m_stream.Send(CommandCode.DataPointRequest);
        }

        public void SubscriptionStream(byte encodingMethod, byte[] buffer)
        {
            m_stream.Clear();
            m_stream.Write(encodingMethod);
            m_stream.Write(buffer);
            m_stream.Send(CommandCode.SubscriptionStream);
        }

       
       

    }
}

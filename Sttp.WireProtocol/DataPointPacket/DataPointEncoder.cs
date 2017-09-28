using Sttp.IO;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sttp.WireProtocol.Codec.DataPointPacket
{
    public class DataPointEncoder
    {
        private MemoryStream m_stream = new MemoryStream();

        /// <summary>
        /// Begins a new metadata packet
        /// </summary>
        public void BeginCommand()
        {
            m_stream.Position = 0;
            m_stream.SetLength(0);
        }

        /// <summary>
        /// Ends a metadata packet and requests the buffer block that was allocated.
        /// </summary>
        /// <returns></returns>
        public byte[] EndCommand()
        {
            return m_stream.ToArray();
        }

        public void MapRuntimeID(DataPointKeyWire point)
        {
            m_stream.Write((byte)DataPointCommand.MapRuntimeID);
            m_stream.Write(point.UniqueID);
            m_stream.Write(point.RuntimeID);
            m_stream.Write((byte)point.Type);
            m_stream.Write((byte)point.Flags);
        }

        public void SendDataPoint(DataPointWire point)
        {
            m_stream.Write(point.ID);
            m_stream.Write(point.Time.Seconds);
            m_stream.Write(point.Time.Fraction);
            m_stream.Write(point.Length);
            m_stream.Write((byte)point.Flags);
            m_stream.Write((byte)point.QualityFlags);
            m_stream.Write(point.BulkDataValueID);
            m_stream.Write(point.Value);
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;

namespace Sttp.WireProtocol.Codec.DataPointPacket
{
    public class Encoder
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

        }

        public void SendDataPoint(DataPointWire point)
        {

        }

    }
}

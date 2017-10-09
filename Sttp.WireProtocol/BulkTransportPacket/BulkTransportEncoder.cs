using System;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Responsible for encoding each command into bytes
    /// </summary>
    public class BulkTransportEncoder
    {
        private Action<byte[], int, int> m_baseEncoder;

        public BulkTransportEncoder(Action<byte[], int, int> baseEncoder)
        {
            m_baseEncoder = baseEncoder;
        }
      
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Contains the details negotiated by the session.
    /// </summary>
    public class SessionDetails
    {
        public readonly ProtocolLimits Limits = new ProtocolLimits();
        public bool SupportsDeflate;
    }
}

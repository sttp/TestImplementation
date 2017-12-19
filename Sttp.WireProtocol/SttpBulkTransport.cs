using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp
{
    public class SttpBulkTransport
    {
        public SttpValueTypeCode FundamentalType;
        public Guid BulkTransportID;
        public long Length;

        public static bool operator ==(SttpBulkTransport a, SttpBulkTransport b)
        {
            return a.FundamentalType == b.FundamentalType && a.BulkTransportID == b.BulkTransportID && a.Length == b.Length;
        }

        public static bool operator !=(SttpBulkTransport a, SttpBulkTransport b)
        {
            return !(a == b);
        }
    }
}

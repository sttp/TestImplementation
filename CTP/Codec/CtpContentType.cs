using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP
{
    /// <summary>
    /// Indicates the content type of the protocol.
    /// </summary>
    public enum CtpChannelCode : byte
    {
        /// <summary>
        /// Requests commands that require interaction between the server and the client.
        /// Example: 
        /// GetMetadata
        /// Subscribe
        /// Unsubscribe
        /// Authenticate
        /// </summary>
        Request = 0,
        /// <summary>
        /// Notifications provide individual complete messages that are not part of a more complex state machine.
        /// 
        /// Example: 
        /// Log Messages,
        /// Metadata Changed
        /// Subscription Completed
        /// </summary>
        Notifications = 1,
        /// <summary>
        /// Data Streams are for providing data out of sequence. The behavior of these data streams are determined in the requests.
        /// Examples include: 
        /// Streaming measurements (Historical or realtime)
        /// Streaming metadata changes (This isn't in the plans, but may be a feature sometime in the future)
        /// Backfilling data.
        /// Message Logs
        /// </summary>
        DataStream1 = 2,
        DataStream2 = 3,

    }
}

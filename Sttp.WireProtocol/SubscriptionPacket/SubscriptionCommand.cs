using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.SubscriptionPacket
{
    public enum SubscriptionCommand : byte
    {
        /// <summary>
        /// An invalid command to indicate that nothing is assigned.
        /// This cannot be sent over the wire.
        /// </summary>
        Invalid = 0x00,

        /// <summary>
        /// Specifies that this subscription is an augmentation of an existing subscription. 
        /// </summary>
        IsAugmentedSubscription,

        /// <summary>
        /// Sets the state information for every data point that is specified after this state. 
        /// Note: This typically will be specified once, then a list of points will follow, 
        /// however, this can be specified for each point if the publisher supports this level
        /// of granularity.
        /// 
        /// Payload:
        /// long DownSamplingSamplesPerSecond = -1
        /// long DownSamplingPerDay = -1
        /// PriorityFlags flags = Normal
        /// ReliabilityFlags flags = Normal
        /// VerificationFlags flags = Normal
        /// PublishByException flags = Normal
        /// ShouldUnsubscribe = false
        /// 
        /// </summary>
        ConfigureDataPointState,



        /// <summary>
        /// Identifies all data points.
        /// </summary>
        AllDataPoints,

        /// <summary>
        /// Identifies all Data Points listed in a table
        /// 
        /// Payload:
        /// TableIndex
        /// </summary>
        TableDataPoints,

        /// <summary>
        /// Specifies a DataPoint by ID
        /// 
        /// Payload: 
        /// int DataPointID
        /// </summary>
        DataPointByID,

        /// <summary>
        /// Sets the start time for a subscription for historical data. Default is null.
        /// 
        /// Payload: 
        /// bool HasValue
        /// SttpTimestamp startTime
        /// </summary>
        StartTime,

        /// <summary>
        /// Sets the stop time for a subscription for historical data. Default is null.
        /// 
        /// Payload:
        /// bool HasValue
        /// SttpTimestamp stopTime
        /// </summary>
        StopTime,

    }
}

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
        /// Indicates that a request for data is being populated.
        /// Payload: 
        /// None
        /// </summary>
        BeginRequest,

        /// <summary>
        /// Indicates that the request for data has been completed and should be executed upon.
        /// Payload: 
        /// None
        /// </summary>
        CompleteRequest,

        /// <summary>
        /// Determines if this subscription is an augmentation of an existing subscription. Default is false.
        /// 
        /// Payload:
        /// bool isAugmentationSubscription
        /// </summary>
        IsAugmentedSubscription,

        /// <summary>
        /// Sets the down-sampling mode for this subscription to be a certain number of samples per day.
        /// This is valid for all data points defined after this point.
        /// 
        /// long desiredSamplesPerDay
        /// </summary>
        SetDownSamplingPerDay,

        /// <summary>
        /// Sets the down-sampling mode for this subscription to be a value every 'X' number of seconds.
        /// This is valid for all data points defined after this point.
        /// 
        /// long secondsPerSample,
        /// </summary>
        SetDownSamplingSamplesPerSecond,

        /// <summary>
        /// Sets the priority 
        /// This is valid for all data points defined after this point.
        /// 
        /// PriorityFlags flags.
        /// </summary>
        SetPriority,

        /// <summary>
        /// Indicates that all of the following data points should be added the subscription.
        /// </summary>
        SubscribeToTheFollowing,

        /// <summary>
        /// Indicates that all of the following data points should be removed from the subscription.
        /// </summary>
        UnsubscribeToTheFollowing,

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
        DatapointByID,

        /// <summary>
        /// Sets the start time for a subscription for historical data. Default is null.
        /// 
        /// Payload: 
        /// bool HasValue
        /// SttpTimestamp startTime
        /// </summary>
        SetStartTime,

        /// <summary>
        /// Sets the stop time for a subscription for historical data. Default is null.
        /// 
        /// Payload:
        /// bool HasValue
        /// SttpTimestamp stopTime
        /// </summary>
        SetStopTime,
        
    }
}

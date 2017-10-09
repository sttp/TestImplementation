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
        /// Stops receiving data for an active subscription
        /// </summary>
        StopSubscription,

        /// <summary>
        /// Resets all options for a pending subscription request.
        /// </summary>
        Reset,

        /// <summary>
        /// Creates a universe subscription
        /// </summary>
        SubscribeToAll,

        /// <summary>
        /// Specifies a list of IDs to subscribe to.
        /// </summary>
        SubsribeToList,

        /// <summary>
        /// Removes items from an existing list.
        /// </summary>
        UnsubscribeFromList,
        
        /// <summary>
        /// Sets the down-sampling mode for this subscription.
        /// </summary>
        SetDownSampling,

        /// <summary>
        /// Sets the start time for a subscription for historical data.
        /// </summary>
        SetStartTime,

        /// <summary>
        /// Sets the stop time for a subscription for historical data.
        /// </summary>
        SetStopTime,

        /// <summary>
        /// Begins the stream for the existing built request.
        /// </summary>
        BeginStream,
        
    }
}

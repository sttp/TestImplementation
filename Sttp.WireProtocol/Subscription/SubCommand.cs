using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.Subscription
{
    public enum SubCommand : byte
    {
        /// <summary>
        /// An invalid command to indicate that nothing is assigned.
        /// This cannot be sent over the wire.
        /// </summary>
        Invalid = 0x00,

        /// <summary>
        /// Indicates that the subscription details should be cleared.
        /// 
        /// </summary>
        UnsubscribeFromAll,

        /// <summary>
        /// Identifies all data points.
        /// SubscribeMode { Replace Existing Subscribe | Remove Subscription | Append Subscription }
        /// SttpNamedSet options
        /// </summary>
        AllDataPoints,

        /// <summary>
        /// Requests all the measurements base as the result of a query. The column selected must be a pointID.
        /// 
        /// Example SELECT Magnitude, Angle, Real, Imaginary FROM VIPair WHERE IsCurrent=true
        /// 
        /// Payload:
        /// SubscribeMode { Replace Existing Subscribe | Remove Subscription | Append Subscription }
        /// SttpQueryExpression expression          - An sttp query expression.
        /// SttpNamedSet options
        /// 
        /// </summary>
        ByQuery,

        /// <summary>
        /// Specifies all of the PointIDs to subscribe to.
        /// 
        /// Payload: 
        /// SubscribeMode { Replace Existing Subscribe | Remove Subscription | Append Subscription }
        /// SttpPointID[] Points
        /// SttpNamedSet options
        /// 
        /// </summary>
        DataPointByID,
    }


}

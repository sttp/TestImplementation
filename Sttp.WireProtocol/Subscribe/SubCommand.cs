﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.Subscribe
{
    public enum SubCommand : byte
    {
        /// <summary>
        /// An invalid command to indicate that nothing is assigned.
        /// This cannot be sent over the wire.
        /// </summary>
        Invalid = 0x00,

        /// <summary>
        /// Specifies an option based connection string to configure the request. 
        /// 
        /// Payload:
        /// SttpNamedSet options
        /// 
        /// </summary>
        ConfigureOptions,

        /// <summary>
        /// Identifies all data points.
        /// SubscribeMode { Replace Existing Subscribe | Remove Subscription | Append Subscription }
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
        /// 
        /// </summary>
        ByQuery,

        /// <summary>
        /// Specifies all of the PointIDs to subscribe to.
        /// 
        /// Payload: 
        /// SubscribeMode { Replace Existing Subscribe | Remove Subscription | Append Subscription }
        /// SttpPointID[] Points
        /// </summary>
        DataPointByID,
    }


}
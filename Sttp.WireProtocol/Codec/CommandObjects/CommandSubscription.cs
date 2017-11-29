using System;
using System.Collections.Generic;

namespace Sttp.Codec
{
    /// <summary>
    /// Responsible for encoding each command into bytes
    /// </summary>
    public class CommandSubscription 
    {
        public CommandCode CommandCode => CommandCode.Subscription;

        public readonly SttpNamedSet Options;
        public readonly SubscriptionAppendMode Mode;
        public readonly List<SttpDataPointID> DataPoints;

        public CommandSubscription(PayloadReader reader)
        {
            Options = reader.ReadSttpNamedSet();
            Mode = (SubscriptionAppendMode) reader.ReadByte();
            DataPoints = reader.ReadListSttpDataPointID();
        }
    }
}

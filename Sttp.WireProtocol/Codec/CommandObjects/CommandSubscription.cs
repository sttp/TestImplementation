using System;

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
        public readonly SttpDataPointID[] DataPoints;

        public CommandSubscription(PayloadReader reader)
        {
            Options = reader.Read<SttpNamedSet>();
            Mode = reader.Read<SubscriptionAppendMode>();
            DataPoints = reader.ReadArray<SttpDataPointID>();
        }
    }
}

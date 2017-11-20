using System;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Responsible for encoding each command into bytes
    /// </summary>
    public class CommandSubscription 
    {
        public CommandCode CommandCode => CommandCode.Subscription;

        public SttpNamedSet Options;
        public SubscriptionAppendMode Mode;
        public SttpDataPointID[] DataPoints;

        public void Fill(PayloadReader reader)
        {
            Options = reader.Read<SttpNamedSet>();
            Mode = reader.Read<SubscriptionAppendMode>();
            DataPoints = reader.ReadArray<SttpDataPointID>();
        }
    }
}

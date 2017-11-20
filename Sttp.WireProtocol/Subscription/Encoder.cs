using System;

namespace Sttp.WireProtocol.Subscription
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.Subscription;

        public Encoder(CommandEncoder commandEncoder, SessionDetails sessionDetails)
            : base(commandEncoder, sessionDetails)
        {

        }

        public void Subscription(SubscriptionAppendMode mode, SttpNamedSet options, SttpDataPointID[] dataPoints)
        {
            Stream.Write(CommandCode.DataPointRequest);
            Stream.Write(mode);
            Stream.Write(options);
            Stream.Write(dataPoints);
        }


    }

    public enum SubscriptionAppendMode : byte
    {
        Replace,
        Remove,
        Append
    }
}

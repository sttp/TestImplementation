using System;

namespace Sttp.WireProtocol.Subscription
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.Subscription;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
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

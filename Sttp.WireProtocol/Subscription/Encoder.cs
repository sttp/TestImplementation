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

        public void ConfigureOptions(SttpNamedSet options)
        {
            Stream.Write(SubCommand.UnsubscribeFromAll);
            Stream.Write(options);
        }

        public void AllDataPoints(SubscriptionAppendMode mode)
        {
            Stream.Write(SubCommand.AllDataPoints);
            Stream.Write(mode);
        }

        public void ByQuery(SubscriptionAppendMode mode, SttpQueryExpression query)
        {
            Stream.Write(SubCommand.ByQuery);
            Stream.Write(mode);
            Stream.Write(query);
        }

        public void DataPointByID(SubscriptionAppendMode mode, SttpDataPointID[] dataPoints)
        {
            Stream.Write(SubCommand.DataPointByID);
            Stream.Write(mode);
            Stream.Write(dataPoints);
        }

        public void UnsubscribeFromAll()
        {
            Stream.Write(SubCommand.UnsubscribeFromAll);
        }

    }

    public enum SubscriptionAppendMode : byte
    {
        Replace,
        Remove,
        Append
    }
}

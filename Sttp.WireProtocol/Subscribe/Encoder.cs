using System;

namespace Sttp.WireProtocol.Subscribe
{
    public class Encoder : BaseEncoder
    {
        public override CommandCode Code => CommandCode.Subscribe;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void ConfigureOptions(SttpNamedSet options)
        {
            Stream.Write(SubCommand.ConfigureOptions);
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

        public void DataPointByID(SubscriptionAppendMode mode, SttpPointID[] points)
        {
            Stream.Write(SubCommand.DataPointByID);
            Stream.Write(mode);
            Stream.Write(points);
        }

    }

    public enum SubscriptionAppendMode : byte
    {
        Replace,
        Remove,
        Append
    }
}

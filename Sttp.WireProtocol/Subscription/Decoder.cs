using System;

namespace Sttp.WireProtocol.Subscription
{
    /// <summary>
    /// Responsible for encoding each command into bytes
    /// </summary>
    public class Decoder 
    {
        public CommandCode CommandCode => CommandCode.DataPointRequest;

        public SttpNamedSet Options;
        public SubscriptionAppendMode Mode;
        public SttpDataPointID[] DataPoints;

        public void Fill(PacketReader reader)
        {
            Options = reader.Read<SttpNamedSet>();
            Mode = reader.Read<SubscriptionAppendMode>();
            DataPoints = reader.ReadArray<SttpDataPointID>();
        }
    }
}

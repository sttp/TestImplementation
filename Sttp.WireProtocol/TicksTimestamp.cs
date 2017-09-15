namespace Sttp.WireProtocol
{
    public class TicksTimestamp
    {
        public long Value; // 100-nanosecond intervals since 1/1/0001, +/-16,384 years
        public TimestampFlags Flags;
    }
    // 9-bytes
}

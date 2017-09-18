namespace Sttp.WireProtocol
{
    public enum ResolutionType
    {
        Latest = 0,           // Data down-sampled to latest received
        Closest = 0x800,      // Data down-sampled to closest timestamp
        BestQuality = 0x1000, // Data down-sampled to item with best quality
        Filter = 0x1800       // Data down-sampled with simple DataType specific filter
    }
    // 2-bits
}

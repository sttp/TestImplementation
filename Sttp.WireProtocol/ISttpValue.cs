namespace Sttp.WireProtocol
{
    public interface ISttpValue
    {
        bool IsNull { get; set; }
        double AsDouble { get; set; }
        long AsInt64 { get; set; }
        ulong AsUInt64 { get; set; }
        float AsSingle { get; set; }
        byte[] AsBuffer { get; set; }
        object AsObject { get; set; }
        SttpFundamentalTypeCode FundamentalTypeCode { get; }

    }
}
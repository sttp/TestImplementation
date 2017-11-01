namespace Sttp.WireProtocol
{
    public interface ISttpValue
    {
        bool IsNull { get; set; }
        double AsDouble { get; set; }
        int AsInt32 { get; set; }
        long AsInt64 { get; set; }
        float AsSingle { get; set; }
        string AsString { get; set; }
        byte[] AsBuffer { get; set; }
        SttpFundamentalTypeCode FundamentalTypeCode { get; }

    }
}
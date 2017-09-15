namespace Sttp.WireProtocol
{
    public class DataPointKeySet
    {
        public SetType Type;
        //uint32 count; <- serialize only
        public DataPointKey[] Keys;
    }
}

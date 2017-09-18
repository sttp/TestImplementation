namespace Sttp.WireProtocol
{
    public class ProtocolVersions : IEncode
    {
        //public byte Count; <-- serialize value only
        public Version[] Versions;

        public byte[] Encode()
        {
            return null;
        }

        public static ProtocolVersions Decode(byte[] buffer, int startIndex, int length)
        {
            return null;
        }
    }
}
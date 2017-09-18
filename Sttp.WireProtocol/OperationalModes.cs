namespace Sttp.WireProtocol
{
    public class OperationalModes : IEncode
    {
        public StringEncodingFlags Encodings;
        public ushort UdpPort;
        public NamedVersions Stateful;
        public NamedVersions Stateless;

        public byte[] Encode()
        {
            return null;
        }

        public static OperationalModes Decode(byte[] buffer, int startIndex, int length)
        {
            return null;
        }
    }
}

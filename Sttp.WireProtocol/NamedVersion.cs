namespace Sttp.WireProtocol
{
    public class NamedVersion
    {
        public string Name;
        public Version Version;

        public byte[] Encode()
        {
            return null;
        }

        public static NamedVersion Decode(byte[] buffer, int startIndex, int length)
        {
            return null;
        }
    }
}

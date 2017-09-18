namespace Sttp.WireProtocol
{
    public class NamedVersions : IEncode
    {
        //public ushort Count; <-- serialize only
        public NamedVersion[] Items;

        public byte[] Encode()
        {
            return null;
        }

        public static NamedVersions Decode(byte[] buffer, int startIndex, int length)
        {
            return null;
        }
    }
}

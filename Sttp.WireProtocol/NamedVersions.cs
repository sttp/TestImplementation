namespace Sttp.WireProtocol
{
    public class NamedVersions 
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

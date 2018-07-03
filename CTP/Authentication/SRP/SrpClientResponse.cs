namespace CTP.SRP
{
    [DocumentName("SrpClientResponse")]
    public class SrpClientResponse
        : DocumentObject<SrpClientResponse>
    {
        [DocumentField()] public byte[] PublicA { get; private set; }
        [DocumentField()] public byte[] ClientChallenge { get; private set; }

        public SrpClientResponse(byte[] publicA, byte[] clientChallenge)
        {
            ClientChallenge = clientChallenge;
            PublicA = publicA;
        }

        private SrpClientResponse()
        {
        }

        public static explicit operator SrpClientResponse(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}
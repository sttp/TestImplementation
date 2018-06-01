using System;
using CTP.Serialization;

namespace CTP.SRP
{
    [CtpCommand("SrpIdentity")]
    public class SrpIdentity
        : CtpDocumentObject<SrpIdentity>
    {
        [CtpSerializeField()]
        public string UserName { get; private set; }

        public SrpIdentity(string userName)
        {
            UserName = userName;
        }

        private SrpIdentity()
        { }

        public static explicit operator SrpIdentity(CtpDocument obj)
        {
            return ConvertFromDocument(obj);
        }
    }

    [CtpCommand("SrpIdentityLookup")]
    public class SrpIdentityLookup
        : CtpDocumentObject<SrpIdentityLookup>
    {
        [CtpSerializeField()] public int SrpStrength { get; private set; }
        [CtpSerializeField()] public byte[] Salt { get; private set; }
        [CtpSerializeField()] public byte[] PublicB { get; private set; }

        public SrpIdentityLookup(SrpStrength strength, byte[] salt, byte[] publicB)
        {
            SrpStrength = (int)strength;
            Salt = salt;
            PublicB = publicB;
        }

        private SrpIdentityLookup()
        {

        }

        public static explicit operator SrpIdentityLookup(CtpDocument obj)
        {
            return ConvertFromDocument(obj);
        }
    }

    [CtpCommand("SrpClientResponse")]
    public class SrpClientResponse
        : CtpDocumentObject<SrpClientResponse>
    {
        [CtpSerializeField()] public byte[] PublicA { get; private set; }
        [CtpSerializeField()] public byte[] ClientChallenge { get; private set; }

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
            return ConvertFromDocument(obj);
        }
    }

    [CtpCommand("SrpServerResponse")]
    public class SrpServerResponse
        : CtpDocumentObject<SrpServerResponse>
    {
        [CtpSerializeField()] public byte[] ServerChallenge { get; private set; }

        public SrpServerResponse(byte[] serverChallenge)
        {
            ServerChallenge = serverChallenge;
        }

        private SrpServerResponse()
        {
        }

        public static explicit operator SrpServerResponse(CtpDocument obj)
        {
            return ConvertFromDocument(obj);
        }
    }
}
using System;
using CTP.Serialization;

namespace CTP.SRP
{
    [DocumentName("SrpIdentity")]
    public class SrpIdentity
        : DocumentObject<SrpIdentity>
    {
        [DocumentField()]
        public string UserName { get; private set; }

        public SrpIdentity(string userName)
        {
            UserName = userName;
        }

        private SrpIdentity()
        { }

        public static explicit operator SrpIdentity(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }

    [DocumentName("SrpIdentityLookup")]
    public class SrpIdentityLookup
        : DocumentObject<SrpIdentityLookup>
    {
        [DocumentField()] public int SrpStrength { get; private set; }
        [DocumentField()] public byte[] Salt { get; private set; }
        [DocumentField()] public byte[] PublicB { get; private set; }

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
            return FromDocument(obj);
        }
    }

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

    [DocumentName("SrpServerResponse")]
    public class SrpServerResponse
        : DocumentObject<SrpServerResponse>
    {
        [DocumentField()] public byte[] ServerChallenge { get; private set; }

        public SrpServerResponse(byte[] serverChallenge)
        {
            ServerChallenge = serverChallenge;
        }

        private SrpServerResponse()
        {
        }

        public static explicit operator SrpServerResponse(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}
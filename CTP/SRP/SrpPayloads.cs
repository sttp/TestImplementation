using System;
using CTP.Serialization;

namespace CTP.SRP
{
    [CtpSerializable]
    public class SrpIdentity
    {
        [CtpSerializeField()]
        public string UserName { get; private set; }

        public SrpIdentity(string userName)
        {
            UserName = userName;
        }

        private SrpIdentity() { }
    }

    [CtpSerializable]
    public class SrpIdentityLookup
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
    }

    [CtpSerializable]
    public class SrpClientResponse
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
    }

    [CtpSerializable]
    public class SrpServerResponse
    {
        [CtpSerializeField()] public byte[] ServerChallenge { get; private set; }

        public SrpServerResponse(byte[] serverChallenge)
        {
            ServerChallenge = serverChallenge;
        }

        private SrpServerResponse()
        {
        }
    }
}
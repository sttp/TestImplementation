using System;
using CTP.Serialization;

namespace CTP.SRP
{
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
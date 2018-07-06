using System;
using CTP.Serialization;

namespace CTP.SRP
{
    [DocumentName("SrpServerProof")]
    public class SrpServerProof
        : DocumentObject<SrpServerProof>
    {
        [DocumentField()] public byte[] ServerProof { get; private set; }

        public SrpServerProof(byte[] serverProof)
        {
            ServerProof = serverProof;
        }

        private SrpServerProof()
        {
        }

        public static explicit operator SrpServerProof(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}
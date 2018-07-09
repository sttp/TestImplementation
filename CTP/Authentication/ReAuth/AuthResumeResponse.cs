using System;

namespace CTP.SRP
{
    [DocumentName("AuthResumeResponse")]
    public class AuthResumeResponse
        : DocumentObject<AuthResumeResponse>
    {
        /// <summary>
        /// A nonce challenge.
        /// </summary>
        [DocumentField()] public byte[] ServerChallenge { get; private set; }

        private AuthResumeResponse()
        {

        }

        public static explicit operator AuthResumeResponse(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}
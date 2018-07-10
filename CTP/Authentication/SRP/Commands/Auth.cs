namespace CTP.SRP
{
    /// <summary>
    /// Starts a new authentication process.
    /// </summary>
    [DocumentName("Auth")]
    public class Auth
        : DocumentObject<Auth>
    {
        /// <summary>
        /// The name associated with the credential that will be used for authentication
        /// </summary>
        [DocumentField()] public string CredentialName { get; private set; }
        /// <summary>
        /// Indicates if this credential is a onetime use pairing credential.
        /// </summary>
        [DocumentField()] public bool IsPairingCredential { get; private set; }

        public Auth(string credentialName, bool isPairingCredential)
        {
            CredentialName = credentialName;
            IsPairingCredential = isPairingCredential;
        }

        private Auth()
        {

        }

        public static explicit operator Auth(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}
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
        /// This will request that the stored key will be changed at the next authentication
        /// </summary>
        [DocumentField()] public bool RotateKey { get; private set; }

        /// <summary>
        /// This will request that the stored password will be changed at the next authentication.
        /// </summary>
        [DocumentField()] public bool ChangePassword { get; private set; }

        public Auth(string credentialName, bool rotateKey, bool changePassword)
        {
            CredentialName = credentialName;
            RotateKey = rotateKey;
            ChangePassword = changePassword;
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
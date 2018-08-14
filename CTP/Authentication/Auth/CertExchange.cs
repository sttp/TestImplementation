namespace CTP.SRP
{
    /// <summary>
    /// Requests that the server and client mutually trust each other's certificates using the provided pairing key.
    /// </summary>
    [DocumentName("CertExchange")]
    public class CertExchange
        : DocumentObject<CertExchange>
    {
        /// <summary>
        /// The name associated with the account that will be used for authentication
        /// </summary>
        [DocumentField()] public string AccountName { get; private set; }

        /// <summary>
        /// The Public-A from the SRP algorithm. A = g^a
        /// </summary>
        [DocumentField()] public byte[] PublicA { get; private set; }

        public CertExchange(string accountName, byte[] publicA)
        {
            AccountName = accountName;
            PublicA = publicA;
        }

        private CertExchange()
        {

        }

        public static explicit operator CertExchange(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}
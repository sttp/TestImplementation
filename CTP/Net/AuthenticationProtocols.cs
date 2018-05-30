namespace CTP.Net
{
    public enum AuthenticationProtocols : byte
    {
        None = 0,
        SRP = 1,
        NegotiateStream = 2,
        OAUTH = 3,
        LDAP = 4,
        ResumeSession = 5,
        CertificatePairing = 6,
        SessionPairing = 7,
    }
}
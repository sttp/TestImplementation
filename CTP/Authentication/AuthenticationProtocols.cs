namespace CTP.Net
{
    public enum AuthenticationProtocols : byte
    {
        None = 0,
        SRP = 1,
        UserCertificate = 2,
        Negotiate = 3,
        OAUTH = 4,
        LDAP = 5,
        ResumeSession = 6,
        CertificatePairing = 7,
        SessionPairing = 8,
    }
}
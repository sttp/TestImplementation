using System;
using System.Net;
using System.Net.Security;
using System.Security;
using System.Security.Principal;

namespace CTP.Net
{
    public static class ClientAuthentication
    {
        public static void AuthenticateWithSRP(CtpSession session, NetworkCredential credential)
        {
            AuthenticateSrp(session, credential.UserName, credential.SecurePassword, out byte[] privateSessionKey);
        }

        private static void AuthenticateSrp(CtpSession stream, string identity, SecureString password, out byte[] privateSessionKey)
        {
            var auth = new AuthenticateSrpAsClient(stream, identity, password);
            auth.Start();
            if (!auth.Wait.WaitOne(3000))
            {
                throw new Exception("Authentication Timed Out");
            }
            privateSessionKey = auth.privateSessionKey;
        }

        public static void AuthenticateWithNegotiate(CtpSession session, NetworkCredential credentials)
        {
            //using (var stream = session.CreateStream())
            //{
            //    session.SendDocument(new AuthNegotiate(stream.StreamID));
            //    session.Win = new NegotiateStream(stream, true);
            //    session.Win.AuthenticateAsClient(credentials, session.HostName ?? string.Empty, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification);
            //}
        }

        public static void AuthenticateWithLDAP(CtpSession session, NetworkCredential credential)
        {

        }

        public static void AuthenticateWithOAUTH(CtpSession session, string ticket)
        {

        }

        public static void ResumeSession(CtpSession session, string ticket)
        {

        }

        public static void PairCertificates(CtpSession session, string pairingUsername, string pairingPassword)
        {

        }

        public static void PairSession(CtpSession session, string pairingUsername, string pairingPassword)
        {

        }
    }
}

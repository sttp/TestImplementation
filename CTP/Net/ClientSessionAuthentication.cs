using System;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Numerics;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using CTP.SRP;

namespace CTP.Net
{
    public class AuthenticateSrpAsClient : IRequestHandler
    {
        private int m_state = 0;
        private CtpSession m_stream;
        private readonly string m_identity;
        private readonly SecureString m_password;
        public byte[] privateSessionKey;
        private byte[] challengeServer;
        public ManualResetEvent Wait = new ManualResetEvent(false);

        public AuthenticateSrpAsClient(CtpSession stream, string identity, SecureString password)
        {
            m_stream = stream;
            m_password = password;
            m_identity = identity.Normalize(NormalizationForm.FormKC).Trim().ToLower();
        }

        public void OnNewPayload(CtpRequest request, byte[] payload)
        {
            throw new NotSupportedException();
        }

        public void OnDocument(CtpRequest request, CtpDocument payload)
        {
            switch (m_state)
            {
                case 1:
                    m_state++;
                    SrpIdentityLookup lookup = (SrpIdentityLookup)payload;


                    var privateA = RNG.CreateSalt(32).ToUnsignedBigInteger();
                    var strength = (SrpStrength)lookup.SrpStrength;
                    var publicB = lookup.PublicB.ToUnsignedBigInteger();
                    var param = SrpConstants.Lookup(strength);
                    var publicA = BigInteger.ModPow(param.g, privateA, param.N);

                    var x = lookup.ComputePassword(m_identity, m_password);
                    var verifier = param.g.ModPow(x, param.N);

                    var u = SrpMethods.ComputeU(param.PaddedBytes, publicA, publicB);
                    var exp1 = privateA.ModAdd(u.ModMul(x, param.N), param.N);
                    var base1 = publicB.ModSub(param.k.ModMul(verifier, param.N), param.N);
                    var sessionKey = base1.ModPow(exp1, param.N);
                    challengeServer = SrpMethods.ComputeChallenge(1, sessionKey, m_stream.LocalCertificate, m_stream.RemoteCertificate);
                    var challengeClient = SrpMethods.ComputeChallenge(2, sessionKey, m_stream.LocalCertificate, m_stream.RemoteCertificate);
                    privateSessionKey = SrpMethods.ComputeChallenge(3, sessionKey, m_stream.LocalCertificate, m_stream.RemoteCertificate);
                    byte[] clientChallenge = challengeClient;

                    request.SendDocument(new SrpClientResponse(publicA.ToUnsignedByteArray(), clientChallenge));
                    break;
                case 2:
                    m_state++;

                    SrpServerResponse cr = (SrpServerResponse)payload;
                    if (!challengeServer.SequenceEqual(cr.ServerChallenge))
                        throw new Exception("Failed server challenge");
                    Wait.Set();
                    break;
                default:
                    throw new NotSupportedException();
            }

        }

        public void Start()
        {
            m_state = 1;
            m_stream.CommandStream.BeginRequest(new SrpIdentity(m_identity), this, null);
        }
    }

    public static class ClientSessionAuthentication
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
            using (var stream = session.CreateStream())
            {
                session.SendDocument(new AuthNegotiate(stream.StreamID));
                session.Win = new NegotiateStream(stream, true);
                session.Win.AuthenticateAsClient(credentials, session.HostName ?? string.Empty, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification);
            }
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

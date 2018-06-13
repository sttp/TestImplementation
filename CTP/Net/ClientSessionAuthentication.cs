using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Numerics;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using CTP.SRP;

namespace CTP.Net
{
    public static class ClientSessionAuthentication
    {
        public static void AuthenticateWithSRP(CtpSession session, NetworkCredential credential)
        {
            AuthenticateSrp(session, credential.UserName, credential.SecurePassword, out byte[] privateSessionKey);
        }

        private static void AuthenticateSrp(CtpSession stream, string identity, SecureString password, out byte[] privateSessionKey)
        {
            identity = identity.Normalize(NormalizationForm.FormKC).Trim().ToLower();
            //password = password.Normalize(NormalizationForm.FormKC);

            stream.SendDocument(new SrpIdentity(identity));
            var lookup = stream.ReadDocument<SrpIdentityLookup>();
            var privateA = RNG.CreateSalt(32).ToUnsignedBigInteger();
            var strength = (SrpStrength)lookup.SrpStrength;
            var publicB = lookup.PublicB.ToUnsignedBigInteger();
            var param = SrpConstants.Lookup(strength);
            var publicA = BigInteger.ModPow(param.g, privateA, param.N);

            var x = lookup.ComputePassword(identity, password);
            var verifier = param.g.ModPow(x, param.N);

            var u = SrpMethods.ComputeU(param.PaddedBytes, publicA, publicB);
            var exp1 = privateA.ModAdd(u.ModMul(x, param.N), param.N);
            var base1 = publicB.ModSub(param.k.ModMul(verifier, param.N), param.N);
            var sessionKey = base1.ModPow(exp1, param.N);
            var challengeServer = SrpMethods.ComputeChallenge(1, sessionKey, stream.LocalCertificate, stream.RemoteCertificate);
            var challengeClient = SrpMethods.ComputeChallenge(2, sessionKey, stream.LocalCertificate, stream.RemoteCertificate);
            privateSessionKey = SrpMethods.ComputeChallenge(3, sessionKey, stream.LocalCertificate, stream.RemoteCertificate);
            byte[] clientChallenge = challengeClient;

            stream.SendDocument(new SrpClientResponse(publicA.ToUnsignedByteArray(), clientChallenge));

            var cr = stream.ReadDocument<SrpServerResponse>();
            if (!challengeServer.SequenceEqual(cr.ServerChallenge))
                throw new Exception("Failed server challenge");
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

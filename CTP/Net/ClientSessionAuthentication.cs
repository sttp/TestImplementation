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

            stream.CommandStream.SendDocumentCommand(new SrpIdentity(identity));

            var lookup = (SrpIdentityLookup)stream.CommandStream.NextCommand(-1).Document;
            var privateA = RNG.CreateSalt(32).ToUnsignedBigInteger();
            var strength = (SrpStrength)lookup.SrpStrength;
            var salt = lookup.Salt;
            var publicB = lookup.PublicB.ToUnsignedBigInteger();
            var param = SrpConstants.Lookup(strength);
            var publicA = BigInteger.ModPow(param.g, privateA, param.N);
            var x = SrpMethods.ComputeX(salt, identity, password).ToUnsignedBigInteger();
            var verifier = param.g.ModPow(x, param.N);

            var u = SrpMethods.ComputeU(param.PaddedBytes, publicA, publicB);
            var exp1 = privateA.ModAdd(u.ModMul(x, param.N), param.N);
            var base1 = publicB.ModSub(param.k.ModMul(verifier, param.N), param.N);
            var sessionKey = base1.ModPow(exp1, param.N);
            var challengeServer = SrpMethods.ComputeChallenge(1, sessionKey, stream.Ssl?.LocalCertificate, stream.Ssl?.RemoteCertificate);
            var challengeClient = SrpMethods.ComputeChallenge(2, sessionKey, stream.Ssl?.LocalCertificate, stream.Ssl?.RemoteCertificate);
            privateSessionKey = SrpMethods.ComputeChallenge(3, sessionKey, stream.Ssl?.LocalCertificate, stream.Ssl?.RemoteCertificate);
            byte[] clientChallenge = challengeClient;

            stream.CommandStream.SendDocumentCommand(new SrpClientResponse(publicA.ToUnsignedByteArray(), clientChallenge));

            var cr = (SrpServerResponse)stream.CommandStream.NextCommand(-1).Document;
            if (!challengeServer.SequenceEqual(cr.ServerChallenge))
                throw new Exception("Failed server challenge");
        }

        private static void AuthenticateWithNegotiate(CtpSession session, NetworkCredential credentials)
        {
            session.Win = new NegotiateStream(session.FinalStream, true);
            session.Win.AuthenticateAsClient(credentials, session.HostName ?? string.Empty, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification);
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

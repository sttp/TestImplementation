using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using GSF.IO;

namespace CTP.SRP
{
    public static class Srp6aClient
    {
        public static void Authenticate(string identity, string password, Stream stream, X509Certificate clientCertificate, X509Certificate serverCertificate)
        {
            Authenticate(identity, password, stream, clientCertificate, serverCertificate, out byte[] privateSessionKey);
        }

        public static void Authenticate(string identity, string password, Stream stream, X509Certificate clientCertificate, X509Certificate serverCertificate, out byte[] privateSessionKey)
        {
            identity = identity.Normalize(NormalizationForm.FormKC).Trim().ToLower();
            password = password.Normalize(NormalizationForm.FormKC);

            stream.WriteDocument(new SrpIdentity(identity),"SrpIdentity");
            stream.Flush();

            var lookup = stream.ReadDocument<SrpIdentityLookup>("SrpIdentityLookup");

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
            var challengeServer = SrpMethods.ComputeChallenge(1, sessionKey, clientCertificate, serverCertificate);
            var challengeClient = SrpMethods.ComputeChallenge(2, sessionKey, clientCertificate, serverCertificate);
            privateSessionKey = SrpMethods.ComputeChallenge(3, sessionKey, clientCertificate, serverCertificate);
            byte[] clientChallenge = challengeClient;

            stream.WriteDocument(new SrpClientResponse(publicA.ToUnsignedByteArray(), clientChallenge), "SrpClientResponse");
            stream.Flush();

            var cr = stream.ReadDocument<SrpServerResponse>("SrpServerResponse");
            if (!challengeServer.SequenceEqual(cr.ServerChallenge))
                throw new Exception("Failed server challenge");
        }
    }
}

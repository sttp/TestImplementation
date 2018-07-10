using System;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using CTP.SRP;

namespace CTP.Net
{
    public class SrpClientAuth
    {
        public static ClientResumeTicket Authenticate(NetworkCredential credentials, CtpStream stream, SslStream sslStream)
        {
            WriteDocument(stream, new Auth(credentials.UserName, false));
            AuthResponse authResponse = (AuthResponse)ReadDocument(stream);
            var credentialName = credentials.UserName.Normalize(NormalizationForm.FormKC).Trim().ToLower();
            var privateA = RNG.CreateSalt(32).ToUnsignedBigInteger();
            var strength = (SrpStrength)authResponse.BitStrength;
            var publicB = authResponse.PublicB.ToUnsignedBigInteger();
            var param = SrpConstants.Lookup(strength);
            var publicA = BigInteger.ModPow(param.g, privateA, param.N);
            var x = authResponse.ComputeX(credentialName, credentials.SecurePassword);
            var verifier = param.g.ModPow(x, param.N);
            var u = SrpMethods.ComputeU(param.PaddedBytes, publicA, publicB);
            var exp1 = privateA.ModAdd(u.ModMul(x, param.N), param.N);
            var base1 = publicB.ModSub(param.k.ModMul(verifier, param.N), param.N);
            var sessionKey = base1.ModPow(exp1, param.N);
            var privateSessionKey = SrpMethods.ComputeChallenge(sessionKey, sslStream?.RemoteCertificate);
            var proof = new AuthClientProof(publicA.ToByteArray(), CreateKey(privateSessionKey, "Client Proof"));
            WriteDocument(stream, proof);
            AuthServerProof cr = (AuthServerProof)ReadDocument(stream);

            byte[] serverProof = CreateKey(privateSessionKey, "Server Proof");
            if (!serverProof.SequenceEqual(cr.ServerProof))
                throw new Exception("Failed server challenge");

            if ((cr.SessionTicket?.Length ?? 0) > 0)
            {
                return cr.CreateResumeTicket(credentials.UserName, CreateKey(privateSessionKey, "Ticket Signing"), CreateKey(privateSessionKey, "Challenge Response"));
            }

            return null;
        }

        private static void WriteDocument(CtpStream stream, DocumentObject command)
        {
            stream.Send(0, command.ToDocument().ToArray());
        }

        private static CtpDocument ReadDocument(CtpStream stream)
        {
            stream.Read(-1);
            return new CtpDocument(stream.Results.Payload);
        }

        private static byte[] CreateKey(byte[] privateSessionKey, string keyName)
        {
            using (var hmac = new HMACSHA256(privateSessionKey))
            {
                byte[] name = Encoding.ASCII.GetBytes(keyName);
                return hmac.ComputeHash(name, 0, name.Length);
            }
        }

    }
}
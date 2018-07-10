using System;
using System.Linq;
using System.Net.Security;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using CTP.SRP;

namespace CTP.Net
{
    public static class SrpServerAuth
    {
        public static SrpUserMapping AuthSrp(ResumeSessionKeys resumeKeys, ServerAuthentication serverAuth, Auth command, CtpStream stream, SslStream ssl)
        {
            SrpCredential<SrpUserMapping> credential = serverAuth.LookupCredential(command);
            SrpConstants param;
            BigInteger verifier;
            BigInteger privateB;
            BigInteger publicB;

            param = SrpConstants.Lookup(credential.Verifier.SrpStrength);
            verifier = credential.Verifier.VerifierCode.ToUnsignedBigInteger();
            privateB = RNG.CreateSalt(32).ToUnsignedBigInteger();
            publicB = param.k.ModMul(verifier, param.N).ModAdd(param.g.ModPow(privateB, param.N), param.N);

            WriteDocument(stream, new AuthResponse(credential.Verifier.SrpStrength, credential.Verifier.Salt, publicB.ToUnsignedByteArray()));

            var clientProof = (AuthClientProof)ReadDocument(stream);
            var publicA = clientProof.PublicA.ToUnsignedBigInteger();
            var u = SrpMethods.ComputeU(param.PaddedBytes, publicA, publicB);
            var sessionKey = publicA.ModMul(verifier.ModPow(u, param.N), param.N).ModPow(privateB, param.N);
            var privateSessionKey = SrpMethods.ComputeChallenge(sessionKey, ssl?.LocalCertificate);

            var cproof = CreateKey(privateSessionKey, "Client Proof");
            var sproof = CreateKey(privateSessionKey, "Server Proof");

            if (!clientProof.ClientProof.SequenceEqual(cproof))
            {
                throw new Exception("Authorization failed");
            }

            var tkey = CreateKey(privateSessionKey, "Ticket Signing");
            var ckey = CreateKey(privateSessionKey, "Challenge Response Key");

            var serverProof = resumeKeys.CreateServerProofAndTicket(sproof, command.CredentialName, credential.Token.Roles, tkey, ckey);

            WriteDocument(stream, serverProof);

            return credential.Token;
        }

        private static byte[] CreateKey(byte[] privateSessionKey, string keyName)
        {
            using (var hmac = new HMACSHA256(privateSessionKey))
            {
                byte[] name = Encoding.ASCII.GetBytes(keyName);
                return hmac.ComputeHash(name, 0, name.Length);
            }
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


    }
}
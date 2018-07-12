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
        public static SrpCredential AuthSrp(MasterSecretDatabase resumeKeys, ServerAuthentication serverAuth, Auth command, CtpStream stream, SslStream ssl)
        {
            SrpCredential credential = serverAuth.LookupCredential(command);
            SrpConstants param;
            BigInteger verifier;
            BigInteger privateB;
            BigInteger publicB;

            param = SrpConstants.Lookup(credential.Verifier.SrpStrength);
            verifier = credential.Verifier.VerifierCode.ToUnsignedBigInteger();
            privateB = Security.CreateSalt(32).ToUnsignedBigInteger();
            publicB = param.k.ModMul(verifier, param.N).ModAdd(param.g.ModPow(privateB, param.N), param.N);

            WriteDocument(stream, new AuthResponse(credential.Verifier.SrpStrength, credential.Verifier.Salt, publicB.ToUnsignedByteArray()));

            var clientProof = (AuthClientProof)ReadDocument(stream);
            var publicA = clientProof.PublicA.ToUnsignedBigInteger();
            var u = SrpMethods.ComputeU(param.PaddedBytes, publicA, publicB);
            var sessionKey = publicA.ModMul(verifier.ModPow(u, param.N), param.N).ModPow(privateB, param.N);
            var privateSessionKey = SrpMethods.ComputeChallenge(sessionKey, ssl?.LocalCertificate);

            var cproof = Security.ComputeHMAC(privateSessionKey, "Client Proof");
            var sproof = Security.ComputeHMAC(privateSessionKey, "Server Proof");

            if (!clientProof.ClientProof.SequenceEqual(cproof))
            {
                throw new Exception("Authorization failed");
            }

            var key = resumeKeys.GetShortKey();
            var signingKey = key.GetSignatureKey(credential.CredentialNameID);
            signingKey = Security.XOR(signingKey, Security.ComputeHMAC(privateSessionKey, "Ticket Key"));
            var serverProof = new AuthServerProof(sproof, key.ID, key.RemainingSeconds, (uint)key.ExpireMinutes, credential.CredentialNameID, signingKey, credential.Roles);

            WriteDocument(stream, serverProof);

            return credential;
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
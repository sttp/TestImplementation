using System;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography;
using CTP.Authentication;
using CTP.SRP;

namespace CTP.Net
{
    public static class ResumeServerAuth
    {
        public static void Authenticate(MasterSecretDatabase keyLookup, AuthResume authResume, CtpStream stream, SslStream sslStream)
        {
            var ticket = new Ticket(authResume.Ticket);
            var key = keyLookup.TryGetKey(ticket.MasterSecretID);
            byte[] ticketKey = key.GetSignatureKey(ticket.CredentialNameID);
            if (ticket.VerifySignature(ticketKey))
            {
                byte[] serverChallenge = Security.CreateSalt(32);
                AuthResumeResponse response = new AuthResumeResponse(serverChallenge);
                WriteDocument(stream, response);

                byte[] challengeKey = Security.ComputeHMAC(ticketKey, ticket.Signature);

                var cp = (AuthResumeClientProof)ReadDocument(stream);
                byte[] cproof = Security.ComputeHMAC(challengeKey, serverChallenge.Concat(cp.ClientChallenge, sslStream?.RemoteCertificate?.GetSerialNumber()));
                byte[] sproof = Security.ComputeHMAC(challengeKey, cp.ClientChallenge.Concat(serverChallenge, sslStream?.RemoteCertificate?.GetSerialNumber()));

                if (!cp.ClientProof.SequenceEqual(cproof))
                {
                    throw new Exception("Authorization Failed");
                }

                var sp = new AuthResumeServerProof(sproof);
                WriteDocument(stream, sp);
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
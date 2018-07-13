using System;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography;
using CTP.SRP;

namespace CTP.Net
{
    public static class ResumeClientAuth
    {
        public static void Authenticate(ClientResumeTicket resumeCredentials, CtpStream stream, SslStream sslStream)
        {
            var auth = new AuthResume(resumeCredentials.Ticket);
            WriteDocument(stream, auth);
            var authResponse = (AuthResumeResponse)ReadDocument(stream);

            byte[] clientChallenge = Security.CreateSalt(32);
            byte[] cproof;
            byte[] sproof;
            using (var hmac = new HMACSHA256(resumeCredentials.ChallengeResponseKey))
            {
                cproof = hmac.ComputeHash(authResponse.ServerChallenge.Concat(clientChallenge, sslStream?.RemoteCertificate?.GetSerialNumber()));
                sproof = hmac.ComputeHash(clientChallenge.Concat(authResponse.ServerChallenge, sslStream?.RemoteCertificate?.GetSerialNumber()));
            }
            var rcp = new AuthResumeClientProof(cproof, clientChallenge);
            WriteDocument(stream, rcp);
            var rsp = (AuthResumeServerProof)ReadDocument(stream);

            if (!rsp.ServerProof.SequenceEqual(sproof))
            {
                throw new Exception("Authorization Failed");
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
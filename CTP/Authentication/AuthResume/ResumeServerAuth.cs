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
        public static void Authenticate(ResumeSessionKeys keyLookup, AuthResume authResume, CtpStream stream, SslStream sslStream)
        {
            if (keyLookup.TryDecryptTicket(authResume, out EncryptedSessionTicketDetails serverSession, out SessionTicket ticket))
            {
                if (ticket.ValidAfter < serverSession.ValidAfter)
                    throw new Exception("Bad Ticket");
                if (ticket.ValidBefore > serverSession.ValidBefore)
                    throw new Exception("Bad Ticket");

                byte[] serverChallenge = RNG.CreateSalt(32);
                AuthResumeResponse response = new AuthResumeResponse(serverChallenge);
                WriteDocument(stream, response);

                var cp = (AuthResumeClientProof)ReadDocument(stream);
                byte[] cproof;
                byte[] sproof;
                using (var hmac = new HMACSHA256(serverSession.ChallengeResponseKey))
                {
                    hmac.Key = hmac.ComputeHash(ticket.TicketSalt);
                    cproof = hmac.ComputeHash(serverChallenge.Concat(cp.ClientChallenge, sslStream.RemoteCertificate.GetSerialNumber()));
                    sproof = hmac.ComputeHash(cp.ClientChallenge.Concat(serverChallenge, sslStream.RemoteCertificate.GetSerialNumber()));
                }

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
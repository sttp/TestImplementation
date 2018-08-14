using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CTP.Authentication;
using CTP.Net;

namespace CTP.SRP
{
    public static class TicketExchange
    {
        public static void ClientRequestTicket()
        {

        }

        public static RequestTicketResponse ServerCreateTicket(MasterSecretDatabase resumeKeys, uint accountRuntimeID, string[] roles)
        {
            var key = resumeKeys.GetShortKey();
            var signingKey = key.GetSignatureKey(accountRuntimeID);
            return new RequestTicketResponse(key.ID, key.RemainingSeconds, (uint)key.ExpireMinutes, accountRuntimeID, signingKey, roles);
        }

        public static void ClientResumeTicket(ClientResumeTicket resumeCredentials, CtpStream stream, SslStream sslStream)
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


        public static void ServerResumeTicket(MasterSecretDatabase keyLookup, AuthResume authResume, CtpStream stream, SslStream sslStream)
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

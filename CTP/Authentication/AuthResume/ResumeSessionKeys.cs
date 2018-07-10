using System;
using System.Collections.Generic;
using CTP.Authentication;
using CTP.SRP;
using GSF;

namespace CTP.Net
{
    public class ResumeSessionKeys
    {
        private class ResumeSessionKey
        {
            public Guid Key;
            public byte[] EncryptionKey;
            public byte[] MACKey;

            public ResumeSessionKey()
            {
                Key = Guid.NewGuid();
                EncryptionKey = RNG.CreateSalt(32);
                MACKey = RNG.CreateSalt(64);
            }
        }

        private Dictionary<Guid, ResumeSessionKey> m_keyCache = new Dictionary<Guid, ResumeSessionKey>();
        private ResumeSessionKey m_currentKey;
        public ResumeSessionKeys()
        {
            m_currentKey = new ResumeSessionKey();
            m_keyCache[m_currentKey.Key] = m_currentKey;
        }

        public AuthServerProof CreateServerProofAndTicket(byte[] sproof, string credentialName, string[] roles, byte[] ticketSigningKey, byte[] challengeResponseKey)
        {
            DateTime start = DateTime.UtcNow.Date.AddDays(-1);
            DateTime end = DateTime.UtcNow.Date.AddDays(2);
            var encKey = new EncryptedSessionTicketDetails(credentialName, ticketSigningKey, challengeResponseKey, start, end, roles);
            byte[] serverSession = encKey.Encrypt(m_currentKey.Key.ToRfcBytes(), m_currentKey.EncryptionKey, m_currentKey.MACKey);


            var serverProof = new AuthServerProof(sproof, serverSession, start, end, roles);
            return serverProof;
        }

        public bool TryDecryptTicket(AuthResume authResume, out EncryptedSessionTicketDetails serverSession, out SessionTicket ticket)
        {
            ticket = (SessionTicket)new CtpDocument(authResume.SessionTicket);
            byte length = ticket.Ticket[0];
            if (length != 16)
            {
                ticket = null;
                serverSession = null;
                return false;
            }

            Guid id = ticket.Ticket.ToRfcGuid(1);
            if (m_keyCache.TryGetValue(id, out ResumeSessionKey key))
            {
                serverSession = EncryptedSessionTicketDetails.TryDecrypt(ticket.Ticket, id.ToRfcBytes(), key.EncryptionKey, key.MACKey);
                if (authResume.VerifySignature(serverSession.TicketSigningKey))
                {
                    return true;
                }
            }
            ticket = null;
            serverSession = null;
            return false;



        }
    }
}
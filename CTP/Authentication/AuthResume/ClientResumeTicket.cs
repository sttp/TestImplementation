namespace CTP.Net
{
    public class ClientResumeTicket
    {
        public byte[] Ticket;
        public byte[] TicketHMAC;
        public byte[] ChallengeResponseKey;

        public ClientResumeTicket(byte[] ticket, byte[] ticketHmac, byte[] challengeResponseKey)
        {
            Ticket = ticket;
            TicketHMAC = ticketHmac;
            ChallengeResponseKey = challengeResponseKey;
        }
    }
}
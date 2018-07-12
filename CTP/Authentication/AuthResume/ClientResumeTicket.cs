namespace CTP.Net
{
    public class ClientResumeTicket
    {
        public byte[] Ticket;
        public byte[] ChallengeResponseKey;

        public ClientResumeTicket(byte[] ticket, byte[] challengeResponseKey)
        {
            Ticket = ticket;
            ChallengeResponseKey = challengeResponseKey;
        }
    }
}
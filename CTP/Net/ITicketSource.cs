using CTP.SRP;

namespace CTP.Net
{
    public interface ITicketSource
    {
        Auth GetTicket();
    }
}
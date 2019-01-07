using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CommandName("SubscriptionRequestFailed")]
    public class CommandSubscriptionRequestFailed
        : CommandObject<CommandSubscriptionRequestFailed>
    {
        [CommandField()]
        public string Reason { get; private set; }
        [CommandField()]
        public string Details { get; private set; }

        public CommandSubscriptionRequestFailed(string reason, string details)
        {
            Reason = reason;
            Details = details;
        }

        private CommandSubscriptionRequestFailed()
        {

        }

        public static explicit operator CommandSubscriptionRequestFailed(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
}
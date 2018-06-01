using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [DocumentName("SubscriptionRequestFailed")]
    public class CommandSubscriptionRequestFailed
        : DocumentObject<CommandSubscriptionRequestFailed>
    {
        [DocumentField()]
        public string Reason { get; private set; }
        [DocumentField()]
        public string Details { get; private set; }

        public CommandSubscriptionRequestFailed(string reason, string details)
        {
            Reason = reason;
            Details = details;
        }

        private CommandSubscriptionRequestFailed()
        {

        }

        public static explicit operator CommandSubscriptionRequestFailed(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}
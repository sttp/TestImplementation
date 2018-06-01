using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpCommand("SubscriptionRequestFailed")]
    public class CommandSubscriptionRequestFailed
        : CtpDocumentObject<CommandSubscriptionRequestFailed>
    {
        [CtpSerializeField()]
        public string Reason { get; private set; }
        [CtpSerializeField()]
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
            return ConvertFromDocument(obj);
        }
    }
}
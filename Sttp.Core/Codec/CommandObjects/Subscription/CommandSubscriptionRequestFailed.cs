using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpSerializable]
    public class CommandSubscriptionRequestFailed
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
      
    }
}
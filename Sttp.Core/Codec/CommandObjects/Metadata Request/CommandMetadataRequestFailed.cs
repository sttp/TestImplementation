using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpSerializable]
    public class CommandMetadataRequestFailed
    {
        [CtpSerializeField()]
        public string Reason { get; private set; }
        [CtpSerializeField()]
        public string Details { get; private set; }

        //Exists to support CtpSerializable
        private CommandMetadataRequestFailed()
        {

        }

        public CommandMetadataRequestFailed(string reason, string details)
        {
            Reason = reason;
            Details = details;
        }
    }
}
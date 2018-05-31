using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpSerializable]
    public class CommandRequestFailed
    {
        [CtpSerializeField()]
        public string OrigionalCommand;
        [CtpSerializeField()]
        public string Reason;
        [CtpSerializeField()]
        public string Details;

        public CommandRequestFailed(string origionalCommand, string reason, string details)
        {
            OrigionalCommand = origionalCommand;
            Reason = reason;
            Details = details;
        }

        //Exists to support CtpSerializable
        private CommandRequestFailed() { }

    }
}
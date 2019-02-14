using CTP;

namespace CTP
{
    [CommandName("RequestFailed")]
    public class CtpRequestFailed
        : CommandObject<CtpRequestFailed>
    {
        [CommandField()]
        public string OriginalCommand;
        [CommandField()]
        public string Reason;
        [CommandField()]
        public string Details;

        public CtpRequestFailed(string originalCommand, string reason, string details)
        {
            OriginalCommand = originalCommand;
            Reason = reason;
            Details = details;
        }

        //Exists to support CtpSerializable
        private CtpRequestFailed()
        { }

        public static explicit operator CtpRequestFailed(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
}
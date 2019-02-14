using CTP;

namespace CTP
{
    [CommandName("Error")]
    public class CtpError
        : CommandObject<CtpError>
    {
        [CommandField()]
        public string Reason;
        [CommandField()]
        public string Details;

        public CtpError(string reason, string details)
        {
            Reason = reason;
            Details = details;
        }

        //Exists to support CtpSerializable
        private CtpError()
        {

        }

        public static explicit operator CtpError(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
}
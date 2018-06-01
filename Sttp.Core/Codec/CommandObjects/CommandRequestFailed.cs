using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [DocumentName("CommandRequestFailed")]
    public class CommandRequestFailed
        : DocumentObject<CommandRequestFailed>
    {
        [DocumentField()]
        public string OrigionalCommand;
        [DocumentField()]
        public string Reason;
        [DocumentField()]
        public string Details;

        public CommandRequestFailed(string origionalCommand, string reason, string details)
        {
            OrigionalCommand = origionalCommand;
            Reason = reason;
            Details = details;
        }

        //Exists to support CtpSerializable
        private CommandRequestFailed()
        { }

        public static explicit operator CommandRequestFailed(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}
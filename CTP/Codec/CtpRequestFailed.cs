using CTP;
using CTP.Serialization;

namespace CTP
{
    [DocumentName("RequestFailed")]
    public class CtpRequestFailed
        : DocumentObject<CtpRequestFailed>
    {
        [DocumentField()]
        public string OriginalCommand;
        [DocumentField()]
        public string Reason;
        [DocumentField()]
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

        public static explicit operator CtpRequestFailed(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}
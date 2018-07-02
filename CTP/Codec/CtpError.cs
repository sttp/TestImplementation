using CTP;
using CTP.Serialization;

namespace CTP
{
    [DocumentName("Error")]
    public class CtpError
        : DocumentObject<CtpError>
    {
        [DocumentField()]
        public string Reason;
        [DocumentField()]
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

        public static explicit operator CtpError(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}
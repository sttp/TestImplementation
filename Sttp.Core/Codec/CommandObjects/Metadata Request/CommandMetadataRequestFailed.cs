using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [DocumentName("MetadataRequestFailed")]
    public class CommandMetadataRequestFailed
        : DocumentObject<CommandMetadataRequestFailed>
    {
        [DocumentField()]
        public string Reason { get; private set; }
        [DocumentField()]
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

        public static explicit operator CommandMetadataRequestFailed(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}
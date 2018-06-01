using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpCommand("MetadataRequestFailed")]
    public class CommandMetadataRequestFailed
        : CtpDocumentObject<CommandMetadataRequestFailed>
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

        public static explicit operator CommandMetadataRequestFailed(CtpDocument obj)
        {
            return ConvertFromDocument(obj);
        }
    }
}
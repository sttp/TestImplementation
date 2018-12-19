using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CommandName("MetadataRequestFailed")]
    public class CommandMetadataRequestFailed
        : CommandObject<CommandMetadataRequestFailed>
    {
        [CommandField()]
        public string Reason { get; private set; }
        [CommandField()]
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

        public static explicit operator CommandMetadataRequestFailed(CtpCommand obj)
        {
            return FromDocument(obj);
        }
    }
}
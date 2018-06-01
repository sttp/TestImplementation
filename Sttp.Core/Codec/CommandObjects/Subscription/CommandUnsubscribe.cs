using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpCommand("Unsubscribe")]
    public class CommandUnsubscribe
        : CtpDocumentObject<CommandUnsubscribe>
    {
        public CommandUnsubscribe()
        {
        }

        public static explicit operator CommandUnsubscribe(CtpDocument obj)
        {
            return ConvertFromDocument(obj);
        }

    }
}

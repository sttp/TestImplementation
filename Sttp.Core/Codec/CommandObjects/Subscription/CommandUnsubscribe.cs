using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [DocumentName("Unsubscribe")]
    public class CommandUnsubscribe
        : DocumentObject<CommandUnsubscribe>
    {
        public CommandUnsubscribe()
        {
        }

        public static explicit operator CommandUnsubscribe(CtpDocument obj)
        {
            return FromDocument(obj);
        }

    }
}

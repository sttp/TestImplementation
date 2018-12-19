using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CommandName("Unsubscribe")]
    public class CommandUnsubscribe
        : CommandObject<CommandUnsubscribe>
    {
        public CommandUnsubscribe()
        {
        }

        public static explicit operator CommandUnsubscribe(CtpCommand obj)
        {
            return FromDocument(obj);
        }

    }
}

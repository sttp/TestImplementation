using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpCommand("SubscribeToAll")]
    public class CommandSubscribeToAll
        : CtpDocumentObject<CommandSubscribeToAll>
    {
        public CommandSubscribeToAll()
        {

        }

        public static explicit operator CommandSubscribeToAll(CtpDocument obj)
        {
            return ConvertFromDocument(obj);
        }
    }
}

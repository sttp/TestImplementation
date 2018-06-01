using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [DocumentName("SubscribeToAll")]
    public class CommandSubscribeToAll
        : DocumentObject<CommandSubscribeToAll>
    {
        public CommandSubscribeToAll()
        {

        }

        public static explicit operator CommandSubscribeToAll(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}

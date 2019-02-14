using CTP;

namespace Sttp.Codec
{
    [CommandName("SubscribeToAll")]
    public class CommandSubscribeToAll
        : CommandObject<CommandSubscribeToAll>
    {
        public CommandSubscribeToAll()
        {

        }

        public static explicit operator CommandSubscribeToAll(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
}

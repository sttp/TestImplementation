using CTP;

namespace Sttp.DataPointEncoding
{
    [CommandName("DataStreamNormal")]
    public class CommandDataStreamNormal
        : CommandObject<CommandDataStreamNormal>
    {
        /// <summary>
        /// Contains the stream of object types.
        /// </summary>
        [CommandField()]
        public byte[] ObjectStream { get; set; }

        public CommandDataStreamNormal(byte[] objectStream)
        {
            ObjectStream = objectStream;
        }

        private CommandDataStreamNormal()
        {

        }

        public static explicit operator CommandDataStreamNormal(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
}
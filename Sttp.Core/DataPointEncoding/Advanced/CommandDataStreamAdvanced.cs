using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTP;

namespace Sttp.Codec
{
    [CommandName("DataStreamAdvanced")]
    public class CommandDataStreamAdvanced
        : CommandObject<CommandDataStreamAdvanced>
    {
        /// <summary>
        /// Contains the stream of object types.
        /// </summary>
        [CommandField()]
        public byte[] ObjectStream { get; set; }

        /// <summary>
        /// Contains the stream of bit data.
        /// </summary>
        [CommandField()]
        public byte[] BitStream { get; set; }

        public CommandDataStreamAdvanced(byte[] objectStream, byte[] bitStream)
        {
            ObjectStream = objectStream;
            BitStream = bitStream;
        }

        private CommandDataStreamAdvanced()
        {

        }

        public static explicit operator CommandDataStreamAdvanced(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
    [CommandName("DataStreamSimple")]
    public class CommandDataStreamSimple
        : CommandObject<CommandDataStreamSimple>
    {
        /// <summary>
        /// Contains the stream of object types.
        /// </summary>
        [CommandField()]
        public byte[] ObjectStream { get; set; }

        /// <summary>
        /// Contains the stream of bit data.
        /// </summary>
        [CommandField()]
        public byte[] BitStream { get; set; }

        public CommandDataStreamSimple(byte[] objectStream, byte[] bitStream)
        {
            ObjectStream = objectStream;
            BitStream = bitStream;
        }

        private CommandDataStreamSimple()
        {

        }

        public static explicit operator CommandDataStreamSimple(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
    [CommandName("DataStreamBasic")]
    public class CommandDataStreamBasic
        : CommandObject<CommandDataStreamBasic>
    {
        /// <summary>
        /// Contains the stream of object types.
        /// </summary>
        [CommandField()]
        public byte[] ObjectStream { get; set; }

        /// <summary>
        /// Contains the stream of bit data.
        /// </summary>
        [CommandField()]
        public byte[] BitStream { get; set; }

        public CommandDataStreamBasic(byte[] objectStream, byte[] bitStream)
        {
            ObjectStream = objectStream;
            BitStream = bitStream;
        }

        private CommandDataStreamBasic()
        {

        }

        public static explicit operator CommandDataStreamBasic(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
    [CommandName("DataStreamRaw")]
    public class CommandDataStreamRaw
        : CommandObject<CommandDataStreamRaw>
    {
        /// <summary>
        /// Contains the stream of object types.
        /// </summary>
        [CommandField()]
        public byte[] ObjectStream { get; set; }

        public CommandDataStreamRaw(byte[] objectStream)
        {
            ObjectStream = objectStream;
        }

        private CommandDataStreamRaw()
        {

        }

        public static explicit operator CommandDataStreamRaw(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
}

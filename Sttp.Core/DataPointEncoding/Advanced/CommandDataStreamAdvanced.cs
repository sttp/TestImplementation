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
        /// The data for this stream
        /// </summary>
        [CommandField()]
        public byte[] Data { get; set; }

        public CommandDataStreamAdvanced(byte[] data)
        {
            Data = data;
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
        /// The data for this stream
        /// </summary>
        [CommandField()]
        public byte[] Data { get; set; }

        public CommandDataStreamSimple(byte[] data)
        {
            Data = data;
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
        /// The data for this stream
        /// </summary>
        [CommandField()]
        public byte[] Data { get; set; }

        public CommandDataStreamBasic(byte[] data)
        {
            Data = data;
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
        /// The data for this stream
        /// </summary>
        [CommandField()]
        public byte[] Data { get; set; }

        public CommandDataStreamRaw(byte[] data)
        {
            Data = data;
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

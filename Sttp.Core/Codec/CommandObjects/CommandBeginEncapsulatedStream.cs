using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTP;

namespace Sttp.Codec
{
    [CommandName("BeginCompressionStream")]
    public class CommandBeginCompressionStream
        : CommandObject<CommandBeginCompressionStream>
    {
        /// <summary>
        /// The channel code for this data stream
        /// </summary>
        [CommandField()]
        public byte ChannelCode { get; set; }

        /// <summary>
        /// Identifies the encoding mechanism that will be used for this data stream.
        /// </summary>
        [CommandField()]
        public string EncodingMechanism { get; set; }

        public CommandBeginCompressionStream(byte channelCode, string encodingMechanism)
        {
            ChannelCode = channelCode;
            EncodingMechanism = encodingMechanism;
        }

        private CommandBeginCompressionStream()
        {

        }

        public static explicit operator CommandBeginCompressionStream(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
}

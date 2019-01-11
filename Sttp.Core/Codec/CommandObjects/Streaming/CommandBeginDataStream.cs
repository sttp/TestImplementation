using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTP;

namespace Sttp.Codec
{
    [CommandName("BeginDataStream")]
    public class CommandBeginDataStream
        : CommandObject<CommandBeginDataStream>
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

        public CommandBeginDataStream(byte channelCode, string encodingMechanism)
        {
            ChannelCode = channelCode;
            EncodingMechanism = encodingMechanism;
        }

        private CommandBeginDataStream()
        {

        }

        public static explicit operator CommandBeginDataStream(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
}

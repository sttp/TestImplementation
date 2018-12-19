using System;
using System.Collections.Generic;
using System.Linq;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CommandName("SubscribeToSome")]
    public class CommandSubscribeToSome
        : CommandObject<CommandSubscribeToSome>
    {
        [CommandField()]
        public string InstanceName { get; private set; }
        [CommandField()]
        public CtpObject[] DataPointIDs { get; private set; }
        [CommandField()]
        public double? SamplePerSecond { get; private set; }

        public CommandSubscribeToSome(string instanceName, CtpObject[] dataPointIDs, double? samplesPerSecond)
        {
            InstanceName = instanceName;
            DataPointIDs = dataPointIDs;
            SamplePerSecond = samplesPerSecond;
        }

        //Exists to support CtpSerializable
        private CommandSubscribeToSome()
        { }

        public static explicit operator CommandSubscribeToSome(CtpCommand obj)
        {
            return FromDocument(obj);
        }
    }
}

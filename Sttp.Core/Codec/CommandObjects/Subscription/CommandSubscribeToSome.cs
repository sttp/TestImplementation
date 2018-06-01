using System;
using System.Collections.Generic;
using System.Linq;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpCommand("SubscribeToSome")]
    public class CommandSubscribeToSome
        : CtpDocumentObject<CommandSubscribeToSome>
    {
        [CtpSerializeField()]
        public string InstanceName { get; private set; }
        [CtpSerializeField()]
        public CtpObject[] DataPointIDs { get; private set; }
        [CtpSerializeField()]
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

        public static explicit operator CommandSubscribeToSome(CtpDocument obj)
        {
            return ConvertFromDocument(obj);
        }
    }
}

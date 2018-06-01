using System;
using System.Collections.Generic;
using System.Linq;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [DocumentName("SubscribeToSome")]
    public class CommandSubscribeToSome
        : DocumentObject<CommandSubscribeToSome>
    {
        [DocumentField()]
        public string InstanceName { get; private set; }
        [DocumentField()]
        public CtpObject[] DataPointIDs { get; private set; }
        [DocumentField()]
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
            return FromDocument(obj);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using CTP;

namespace Sttp.Codec
{
    public class CommandSubscribeToSome : CommandBase
    {
        public readonly string InstanceName;
        public readonly CtpValue[] DataPointIDs;
        public readonly double? SamplePerSecond;

        public CommandSubscribeToSome(string instanceName, CtpValue[] dataPointIDs, double? samplesPerSecond)
            : base("SubscribeToSome")
        {
            InstanceName = instanceName;
            DataPointIDs = dataPointIDs;
            SamplePerSecond = samplesPerSecond;
        }

        public CommandSubscribeToSome(CtpDocumentReader reader)
            : base("SubscribeToSome")
        {
            var element = reader.ReadEntireElement();

            InstanceName = (string)element.GetValue("InstanceName");
            DataPointIDs = element.GetElement("PointList").ForEachValue("ID").ToArray();
            SamplePerSecond = (double?)element.GetValue("SamplePerSecond");

            element.ErrorIfNotHandled();
        }


        public override void Save(CtpDocumentWriter writer)
        {
            writer.WriteValue("InstanceName", InstanceName);
            using (writer.StartElement("PointList"))
            {
                foreach (var id in DataPointIDs)
                {
                    writer.WriteValue("ID", id);
                }
            }
            writer.WriteValue("SamplePerSecond", SamplePerSecond);
        }
    }
}

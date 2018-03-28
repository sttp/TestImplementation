using System;
using System.Collections.Generic;
using System.Linq;

namespace Sttp.Codec
{
    public class CommandSubscribe : CommandBase
    {
        public readonly string InstanceName;
        public readonly SttpValue[] DataPointIDs;
        public readonly double? SamplePerSecond;

        public CommandSubscribe(string instanceName, SttpValue[] dataPointIDs, double? samplesPerSecond)
            : base("Subscribe")
        {
            InstanceName = instanceName;
            DataPointIDs = dataPointIDs;
            SamplePerSecond = samplesPerSecond;
        }

        public CommandSubscribe(SttpMarkupReader reader)
            : base("Subscribe")
        {
            var element = reader.ReadEntireElement();

            InstanceName = (string)element.GetValue("InstanceName");
            DataPointIDs = element.GetElement("PointList").ForEachValue("ID").ToArray();
            SamplePerSecond = (double?)element.GetValue("SamplePerSecond");

            element.ErrorIfNotHandled();
        }


        public override void Save(SttpMarkupWriter writer)
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

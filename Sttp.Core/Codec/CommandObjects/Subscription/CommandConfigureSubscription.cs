using System;
using System.Collections.Generic;
using System.Linq;

namespace Sttp.Codec
{
    public class CommandConfigureSubscription : CommandBase
    {
        public readonly string InstanceName;
        public readonly SttpValue[] DataPointIDs;
        public readonly double? SamplePerSecond;

        public CommandConfigureSubscription(string instanceName, SttpValue[] dataPointIDs, double? samplesPerSecond)
            : base("ConfigureSubscription")
        {
            InstanceName = instanceName;
            DataPointIDs = dataPointIDs;
            SamplePerSecond = samplesPerSecond;
        }

        public CommandConfigureSubscription(SttpMarkupReader reader)
            : base("ConfigureSubscription")
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

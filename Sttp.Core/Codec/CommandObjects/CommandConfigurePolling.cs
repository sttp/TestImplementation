using System;
using System.Collections.Generic;
using System.Linq;

namespace Sttp.Codec
{
    public class CommandConfigurePolling : CommandBase
    {
        public readonly string InstanceName;
        public readonly SttpValue[] DataPointIDs;

        public CommandConfigurePolling(string instanceName, SttpValue[] dataPointIDs)
            : base("ConfigurePolling")
        {
            InstanceName = instanceName;
            DataPointIDs = dataPointIDs;
        }

        public CommandConfigurePolling(SttpMarkupReader reader)
            : base("ConfigurePolling")
        {
            var element = reader.ReadEntireElement();

            InstanceName = (string)element.GetValue("InstanceName");
            DataPointIDs = element.GetElement("PointList").ForEachValue("ID").ToArray();

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
        }
    }
}

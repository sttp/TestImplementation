using System;
using System.Linq;

namespace Sttp.Codec
{
    public class CommandDataPointRequest : CommandBase
    {
        public readonly Guid? RequestID;
        public readonly SttpTime StartTime;
        public readonly SttpTime StopTime;
        public readonly SttpValue[] DataPointIDs;
        public readonly double? SamplePerSecond;

        public CommandDataPointRequest(Guid? requestID, SttpTime startTime, SttpTime stopTime, SttpValue[] dataPointIDs, double? samplesPerSecond)
            : base("DataPointRequest")
        {
            RequestID = requestID;
            StartTime = startTime;
            StopTime = stopTime;
            DataPointIDs = dataPointIDs;
            SamplePerSecond = samplesPerSecond;
        }

        public CommandDataPointRequest(SttpMarkupReader reader)
            : base("DataPointReply")
        {
            var element = reader.ReadEntireElement();

            RequestID = (Guid?)element.GetValue("RequestID");
            StartTime = (SttpTime)element.GetValue("StartTime");
            StopTime = (SttpTime)element.GetValue("StopTime");
            DataPointIDs = element.GetElement("PointList").ForEachValue("ID").ToArray();
            SamplePerSecond = (double?)element.GetValue("SamplePerSecond");

            element.ErrorIfNotHandled();
        }


        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("RequestID", RequestID);
            writer.WriteValue("StartTime", StartTime);
            writer.WriteValue("StopTime", StopTime);
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

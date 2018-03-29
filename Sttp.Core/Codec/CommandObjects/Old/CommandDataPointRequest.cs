//using System;
//using System.Linq;

//namespace Sttp.Codec
//{
//    public class CommandDataPointRequest : CommandBase
//    {
//        public readonly string InstanceName;
//        public readonly SttpTime StartTime;
//        public readonly SttpTime StopTime;
//        public readonly SttpValue[] DataPointIDs;
//        public readonly double? SamplePerSecond;

//        public CommandDataPointRequest(string instanceName, SttpTime startTime, SttpTime stopTime, SttpValue[] dataPointIDs, double? samplesPerSecond)
//            : base("DataPointRequest")
//        {
//            InstanceName = instanceName;
//            StartTime = startTime;
//            StopTime = stopTime;
//            DataPointIDs = dataPointIDs;
//            SamplePerSecond = samplesPerSecond;
//        }

//        public CommandDataPointRequest(SttpMarkupReader reader)
//            : base("DataPointReply")
//        {
//            var element = reader.ReadEntireElement();

//            InstanceName = (string)element.GetValue("InstanceName");
//            StartTime = (SttpTime)element.GetValue("StartTime");
//            StopTime = (SttpTime)element.GetValue("StopTime");
//            DataPointIDs = element.GetElement("PointList").ForEachValue("ID").ToArray();
//            SamplePerSecond = (double?)element.GetValue("SamplePerSecond");

//            element.ErrorIfNotHandled();
//        }


//        public override void Save(SttpMarkupWriter writer)
//        {
//            writer.WriteValue("InstanceName", InstanceName);
//            writer.WriteValue("StartTime", StartTime);
//            writer.WriteValue("StopTime", StopTime);
//            using (writer.StartElement("PointList"))
//            {
//                foreach (var id in DataPointIDs)
//                {
//                    writer.WriteValue("ID", id);
//                }
//            }
//            writer.WriteValue("SamplePerSecond", SamplePerSecond);
//        }
//    }
//}

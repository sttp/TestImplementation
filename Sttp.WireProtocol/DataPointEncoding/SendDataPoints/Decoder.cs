//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Sttp.WireProtocol.SendDataPoints
//{
//    public class Decoder
//    {
//        public List<SttpDataPoint> Points;

//        public CommandCode CommandCode => CommandCode.SendDataPoints;

//        public List<SttpDataPoint> DataPoints;

//        public void Fill(PacketReader reader)
//        {
//            int lastPointID = 0;
//            long lastTimestamp = 0;
//            byte lastTimeQuality = 0;
//            byte lastValueQuality = 0;

//            int count = reader.ReadInt7Bit();
//            DataPoints = new List<SttpDataPoint>(count);
//            while (count > 0)
//            {
//                count--;
//            }
//        }
//    }
//}

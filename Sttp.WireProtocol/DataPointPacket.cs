using System.Collections.Generic;
using System.IO;

namespace Sttp.WireProtocol
{
    public class DataPointPacket : Command
    {
        public DataPointPacket()
        {
            CommandCode = CommandCode.DataPointPacket;
        }

        public static List<DataPointPacket> GetDataPointPackets(DataPoint[] dataPoints, ushort targetPacketSize)
        {
            List<DataPointPacket> dataPointPackets = new List<DataPointPacket>();
            DataPointPacket packet = new DataPointPacket();
            MemoryStream stream = new MemoryStream();
            byte[] dataPointPayload;

            // Fragment data point packets into 16kb chunks
            for (int i = 0; i < dataPoints.Length; i++)
            {
                dataPointPayload = dataPoints[i].Encode();

                if (stream.Length + dataPointPayload.Length > targetPacketSize)
                {
                    packet.Payload = stream.ToArray();
                    stream.Dispose();
                    dataPointPackets.Add(packet);
                    packet = new DataPointPacket();
                    stream = new MemoryStream();
                }

                stream.Write(dataPointPayload, 0, dataPointPayload.Length);
            }

            packet.Payload = stream.ToArray();
            stream.Dispose();
            dataPointPackets.Add(packet);

            return dataPointPackets;
        }
    }
}

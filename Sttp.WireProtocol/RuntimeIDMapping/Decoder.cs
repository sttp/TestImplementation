using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.RuntimeIDMapping
{
    public class Decoder
    {
        public List<SttpDataPointID> Points;

        public CommandCode CommandCode => CommandCode.RuntimeIDMapping;

        public void Fill(PacketReader reader)
        {
            int count = reader.ReadInt32();
            Points = new List<SttpDataPointID>(count);
            while (count > 0)
            {
                count--;
                int id = reader.ReadInt32();
                var point = new SttpDataPointID();
                point.RuntimeID = id;

                switch (reader.Read<SttpDataPointIDTypeCode>())
                {
                    case SttpDataPointIDTypeCode.Null:
                        throw new InvalidOperationException("A registered pointID cannot be null");
                    case SttpDataPointIDTypeCode.Guid:
                        point.AsGuid = reader.ReadGuid();
                        break;
                    case SttpDataPointIDTypeCode.String:
                        point.AsString = reader.ReadString();
                        break;
                    case SttpDataPointIDTypeCode.NamedSet:
                        point.AsNamedSet = reader.Read<SttpNamedSet>();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                Points.Add(point);
            }
        }
    }
}

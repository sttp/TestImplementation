using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.RegisterDataPointRuntimeIdentifier
{
    public class Decoder
    {
        public List<SttpPointID> Points;

        public CommandCode CommandCode => CommandCode.RegisterDataPointRuntimeIdentifier;

        public void Fill(PacketReader reader)
        {
            int count = reader.ReadInt32();
            Points = new List<SttpPointID>(count);
            while (count > 0)
            {
                count--;
                int id = reader.ReadInt32();
                var point = new SttpPointID();
                point.RuntimeID = id;

                switch (reader.Read<SttpPointIDTypeCode>())
                {
                    case SttpPointIDTypeCode.Null:
                        throw new InvalidOperationException("A registered pointID cannot be null");
                    case SttpPointIDTypeCode.Guid:
                        point.AsGuid = reader.ReadGuid();
                        break;
                    case SttpPointIDTypeCode.String:
                        point.AsString = reader.ReadString();
                        break;
                    case SttpPointIDTypeCode.NamedSet:
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

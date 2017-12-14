using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    public class CommandMapRuntimeIDs
    {
        public CommandCode CommandCode => CommandCode.MapRuntimeIDs;
        public readonly List<SttpDataPointID> Points;

        public CommandMapRuntimeIDs(PayloadReader reader)
        {
            int count = reader.ReadInt32();
            Points = new List<SttpDataPointID>(count);
            while (count > 0)
            {
                count--;
                int id = reader.ReadInt32();
                var point = new SttpDataPointID();
                point.RuntimeID = id;

                switch ((SttpDataPointIDTypeCode)reader.ReadByte())
                {
                    case SttpDataPointIDTypeCode.Null:
                        throw new InvalidOperationException("A registered pointID cannot be null");
                    case SttpDataPointIDTypeCode.Guid:
                        point.AsGuid = reader.ReadGuid();
                        break;
                    case SttpDataPointIDTypeCode.String:
                        point.AsString = reader.ReadString();
                        break;
                    case SttpDataPointIDTypeCode.SttpMarkup:
                        point.AsSttpMarkup = reader.ReadSttpMarkup();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                Points.Add(point);
            }
        }
    }
}

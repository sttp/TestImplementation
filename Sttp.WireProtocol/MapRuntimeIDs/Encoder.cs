using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.MapRuntimeIDs
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.MapRuntimeIDs;

        public Encoder(CommandEncoder commandEncoder, SessionDetails sessionDetails)
            : base(commandEncoder, sessionDetails)
        {

        }

        public void RuntimeIDMapping(List<SttpDataPointID> points)
        {
            BeginCommand();
            Stream.Write(points.Count);
            foreach (var point in points)
            {
                Stream.Write(point.RuntimeID);
                Stream.Write(point.ValueTypeCode);
                switch (point.ValueTypeCode)
                {
                    case SttpDataPointIDTypeCode.Null:
                        throw new InvalidOperationException("A registered pointID cannot be null");
                    case SttpDataPointIDTypeCode.Guid:
                        Stream.Write(point.AsGuid);
                        break;
                    case SttpDataPointIDTypeCode.String:
                        Stream.Write(point.AsString);
                        break;
                    case SttpDataPointIDTypeCode.NamedSet:
                        Stream.Write(point.AsNamedSet);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }
            EndCommand();
        }


    }
}

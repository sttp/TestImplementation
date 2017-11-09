using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.RegisterDataPointRuntimeIdentifier
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.RegisterDataPointRuntimeIdentifier;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void RegisterDataPointRuntimeIdentifier(List<SttpPointID> points)
        {
            BeginCommand();
            Stream.Write(points.Count);
            foreach (var point in points)
            {
                Stream.Write(point.RuntimeID);
                Stream.Write(point.ValueTypeCode);
                switch (point.ValueTypeCode)
                {
                    case SttpPointIDTypeCode.Null:
                        throw new InvalidOperationException("A registered pointID cannot be null");
                    case SttpPointIDTypeCode.Guid:
                        Stream.Write(point.AsGuid);
                        break;
                    case SttpPointIDTypeCode.String:
                        Stream.Write(point.AsString);
                        break;
                    case SttpPointIDTypeCode.NamedSet:
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

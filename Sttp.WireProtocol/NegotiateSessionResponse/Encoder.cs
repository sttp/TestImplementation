using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.NegotiateSessionResponse
{
    public class Encoder : BaseEncoder
    {
        public override CommandCode Code => CommandCode.NegotiateSession;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void ReverseConnectionSuccess()
        {
            Stream.Write(SubCommand.ReverseConnectionSuccess);
        }

        public void DesiredOperation(SttpNamedSet options)
        {
            Stream.Write(SubCommand.DesiredOperation);
            Stream.Write(options);
        }

        public void ChangeInstanceSuccess()
        {
            Stream.Write(SubCommand.ChangeInstanceSuccess);
        }

        public void InstanceList(List<Tuple<string, string>> instances)
        {
            Stream.Write(SubCommand.InstanceList);
            Stream.Write(instances);
        }

        public void ChangeUdpCiperResponse(byte[] nonce)
        {
            Stream.Write(SubCommand.ChangeUdpCiperResponse);
            Stream.Write(nonce);
        }

    }
}

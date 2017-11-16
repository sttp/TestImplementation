using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.NegotiateSession
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.NegotiateSession;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void InitiateReverseConnection()
        {
            Stream.Write(SubCommand.InitiateReverseConnection);
        }

        public void SupportedFunctionality(SttpNamedSet options)
        {
            Stream.Write(SubCommand.SupportedFunctionality);
            Stream.Write(options);
        }

        public void ChangeInstance(string instanceName)
        {
            Stream.Write(SubCommand.ChangeInstance);
            Stream.Write(instanceName);
        }

        public void GetAllInstances()
        {
            Stream.Write(SubCommand.GetAllInstances);
        }

        public void ChangeUdpCiper(byte[] nonce)
        {
            Stream.Write(SubCommand.ChangeUdpCiper);
            Stream.Write(nonce);
        }

    }
}

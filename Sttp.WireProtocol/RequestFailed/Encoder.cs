﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.RequestFailed
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.RequestFailed;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void GetMetadataSchema(CommandCode failedCommand,bool terminateConnection, string reason, string details)
        {
            BeginCommand();
            Stream.Write(failedCommand);
            Stream.Write(terminateConnection);
            Stream.Write(reason);
            Stream.Write(details);
            EndCommand();
        }


    }
}
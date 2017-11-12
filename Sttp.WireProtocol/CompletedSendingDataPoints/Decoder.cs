using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.CompletedSendingDataPoints
{
    public class Decoder
    {
        public CommandCode CommandCode => CommandCode.CompletedSendingDataPoints;

        public void Fill(PacketReader reader)
        {
          
        }
    }

    
}

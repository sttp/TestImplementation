using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.SendComplete
{
    public class Decoder
    {
        public CommandCode CommandCode => CommandCode.SendComplete;

        public void Fill(PacketReader reader)
        {
          
        }
    }

    
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Sttp.Codec
//{
//    public class CommandNegotiateSession
//    {
//        public readonly SttpMarkup Config;

//        public CommandCode CommandCode => CommandCode.NegotiateSession;

//        public CommandNegotiateSession(PayloadReader reader)
//        {
//            Config = reader.ReadSttpMarkup();
//        }
//    }
//}

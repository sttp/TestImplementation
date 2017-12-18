//using System;
//using System.Collections.Generic;

//namespace Sttp.Codec
//{
//    /// <summary>
//    /// Responsible for encoding each command into bytes
//    /// </summary>
//    public class CommandSubscription 
//    {
//        public CommandCode CommandCode => CommandCode.Subscription;

//        public readonly SubscriptionAppendMode Mode;
//        public readonly SttpMarkup Options;
//        public readonly List<SttpDataPointID> DataPoints;

//        public CommandSubscription(PayloadReader reader)
//        {
//            Mode = (SubscriptionAppendMode) reader.ReadByte();
//            Options = reader.ReadSttpMarkup();
//            DataPoints = reader.ReadListSttpDataPointID();
//        }
//    }
//}

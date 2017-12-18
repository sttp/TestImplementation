//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Sttp.Codec
//{
//    public class CommandDataPointReply
//    {
//        public CommandCode CommandCode => CommandCode.DataPointReply;

//        public readonly Guid RequestID;
//        public readonly bool IsEndOfResponse;
//        public readonly byte EncodingMethod;
//        public readonly byte[] Data;

//        public CommandDataPointReply(PayloadReader reader)
//        {
//            RequestID = reader.ReadGuid();
//            IsEndOfResponse = reader.ReadBoolean();
//            EncodingMethod = reader.ReadByte();
//            Data = reader.ReadBytes();
//        }
//    }
//}

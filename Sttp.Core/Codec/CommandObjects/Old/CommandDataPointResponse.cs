//using System;
//using System.Linq;

//namespace Sttp.Codec
//{
//    public class CommandDataPointResponse : CommandBase
//    {
//        public readonly byte RawCommandCode;
//        public readonly Guid EncodingMethod;

//        public CommandDataPointResponse(byte rawCommandCode, Guid encodingMethod)
//            : base("DataPointResponse")
//        {
//            RawCommandCode = rawCommandCode;
//            EncodingMethod = encodingMethod;
//        }

//        public CommandDataPointResponse(SttpMarkupReader reader)
//            : base("DataPointResponse")
//        {
//            var element = reader.ReadEntireElement();

//            RawCommandCode = (byte)element.GetValue("RawCommandCode");
//            EncodingMethod = (Guid)element.GetValue("EncodingMethod");

//            element.ErrorIfNotHandled();
//        }


//        public override void Save(SttpMarkupWriter writer)
//        {
//            writer.WriteValue("RawCommandCode", RawCommandCode);
//            writer.WriteValue("EncodingMethod", EncodingMethod);
//        }
//    }
//}

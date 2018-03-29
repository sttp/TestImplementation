//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Sttp.Codec
//{
//    public class CommandRequestSucceeded : CommandBase
//    {
//        public readonly string CommandSucceeded;
//        public readonly string Reason;
//        public readonly string Details;

//        public CommandRequestSucceeded(string commandSucceeded, string reason, string details)
//            : base("RequestSucceeded")
//        {
//            CommandSucceeded = commandSucceeded;
//            Reason = reason;
//            Details = details;
//        }

//        public CommandRequestSucceeded(SttpMarkupReader reader)
//            : base("RequestSucceeded")
//        {
//            var element = reader.ReadEntireElement();

//            CommandSucceeded = (string)element.GetValue("CommandSucceeded");
//            Reason = (string)element.GetValue("Reason");
//            Details = (string)element.GetValue("Details");


//            element.ErrorIfNotHandled();
//        }

//        public override void Save(SttpMarkupWriter writer)
//        {
//            writer.WriteValue("CommandSucceeded", CommandSucceeded);
//            writer.WriteValue("Reason", Reason);
//            writer.WriteValue("Details", Details);
//        }

//    }
//}

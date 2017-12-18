using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    public class CommandRequestFailed : CommandBase
    {
        public readonly string FailedCommand;
        public readonly bool TerminateConnection;
        public readonly string Reason;
        public readonly string Details;

        public CommandRequestFailed(string failedCommand, bool terminateConnection, string reason, string details)
            : base("RequestFailed")
        {
            FailedCommand = failedCommand;
            TerminateConnection = terminateConnection;
            Reason = reason;
            Details = details;
        }

        public CommandRequestFailed(SttpMarkupReader reader)
              : base("RequestFailed")
        {
            var element = reader.ReadEntireElement();
            if (element.ElementName != CommandName)
                throw new Exception("Invalid command");

            FailedCommand = (string)element.GetValue("FailedCommand");
            TerminateConnection = (bool)element.GetValue("TerminateConnection");
            Reason = (string)element.GetValue("Reason");
            Details = (string)element.GetValue("Details");

            
            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
            using (writer.StartElement(CommandName))
            {
                writer.WriteValue("FailedCommand", FailedCommand);
                writer.WriteValue("TerminateConnection", TerminateConnection);
                writer.WriteValue("Reason", Reason);
                writer.WriteValue("Details", Details);
            }
        }
    }
}

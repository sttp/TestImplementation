using CTP;

namespace Sttp.Codec
{
    public class CommandRequestFailed : DocumentCommandBase
    {
        public readonly string OrigionalCommand;
        public readonly string Reason;
        public readonly string Details;

        public CommandRequestFailed(string origionalCommand, string reason, string details)
            : base("RequestFailed")
        {
            OrigionalCommand = origionalCommand;
            Reason = reason;
            Details = details;
        }

        public CommandRequestFailed(CtpDocumentReader reader)
            : base("RequestFailed")
        {
            var element = reader.ReadEntireElement();

            OrigionalCommand = (string)element.GetValue("OrigionalCommand");
            Reason = (string)element.GetValue("Reason");
            Details = (string)element.GetValue("Details");


            element.ErrorIfNotHandled();
        }

        public override void Save(CtpDocumentWriter writer)
        {
            writer.WriteValue("OrigionalCommand", OrigionalCommand);
            writer.WriteValue("Reason", Reason);
            writer.WriteValue("Details", Details);
        }
    }
}
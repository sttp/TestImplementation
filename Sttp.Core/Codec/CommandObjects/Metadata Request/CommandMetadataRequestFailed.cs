using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpSerializable]
    public class CommandMetadataRequestFailed : DocumentCommandBase
    {
        [CtpSerializeField()]
        public string Reason { get; private set; }
        [CtpSerializeField()]
        public string Details { get; private set; }

        /// <summary>
        /// Called by serialization
        /// </summary>
        private CommandMetadataRequestFailed()
            : base("MetadataRequestFailed")
        {

        }

        public CommandMetadataRequestFailed(string reason, string details)
            : base("MetadataRequestFailed")
        {
            Reason = reason;
            Details = details;
        }

        public CommandMetadataRequestFailed(CtpDocumentReader reader)
            : base("MetadataRequestFailed")
        {
            var element = reader.ReadEntireElement();

            Reason = (string)element.GetValue("Reason");
            Details = (string)element.GetValue("Details");

            element.ErrorIfNotHandled();
        }

        public override void Save(CtpDocumentWriter writer)
        {
            writer.WriteValue("Reason", Reason);
            writer.WriteValue("Details", Details);
        }
    }
}
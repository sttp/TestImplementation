using CTP;

namespace Sttp.Codec
{
    public class CommandUnsubscribeResponse : DocumentCommandBase
    {
        public readonly int BinaryChannelCode;

        public CommandUnsubscribeResponse(int binaryChannelCode)
            : base("UnsubscribeResponse")
        {
            BinaryChannelCode = binaryChannelCode;
        }

        public CommandUnsubscribeResponse(CtpDocumentReader reader)
            : base("UnsubscribeResponse")
        {
            var element = reader.ReadEntireElement();

            BinaryChannelCode = (int)element.GetValue("BinaryChannelCode");

            element.ErrorIfNotHandled();
        }

        public override void Save(CtpDocumentWriter writer)
        {
            writer.WriteValue("BinaryChannelCode", BinaryChannelCode);

        }
    }
}

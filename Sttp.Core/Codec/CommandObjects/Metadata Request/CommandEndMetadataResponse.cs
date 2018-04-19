using CTP;

namespace Sttp.Codec
{
    public class CommandEndMetadataResponse : DocumentCommandBase
    {
        public readonly int BinaryChannelCode;
        public readonly int RowCount;

        public CommandEndMetadataResponse(int binaryChannelCode, int rowCount)
            : base("EndMetadataResponse")
        {
            BinaryChannelCode = binaryChannelCode;
            RowCount = rowCount;
        }

        public CommandEndMetadataResponse(CtpDocumentReader reader)
            : base("EndMetadataResponse")
        {
            var element = reader.ReadEntireElement();
            BinaryChannelCode = (int)element.GetValue("BinaryChannelCode");
            RowCount = (int)element.GetValue("RowCount");
            element.ErrorIfNotHandled();
        }

        public override void Save(CtpDocumentWriter writer)
        {
            writer.WriteValue("BinaryChannelCode", BinaryChannelCode);
            writer.WriteValue("RowCount", RowCount);
        }
    }
}

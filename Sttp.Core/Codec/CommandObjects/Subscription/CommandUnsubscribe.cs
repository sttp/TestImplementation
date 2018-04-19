using CTP;

namespace Sttp.Codec
{
    public class CommandUnsubscribe : DocumentCommandBase
    {
        public CommandUnsubscribe()
            : base("Unsubscribe")
        {
        }

        public CommandUnsubscribe(CtpDocumentReader reader)
            : base("Unsubscribe")
        {
            var element = reader.ReadEntireElement();

            element.ErrorIfNotHandled();
        }

        public override void Save(CtpDocumentWriter writer)
        {
        }
    }
}

namespace CTP
{
    [CommandName("RequestSuccess")]
    public class CtpRequestSuccess
        : CommandObject<CtpRequestSuccess>
    {
        [CommandField()]
        public string OriginalCommand;

        public CtpRequestSuccess(string originalCommand)
        {
            OriginalCommand = originalCommand;
        }

        //Exists to support CtpSerializable
        private CtpRequestSuccess()
        { }

        public static explicit operator CtpRequestSuccess(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
}
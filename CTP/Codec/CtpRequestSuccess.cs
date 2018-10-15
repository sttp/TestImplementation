namespace CTP
{
    [DocumentName("RequestSuccess")]
    public class CtpRequestSuccess
        : DocumentObject<CtpRequestSuccess>
    {
        [DocumentField()]
        public string OriginalCommand;

        public CtpRequestSuccess(string originalCommand)
        {
            OriginalCommand = originalCommand;
        }

        //Exists to support CtpSerializable
        private CtpRequestSuccess()
        { }

        public static explicit operator CtpRequestSuccess(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}
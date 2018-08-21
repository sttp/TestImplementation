namespace CTP.Net
{
    [DocumentName("IpAndMask")]
    public class IpAndMask
        : DocumentObject<IpAndMask>
    {
        [DocumentField()]
        public string IpAddress { get; set; }

        [DocumentField()]
        public int MaskBits { get; set; }

        public IpAndMask()
        {

        }

        public static explicit operator IpAndMask(CtpDocument obj)
        {
            return FromDocument(obj);
        }

        public string DisplayMember
        {
            get
            {
                return $"{IpAddress}/{MaskBits}";
            }
        }
    }
}
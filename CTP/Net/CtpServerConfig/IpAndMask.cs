namespace CTP.Net
{
    [CommandName("IpAndMask")]
    public class IpAndMask
        : CommandObject<IpAndMask>
    {
        [CommandField()]
        public string IpAddress { get; set; }

        [CommandField()]
        public int MaskBits { get; set; }

        public IpAndMask()
        {

        }

        public static explicit operator IpAndMask(CtpCommand obj)
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
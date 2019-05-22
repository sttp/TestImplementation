using System.Net;

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
            return FromCommand(obj);
        }

        public string DisplayMember
        {
            get
            {
                return $"{IpAddress}/{MaskBits}";
            }
        }

        private IpMatchDefinition m_ipMatch;

        public bool IsMatch(byte[] ipBytes)
        {
            if (m_ipMatch == null)
            {
                m_ipMatch = new IpMatchDefinition(IPAddress.Parse(IpAddress), MaskBits);
            }
            return m_ipMatch.IsMatch(ipBytes);
        }
    }
}
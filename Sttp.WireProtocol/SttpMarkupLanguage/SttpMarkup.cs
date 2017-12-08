using Sttp.Codec;

namespace Sttp
{
    public enum SttpMarkupCompatiblity : byte
    {
        /// <summary>
        /// Indicates that this option must be interpreted. If the option is not recognized, the request must be denied. 
        /// This also requires that if the option is properly interpreted, it must be enforced, otherwise the request must be denied.
        /// </summary>
        KnownAndEnforced,
        /// <summary>
        /// Indicates that this option must be interpreted. If the option is not recognized, the request must be denied.
        /// However, the option can be ignored if desired, but only by an entity that recognizes the option.
        /// </summary>
        Known,
        /// <summary>
        /// Indicates that if the server does not recognized this item. It can be safely ignored.
        /// </summary>
        Unknown,
    }

    public class SttpMarkup
    {
        private byte[] m_data;
        public SttpMarkup(ByteReader rd)
        {
            m_data = rd.ReadBytes();
        }
        public SttpMarkup(byte[] data)
        {
            m_data = data;
        }
        public void Write(ByteWriter wr)
        {
            wr.Write(m_data);
        }

        public SttpMarkupReader MakeReader()
        {
            return new SttpMarkupReader(m_data);
        }
    }

    public enum SttpMarkupNodeType
    {
        Element,
        Value,
        EndElement
    }
}

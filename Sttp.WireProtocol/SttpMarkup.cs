using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;
using Sttp.IO;

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
        public SttpMarkup(PayloadReader rd)
        {
            m_data = rd.ReadBytes();
        }
        public SttpMarkup(byte[] data)
        {
            m_data = data;
        }
        public void Write(PayloadWriter wr)
        {
            wr.Write(m_data);
        }

        public SttpMarkupReader MakeReader()
        {
            return new SttpMarkupReader(m_data);
        }
    }

    public enum SttpMarkupType
    {
        Element,
        Value,
        EndElement
    }

    public class SttpMarkupReader
    {
        private MemoryStream m_stream;
        private List<string> m_elements = new List<string>();
        private Stack<string> m_elementStack = new Stack<string>();
        private SttpValue m_value = new SttpValue();

        public string Element;
        public SttpMarkupCompatiblity ElementCompatibility;
        public string ValueName;
        public SttpValue Value;
        public SttpMarkupCompatiblity ValueCompatibility;

        internal SttpMarkupReader(byte[] data)
        {
            m_stream = new MemoryStream(data);
        }

        public bool Next()
        {
            return true;
        }

        public void Reset()
        {
            m_stream.Position = 0;
        }

    }

    public class SttpMarkupWriter
    {
        public class ElementEndElementHelper : IDisposable
        {
            private SttpMarkupWriter m_parent;
            public ElementEndElementHelper(SttpMarkupWriter parent)
            {
                m_parent = parent;
            }
            public void Dispose()
            {
                m_parent.EndElement();
            }
        }
        private Dictionary<string, int> m_elementCache = new Dictionary<string, int>();
        private Stack<string> m_elementStack = new Stack<string>();
        private MemoryStream m_stream = new MemoryStream();
        private ElementEndElementHelper m_endElementHelper;
        private SttpValue m_tmpValue = new SttpValue();

        public SttpMarkupWriter()
        {
            m_endElementHelper = new ElementEndElementHelper(this);
        }

        //      20 bits for Name's dictionary cache index, OR Length of the element string name.
        //      1 bit for String/Int representation of the Name (If Element is Int).
        //      5 bits for the type of the Value (If SttpValue Exists)
        //      1 bit if SttpValue Exists
        //      2 bits for Compatibility (If Element).
        //      1 bit for Element/EndElement (1: End Element, 0 otherwise)

        public string CurrentElement
        {
            get
            {
                if (m_elementStack.Count == 0)
                    return string.Empty;
                return m_elementStack.Peek();
            }
        }

        public ElementEndElementHelper StartElement(string name, SttpMarkupCompatiblity compatiblity = SttpMarkupCompatiblity.KnownAndEnforced)
        {
            if (name == null || name.Length == 0)
                throw new ArgumentNullException(nameof(name));
            if (compatiblity >= SttpMarkupCompatiblity.KnownAndEnforced && compatiblity <= SttpMarkupCompatiblity.Unknown)
                throw new InvalidEnumArgumentException(nameof(compatiblity), (int)compatiblity, typeof(SttpMarkupCompatiblity));

            m_elementStack.Push(name);
            if (m_elementCache.TryGetValue(name, out int index))
            {
                Encode(index, compatiblity);
            }
            else
            {
                m_elementCache[name] = m_elementCache.Count;
                Encode(name, compatiblity);
            }
            return m_endElementHelper;
        }

        public void WriteValue(string name, SttpValue value, SttpMarkupCompatiblity compatiblity = SttpMarkupCompatiblity.KnownAndEnforced)
        {
            if (name == null || name.Length == 0)
                throw new ArgumentNullException(nameof(name));
            if (compatiblity >= SttpMarkupCompatiblity.KnownAndEnforced && compatiblity <= SttpMarkupCompatiblity.Unknown)
                throw new InvalidEnumArgumentException(nameof(compatiblity), (int)compatiblity, typeof(SttpMarkupCompatiblity));
            if ((object)value == null)
                throw new ArgumentNullException(nameof(value));

            if (m_elementCache.TryGetValue(name, out int index))
            {
                Encode(index, compatiblity, value);
            }
            else
            {
                m_elementCache[name] = m_elementCache.Count;
                Encode(name, compatiblity, value);
            }

        }

        //1 bit for String/Int representation of the Name(16: string, 0: int)
        //1 bit if SttpValue Exists(0: Missing, 8: Exists)
        //1 bit for Element/EndElement(4: End Element, 0 Element)
        //2 bits for Compatibility

        private void Encode(int nameIndex, SttpMarkupCompatiblity compatiblity)
        {
            m_stream.Write((byte)compatiblity);
            m_stream.Write7BitInt(nameIndex);
        }

        private void Encode(string name, SttpMarkupCompatiblity compatiblity)
        {
            m_stream.Write((byte)((byte)compatiblity | 16));
            m_stream.Write(name);
        }

        private void Encode(int nameIndex, SttpMarkupCompatiblity compatiblity, SttpValue value)
        {
            m_stream.Write((byte)((byte)compatiblity | 8));
            m_stream.Write7BitInt(nameIndex);
            value.Save(m_stream);
        }

        private void Encode(string name, SttpMarkupCompatiblity compatiblity, SttpValue value)
        {
            m_stream.Write((byte)((byte)compatiblity | 16 | 8));
            m_stream.Write(name);
            value.Save(m_stream);
        }

        public void EndElement()
        {
            if (m_elementStack.Count == 0)
            {
                m_elementStack.Pop();
            }
            m_stream.Write((byte)4);
        }

        public void WriteValue(string name, object value, SttpMarkupCompatiblity compatiblity = SttpMarkupCompatiblity.KnownAndEnforced)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue, compatiblity);
        }

        public SttpMarkup ToSttpMarkup()
        {
            return new SttpMarkup(m_stream.ToArray());
        }
    }
}

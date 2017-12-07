using System;
using System.Collections.Generic;
using System.IO;
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

    public class SttpMarkupReader
    {
        private MemoryStream m_stream;
        private List<string> m_elements = new List<string>();
        private Stack<string> m_elementStack = new Stack<string>();
        private Stack<SttpMarkupCompatiblity> m_elementStackCompatibility = new Stack<SttpMarkupCompatiblity>();
        private SttpValueMutable m_value = new SttpValueMutable();
        private int m_prevNameAsInt = 0;

        internal SttpMarkupReader(byte[] data)
        {
            m_stream = new MemoryStream(data);
        }


        public string Element { get; private set; }
        public SttpMarkupCompatiblity ElementCompatibility { get; private set; }
        public string ValueName { get; private set; }
        public SttpValueMutable Value { get; private set; }
        public SttpMarkupCompatiblity ValueCompatibility { get; private set; }

        public SttpMarkupNodeType CurrentNodeType { get; private set; }

        public bool Next()
        {
            if (m_stream.Position == m_stream.Length)
                return false;

            if (CurrentNodeType == SttpMarkupNodeType.EndElement)
            {
                Element = CurrentElement;
                ElementCompatibility = CurrentElementCompatiblity;
            }

            byte code = m_stream.ReadNextByte();
            bool isNameAsString = ((code >> 4) & 1) == 1;
            int nameDelta = (code >> 5);

            CurrentNodeType = (SttpMarkupNodeType)(code & 3);
            switch (CurrentNodeType)
            {
                case SttpMarkupNodeType.Element:
                    Value.SetNull();
                    ValueCompatibility = SttpMarkupCompatiblity.Unknown;
                    ElementCompatibility = (SttpMarkupCompatiblity)((code >> 2) & 3);

                    if (isNameAsString)
                    {
                        Element = m_stream.ReadString();
                        m_prevNameAsInt = m_elements.Count;
                        m_elements.Add(Element);
                    }
                    else
                    {
                        if (nameDelta == 7)
                            m_prevNameAsInt = m_stream.Read7BitInt();
                        else
                            m_prevNameAsInt += nameDelta;
                        Element = m_elements[m_prevNameAsInt];
                    }

                    break;
                case SttpMarkupNodeType.Value:
                    ValueCompatibility = (SttpMarkupCompatiblity)((code >> 2) & 3);
                    if (isNameAsString)
                    {
                        ValueName = m_stream.ReadString();
                        m_prevNameAsInt = m_elements.Count;
                        m_elements.Add(ValueName);
                    }
                    else
                    {
                        if (nameDelta == 7)
                            m_prevNameAsInt = m_stream.Read7BitInt();
                        else
                            m_prevNameAsInt += nameDelta;
                        ValueName = m_elements[m_prevNameAsInt];
                    }
                    //m_value.Load(m_stream);

                    break;
                case SttpMarkupNodeType.EndElement:
                    Element = CurrentElement;
                    ElementCompatibility = CurrentElementCompatiblity;
                    m_elementStack.Pop();
                    m_elementStackCompatibility.Pop();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return true;
        }

        private string CurrentElement
        {
            get
            {
                if (m_elementStack.Count == 0)
                    return string.Empty;
                return m_elementStack.Peek();
            }
        }
        private SttpMarkupCompatiblity CurrentElementCompatiblity
        {
            get
            {
                if (m_elementStackCompatibility.Count == 0)
                    return SttpMarkupCompatiblity.Unknown;
                return m_elementStackCompatibility.Peek();
            }
        }


        public void Reset()
        {
            m_stream.Position = 0;
        }

    }
}

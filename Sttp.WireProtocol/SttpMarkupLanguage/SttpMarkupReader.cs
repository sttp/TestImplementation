using System;
using System.Collections.Generic;
using System.IO;
using Sttp.IO;

namespace Sttp
{
    public class SttpMarkupReader
    {
        private ByteReader m_stream;
        private List<string> m_elements = new List<string>();
        private Stack<string> m_elementStack = new Stack<string>();
        private Stack<SttpMarkupCompatiblity> m_elementStackCompatibility = new Stack<SttpMarkupCompatiblity>();
        private int m_prevNameAsInt = 0;

        internal SttpMarkupReader(byte[] data)
        {
            Value = new SttpValueMutable();
            m_stream = new ByteReader(data);
        }

        public int ElementDepth => m_elementStack.Count;
        public string ElementName { get; private set; }
        public SttpMarkupCompatiblity ElementCompatibility { get; private set; }
        public string ValueName { get; private set; }
        public SttpValueMutable Value { get; private set; }
        public SttpMarkupCompatiblity ValueCompatibility { get; private set; }
        public SttpMarkupNodeType NodeType { get; private set; }

        public bool Read()
        {
            if (m_stream.Position == m_stream.Length)
                return false;

            if (NodeType == SttpMarkupNodeType.EndElement)
            {
                ElementName = CurrentElement;
                ElementCompatibility = CurrentElementCompatiblity;
            }

            byte code = m_stream.ReadByte();
            bool isNameAsString = ((code >> 4) & 1) == 1;
            int nameDelta = (code >> 5);

            NodeType = (SttpMarkupNodeType)(code & 3);
            switch (NodeType)
            {
                case SttpMarkupNodeType.Element:
                    Value.SetNull();
                    ValueCompatibility = SttpMarkupCompatiblity.Unknown;
                    ElementCompatibility = (SttpMarkupCompatiblity)((code >> 2) & 3);
                   


                    if (isNameAsString)
                    {
                        ElementName = m_stream.ReadString();
                        m_prevNameAsInt = m_elements.Count;
                        m_elements.Add(ElementName);
                    }
                    else
                    {
                        if (nameDelta == 7)
                            m_prevNameAsInt = m_stream.ReadInt7Bit();
                        else
                            m_prevNameAsInt += nameDelta;
                        ElementName = m_elements[m_prevNameAsInt];
                    }

                    m_elementStack.Push(ElementName);
                    m_elementStackCompatibility.Push(ElementCompatibility);

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
                            m_prevNameAsInt = m_stream.ReadInt7Bit();
                        else
                            m_prevNameAsInt += nameDelta;
                        ValueName = m_elements[m_prevNameAsInt];
                    }
                    Value.Load(m_stream);

                    break;
                case SttpMarkupNodeType.EndElement:
                    ElementName = CurrentElement;
                    ElementCompatibility = CurrentElementCompatiblity;
                    m_elementStack.Pop();
                    m_elementStackCompatibility.Pop();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return true;
        }

        public SttpMarkupElement ReadEntireElement()
        {
            return new SttpMarkupElement(this);
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
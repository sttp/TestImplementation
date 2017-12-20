using System;
using System.Collections.Generic;
using System.IO;
using Sttp.IO;

namespace Sttp
{
    public class SttpMarkupReader
    {
        private class NameLookupCache
        {
            public string Name;
            public int NextNameID;
            public SttpValueTypeCode PrevValueTypeCode;

            public NameLookupCache(string name, int nextNameID)
            {
                Name = name;
                NextNameID = nextNameID;
                PrevValueTypeCode = SttpValueTypeCode.Null;
            }
        }

        private ByteReader m_stream;
        private List<NameLookupCache> m_elements = new List<NameLookupCache>();
        private Stack<NameLookupCache> m_elementStack = new Stack<NameLookupCache>();
        private NameLookupCache m_prevName;
        private string m_rootElement;

        internal SttpMarkupReader(byte[] data)
        {
            m_stream = new ByteReader(data, 0, data.Length);
            Value = new SttpValueMutable();
            m_prevName = new NameLookupCache(string.Empty, 0);
            NodeType = SttpMarkupNodeType.StartOfDocument;
            m_rootElement = m_stream.ReadString();
            ElementName = CurrentElement;
        }

        public string RootElement => m_rootElement;
        public int ElementDepth => m_elementStack.Count;
        public string ElementName { get; private set; }
        public string ValueName { get; private set; }
        public SttpValueMutable Value { get; private set; }
        public SttpMarkupNodeType NodeType { get; private set; }

        public bool Read()
        {
            if (NodeType == SttpMarkupNodeType.EndOfDocument)
                return false;

            if (NodeType == SttpMarkupNodeType.EndElement)
            {
                ElementName = CurrentElement;
            }

            NodeType = (SttpMarkupNodeType)m_stream.ReadBits2();
            switch (NodeType)
            {
                case SttpMarkupNodeType.Element:
                    Value.SetNull();
                    ReadName();
                    m_elementStack.Push(m_prevName);
                    ElementName = m_prevName.Name;
                    break;
                case SttpMarkupNodeType.Value:
                    ReadName();
                    if (m_stream.ReadBits1() == 0)
                    {
                        //Same type code;
                        SttpValueEncodingWithoutType.Load(m_stream, m_prevName.PrevValueTypeCode, Value);
                    }
                    else
                    {
                        SttpValueEncodingNative.Load(m_stream, Value);
                        m_prevName.PrevValueTypeCode = Value.ValueTypeCode;
                    }
                    ValueName = m_prevName.Name;
                    break;
                case SttpMarkupNodeType.EndElement:
                    ElementName = CurrentElement;
                    m_elementStack.Pop();
                    break;
                case SttpMarkupNodeType.EndOfDocument:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return true;
        }

        private void ReadName()
        {
            if (m_stream.ReadBits1() == 1)
            {
                if (m_stream.ReadBits1() == 1)
                {
                    m_elements.Add(new NameLookupCache(m_stream.ReadString(), m_elements.Count));
                    m_prevName.NextNameID = m_elements.Count - 1;
                }
                else
                {
                    int index = (int)m_stream.Read8BitSegments();
                    m_prevName.NextNameID = index;
                }
            }
            m_prevName = m_elements[m_prevName.NextNameID];
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
                    return m_rootElement;
                return m_elementStack.Peek().Name;
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.IO;
using Sttp.IO;
using Sttp.SttpValueClasses;

namespace Sttp
{
    public class SttpMarkupReader
    {
        private class NameLookupCache
        {
            public string Name;
            public int NextNameID;
            public SttpValueMutable PrevValue;

            public NameLookupCache(string name, int nextNameID)
            {
                Name = name;
                NextNameID = nextNameID;
                PrevValue = new SttpValueMutable();
            }
        }

        private ByteReader m_stream;
        private List<NameLookupCache> m_elements = new List<NameLookupCache>();
        private Stack<NameLookupCache> m_elementStack = new Stack<NameLookupCache>();
        private NameLookupCache m_prevName;

        internal SttpMarkupReader(byte[] data)
        {
            m_stream = new ByteReader(data, 0, data.Length);
            Value = new SttpValueMutable();
            m_prevName = new NameLookupCache(string.Empty, 0);
        }

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
                    SttpValueEncodingDelta.Load(m_stream, m_prevName.PrevValue, Value);
                    m_prevName.PrevValue.SetValue(Value);
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
                    return string.Empty;
                return m_elementStack.Peek().Name;
            }
        }

        public void Reset()
        {
            m_stream.Position = 0;
        }

    }
}
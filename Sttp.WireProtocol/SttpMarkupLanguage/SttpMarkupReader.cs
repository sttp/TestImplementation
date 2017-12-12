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
        private BitStreamReader m_bitStream;

        internal SttpMarkupReader(byte[] data)
        {
            Value = new SttpValueMutable();
            int byteStreamLength = BigEndian.ToInt32(data, 0);
            int bitStreamLength = BigEndian.ToInt32(data, 4);

            m_stream = new ByteReader(data, 8, data.Length - bitStreamLength - 8);
            int cnt = m_stream.ReadInt7Bit();
            while (cnt > 0)
            {
                cnt--;
                m_elements.Add(new NameLookupCache(m_stream.ReadString(), m_elements.Count + 1));
            }

            m_bitStream = new BitStreamReader(data, m_stream.Position + byteStreamLength + 8, bitStreamLength);
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

            NodeType = (SttpMarkupNodeType)m_bitStream.ReadBits2();
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
                    Value.LoadDelta(m_bitStream, m_stream, m_prevName.PrevValue);
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
            if (m_bitStream.ReadBits1() != 0)
            {
                int index = (int)m_bitStream.Read8BitSegments(m_stream);
                m_prevName.NextNameID = index;
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
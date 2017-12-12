using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Sttp.IO;

namespace Sttp
{
    public class SttpMarkupWriter
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

        private Dictionary<string, int> m_nameCache = new Dictionary<string, int>();
        private List<NameLookupCache> m_namesList = new List<NameLookupCache>();
        private Stack<string> m_elementStack = new Stack<string>();
        private ByteWriter m_stream = new ByteWriter();
        private BitStreamWriter m_bitStream = new BitStreamWriter();
        private ElementEndElementHelper m_endElementHelper;
        private SttpValueMutable m_tmpValue = new SttpValueMutable();
        private NameLookupCache m_prevName;
        private bool m_disposed;

        public SttpMarkupWriter()
        {
            m_endElementHelper = new ElementEndElementHelper(this);
            m_prevName = new NameLookupCache(string.Empty, 0);
        }

        public string CurrentElement
        {
            get
            {
                if (m_elementStack.Count == 0)
                    return string.Empty;
                return m_elementStack.Peek();
            }
        }

        //Encoding Scheme: 
        //
        // First, determine the Node Type.
        // 2 bits, SttpMarkupType
        // If EndElement, then exit.
        //
        // Second, Determine the next NameIndex
        // 0: Next NameIndex is same as the last time it was encountered.
        // 1: It's not, Next index is 8 bit encoded number.
        // Third, If NodeType:Value, write the value.

        public ElementEndElementHelper StartElement(string name)
        {
            if (name == null || name.Length == 0)
                throw new ArgumentNullException(nameof(name));

            m_elementStack.Push(name);
            m_bitStream.WriteBits2((uint)SttpMarkupNodeType.Element);
            WriteName(name);

            return m_endElementHelper;
        }

        public void EndElement()
        {
            m_elementStack.Pop();
            m_bitStream.WriteBits2((uint)SttpMarkupNodeType.EndElement);
        }

        public void WriteValue(string name, SttpValue value)
        {
            if (name == null || name.Length == 0)
                throw new ArgumentNullException(nameof(name));
            if ((object)value == null)
                throw new ArgumentNullException(nameof(value));

            m_bitStream.WriteBits2((uint)SttpMarkupNodeType.Value);
            WriteName(name);
            value.SaveDelta(m_bitStream, m_stream, m_prevName.PrevValue);
            m_prevName.PrevValue.SetValue(value);
        }

        private void WriteName(string name)
        {
            if (!m_nameCache.TryGetValue(name, out int index))
            {
                m_nameCache[name] = m_nameCache.Count;
                m_namesList.Add(new NameLookupCache(name, m_nameCache.Count));
                index = m_nameCache.Count - 1;
            }
            if (m_prevName.NextNameID == index)
            {
                m_bitStream.WriteBits1(0);
            }
            else
            {
                m_bitStream.WriteBits1(1);
                m_bitStream.Write8BitSegments((uint)index);
            }
            m_prevName.NextNameID = index;
            m_prevName = m_namesList[index];
        }

        public void WriteValue(string name, object value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        public SttpMarkup ToSttpMarkup()
        {
            if (!m_disposed)
            {
                m_bitStream.WriteBits2((byte)SttpMarkupNodeType.EndOfDocument);
                m_disposed = true;
            }

            byte[] byteStream = m_stream.ToArray();
            m_bitStream.GetBuffer(out byte[] bitStream, out int bitStreamLength);
            var stream = new ByteWriter();
            stream.Write(byteStream.Length);
            stream.Write(bitStreamLength);
            stream.WriteInt7Bit(m_namesList.Count);
            foreach (var item in m_namesList)
            {
                stream.Write(item.Name);
            }
            stream.WriteWithoutLength(byteStream, 0, byteStream.Length);
            stream.WriteWithoutLength(bitStream, 0, bitStreamLength);
            return new SttpMarkup(stream.ToArray());
        }
    }
}
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
            public SttpValueTypeCode PrevValueTypeCode;

            public NameLookupCache(string name, int nextNameID)
            {
                Name = name;
                NextNameID = nextNameID;
                PrevValueTypeCode = SttpValueTypeCode.Null;
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
        private ElementEndElementHelper m_endElementHelper;
        private SttpValueMutable m_tmpValue = new SttpValueMutable();
        private NameLookupCache m_prevName;
        private bool m_disposed;
        private string m_rootElement;

        public SttpMarkupWriter(string rootElement)
        {
            m_rootElement = rootElement;
            m_endElementHelper = new ElementEndElementHelper(this);
            m_prevName = new NameLookupCache(string.Empty, 0);
            m_stream.Write(m_rootElement);
        }

        public string RootElement => m_rootElement;

        public string CurrentElement
        {
            get
            {
                if (m_elementStack.Count == 0)
                    return m_rootElement;
                return m_elementStack.Peek();
            }
        }

        public int CurrentSize => m_stream.Length;

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
            m_stream.WriteBits2((uint)SttpMarkupNodeType.Element);
            WriteName(name);

            return m_endElementHelper;
        }

        public void EndElement()
        {
            m_elementStack.Pop();
            m_stream.WriteBits2((uint)SttpMarkupNodeType.EndElement);
        }

        public void WriteValue(string name, SttpValue value)
        {
            if (name == null || name.Length == 0)
                throw new ArgumentNullException(nameof(name));
            if ((object)value == null)
                throw new ArgumentNullException(nameof(value));

            m_stream.WriteBits2((uint)SttpMarkupNodeType.Value);
            WriteName(name);
            if (value.ValueTypeCode == m_prevName.PrevValueTypeCode)
            {
                m_stream.WriteBits1(0);
                SttpValueEncodingWithoutType.Save(m_stream, value);
            }
            else
            {
                m_stream.WriteBits1(1);
                SttpValueEncodingNative.Save(m_stream, value);
                m_prevName.PrevValueTypeCode = value.ValueTypeCode;
            }
        }

        private void WriteName(string name)
        {
            if (!m_nameCache.TryGetValue(name, out int index))
            {
                m_nameCache[name] = m_nameCache.Count;
                m_namesList.Add(new NameLookupCache(name, m_nameCache.Count));
                index = m_nameCache.Count - 1;
                m_stream.WriteBits1(1);
                m_stream.WriteBits1(1);
                m_stream.Write(name);
            }
            else if (m_prevName.NextNameID == index)
            {
                m_stream.WriteBits1(0);
            }
            else
            {
                m_stream.WriteBits1(1);
                m_stream.WriteBits1(0);
                m_stream.Write8BitSegments((uint)index);
            }
            m_prevName.NextNameID = index;
            m_prevName = m_namesList[index];
        }

        public void WriteValue(string name, object value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        public void WriteValue(string name, byte[] value, int offset, int length)
        {
            value.ValidateParameters(offset, length);
            byte[] data2 = new byte[length];
            Array.Copy(value, offset, data2, 0, length);
            WriteValue(name, data2);
        }

        public SttpMarkup ToSttpMarkup()
        {
            if (!m_disposed)
            {
                m_stream.WriteBits2((byte)SttpMarkupNodeType.EndOfDocument);
                m_disposed = true;
            }
            return new SttpMarkup(m_stream.ToArray());
        }

        public void UnionWith(SttpMarkup queries)
        {
            var rdr = queries.MakeReader();
            while (rdr.Read())
            {
                switch (rdr.NodeType)
                {
                    case SttpMarkupNodeType.Element:
                        StartElement(rdr.ElementName);
                        break;
                    case SttpMarkupNodeType.Value:
                        WriteValue(rdr.ValueName, rdr.Value);
                        break;
                    case SttpMarkupNodeType.EndElement:
                        EndElement();
                        break;
                    case SttpMarkupNodeType.EndOfDocument:
                        break;
                    case SttpMarkupNodeType.StartOfDocument:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }
    }
}
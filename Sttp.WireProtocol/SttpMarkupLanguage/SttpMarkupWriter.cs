using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Sttp.IO;

namespace Sttp
{
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
        private Dictionary<string, int> m_nameCache = new Dictionary<string, int>();
        private Stack<string> m_elementStack = new Stack<string>();
        private ByteWriter m_stream = new ByteWriter();
        private ElementEndElementHelper m_endElementHelper;
        private SttpValueMutable m_tmpValue = new SttpValueMutable();
        private int m_prevNameAsInt = 0;

        public SttpMarkupWriter()
        {
            m_endElementHelper = new ElementEndElementHelper(this);
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

        public ElementEndElementHelper StartElement(string name, SttpMarkupCompatiblity compatiblity = SttpMarkupCompatiblity.KnownAndEnforced)
        {
            if (name == null || name.Length == 0)
                throw new ArgumentNullException(nameof(name));
            if (compatiblity < SttpMarkupCompatiblity.KnownAndEnforced || compatiblity > SttpMarkupCompatiblity.Unknown)
                throw new InvalidEnumArgumentException(nameof(compatiblity), (int)compatiblity, typeof(SttpMarkupCompatiblity));

            m_elementStack.Push(name);
            if (m_nameCache.TryGetValue(name, out int index))
            {
                Encode(index, compatiblity);
            }
            else
            {
                m_nameCache[name] = m_nameCache.Count;
                Encode(name, compatiblity, m_nameCache.Count - 1);
            }
            return m_endElementHelper;
        }

        public void EndElement()
        {
            m_elementStack.Pop();
            m_stream.Write((byte)SttpMarkupNodeType.EndElement);
        }


        public void WriteValue(string name, SttpValue value, SttpMarkupCompatiblity compatiblity = SttpMarkupCompatiblity.KnownAndEnforced)
        {
            if (name == null || name.Length == 0)
                throw new ArgumentNullException(nameof(name));
            if (compatiblity < SttpMarkupCompatiblity.KnownAndEnforced || compatiblity > SttpMarkupCompatiblity.Unknown)
                throw new InvalidEnumArgumentException(nameof(compatiblity), (int)compatiblity, typeof(SttpMarkupCompatiblity));
            if ((object)value == null)
                throw new ArgumentNullException(nameof(value));

            if (m_nameCache.TryGetValue(name, out int index))
            {
                Encode(index, compatiblity, value);
            }
            else
            {
                m_nameCache[name] = m_nameCache.Count;
                Encode(name, compatiblity, value, m_nameCache.Count - 1);
            }
        }

        //Encoding for the prefix character:
        // Bits 0,1: SttpMarkupType
        // Bits 2,3: SttpMarkupCompatiblity (0 if EndElement)
        // Bits 4: 1: Name as String, 0: Name as Int32
        // Bits 5,6,7: 7: Name as int, 7bit int. 0-6. Previous int + value.

        private void Encode(int nameIndex, SttpMarkupCompatiblity compatiblity)
        {
            int delta = nameIndex - m_prevNameAsInt;
            if (delta >= 0 && delta < 7)
            {
                m_stream.Write((byte)((int)SttpMarkupNodeType.Element | ((int)compatiblity << 2) | (0 << 4) | (delta << 5)));
            }
            else
            {
                m_stream.Write((byte)((int)SttpMarkupNodeType.Element | ((int)compatiblity << 2) | (0 << 4) | (7 << 5)));
                m_stream.WriteInt7Bit(nameIndex);
            }
            m_prevNameAsInt = nameIndex;
        }

        private void Encode(string name, SttpMarkupCompatiblity compatiblity, int nameIndex)
        {
            m_stream.Write((byte)((int)SttpMarkupNodeType.Element | ((int)compatiblity << 2) | (1 << 4) | (0 << 5)));
            m_stream.Write(name);
            m_prevNameAsInt = nameIndex;
        }

        private void Encode(int nameIndex, SttpMarkupCompatiblity compatiblity, SttpValue value)
        {
            int delta = nameIndex - m_prevNameAsInt;
            if (delta >= 0 && delta < 7)
            {
                m_stream.Write((byte)((int)SttpMarkupNodeType.Value | ((int)compatiblity << 2) | (0 << 4) | (delta << 5)));
            }
            else
            {
                m_stream.Write((byte)((int)SttpMarkupNodeType.Value | ((int)compatiblity << 2) | (0 << 4) | (7 << 5)));
                m_stream.WriteInt7Bit(nameIndex);
            }
            m_prevNameAsInt = nameIndex;
            value.Save(m_stream);
        }

        private void Encode(string name, SttpMarkupCompatiblity compatiblity, SttpValue value, int nameIndex)
        {
            m_stream.Write((byte)((int)SttpMarkupNodeType.Value | ((int)compatiblity << 2) | (1 << 4) | (0 << 5)));
            m_stream.Write(name);
            value.Save(m_stream);
            m_prevNameAsInt = nameIndex;
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
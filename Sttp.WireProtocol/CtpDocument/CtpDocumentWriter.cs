using System;
using System.Collections.Generic;

namespace CTP
{
    /// <summary>
    /// A user friendly means of writing a <see cref="CtpDocument"/>.
    /// </summary>
    public class CtpDocumentWriter
    {
        /// <summary>
        /// Helper class that contains the state data to assist in compressing the data.
        /// </summary>
        private class NameLookupCache
        {
            /// <summary>
            /// The index position of the next NameID. This defaults to the current index + 1, 
            /// and always matches the most recent name ID last time this name traversed to a new ID. (Can be the same)
            /// </summary>
            public int NextNameID;
            /// <summary>
            /// The value type code used to encode this value. Defaults to null, and is assigned every time a value is
            /// saved using this name. If elements are saved, the value is unchanged.
            /// </summary>
            public CtpTypeCode PrevValueTypeCode;

            public NameLookupCache(int nextNameID)
            {
                NextNameID = nextNameID;
                PrevValueTypeCode = CtpTypeCode.Null;
            }
        }

        /// <summary>
        /// A helper class so calls to <see cref="EndElement"/> can be wrapped in a using clause.
        /// Note: this class is a single instance class and does not protect against multiple calls to Dispose. Therefore,
        /// it's not intended to be used outside of making it easier to section out code.
        /// </summary>
        public class ElementEndElementHelper : IDisposable
        {
            private CtpDocumentWriter m_parent;
            public ElementEndElementHelper(CtpDocumentWriter parent)
            {
                m_parent = parent;
            }
            public void Dispose()
            {
                m_parent.EndElement();
            }
        }

        /// <summary>
        /// A lookup of all names that have been registered.
        /// </summary>
        private Dictionary<string, int> m_nameCache = new Dictionary<string, int>();
        /// <summary>
        /// A list of all names and the state data associated with these names.
        /// </summary>
        private List<NameLookupCache> m_namesList = new List<NameLookupCache>();
        /// <summary>
        /// The list of elements so an error can occur when the element tree is invalid..
        /// </summary>
        private Stack<string> m_elementStack = new Stack<string>();
        /// <summary>
        /// Where to write the data.
        /// </summary>
        private DocumentBitWriter m_stream = new DocumentBitWriter();
        /// <summary>
        /// A reusable class to ease in calling the <see cref="EndElement"/> method.
        /// </summary>
        private ElementEndElementHelper m_endElementHelper;
        /// <summary>
        /// A temporary value so this class can support setting from an object type.
        /// </summary>
        private CtpObject m_tmpValue = new CtpObject();
        /// <summary>
        /// The most recent name that was encountered
        /// </summary>
        private NameLookupCache m_prevName;
        /// <summary>
        /// Gets if ToSttpMarkup has been called. 
        /// </summary>
        private bool m_disposed;

        /// <summary>
        /// Create a new writer with the provided root element.
        /// </summary>
        /// <param name="rootElement"></param>
        public CtpDocumentWriter(string rootElement)
        {
            m_endElementHelper = new ElementEndElementHelper(this);
            m_prevName = new NameLookupCache(0);
            m_stream.WriteAscii(rootElement);
        }

        /// <summary>
        /// The approximate current size of the writer. It's not exact until <see cref="ToCtpDocument"/> has been called.
        /// </summary>
        public int CurrentSize => m_stream.Length;

        //Encoding Scheme: 
        //
        // First, determine the Node Type.
        // 2 bits, CtpDocumentNodeType
        // If EndElement, then exit.
        //
        // Second, Determine the next NameIndex
        // 0: Next NameIndex is same as the last time it was encountered.
        // 1: It's not, Next index is 8 bit encoded number.
        // Third, If NodeType:Value, write the value.

        /// <summary>
        /// Starts a new element with the specified name. 
        /// </summary>
        /// <param name="name">The name of the element. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <returns>an object that can be used in a using block to make the code cleaner.</returns>
        public ElementEndElementHelper StartElement(string name)
        {
            if (m_disposed)
                throw new ObjectDisposedException("Once ToCtpDocument has been called, no more data can be written to this object.");

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            m_elementStack.Push(name);
            m_stream.WriteBits2((uint)CtpDocumentNodeType.Element);
            WriteName(name);

            return m_endElementHelper;
        }

        /// <summary>
        /// Ends the current element. This should not be called if <see cref="StartElement"/> is inside a using block, since 
        /// this will automatically be called when exiting the using block.
        /// </summary>
        public void EndElement()
        {
            if (m_disposed)
                throw new ObjectDisposedException("Once ToCtpDocument has been called, no more data can be written to this object.");

            m_elementStack.Pop();
            m_stream.WriteBits2((uint)CtpDocumentNodeType.EndElement);
        }


        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(string name, CtpObject value)
        {
            if (m_disposed)
                throw new ObjectDisposedException("Once ToCtpDocument has been called, no more data can be written to this object.");

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if ((object)value == null)
                throw new ArgumentNullException(nameof(value));

            m_stream.WriteBits2((uint)CtpDocumentNodeType.Value);
            WriteName(name);
            if (value.ValueTypeCode == m_prevName.PrevValueTypeCode)
            {
                m_stream.WriteBits1(0);
            }
            else
            {
                m_stream.WriteBits1(1);
                m_stream.WriteBits4((byte)value.ValueTypeCode);
                m_prevName.PrevValueTypeCode = value.ValueTypeCode;
            }

            switch (value.ValueTypeCode)
            {
                case CtpTypeCode.Null:
                    break;
                case CtpTypeCode.Int64:
                    m_stream.Write8BitSegments((ulong)PackSign(value.AsInt64));
                    break;
                case CtpTypeCode.Single:
                    m_stream.Write(value.AsSingle);
                    break;
                case CtpTypeCode.Double:
                    m_stream.Write(value.AsDouble);
                    break;
                case CtpTypeCode.CtpTime:
                    m_stream.Write(value.AsCtpTime.Ticks);
                    break;
                case CtpTypeCode.Boolean:
                    m_stream.WriteBits1(value.AsBoolean);
                    break;
                case CtpTypeCode.Guid:
                    m_stream.Write(value.AsGuid);
                    break;
                case CtpTypeCode.String:
                    m_stream.Write(value.AsString);
                    break;
                case CtpTypeCode.CtpBuffer:
                    m_stream.Write(value.AsCtpBuffer.ToBuffer());
                    break;
                case CtpTypeCode.CtpDocument:
                    m_stream.Write(value.AsDocument.ToBuffer());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void WriteName(string name)
        {
            if (!m_nameCache.TryGetValue(name, out int index))
            {
                m_nameCache[name] = m_nameCache.Count;
                m_namesList.Add(new NameLookupCache(m_nameCache.Count));
                index = m_nameCache.Count - 1;
                m_stream.WriteBits1(1);
                m_stream.WriteBits1(1);
                m_stream.WriteAscii(name);
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

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(string name, object value)
        {
            if (m_disposed)
                throw new ObjectDisposedException("Once ToSttpMarkup has been called, no more data can be written to this object.");

            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Completes the writing to an <see cref="CtpDocument"/> and returns the completed buffer. This may be called multiple times.
        /// </summary>
        /// <returns></returns>
        public CtpDocument ToCtpDocument()
        {
            if (!m_disposed)
            {
                m_stream.WriteBits2((byte)CtpDocumentNodeType.EndOfDocument);
                m_disposed = true;
            }
            return new CtpDocument(m_stream.ToArray());
        }

        private static long PackSign(long value)
        {
            //since negative signed values have leading 1's and positive have leading 0's, 
            //it's important to change it into a common format.
            //Basically, we rotate left to move the leading sign bit to bit0, and if bit 0 is set, we invert bits 1-63.
            if (value >= 0)
                return value << 1;
            return (~value << 1) + 1;
        }

    }
}
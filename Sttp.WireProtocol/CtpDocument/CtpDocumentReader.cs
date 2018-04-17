using System;
using System.Collections.Generic;

namespace CTP
{
    /// <summary>
    /// A class for reading SttpMarkup documents.
    /// </summary>
    public class CtpDocumentReader
    {
        /// <summary>
        /// Helper class that contains the state data to assist in decompressing the data.
        /// </summary>
        private class NameLookupCache
        {
            /// <summary>
            /// The name of the element/value. Must be less than 256 characters and 7-bit ASCII.
            /// </summary>
            public string Name;
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

            public NameLookupCache(string name, int nextNameID)
            {
                Name = name;
                NextNameID = nextNameID;
                PrevValueTypeCode = CtpTypeCode.Null;
            }
        }

        /// <summary>
        /// The stream for reading the byte array.
        /// </summary>
        private DocumentBitReader m_stream;
        /// <summary>
        /// A list of all names and the state data associated with these names.
        /// </summary>
        private List<NameLookupCache> m_namesList = new List<NameLookupCache>();
        /// <summary>
        /// The list of elements so the <see cref="ElementName"/> can be retrieved.
        /// </summary>
        private Stack<NameLookupCache> m_elementStack = new Stack<NameLookupCache>();
        /// <summary>
        /// The most recent name that was encountered
        /// </summary>
        private NameLookupCache m_prevName;
        /// <summary>
        /// The root element.
        /// </summary>
        private string m_rootElement;

        /// <summary>
        /// Creates a markup reader from the specified byte array.
        /// </summary>
        /// <param name="data"></param>
        internal CtpDocumentReader(byte[] data)
        {
            m_stream = new DocumentBitReader(data, 0, data.Length);
            Value = new CtpObject();
            m_prevName = new NameLookupCache(string.Empty, 0);
            NodeType = CtpDocumentNodeType.StartOfDocument;
            m_rootElement = m_stream.ReadAsciiShort();
            ElementName = GetCurrentElement();
        }

        /// <summary>
        /// The name of the root element.
        /// </summary>
        public string RootElement => m_rootElement;
        /// <summary>
        /// The depth of the element stack. 0 means the depth is at the root element.
        /// </summary>
        public int ElementDepth => m_elementStack.Count;
        /// <summary>
        /// The current name of the current element. Can be the RootElement if ElementDepth is 0 and <see cref="NodeType"/> is not <see cref="CtpDocumentNodeType.EndElement"/>.
        /// In this event, the ElementName does not change and refers to the element that has just ended.
        /// </summary>
        public string ElementName { get; private set; }
        /// <summary>
        /// If <see cref="NodeType"/> is <see cref="CtpDocumentNodeType.Value"/>, the name of the value. Otherwise, null.
        /// </summary>
        public string ValueName { get; private set; }
        /// <summary>
        /// If <see cref="NodeType"/> is <see cref="CtpDocumentNodeType.Value"/>, the value. Otherwise, SttpValue.Null.
        /// Note, this is a mutable value and it's contents will change with each iteration. To keep a copy of the 
        /// contents, be sure to call <see cref="CtpValue.Clone"/>
        /// </summary>
        public CtpObject Value { get; private set; }

        /// <summary>
        /// The type of the current node. To Advance the nodes calll <see cref="Read"/>
        /// </summary>
        public CtpDocumentNodeType NodeType { get; private set; }

        /// <summary>
        /// Reads to the next node. If the next node is the end of the document. False is returned. Otherwise true.
        /// </summary>
        /// <returns></returns>
        public bool Read()
        {
            if (NodeType == CtpDocumentNodeType.EndOfDocument)
                return false;

            if (NodeType == CtpDocumentNodeType.EndElement)
            {
                ElementName = GetCurrentElement();
            }

            NodeType = (CtpDocumentNodeType)m_stream.ReadBits2();
            switch (NodeType)
            {
                case CtpDocumentNodeType.Element:
                    Value.SetNull();
                    ReadName();
                    m_elementStack.Push(m_prevName);
                    ElementName = m_prevName.Name;
                    ValueName = null;
                    break;
                case CtpDocumentNodeType.Value:
                    ReadName();
                    if (m_stream.ReadBits1() == 0)
                    {
                        //Same type code;
                        CtpValueEncodingWithoutType.Load(m_stream, m_prevName.PrevValueTypeCode, Value);
                    }
                    else
                    {
                        CtpValueEncodingNative.Load(m_stream, Value);
                        m_prevName.PrevValueTypeCode = Value.ValueTypeCode;
                    }
                    ValueName = m_prevName.Name;
                    break;
                case CtpDocumentNodeType.EndElement:
                    ElementName = GetCurrentElement();
                    m_elementStack.Pop();
                    break;
                case CtpDocumentNodeType.EndOfDocument:
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
                    m_namesList.Add(new NameLookupCache(m_stream.ReadAsciiShort(), m_namesList.Count));
                    m_prevName.NextNameID = m_namesList.Count - 1;
                }
                else
                {
                    int index = (int)m_stream.Read8BitSegments();
                    m_prevName.NextNameID = index;
                }
            }
            m_prevName = m_namesList[m_prevName.NextNameID];
        }

        /// <summary>
        /// Reads the entire element into an in-memory object, advancing to the end of this element. This is the most convenient method of reading
        /// but is impractical for large elements. The intended mode of operation is to interweave calls to <see cref="Read"/> with <see cref="ReadEntireElement"/> to assist in parsing.
        /// </summary>
        /// <returns></returns>
        public CtpDocumentElement ReadEntireElement()
        {
            return new CtpDocumentElement(this);
        }

        private string GetCurrentElement()
        {
            if (m_elementStack.Count == 0)
                return m_rootElement;
            return m_elementStack.Peek().Name;
        }

    }
}
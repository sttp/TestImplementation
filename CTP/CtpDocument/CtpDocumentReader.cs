using System;
using System.Collections.Generic;

namespace CTP
{
    /// <summary>
    /// A class for reading CtpDocument documents.
    /// </summary>
    public class CtpDocumentReader
    {
        /// <summary>
        /// The stream for reading the byte array.
        /// </summary>
        private CtpDocumentBitReader m_stream;
        /// <summary>
        /// A list of all names and the state data associated with these names.
        /// </summary>
        private CtpDocumentNames[] m_elementNamesList;
        private CtpDocumentNames[] m_valueNamesList;
        /// <summary>
        /// The list of elements so the <see cref="ElementName"/> can be retrieved.
        /// </summary>
        private Stack<CtpDocumentNames> m_elementStack = new Stack<CtpDocumentNames>();

        /// <summary>
        /// The root element.
        /// </summary>
        private CtpDocumentNames m_rootElement;

        /// <summary>
        /// Creates a markup reader from the specified byte array.
        /// </summary>
        /// <param name="data"></param>
        internal CtpDocumentReader(byte[] data)
        {
            m_stream = new CtpDocumentBitReader(data, 0, data.Length);
            Value = new CtpObject();
            NodeType = CtpDocumentNodeType.StartOfDocument;
            m_rootElement = m_stream.ReadAscii();
            int elementCount = (int)m_stream.ReadBits16();
            int valueCount = (int)m_stream.ReadBits16();
            m_elementNamesList = new CtpDocumentNames[elementCount];
            m_valueNamesList = new CtpDocumentNames[valueCount];
            for (int x = 0; x < elementCount; x++)
            {
                m_elementNamesList[x] = m_stream.ReadAscii();
            }
            for (int x = 0; x < valueCount; x++)
            {
                m_valueNamesList[x] = m_stream.ReadAscii();
            }
            ElementName = GetCurrentElement();
        }

        /// <summary>
        /// The name of the root element.
        /// </summary>
        public CtpDocumentNames RootElement => m_rootElement;
        /// <summary>
        /// The depth of the element stack. 0 means the depth is at the root element.
        /// </summary>
        public int ElementDepth => m_elementStack.Count;
        /// <summary>
        /// The current name of the current element. Can be the RootElement if ElementDepth is 0 and <see cref="NodeType"/> is not <see cref="CtpDocumentNodeType.EndElement"/>.
        /// In this event, the ElementName does not change and refers to the element that has just ended.
        /// </summary>
        public CtpDocumentNames ElementName { get; private set; }
        /// <summary>
        /// If <see cref="NodeType"/> is <see cref="CtpDocumentNodeType.Value"/>, the name of the value. Otherwise, null.
        /// </summary>
        public CtpDocumentNames ValueName { get; private set; }

        /// <summary>
        /// If <see cref="NodeType"/> is <see cref="CtpDocumentNodeType.Value"/>, the value. Otherwise, SttpValue.Null.
        /// Note, this is a mutable value and it's contents will change with each iteration. To keep a copy of the 
        /// contents, be sure to call <see cref="CtpObject.Clone"/>
        /// </summary>
        public CtpObject Value { get; private set; }

        /// <summary>
        /// The type of the current node. To Advance the nodes call <see cref="Read"/>
        /// </summary>
        internal CtpDocumentNodeType NodeType { get; private set; }

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

            if (m_stream.IsEos)
            {
                NodeType = CtpDocumentNodeType.EndOfDocument;
                return false;
            }

            uint code = (uint)m_stream.Read7BitInt();
            CtpDocumentHeader header = (CtpDocumentHeader)(code & 15);

            if (header >= CtpDocumentHeader.ValueNull)
            {
                NodeType = CtpDocumentNodeType.Value;
                ValueName = m_valueNamesList[code >> 4];

                switch (header)
                {
                    case CtpDocumentHeader.ValueNull:
                        Value.SetNull();
                        break;
                    case CtpDocumentHeader.ValueInt64:
                        Value.SetValue((long)m_stream.Read7BitInt());
                        break;
                    case CtpDocumentHeader.ValueInvertedInt64:
                        Value.SetValue((long)~m_stream.Read7BitInt());
                        break;
                    case CtpDocumentHeader.ValueSingle:
                        Value.SetValue(m_stream.ReadSingle());
                        break;
                    case CtpDocumentHeader.ValueDouble:
                        Value.SetValue(m_stream.ReadDouble());
                        break;
                    case CtpDocumentHeader.ValueCtpTime:
                        Value.SetValue(new CtpTime(m_stream.ReadInt64()));
                        break;
                    case CtpDocumentHeader.ValueBooleanTrue:
                        Value.SetValue(true);
                        break;
                    case CtpDocumentHeader.ValueBooleanFalse:
                        Value.SetValue(false);
                        break;
                    case CtpDocumentHeader.ValueGuid:
                        Value.SetValue(m_stream.ReadGuid());
                        break;
                    case CtpDocumentHeader.ValueString:
                        Value.SetValue(m_stream.ReadString());
                        break;
                    case CtpDocumentHeader.ValueCtpBuffer:
                        Value.SetValue(new CtpBuffer(m_stream.ReadBytes()));
                        break;
                    case CtpDocumentHeader.ValueCtpDocument:
                        Value.SetValue(new CtpDocument(m_stream.ReadBytes(), false));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (header == CtpDocumentHeader.StartElement)
            {
                NodeType = CtpDocumentNodeType.StartElement;
                Value.SetNull();
                ElementName = m_elementNamesList[code >> 4];
                ValueName = null;
                m_elementStack.Push(ElementName);
            }
            else if (header == CtpDocumentHeader.EndElement)
            {
                NodeType = CtpDocumentNodeType.EndElement;
                ElementName = GetCurrentElement();
                m_elementStack.Pop();
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
            return true;
        }

        private CtpDocumentNames GetCurrentElement()
        {
            if (m_elementStack.Count == 0)
                return m_rootElement;
            return m_elementStack.Peek();
        }

        public void SkipElement()
        {
            int stack = m_elementStack.Count;
            while (Read())
            {
                if (m_elementStack.Count < stack)
                    return;
            }
        }
    }
}
using System;
using System.Collections.Generic;

namespace CTP
{
    /// <summary>
    /// A class for reading <see cref="CtpCommand"/>s.
    /// </summary>
    internal class CtpCommandReader
    {
        /// <summary>
        /// The stream for reading the byte array.
        /// </summary>
        private CtpCommandBitReader m_stream;
        /// <summary>
        /// A list of all names and the state data associated with these names.
        /// </summary>
        private CtpCommandKeyword[] m_elementNamesList;
        private CtpCommandKeyword[] m_valueNamesList;
        /// <summary>
        /// The list of elements so the <see cref="ElementName"/> can be retrieved.
        /// </summary>
        private Stack<CtpCommandKeyword> m_elementStack = new Stack<CtpCommandKeyword>();

        /// <summary>
        /// The root element.
        /// </summary>
        private CtpCommandKeyword m_rootElement;

        /// <summary>
        /// Creates a <see cref="CtpCommandReader"/> from the specified byte array.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        public CtpCommandReader(byte[] data, int offset)
        {
            m_stream = new CtpCommandBitReader(data, offset, data.Length - offset);
            Value = new CtpObject();
            NodeType = CtpCommandNodeType.StartOfCommand;
            m_rootElement = m_stream.ReadCommandKeyword();
            int elementCount = (int)m_stream.ReadBits16();
            int valueCount = (int)m_stream.ReadBits16();
            m_elementNamesList = new CtpCommandKeyword[elementCount];
            m_valueNamesList = new CtpCommandKeyword[valueCount];
            for (int x = 0; x < elementCount; x++)
            {
                m_elementNamesList[x] = m_stream.ReadCommandKeyword();
            }
            for (int x = 0; x < valueCount; x++)
            {
                m_valueNamesList[x] = m_stream.ReadCommandKeyword();
            }
            ElementName = GetCurrentElement();
        }

        /// <summary>
        /// The name of the root element.
        /// </summary>
        public CtpCommandKeyword RootElement => m_rootElement;
        /// <summary>
        /// The depth of the element stack. 0 means the depth is at the root element.
        /// </summary>
        public int ElementDepth => m_elementStack.Count;
        /// <summary>
        /// The current name of the current element. Can be the RootElement if ElementDepth is 0
        /// and <see cref="NodeType"/> is not <see cref="CtpCommandNodeType.EndElement"/>.
        /// In this event, the ElementName does not change and refers to the element that has just ended.
        /// </summary>
        public CtpCommandKeyword ElementName { get; private set; }
        /// <summary>
        /// If <see cref="NodeType"/> is <see cref="CtpCommandNodeType.Value"/>, the name of the value. Otherwise, null.
        /// </summary>
        public CtpCommandKeyword ValueName { get; private set; }

        /// <summary>
        /// If <see cref="NodeType"/> is <see cref="CtpCommandNodeType.Value"/>, the value. Otherwise, CtpObject.Null.
        /// Note, this is a mutable value and it's contents will change with each iteration. To keep a copy of the 
        /// contents, be sure to call <see cref="CtpObject.Clone"/>
        /// </summary>
        public CtpObject Value { get; private set; }

        /// <summary>
        /// The type of the current node. To Advance the nodes call <see cref="Read"/>
        /// </summary>
        public CtpCommandNodeType NodeType { get; private set; }

        /// <summary>
        /// Reads to the next node. If the next node is the end of the document. False is returned. Otherwise true.
        /// </summary>
        /// <returns></returns>
        public bool Read()
        {
            if (NodeType == CtpCommandNodeType.EndOfCommand)
                return false;

            if (NodeType == CtpCommandNodeType.EndElement)
            {
                ElementName = GetCurrentElement();
            }

            if (m_stream.IsEos)
            {
                NodeType = CtpCommandNodeType.EndOfCommand;
                return false;
            }

            uint code = (uint)m_stream.Read7BitInt();
            CtpCommandHeader header = (CtpCommandHeader)(code & 15);

            if (header >= CtpCommandHeader.ValueNull)
            {
                NodeType = CtpCommandNodeType.Value;
                ValueName = m_valueNamesList[code >> 4];

                switch (header)
                {
                    case CtpCommandHeader.ValueNull:
                        Value.SetNull();
                        break;
                    case CtpCommandHeader.ValueInt64:
                        Value.SetValue((long)m_stream.Read7BitInt());
                        break;
                    case CtpCommandHeader.ValueInvertedInt64:
                        Value.SetValue((long)~m_stream.Read7BitInt());
                        break;
                    case CtpCommandHeader.ValueSingle:
                        Value.SetValue(m_stream.ReadSingle());
                        break;
                    case CtpCommandHeader.ValueDouble:
                        Value.SetValue(m_stream.ReadDouble());
                        break;
                    case CtpCommandHeader.ValueCtpTime:
                        Value.SetValue(m_stream.ReadTime());
                        break;
                    case CtpCommandHeader.ValueBooleanTrue:
                        Value.SetValue(true);
                        break;
                    case CtpCommandHeader.ValueBooleanFalse:
                        Value.SetValue(false);
                        break;
                    case CtpCommandHeader.ValueGuid:
                        Value.SetValue(m_stream.ReadGuid());
                        break;
                    case CtpCommandHeader.ValueString:
                        Value.SetValue(m_stream.ReadString());
                        break;
                    case CtpCommandHeader.ValueCtpBuffer:
                        Value.SetValue(m_stream.ReadBuffer());
                        break;
                    case CtpCommandHeader.ValueCtpCommand:
                        Value.SetValue(m_stream.ReadCommand());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (header == CtpCommandHeader.StartElement)
            {
                NodeType = CtpCommandNodeType.StartElement;
                Value.SetNull();
                ElementName = m_elementNamesList[code >> 4];
                ValueName = null;
                m_elementStack.Push(ElementName);
            }
            else if (header == CtpCommandHeader.EndElement)
            {
                NodeType = CtpCommandNodeType.EndElement;
                ElementName = GetCurrentElement();
                m_elementStack.Pop();
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
            return true;
        }

        private CtpCommandKeyword GetCurrentElement()
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
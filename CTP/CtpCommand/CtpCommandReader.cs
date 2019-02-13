using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

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
        private ByteReader m_stream;
        /// <summary>
        /// A list of all names and the state data associated with these names.
        /// </summary>
        private CtpCommandKeyword[] m_namesList;
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
            Value = CtpObject.Null;
            NodeType = CtpCommandNodeType.StartOfCommand;
            m_rootElement = ReadCommandKeyword(data, ref offset, data.Length);
            int count = (int)ReadBits16(data, ref offset, data.Length);
            m_namesList = new CtpCommandKeyword[count];
            for (int x = 0; x < count; x++)
            {
                m_namesList[x] = ReadCommandKeyword(data, ref offset, data.Length);
            }
            ElementName = GetCurrentElement();
            m_stream = CreateReader(data, offset, data.Length);
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

            if (m_stream.IsEmpty)
            {
                NodeType = CtpCommandNodeType.EndOfCommand;
                return false;
            }

            if (m_stream.ReadBits1() == 0)
            {
                if (m_stream.ReadBits1() == 0)
                {
                    //00 = Start Element
                    NodeType = CtpCommandNodeType.StartElement;
                    Value = CtpObject.Null;
                    ElementName = m_namesList[m_stream.Read4BitSegments()];
                    ValueName = null;
                    m_elementStack.Push(ElementName);
                }
                else
                {
                    //01 = End Element
                    NodeType = CtpCommandNodeType.EndElement;
                    ElementName = GetCurrentElement();
                    m_elementStack.Pop();
                }
            }
            else
            {
                //1 = WriteValue
                NodeType = CtpCommandNodeType.Value;
                ValueName = m_namesList[m_stream.Read4BitSegments()];
                Value = m_stream.ReadObject();
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

        public static ByteReader CreateReader(byte[] m_buffer, int m_position, int m_length)
        {
            var b = new ByteReader();
            b.SetBuffer(m_buffer, m_position, m_length - m_position);
            return b;
        }

        public static CtpCommandKeyword ReadCommandKeyword(byte[] m_buffer, ref int m_position, int m_length)
        {
            if (m_position + 1 > m_length)
            {
                throw new EndOfStreamException();
            }
            if (m_position + 1 + m_buffer[m_position] > m_length)
            {
                throw new EndOfStreamException();
            }
            var rv = CtpCommandKeyword.Lookup(m_buffer, m_position);
            m_position += m_buffer[m_position] + 1;
            return rv;
        }

        public static uint ReadBits16(byte[] m_buffer, ref int m_position, int m_length)
        {
            if (m_position + 2 > m_length)
            {
                throw new EndOfStreamException();
            }
            uint rv = (uint)m_buffer[m_position] << 8
                      | (uint)m_buffer[m_position + 1];
            m_position += 2;
            return rv;
        }

    }
}
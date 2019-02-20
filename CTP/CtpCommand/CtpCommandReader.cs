using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using GSF;

namespace CTP
{
    /// <summary>
    /// A class for reading <see cref="CtpCommand"/>s.
    /// </summary>
    internal class CtpCommandReader
    {
        private class StackDefinition
        {
            public CommandSchemaNode Node;
            public int ArrayCount;

            public bool IsArray => Node.Symbol == CommandSchemaSymbol.DefineArray;

            public StackDefinition(CommandSchemaNode node, int arrayCount)
            {
                Node = node;
                ArrayCount = arrayCount;
            }
        }

        private CtpCommandSchema m_schema;
        /// <summary>
        /// The stream for reading the byte array.
        /// </summary>
        private CtpObjectReader m_stream;
        /// <summary>
        /// The list of elements so the <see cref="ElementName"/> can be retrieved.
        /// </summary>
        private Stack<StackDefinition> m_elementStack = new Stack<StackDefinition>();

        public readonly string RootElement;

        /// <summary>
        /// Creates a <see cref="CtpCommandReader"/> from the specified byte array.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="data"></param>
        public CtpCommandReader(CtpCommandSchema schema, byte[] data)
        {
            m_schema = schema;
            Value = CtpObject.Null;
            NodeType = CtpCommandNodeType.StartOfCommand;
            RootElement = m_schema[0].NodeName;
            m_elementStack.Push(new StackDefinition(m_schema[0], 0));
            m_stream = new CtpObjectReader(data);
            m_currentSchemaIndex = 0;
        }

        /// <summary>
        /// The name of the root element.
        /// </summary>

        /// <summary>
        /// The current name of the current element. Can be the RootElement if ElementDepth is 0
        /// and <see cref="NodeType"/> is not <see cref="CtpCommandNodeType.EndElement"/>.
        /// In this event, the ElementName does not change and refers to the element that has just ended.
        /// </summary>
        public string ElementName
        {
            get
            {
                if (m_elementStack.Count == 0)
                    return RootElement;
                return m_elementStack.Peek().Node.NodeName;
            }
        }

        /// <summary>
        /// If <see cref="NodeType"/> is <see cref="CtpCommandNodeType.Value"/>, the name of the value. Otherwise, null.
        /// </summary>
        public string ValueName { get; private set; }

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

        public bool IsArray
        {
            get
            {
                if (m_elementStack.Count == 0)
                    return false;
                return m_elementStack.Peek().Node.Symbol == CommandSchemaSymbol.DefineArray;
            }
        }

        private int m_currentSchemaIndex;

        /// <summary>
        /// Reads to the next node. If the next node is the end of the document. False is returned. Otherwise true.
        /// </summary>
        /// <returns></returns>
        public bool Read()
        {
            if (NodeType == CtpCommandNodeType.EndOfCommand)
                return false;

            if (NodeType == CtpCommandNodeType.StartOfCommand)
            {
                NodeType = CtpCommandNodeType.StartElement;
                return true;
            }

            if (m_elementStack.Count == 0)
            {
                NodeType = CtpCommandNodeType.EndOfCommand;
                return false;
            }

            var currentElement = m_elementStack.Peek();
            if (currentElement.IsArray)
            {
                if (currentElement.ArrayCount == 0)
                {
                    m_elementStack.Pop();
                    NodeType = CtpCommandNodeType.EndElement;
                    ValueName = null;
                    Value = CtpObject.Null;
                    return true;
                }
                else
                {
                    m_currentSchemaIndex = currentElement.Node.PositionIndex;
                    currentElement.ArrayCount--;
                }
            }


            m_currentSchemaIndex++;
            var node = m_schema[m_currentSchemaIndex];

            switch (node.Symbol)
            {
                case CommandSchemaSymbol.DefineArray:
                    NodeType = CtpCommandNodeType.StartElement;
                    Value = CtpObject.Null;
                    ValueName = null;
                    m_elementStack.Push(new StackDefinition(node, (int)m_stream.Read()));
                    break;
                case CommandSchemaSymbol.DefineElement:
                    NodeType = CtpCommandNodeType.StartElement;
                    Value = CtpObject.Null;
                    ValueName = null;
                    m_elementStack.Push(new StackDefinition(node, 0));
                    break;
                case CommandSchemaSymbol.DefineValue:
                    NodeType = CtpCommandNodeType.Value;
                    ValueName = node.NodeName;
                    Value = m_stream.Read();
                    break;
                case CommandSchemaSymbol.EndElement:
                    m_elementStack.Pop();
                    NodeType = CtpCommandNodeType.EndElement;
                    ValueName = null;
                    Value = CtpObject.Null;
                    return true;
                case CommandSchemaSymbol.EndOFStream:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return true;
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

        public override string ToString()
        {
            switch (NodeType)
            {
                case CtpCommandNodeType.StartElement:
                    return $"+{ElementName}";
                case CtpCommandNodeType.Value:
                    return $" {ElementName}/{ValueName} = {Value}";
                case CtpCommandNodeType.EndElement:
                    return $"-{ElementName}";
                case CtpCommandNodeType.EndOfCommand:
                    return $"End Command {RootElement}";
                case CtpCommandNodeType.StartOfCommand:
                    return $"Start Command {RootElement}";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
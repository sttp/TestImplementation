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
        private CtpCommandSchema m_schema;
        /// <summary>
        /// The stream for reading the byte array.
        /// </summary>
        private CtpObjectReader m_stream;

        private CommandSchemaNode m_currentNode;

        private Stack<int> m_arrayCountStack = new Stack<int>();

        /// <summary>
        /// The name of the root element.
        /// </summary>
        public readonly string RootElement;

        /// <summary>
        /// If <see cref="NodeType"/> is <see cref="CtpCommandNodeType.Value"/>, the name of the value. Otherwise, null.
        /// </summary>
        public string ValueName { get; private set; }

        /// <summary>
        /// If <see cref="NodeType"/> is <see cref="CtpCommandNodeType.Value"/>, the value. Otherwise, CtpObject.Null.
        /// </summary>
        public CtpObject Value { get; private set; }

        /// <summary>
        /// The type of the current node. To Advance the nodes call <see cref="Read"/>
        /// </summary>
        public CtpCommandNodeType NodeType { get; private set; }

        public bool IsElementOrArrayNull { get; private set; }

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
            m_stream = new CtpObjectReader(data);
        }

        /// <summary>
        /// The current name of the current element. Can be the RootElement if ElementDepth is 0
        /// and <see cref="NodeType"/> is not <see cref="CtpCommandNodeType.EndElement"/>.
        /// In this event, the ElementName does not change and refers to the element that has just ended.
        /// </summary>
        public string ElementName
        {
            get
            {
                if (m_currentNode == null)
                {
                    return RootElement;
                }

                switch (m_currentNode.Symbol)
                {
                    case CommandSchemaSymbol.DefineArray:
                    case CommandSchemaSymbol.DefineElement:
                    case CommandSchemaSymbol.EndElement:
                    case CommandSchemaSymbol.EndArray:
                        return m_currentNode.NodeName;
                    case CommandSchemaSymbol.DefineValue:
                        return m_currentNode.ParentNode.NodeName;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// The current name of the current element. Can be the RootElement if ElementDepth is 0
        /// and <see cref="NodeType"/> is not <see cref="CtpCommandNodeType.EndElement"/>.
        /// In this event, the ElementName does not change and refers to the element that has just ended.
        /// </summary>
        public bool IsArray
        {
            get
            {
                if (m_currentNode == null)
                    return false;
                return m_currentNode.Symbol == CommandSchemaSymbol.DefineArray;
            }
        }

        /// <summary>
        /// Reads to the next node. If the next node is the end of the document. False is returned. Otherwise true.
        /// </summary>
        /// <returns></returns>
        public bool Read()
        {
            TryAgain:

            if (NodeType == CtpCommandNodeType.EndOfCommand)
                return false;


            if (m_currentNode == null)
            {
                m_currentNode = m_schema[0];
            }
            else
            {
                if (IsElementOrArrayNull)
                {
                    IsElementOrArrayNull = false;
                    m_currentNode = m_currentNode.PairedNode;
                }
                else
                {
                    m_currentNode = m_currentNode.NextNode;
                }
            }

            if (m_currentNode == null)
            {
                NodeType = CtpCommandNodeType.EndOfCommand;
                return false;
            }

            switch (m_currentNode.Symbol)
            {
                case CommandSchemaSymbol.DefineArray:
                    NodeType = CtpCommandNodeType.StartArray;
                    Value = CtpObject.Null;
                    ValueName = null;
                    CtpObject count = m_stream.Read();
                    if (count.IsNull)
                    {
                        IsElementOrArrayNull = true;
                        m_arrayCountStack.Push(0);
                    }
                    else
                    {
                        IsElementOrArrayNull = false;
                        m_arrayCountStack.Push((int)count);
                    }
                    break;
                case CommandSchemaSymbol.DefineElement:
                    NodeType = CtpCommandNodeType.StartElement;
                    Value = CtpObject.Null;
                    ValueName = null;
                    IsElementOrArrayNull = !m_stream.Read().IsBoolean;
                    break;
                case CommandSchemaSymbol.DefineValue:
                    NodeType = CtpCommandNodeType.Value;
                    ValueName = m_currentNode.NodeName;
                    Value = m_stream.Read();
                    break;
                case CommandSchemaSymbol.EndElement:
                    NodeType = CtpCommandNodeType.EndElement;
                    ValueName = null;
                    Value = CtpObject.Null;
                    return true;
                case CommandSchemaSymbol.EndOFStream:
                    break;
                case CommandSchemaSymbol.EndArray:
                    var arrayCount = m_arrayCountStack.Pop() - 1;
                    if (arrayCount <= 0)
                    {
                        NodeType = CtpCommandNodeType.EndArray;
                        ValueName = null;
                        Value = CtpObject.Null;
                        return true;
                    }
                    else
                    {
                        m_arrayCountStack.Push(arrayCount);
                        m_currentNode = m_currentNode.PairedNode;
                        goto TryAgain;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return true;
        }

        public void SkipElement()
        {
            var endElement = m_currentNode.PairedNode;
            while (Read())
            {
                if (m_currentNode == endElement)
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
                case CtpCommandNodeType.StartArray:
                    return $"+{ElementName}[]";
                case CtpCommandNodeType.EndArray:
                    return $"-{ElementName}[]";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
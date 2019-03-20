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
        private bool m_isElementOrArrayNull;
        private CtpObject m_value;
        private bool m_initialized;
        private bool m_completed;

        /// <summary>
        /// The type of the current node. To Advance the nodes call <see cref="Read"/>
        /// </summary>
        public CommandSchemaSymbol NodeType { get; private set; }

        /// <summary>
        /// Creates a <see cref="CtpCommandReader"/> from the specified byte array.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="data"></param>
        public CtpCommandReader(CtpCommandSchema schema, byte[] data)
        {
            m_schema = schema;
            NodeType = (CommandSchemaSymbol)10;
            m_stream = new CtpObjectReader(data);
        }

        /// <summary>
        /// The name of the root element.
        /// </summary>
        public string CommandName => m_schema[0].NodeName;

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
                    return CommandName;
                }

                switch (m_currentNode.Symbol)
                {
                    case CommandSchemaSymbol.StartArray:
                    case CommandSchemaSymbol.StartElement:
                    case CommandSchemaSymbol.EndElement:
                    case CommandSchemaSymbol.EndArray:
                        return m_currentNode.NodeName;
                    case CommandSchemaSymbol.Value:
                        return m_currentNode.ParentNode.NodeName;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// If <see cref="NodeType"/> is <see cref="CtpCommandNodeType.Value"/>, the name of the value. Otherwise, null.
        /// </summary>
        public string ValueName
        {
            get
            {
                if (m_currentNode == null || m_currentNode.Symbol != CommandSchemaSymbol.Value)
                {
                    return null;
                }
                return m_currentNode.NodeName;
            }
        }

        /// <summary>
        /// If <see cref="NodeType"/> is <see cref="CtpCommandNodeType.Value"/>, the value. Otherwise, CtpObject.Null.
        /// </summary>
        public CtpObject Value
        {
            get
            {
                if (m_currentNode == null || m_currentNode.Symbol != CommandSchemaSymbol.Value)
                {
                    return CtpObject.Null;
                }
                return m_value;
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
                return m_currentNode.Symbol == CommandSchemaSymbol.StartArray;
            }
        }

        public bool IsElementOrArrayNull
        {
            get
            {
                if (m_currentNode != null
                    && (m_currentNode.Symbol != CommandSchemaSymbol.StartElement
                     || m_currentNode.Symbol != CommandSchemaSymbol.StartArray))
                {
                    return m_isElementOrArrayNull;
                }
                return false;
            }
        }

        /// <summary>
        /// Reads to the next node. If the next node is the end of the document. False is returned. Otherwise true.
        /// </summary>
        /// <returns></returns>
        public bool Read()
        {
            TryAgain:

            if (m_completed)
                return false;

            if (!m_initialized)
            {
                m_initialized = true;
                m_currentNode = m_schema[0];
            }
            else
            {
                if (IsElementOrArrayNull)
                {
                    m_isElementOrArrayNull = false;
                    m_currentNode = m_currentNode.PairedNode;
                }
                else
                {
                    m_currentNode = m_currentNode.NextNode;
                }
            }

            if (m_currentNode == null)
            {
                m_completed = true;
                return false;
            }

            switch (m_currentNode.Symbol)
            {
                case CommandSchemaSymbol.StartArray:
                    NodeType = CommandSchemaSymbol.StartArray;
                    CtpObject count = m_stream.Read();
                    if (count.IsNull)
                    {
                        m_isElementOrArrayNull = true;
                        m_arrayCountStack.Push(0);
                    }
                    else
                    {
                        m_isElementOrArrayNull = false;
                        m_arrayCountStack.Push((int)count);
                    }
                    break;
                case CommandSchemaSymbol.StartElement:
                    NodeType = CommandSchemaSymbol.StartElement;
                    m_isElementOrArrayNull = !m_stream.Read().IsBoolean;
                    break;
                case CommandSchemaSymbol.Value:
                    NodeType = CommandSchemaSymbol.Value;
                    m_value = m_stream.Read();
                    break;
                case CommandSchemaSymbol.EndElement:
                    NodeType = CommandSchemaSymbol.EndElement;
                    return true;
                case CommandSchemaSymbol.EndArray:
                    var arrayCount = m_arrayCountStack.Pop() - 1;
                    if (arrayCount <= 0)
                    {
                        NodeType = CommandSchemaSymbol.EndArray;
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
                case CommandSchemaSymbol.StartElement:
                    return $"+{ElementName}";
                case CommandSchemaSymbol.Value:
                    return $" {ElementName}/{ValueName} = {Value}";
                case CommandSchemaSymbol.EndElement:
                    return $"-{ElementName}";
                case CommandSchemaSymbol.StartArray:
                    return $"+{ElementName}[]";
                case CommandSchemaSymbol.EndArray:
                    return $"-{ElementName}[]";
                default:
                    {
                        if (!m_initialized)
                        {
                            return $"Start Command {CommandName}";
                        }
                        else if (m_completed)
                        {
                            return $"End Command {CommandName}";
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                    }
            }
        }
    }
}
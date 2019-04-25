using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTP.Collection;
using CTP.IO;

namespace CTP
{
    public class CtpCommandSchema : ICollection<CommandSchemaNode>
    {
        /// <summary>
        /// Schemas are provided a runtime id if they are saved from <see cref="CommandObject"/>. When they are loaded or serialized in some way, this ID is missing.
        /// Internally this value is used to determine if a schema can be negotiated once and transmitted without including the schema data. If present,
        /// this will uniquely define a schema.
        /// </summary>
        public readonly int? ProcessRuntimeID;
        /// <summary>
        /// The case sensitive name of the command associated with this schema.
        /// </summary>
        public readonly string CommandName;
        private readonly byte[] m_data;
        private List<CommandSchemaNode> m_nodes = new List<CommandSchemaNode>();

        public CtpCommandSchema(byte[] data)
          : this(data, null)
        {

        }

        /// <summary>
        /// Do not call from user code. Protecting this 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="processRuntimeID"></param>
        internal CtpCommandSchema(byte[] data, int? processRuntimeID)
        {
            m_data = data ?? throw new ArgumentNullException(nameof(data));
            ProcessRuntimeID = processRuntimeID;
            Stack<CommandSchemaNode> stack = new Stack<CommandSchemaNode>();

            var reader = new CtpObjectReader(data);
            while (!reader.IsEmpty)
            {
                var symbol = (CommandSchemaSymbol)(byte)reader.Read();
                switch (symbol)
                {
                    case CommandSchemaSymbol.StartArray:
                    case CommandSchemaSymbol.StartElement:
                    case CommandSchemaSymbol.Value:
                        m_nodes.Add(new CommandSchemaNode((string)reader.Read(), symbol));
                        break;
                    case CommandSchemaSymbol.EndArray:
                    case CommandSchemaSymbol.EndElement:
                        m_nodes.Add(new CommandSchemaNode(null, symbol));
                        break;
                    default:
                        throw new Exception("Wrong version number");
                }
            }

            for (int x = 0; x < m_nodes.Count - 1; x++)
            {
                m_nodes[x].SetNextNode(m_nodes[x + 1]);
            }

            for (int x = 0; x < m_nodes.Count; x++)
            {
                CommandSchemaNode node = m_nodes[x];
                switch (node.Symbol)
                {
                    case CommandSchemaSymbol.StartArray:
                    case CommandSchemaSymbol.StartElement:
                        {
                            if (x > 0)
                                node.SetParentNode(stack.Peek());
                            stack.Push(node);
                            break;
                        }
                    case CommandSchemaSymbol.EndArray when stack.Peek().Symbol == CommandSchemaSymbol.StartArray:
                    case CommandSchemaSymbol.EndElement when stack.Peek().Symbol == CommandSchemaSymbol.StartElement:
                        {
                            var startNode = stack.Pop();
                            startNode.SetPairedNode(node);
                            node.SetPairedNode(startNode);
                            node.SetNodeName(startNode.NodeName);
                            if (x + 1 < m_nodes.Count)
                            {
                                node.SetParentNode(stack.Peek());
                            }
                            break;
                        }
                    case CommandSchemaSymbol.Value:
                        node.SetParentNode(stack.Peek());
                        break;
                    default:
                        throw new Exception("Malformed Schema");
                }
            }
            if (stack.Count != 0)
                throw new Exception("Malformed Schema");

            for (int x = 0; x < m_nodes.Count; x++)
            {
                m_nodes[x].SetReadOnly();
            }

            CommandName = m_nodes[0].NodeName;
        }

        public CommandSchemaNode this[int index] => m_nodes[index];

        public int NodeCount => m_nodes.Count;

        public int DataLength => m_data.Length;

        public override string ToString()
        {
            var sb = new StringBuilder();
            var prefix = "";
            for (int x = 0; x < NodeCount; x++)
            {
                switch (m_nodes[x].Symbol)
                {
                    case CommandSchemaSymbol.StartArray:
                        sb.Append(prefix);
                        sb.AppendLine(m_nodes[x].NodeName + "[]");
                        prefix += " ";
                        break;
                    case CommandSchemaSymbol.StartElement:
                        sb.Append(prefix);
                        sb.AppendLine("Element: " + m_nodes[x].NodeName);
                        prefix += "|";
                        break;
                    case CommandSchemaSymbol.Value:
                        sb.Append(prefix);
                        sb.AppendLine(m_nodes[x].NodeName);
                        break;
                    case CommandSchemaSymbol.EndElement:
                        prefix = prefix.Substring(0, prefix.Length - 1);
                        sb.Append(prefix);
                        sb.AppendLine("-" + m_nodes[x].NodeName);
                        break;
                    case CommandSchemaSymbol.EndArray:
                        prefix = prefix.Substring(0, prefix.Length - 1);
                        sb.Append(prefix);
                        sb.AppendLine("-" + m_nodes[x].NodeName);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return sb.ToString();
        }

        public PooledBuffer ToCommand(int schemaRuntimeID)
        {
            return PacketMethods.CreatePacket(PacketContents.CommandSchema, schemaRuntimeID, m_data);
        }

        public void CopyTo(byte[] data, int offset)
        {
            m_data.CopyTo(data, offset);
        }

        public IEnumerator<CommandSchemaNode> GetEnumerator()
        {
            return m_nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection<CommandSchemaNode>.Add(CommandSchemaNode item)
        {
            throw new NotSupportedException();
        }

        void ICollection<CommandSchemaNode>.Clear()
        {
            throw new NotSupportedException();
        }

        bool ICollection<CommandSchemaNode>.Contains(CommandSchemaNode item)
        {
            return m_nodes.Contains(item);
        }

        void ICollection<CommandSchemaNode>.CopyTo(CommandSchemaNode[] array, int arrayIndex)
        {
            m_nodes.CopyTo(array, arrayIndex);
        }

        bool ICollection<CommandSchemaNode>.Remove(CommandSchemaNode item)
        {
            throw new NotSupportedException();
        }

        int ICollection<CommandSchemaNode>.Count => m_nodes.Count;

        bool ICollection<CommandSchemaNode>.IsReadOnly => true;
    }


}

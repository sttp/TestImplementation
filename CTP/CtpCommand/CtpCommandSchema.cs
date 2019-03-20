using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CTP.Collection;
using CTP.IO;

namespace CTP
{
    public enum CommandSchemaSymbol
    {
        StartArray,
        StartElement,
        Value,
        EndElement,
        EndArray
    }

    internal class CommandSchemaWriter
    {
        private static int ProcessRuntimeID;

        private CtpObjectWriter m_stream;

        public CommandSchemaWriter()
        {
            m_stream = new CtpObjectWriter();
        }

        public void DefineArray(string name)
        {
            m_stream.Write((byte)CommandSchemaSymbol.StartArray);
            m_stream.Write(name);
        }

        public void DefineElement(string name)
        {
            m_stream.Write((byte)CommandSchemaSymbol.StartElement);
            m_stream.Write(name);
        }

        public void EndElement()
        {
            m_stream.Write((byte)CommandSchemaSymbol.EndElement);
        }

        public void EndArray()
        {
            m_stream.Write((byte)CommandSchemaSymbol.EndArray);
        }

        public void DefineValue(string name)
        {
            m_stream.Write((byte)CommandSchemaSymbol.Value);
            m_stream.Write(name);
        }

        public CtpCommandSchema ToSchema()
        {
            if (m_stream == null)
                throw new Exception("Duplicate calls are not supported.");
            var data = m_stream.ToArray();
            m_stream = null;
            return new CtpCommandSchema(data, Interlocked.Increment(ref ProcessRuntimeID));
        }
    }
    
   

    public class CommandSchemaNode
    {
        public readonly CommandSchemaSymbol Symbol;
        public readonly string NodeName;
        /// <summary>
        /// This is the element or array that this node belongs to.
        /// </summary>
        public CommandSchemaNode ParentNode { get; private set; }
        /// <summary>
        /// This is the node that is next in sequence as defined by the CommandSchema.
        /// Null at the end.
        /// </summary>
        public CommandSchemaNode NextNode { get; private set; }
        /// <summary>
        /// This is the other node for Array's or Elements. For Define, it's End, for End, it's Define. For values, it's null
        /// </summary>
        public CommandSchemaNode PairedNode { get; private set; }

        private bool m_isReadOnly;

        public CommandSchemaNode(string nodeName, CommandSchemaSymbol symbol)
        {
            NodeName = nodeName;
            Symbol = symbol;
        }

        public void SetNextNode(CommandSchemaNode node)
        {
            if (m_isReadOnly)
                throw new InvalidOperationException("Node already marked as read only");
            NextNode = node;
        }

        public void SetPairedNode(CommandSchemaNode node)
        {
            if (m_isReadOnly)
                throw new InvalidOperationException("Node already marked as read only");
            PairedNode = node;
        }

        public void SetParentNode(CommandSchemaNode node)
        {
            if (m_isReadOnly)
                throw new InvalidOperationException("Node already marked as read only");
            ParentNode = node;
        }

        internal void SetReadOnly()
        {
            m_isReadOnly = true;
        }

        public override string ToString()
        {
            return $"{Symbol} {NodeName}";
        }
    }

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
                        m_nodes.Add(new CommandSchemaNode(string.Empty, symbol));
                        break;
                    default:
                        throw new Exception("Wrong version number");
                }
            }
           

            for (int x = 0; x < m_nodes.Count - 1; x++)
            {
                m_nodes[x].SetNextNode(m_nodes[x + 1]);
            }

            Stack<CommandSchemaNode> stack = new Stack<CommandSchemaNode>();

            for (int x = 0; x < m_nodes.Count; x++)
            {
                CommandSchemaNode node = m_nodes[x];
                if (node.Symbol == CommandSchemaSymbol.StartArray || node.Symbol == CommandSchemaSymbol.StartElement)
                {
                    if (x > 0)
                        node.SetParentNode(stack.Peek());
                    stack.Push(node);
                }
                else if (node.Symbol == CommandSchemaSymbol.EndArray || node.Symbol == CommandSchemaSymbol.EndElement)
                {
                    var n = stack.Pop();
                    if (node.Symbol == CommandSchemaSymbol.EndArray && n.Symbol == CommandSchemaSymbol.StartArray 
                        || node.Symbol == CommandSchemaSymbol.EndElement && n.Symbol == CommandSchemaSymbol.StartElement)
                    {
                        n.SetPairedNode(node);
                        node.SetPairedNode(n);
                        if (x + 1 < m_nodes.Count)
                        {
                            node.SetParentNode(stack.Peek());
                        }
                    }
                    else
                    {
                        throw new Exception("Malformed Schema");
                    }
                }
                else
                {
                    node.SetParentNode(stack.Peek());
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

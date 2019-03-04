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
            m_stream.Write((byte)CommandSchemaSymbol.DefineArray);
            m_stream.Write(name);
        }

        public void DefineElement(string name)
        {
            m_stream.Write((byte)CommandSchemaSymbol.DefineElement);
            m_stream.Write(name);
        }

        public void EndElement()
        {
            m_stream.Write((byte)CommandSchemaSymbol.EndElement);
        }

        public void DefineValue(string name)
        {
            m_stream.Write((byte)CommandSchemaSymbol.DefineValue);
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

    public enum CommandSchemaSymbol
    {
        DefineArray,
        DefineElement,
        DefineValue,
        EndElement,
        EndOFStream
    }

    internal class CommandSchemaReader
    {
        private CtpObjectReader m_stream;

        public CommandSchemaSymbol Symbol;

        public string Name;

        public CommandSchemaReader(byte[] data)
        {
            m_stream = new CtpObjectReader(data);
        }

        public bool Next()
        {
            if (m_stream.IsEmpty)
            {
                Symbol = CommandSchemaSymbol.EndOFStream;
                Name = null;
                return false;
            }

            Symbol = (CommandSchemaSymbol)(byte)m_stream.Read();
            switch (Symbol)
            {
                case CommandSchemaSymbol.DefineArray:
                case CommandSchemaSymbol.DefineElement:
                case CommandSchemaSymbol.DefineValue:
                    Name = (string)m_stream.Read();
                    return true;
                case CommandSchemaSymbol.EndElement:
                    Name = string.Empty;
                    return true;
                default:
                    throw new Exception("Wrong version number");
            }
        }
    }

    public class CommandSchemaNode
    {
        public readonly CommandSchemaSymbol Symbol;
        public readonly string NodeName;
        public readonly int PositionIndex;

        public CommandSchemaNode(string nodeName, CommandSchemaSymbol symbol, int positionIndex)
        {
            PositionIndex = positionIndex;
            NodeName = nodeName;
            Symbol = symbol;
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
            var reader = new CommandSchemaReader(m_data);
            ProcessRuntimeID = processRuntimeID;
            while (reader.Next())
            {
                m_nodes.Add(new CommandSchemaNode(reader.Name, reader.Symbol, m_nodes.Count));
            }
            CommandName = m_nodes[0].NodeName;
        }

        public CommandSchemaNode this[int index]
        {
            get
            {
                if (index >= m_nodes.Count)
                    return null;
                return m_nodes[index];
            }
        }

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

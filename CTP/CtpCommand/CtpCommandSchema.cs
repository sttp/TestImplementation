using System;
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

    internal class CommandSchemaNode
    {
        public readonly CommandSchemaSymbol Symbol;
        public readonly string NodeName;
        public readonly int PositionIndex;

        public CommandSchemaNode(CommandSchemaReader reader, int positionIndex)
        {
            PositionIndex = positionIndex;
            NodeName = reader.Name;
            Symbol = reader.Symbol;
        }

        public override string ToString()
        {
            return $"{Symbol} {NodeName}";
        }
    }

    public class CtpCommandSchema
    {
        public readonly int? ProcessRuntimeID;
        public readonly string RootElement;

        private readonly byte[] m_data;
        private List<CommandSchemaNode> m_nodes = new List<CommandSchemaNode>();

        public CtpCommandSchema(byte[] data)
          : this(data, null)
        {
        }

        internal CtpCommandSchema(byte[] data, int? processRuntimeID)
        {
            m_data = data ?? throw new ArgumentNullException(nameof(data));
            var reader = new CommandSchemaReader(m_data);
            ProcessRuntimeID = processRuntimeID;
            while (reader.Next())
            {
                m_nodes.Add(new CommandSchemaNode(reader, m_nodes.Count));
            }
            RootElement = m_nodes[0].NodeName;
        }

        internal CommandSchemaNode this[int index]
        {
            get
            {
                if (index >= m_nodes.Count)
                    return null;
                return m_nodes[index];
            }
        }

        internal int NodeCount => m_nodes.Count;

        public int DataLength => m_data.Length;

        public PooledBuffer ToCommand(int schemaRuntimeID)
        {
            return PacketMethods.CreatePacket(PacketContents.CommandSchema, schemaRuntimeID, m_data);
        }
       
        public void CopyTo(byte[] data, int offset)
        {
            m_data.CopyTo(data, offset);
        }
    }


}

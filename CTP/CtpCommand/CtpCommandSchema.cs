using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTP.IO;

namespace CTP
{
    internal class CommandSchemaWriter
    {
        private CtpObjectWriter m_stream;

        public CommandSchemaWriter()
        {
            m_stream = new CtpObjectWriter();
            m_stream.Write(Guid.NewGuid());
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
            return new CtpCommandSchema(m_stream.ToArray());
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
        public readonly Guid Identifier;

        public CommandSchemaSymbol Symbol;

        public string Name;

        public CommandSchemaReader(byte[] data)
        {
            m_stream = new CtpObjectReader(data);
            Identifier = m_stream.Read().AsGuid;
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

    public class CtpCommandSchema : IEquatable<CtpCommandSchema>
    {
        public readonly Guid Identifier;
        public readonly string RootElement;

        private readonly byte[] m_data;
        private List<CommandSchemaNode> m_nodes = new List<CommandSchemaNode>();

        public CtpCommandSchema(byte[] data)
        {
            m_data = data ?? throw new ArgumentNullException(nameof(data));
            var reader = new CommandSchemaReader(m_data);
            Identifier = reader.Identifier;
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

        public int Length => m_data.Length;

        public bool Equals(CtpCommandSchema other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (Identifier != other.Identifier)
                return false;
            if (m_data.Length != other.m_data.Length)
                return false;
            for (var x = 0; x < m_data.Length; x++)
            {
                if (m_data[x] != other.m_data[x])
                    return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((CtpCommandSchema)obj);
        }

        public override int GetHashCode()
        {
            return (Identifier.GetHashCode() * 397) ^ m_data.GetHashCode();
        }

        public static bool operator ==(CtpCommandSchema left, CtpCommandSchema right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CtpCommandSchema left, CtpCommandSchema right)
        {
            return !Equals(left, right);
        }

        public byte[] ToCommand(int schemaRuntimeID)
        {
            return PacketMethods.CreatePacket(PacketContents.CommandSchema, schemaRuntimeID, m_data);
        }

        public void CopyTo(byte[] data, int offset)
        {
            m_data.CopyTo(data, offset);
        }
    }


}

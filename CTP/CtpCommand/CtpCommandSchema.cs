using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP
{
    internal class CommandSchemaWriter
    {
        private CtpObjectWriter m_stream;

        public CommandSchemaWriter()
        {
            m_stream = new CtpObjectWriter();
        }

        public void DefineArray(string name)
        {
            m_stream.Write(0);
            m_stream.Write(name);
        }

        public void DefineElement(string name, int count)
        {
            m_stream.Write(1);
            m_stream.Write(name);
            m_stream.Write(count);
        }

        public void DefineValue(string name)
        {
            m_stream.Write(2);
            m_stream.Write(name);
        }

        public CtpCommandSchema ToSchema()
        {
            return new CtpCommandSchema(Guid.NewGuid(), m_stream.ToArray());
        }
    }

    public enum CommandSchemaSymbol
    {
        DefineArray,
        DefineElement,
        DefineValue,
        EndOFStream
    }

    internal class CommandSchemaReader
    {
        private CtpObjectReader m_stream;

        public CommandSchemaSymbol Symbol;

        public string Name;

        public int ElementCount;

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
                ElementCount = 0;
                return false;
            }

            switch ((int)m_stream.Read())
            {
                case 0:
                    Symbol = CommandSchemaSymbol.DefineArray;
                    Name = (string)m_stream.Read();
                    ElementCount = 0;
                    return true;
                case 1:
                    Symbol = CommandSchemaSymbol.DefineElement;
                    Name = (string)m_stream.Read();
                    ElementCount = (int)m_stream.Read();
                    return true;
                case 2:
                    Symbol = CommandSchemaSymbol.DefineValue;
                    Name = (string)m_stream.Read();
                    ElementCount = 0;
                    return true;
                default:
                    throw new Exception("Wrong version number");
            }
        }
    }

    internal class CommandSchemaCompiled
    {
        public class Node
        {
            public readonly CommandSchemaSymbol Symbol;
            public readonly CtpCommandKeyword NodeName;
            public readonly int ElementCount;
            public readonly int PositionIndex;

            public Node(CommandSchemaReader reader, int positionIndex)
            {
                PositionIndex = positionIndex;
                NodeName = CtpCommandKeyword.Create(reader.Name);
                Symbol = reader.Symbol;
                ElementCount = reader.ElementCount;
            }

            public override string ToString()
            {
                return $"{Symbol} {NodeName} {ElementCount}";
            }
        }

        private List<Node> m_nodes = new List<Node>();

        public CommandSchemaCompiled(CtpCommandSchema schema)
        {
            var reader = schema.CreateReader();
            while (reader.Next())
            {
                m_nodes.Add(new Node(reader, m_nodes.Count));
            }
        }

        public Node this[int index]
        {
            get
            {
                if (index >= m_nodes.Count)
                    return null;
                return m_nodes[index];
            }
        }
    }

    public class CtpCommandSchema : IEquatable<CtpCommandSchema>
    {
        public readonly Guid Identifier;
        private byte[] m_data;
        private CommandSchemaCompiled m_compiled;

        public CtpCommandSchema(Guid identifier, byte[] data)
        {
            Identifier = identifier;
            m_data = data ?? throw new ArgumentNullException(nameof(data));
        }

        internal CommandSchemaReader CreateReader()
        {
            return new CommandSchemaReader(m_data);
        }

        internal CommandSchemaCompiled CompiledReader()
        {
            if (m_compiled == null)
            {
                m_compiled = new CommandSchemaCompiled(this);
            }
            return m_compiled;
        }

        public bool Equals(CtpCommandSchema other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (Identifier!=other.Identifier)
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
    }


}

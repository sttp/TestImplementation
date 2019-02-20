using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using GSF;

namespace CTP
{
    /// <summary>
    /// A packet that can be serialized.
    /// </summary>
    public class CtpCommand : IEquatable<CtpCommand>
    {
        private CtpCommandSchema m_schema;
        private byte[] m_data;

        public CtpCommand(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            int schemaLength = BigEndian.ToUInt16(data, 0);
            byte[] schema = new byte[schemaLength];
            Array.Copy(data, 2, schema, 0, schemaLength);
            m_schema = new CtpCommandSchema(schema);
            m_data = new byte[data.Length - 2 - schemaLength];
            Array.Copy(data, data.Length - m_data.Length, m_data, 0, m_data.Length);
        }

        /// <summary>
        /// Creates an <see cref="CtpCommand"/> from a byte array. This method also validates the data.
        /// </summary>
        /// <param name="data">The data</param>
        /// <param name="schema"></param>
        public CtpCommand(CtpCommandSchema schema, byte[] data)
        {
            m_schema = schema ?? throw new ArgumentNullException(nameof(schema));
            m_data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public byte[] ToArray()
        {
            var rv = new byte[2 + m_schema.Length + m_data.Length];
            BigEndian.CopyBytes((ushort)m_schema.Length, rv, 0);
            m_schema.CopyTo(rv, 2);
            m_data.CopyTo(rv, 2 + m_schema.Length);
            return rv;
        }

        public Guid SchemaID => m_schema.Identifier;
        public string RootElement => m_schema.RootElement;
        public int DataLength => m_data.Length;

        /// <summary>
        /// Create a means for reading the data from the CtpDocument.
        /// </summary>
        /// <returns></returns>
        internal CtpCommandReader MakeReader()
        {
            if (m_schema == null)
                throw new InvalidOperationException("Cannot parse a command without it's schema");
            return new CtpCommandReader(m_schema, m_data);
        }

        internal CtpObjectReader MakeDataReader()
        {
            return new CtpObjectReader(m_data);
        }

        /// <summary>
        /// Converts CtpObject to a <see cref="CommandObject"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ToObject<T>()
            where T : CommandObject<T>
        {
            return CommandObject<T>.FromCommand(this);
        }

        /// <summary>
        /// Creates an XML string representation of this CtpDocument file.
        /// </summary>
        /// <returns></returns>
        public string ToXML()
        {
            var sb = new StringBuilder();
            var reader = MakeReader();

            var settings = new XmlWriterSettings();
            settings.Indent = true;
            var xml = XmlWriter.Create(sb, settings);

            xml.WriteStartElement(reader.RootElement);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case CtpCommandNodeType.StartElement:
                        xml.WriteStartElement(reader.ElementName);
                        break;
                    case CtpCommandNodeType.Value:
                        xml.WriteStartElement(reader.ValueName);
                        xml.WriteAttributeString("ValueType", reader.Value.ValueTypeCode.ToString());
                        var str = reader.Value.AsString ?? string.Empty;
                        str = str.Replace('\0', ' ');
                        xml.WriteValue(str);
                        xml.WriteEndElement();
                        break;
                    case CtpCommandNodeType.EndElement:
                        xml.WriteEndElement();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            xml.WriteEndElement();
            xml.Flush();
            return sb.ToString();
        }

        /// <summary>
        /// Creates a JSON string representation of this CtpDocument file.
        /// </summary>
        /// <returns></returns>
        public string ToJSON()
        {
            var sb = new StringBuilder();
            var reader = MakeReader();

            Stack<string> prefix = new Stack<string>();
            prefix.Push("  ");

            sb.Append('"');
            sb.Append(reader.RootElement);
            sb.AppendLine("\": {");

            //Note: There's an issue with a trailing commas.

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case CtpCommandNodeType.StartElement:
                        sb.Append(prefix.Peek());
                        sb.Append('"');
                        sb.Append(reader.ElementName);
                        sb.AppendLine("\": {");
                        prefix.Push(prefix.Peek() + "  ");
                        break;
                    case CtpCommandNodeType.Value:
                        sb.Append(prefix.Peek());
                        sb.Append('"');
                        sb.Append(reader.ValueName);
                        sb.Append("\": \"");
                        sb.Append(reader.Value.ToTypeString);
                        sb.AppendLine("\",");
                        break;
                    case CtpCommandNodeType.EndElement:
                        prefix.Pop();
                        sb.Append(prefix.Peek());
                        sb.AppendLine("},");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            sb.AppendLine("}");
            return sb.ToString();
        }

        /// <summary>
        /// Creates a YAML string representation of this CtpDocument file.
        /// </summary>
        /// <returns></returns>
        public string ToYAML()
        {
            var sb = new StringBuilder();

            var reader = MakeReader();
            reader.Read();
            Stack<string> prefix = new Stack<string>();
            prefix.Push(" ");
            sb.Append("---");
            sb.Append(reader.ElementName);
            sb.AppendLine();

            //Note: There's an issue with a trailing commas.

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case CtpCommandNodeType.StartElement:
                        sb.Append(prefix.Peek());
                        sb.Append(reader.ElementName);
                        sb.AppendLine(":");
                        prefix.Push(prefix.Peek() + " ");
                        break;
                    case CtpCommandNodeType.Value:
                        sb.Append(prefix.Peek());
                        sb.Append(reader.ValueName);
                        sb.Append(": ");
                        if (reader.Value.ValueTypeCode == CtpTypeCode.CtpCommand)
                        {
                            sb.AppendLine("(CtpCommand)");
                            string str = Environment.NewLine + prefix.Peek() + " ";
                            sb.Append(prefix.Peek() + " " + reader.Value.AsString.Replace(Environment.NewLine, str));
                        }
                        else
                        {
                            sb.Append(reader.Value.ToTypeString);
                        }
                        sb.AppendLine();
                        break;
                    case CtpCommandNodeType.EndElement:
                        prefix.Pop();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            sb.Length -= Environment.NewLine.Length;
            return sb.ToString();

        }

        public override string ToString()
        {
            return ToYAML();
        }

        /// <summary>
        /// Checks if the byte representation of two separate CtpCommand objects are the same. 
        /// Note: Due to reordering and encoding mechanics, it's possible for two records to be 
        /// externally the same, while internally they are not.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(CtpCommand other)
        {
            return this == other;
        }

        /// <summary>
        /// Checks if two objects are equal.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((CtpCommand)obj);
        }

        /// <summary>
        /// Computes a hashcode for this data.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashCode = 27;
            for (int x = 0; x < m_data.Length; x++)
            {
                hashCode = hashCode * 13 + m_data[x];
            }
            return hashCode;
        }

        /// <summary>
        /// Compares two object for equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(CtpCommand a, CtpCommand b)
        {
            if (ReferenceEquals(a, b))
                return true;
            if ((object)a == null || (object)b == null)
                return false;
            if (a.m_data.Length != b.m_data.Length)
                return false;
            for (int x = 0; x < a.m_data.Length; x++)
            {
                if (a.m_data[x] != b.m_data[x])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Compares two object for inequality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static bool operator !=(CtpCommand a, CtpCommand b)
        {
            return !Equals(a, b);
        }

        public byte[] ToCommandSchema(int schemaRuntimeID)
        {
            return m_schema.ToCommand(schemaRuntimeID);
        }

        public byte[] ToCommandData(int schemaRuntimeID)
        {
            return CtpObjectWriter.CreatePacket(PacketContents.CommandData, schemaRuntimeID, m_data);
        }
    }
}
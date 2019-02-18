using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using GSF;

namespace CTP
{
    public enum PacketContents
    {
        Raw,
        CommandSchema,
        CommandData,
    }

    /// <summary>
    /// A packet that can be serialized.
    /// </summary>
    public class CtpCommand : IEquatable<CtpCommand>
    {
        private CtpCommandSchema m_schema;
        private readonly byte[] m_data;
        private bool m_isRaw;
        private string m_commandName;
        private int m_length;
        private byte m_channelNumber;
        private int m_headerLength;

        /// <summary>
        /// Creates an <see cref="CtpCommand"/> from a byte array. This method also validates the data.
        /// </summary>
        /// <param name="data">The data</param>
        /// <param name="schema"></param>
        internal CtpCommand(byte[] data, CtpCommandSchema schema)
        {
            m_data = data;
            m_schema = schema;
        }

        private void ValidateData()
        {
            if (m_data.Length < 2)
                throw new Exception("Packet wrong size");
            if (m_data[0] >= 64)
                throw new Exception("Wrong Version");

            m_isRaw = (m_data[0] & 32) > 0;
            bool longPayload = (m_data[0] & 16) > 0;
            if (!longPayload)
            {
                m_headerLength = 2;
                m_length = BigEndian.ToInt16(m_data, 0) & ((1 << 12) - 1);
            }
            else
            {
                m_headerLength = 4;
                if (m_data.Length < 4)
                    throw new Exception("Packet wrong size");
                m_length = BigEndian.ToInt32(m_data, 0) & ((1 << 28) - 1);

            }
            if (m_data.Length < m_length)
                throw new Exception("Packet wrong size");

            if (m_isRaw)
            {
                m_channelNumber = m_data[m_headerLength];
                m_headerLength++;
            }
            else
            {
                m_channelNumber = 0;
            }
        }

        public string RootElement
        {
            get
            {
                if (m_commandName == null)
                {
                    if (m_isRaw)
                        m_commandName = "Raw";
                    else if (m_schema == null)
                    {
                        m_commandName = "(Undefined Schema)";
                    }
                    else
                    {
                        m_commandName = m_schema.CompiledReader()[0].NodeName.Value;
                    }
                }
                return m_commandName;
            }
        }

        /// <summary>
        /// Gets if this command is encoded as a raw payload.
        /// </summary>
        public bool IsRaw => m_isRaw;

        /// <summary>
        /// Gets the length of the entire command.
        /// </summary>
        public int Length => m_length;

        /// <summary>
        /// Create a means for reading the data from the CtpDocument.
        /// </summary>
        /// <returns></returns>
        internal CtpCommandReader MakeReader()
        {
            if (m_isRaw)
                throw new InvalidOperationException("A raw file cannot have a reader");
            if (m_schema == null)
                throw new InvalidOperationException("Cannot parse a command without it's schema");
            return new CtpCommandReader(m_data, m_headerLength, m_schema);
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
        /// Clones the internal data and returns it as an array.
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            return (byte[])m_data.Clone();
        }

        /// <summary>
        /// Copies the internal buffer to the provided byte array.
        /// Be sure to call <see cref="Length"/> to ensure that the destination buffer
        /// has enough space to receive the copy.
        /// </summary>
        /// <param name="buffer">the buffer to copy to.</param>
        /// <param name="offset">the offset position of <see pref="buffer"/></param>
        public void CopyTo(byte[] buffer, int offset)
        {
            Array.Copy(m_data, 0, buffer, offset, m_data.Length); // write data
        }

        public void CopyTo(Stream stream)
        {
            stream.Write(m_data, 0, m_data.Length);
        }

        /// <summary>
        /// Creates an XML string representation of this CtpDocument file.
        /// </summary>
        /// <returns></returns>
        public string ToXML()
        {
            var sb = new StringBuilder();

            if (m_isRaw)
            {
                //ToDo: Fix the XML serialization
                sb.AppendLine("---Raw");
                sb.AppendLine(" Channel: " + m_channelNumber);
                sb.Append(" Data: 0x");
                for (int x = m_headerLength; x < m_data.Length; x++)
                {
                    sb.Append(m_data[x].ToString("X2"));
                }
                sb.AppendLine();
                sb.Length -= Environment.NewLine.Length;
                return sb.ToString();
            }

            var reader = MakeReader();

            var settings = new XmlWriterSettings();
            settings.Indent = true;
            var xml = XmlWriter.Create(sb, settings);

            xml.WriteStartElement(reader.RootElement.Value);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case CtpCommandNodeType.StartElement:
                        xml.WriteStartElement(reader.ElementName.Value);
                        break;
                    case CtpCommandNodeType.Value:
                        xml.WriteStartElement(reader.ValueName.Value);
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

            if (m_isRaw)
            {
                //ToDo: Fix the JSON serialization
                sb.AppendLine("---Raw");
                sb.AppendLine(" Channel: " + m_channelNumber);
                sb.Append(" Data: 0x");
                for (int x = m_headerLength; x < m_data.Length; x++)
                {
                    sb.Append(m_data[x].ToString("X2"));
                }
                sb.AppendLine();
                sb.Length -= Environment.NewLine.Length;
                return sb.ToString();
            }

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

            if (m_isRaw)
            {
                sb.AppendLine("---Raw");
                sb.AppendLine(" Channel: " + m_channelNumber);
                sb.Append(" Data: 0x");
                for (int x = m_headerLength; x < m_data.Length; x++)
                {
                    sb.Append(m_data[x].ToString("X2"));
                }
                sb.AppendLine();
                sb.Length -= Environment.NewLine.Length;
                return sb.ToString();
            }

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
        /// Extracts a <see cref="CtpRaw"/> from this command. Only works if the command is flagged as a raw command.
        /// this method is called by the type casting library. The user should simply type cast this object.
        /// </summary>
        /// <returns></returns>
        internal CtpRaw ToCtpRaw()
        {
            if (!m_isRaw)
                throw new InvalidOperationException("Cannot convert a document to a CTPRaw with this method");
            byte[] data = new byte[m_length - m_headerLength];
            Array.Copy(m_data, m_headerLength, data, 0, data.Length);
            return new CtpRaw(data, m_channelNumber);
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

        /// <summary>
        /// Creates a <see cref="CtpCommand"/> from a byte array. This method also validates the data.
        /// </summary>
        public static CtpCommand Load(byte[] data, bool shouldCloneArray, CtpCommandSchema schema)
        {
            CtpCommand rv;
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (shouldCloneArray)
            {
                rv = new CtpCommand((byte[])data.Clone(), schema);
            }
            else
            {
                rv = new CtpCommand(data, schema);
            }
            rv.ValidateData();
            return rv;
        }

        /// <summary>
        /// Forms a <see cref="CtpCommand"/> object from a set of raw data.
        /// </summary>
        /// <param name="rawData">the raw data to serialize</param>
        /// <param name="channelNumber">the channel number to assign the raw data.</param>
        /// <returns></returns>
        public static CtpCommand CreateRaw(byte[] rawData, byte channelNumber)
        {
            byte[] data;
            if (rawData.Length >= 4093)
            {
                data = new byte[rawData.Length + 5];
                rawData.CopyTo(data, 5);
                data[4] = channelNumber;
                BigEndian.CopyBytes(rawData.Length + 5 + (1 << 28) + (1 << 29), data, 0);
            }
            else
            {
                data = new byte[rawData.Length + 3];
                rawData.CopyTo(data, 3);
                data[2] = channelNumber;
                BigEndian.CopyBytes((ushort)(rawData.Length + 3 + (1 << 13)), data, 0);
            }
            CtpCommand rv;
            rv = new CtpCommand(data, null);
            rv.ValidateData();
            return rv;
        }
    }
}
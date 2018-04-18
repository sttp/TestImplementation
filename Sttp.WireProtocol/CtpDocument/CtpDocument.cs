using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CTP
{
    /// <summary>
    /// A container class around the byte array containing the Document data. To read data from this class, call <see cref="MakeReader"/>.
    /// This class is Immutable.
    /// 
    /// To write an CtpDocument object use <see cref="CtpDocumentWriter"/>
    /// </summary>
    public class CtpDocument : IEquatable<CtpDocument>
    {
        private readonly byte[] m_data;

        /// <summary>
        /// Creates an CtpDocument from a byte array.
        /// </summary>
        /// <param name="data"></param>
        public CtpDocument(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            m_data = (byte[])data.Clone();
        }

        /// <summary>
        /// The size of the data block.
        /// </summary>
        public int Length => m_data.Length;

        //ToDo: These methods will eventually be removed. This is just to compare compression sizes of data.
        public int CompressedSize => DeflateHelper.Compress(m_data).Length;

        /// <summary>
        /// Create a means for reading the data from the CtpDocument.
        /// </summary>
        /// <returns></returns>
        public CtpDocumentReader MakeReader()
        {
            return new CtpDocumentReader(m_data);
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

        /// <summary>
        /// Creates an XML string representation of this CtpDocument file.
        /// </summary>
        /// <returns></returns>
        public string ToXML()
        {
            var reader = MakeReader();

            var sb = new StringBuilder();
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            var xml = XmlWriter.Create(sb, settings);

            xml.WriteStartElement(reader.RootElement);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case CtpDocumentNodeType.StartElement:
                        xml.WriteStartElement(reader.ElementName);
                        break;
                    case CtpDocumentNodeType.Value:
                        xml.WriteStartElement(reader.ValueName);
                        xml.WriteAttributeString("ValueType", reader.Value.ValueTypeCode.ToString());
                        xml.WriteValue(reader.Value.AsString ?? string.Empty);
                        xml.WriteEndElement();
                        break;
                    case CtpDocumentNodeType.EndElement:
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
            var reader = MakeReader();

            var sb = new StringBuilder();

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
                    case CtpDocumentNodeType.StartElement:
                        sb.Append(prefix.Peek());
                        sb.Append('"');
                        sb.Append(reader.ElementName);
                        sb.AppendLine("\": {");
                        prefix.Push(prefix.Peek() + "  ");
                        break;
                    case CtpDocumentNodeType.Value:
                        sb.Append(prefix.Peek());
                        sb.Append('"');
                        sb.Append(reader.ValueName);
                        sb.Append("\": \"");
                        sb.Append(reader.Value.ToTypeString);
                        sb.AppendLine("\",");
                        break;
                    case CtpDocumentNodeType.EndElement:
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
            var reader = MakeReader();

            var sb = new StringBuilder();

            Stack<string> prefix = new Stack<string>();
            prefix.Push(" ");

            sb.Append("---");
            sb.Append(reader.RootElement);
            sb.AppendLine();

            //Note: There's an issue with a trailing commas.

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case CtpDocumentNodeType.StartElement:
                        sb.Append(prefix.Peek());
                        sb.Append(reader.ElementName);
                        sb.AppendLine(":");
                        prefix.Push(prefix.Peek() + " ");
                        break;
                    case CtpDocumentNodeType.Value:
                        sb.Append(prefix.Peek());
                        sb.Append(reader.ValueName);
                        sb.Append(": ");
                        sb.Append(reader.Value.ToTypeString);
                        sb.AppendLine();
                        break;
                    case CtpDocumentNodeType.EndElement:
                        prefix.Pop();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Checks if the byte representation of two separate CtpDocument files are the same. 
        /// Note: Due to reordering and encoding mechanics, it's possible for two records to be 
        /// externally the same, while internally they are not.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(CtpDocument other)
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
            return Equals((CtpDocument)obj);
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
        public static bool operator ==(CtpDocument a, CtpDocument b)
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
        /// <
        public static bool operator !=(CtpDocument a, CtpDocument b)
        {
            return !Equals(a, b);
        }
    }


}

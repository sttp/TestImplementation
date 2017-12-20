using System;
using System.Collections.Generic;
using System.Linq;
using Sttp.Codec;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using OGE.Core.Aced;
using Sttp.Codec.CompressionLibraries;

namespace Sttp
{
    public class SttpMarkup : IEquatable<SttpMarkup>
    {
        private readonly byte[] m_data;
        public SttpMarkup(ByteReader rd)
        {
            m_data = rd.ReadBytes();
        }

        public SttpMarkup(byte[] data)
        {
            m_data = data;
        }

        public int EncodedSize => m_data.Length;
        public int CompressedSize => AcedDeflator.Instance.Compress(m_data, 0, m_data.Length, AcedCompressionLevel.Fastest, 0, 0).Length;
        public int CompressedSize2 => Sttp.Codec.CompressionLibraries.Ionic.Zlib.ZLibTools.Compress(m_data).Length;
        public int CompressedSize3 => LZ4.Compress(m_data).Length;
        public int Length => m_data.Length;

        public void Write(ByteWriter wr)
        {
            wr.Write(m_data);
        }

        public SttpMarkupReader MakeReader()
        {
            return new SttpMarkupReader(m_data);
        }

        public void CopyTo(byte[] buffer, int offset)
        {
            Array.Copy(m_data, 0, buffer, offset, m_data.Length); // write data
        }

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
                    case SttpMarkupNodeType.Element:
                        xml.WriteStartElement(reader.ElementName);
                        break;
                    case SttpMarkupNodeType.Value:
                        xml.WriteStartElement(reader.ValueName);
                        xml.WriteAttributeString("ValueType", reader.Value.ValueTypeCode.ToString());
                        xml.WriteValue(reader.Value.AsString ?? string.Empty);
                        xml.WriteEndElement();
                        break;
                    case SttpMarkupNodeType.EndElement:
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
                    case SttpMarkupNodeType.Element:
                        sb.Append(prefix.Peek());
                        sb.Append('"');
                        sb.Append(reader.ElementName);
                        sb.AppendLine("\": {");
                        prefix.Push(prefix.Peek() + "  ");
                        break;
                    case SttpMarkupNodeType.Value:
                        sb.Append(prefix.Peek());
                        sb.Append('"');
                        sb.Append(reader.ValueName);
                        sb.Append("\": \"");
                        sb.Append(reader.Value.ToTypeString);
                        sb.AppendLine("\",");
                        break;
                    case SttpMarkupNodeType.EndElement:
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
                    case SttpMarkupNodeType.Element:
                        sb.Append(prefix.Peek());
                        sb.Append(reader.ElementName);
                        sb.AppendLine(":");
                        prefix.Push(prefix.Peek() + " ");
                        break;
                    case SttpMarkupNodeType.Value:
                        sb.Append(prefix.Peek());
                        sb.Append(reader.ValueName);
                        sb.Append(": ");
                        sb.Append(reader.Value.ToTypeString);
                        sb.AppendLine();
                        break;
                    case SttpMarkupNodeType.EndElement:
                        prefix.Pop();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return sb.ToString();
        }

        public bool Equals(SttpMarkup other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return m_data.SequenceEqual(other.m_data);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((SttpMarkup)obj);
        }

        public override int GetHashCode()
        {
            return (m_data != null ? m_data.GetHashCode() : 0);
        }

        public static bool operator ==(SttpMarkup left, SttpMarkup right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SttpMarkup left, SttpMarkup right)
        {
            return !Equals(left, right);
        }
    }


}

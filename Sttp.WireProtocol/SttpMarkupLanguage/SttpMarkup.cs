using System;
using Sttp.Codec;
using System.Text;
using System.Xml;

namespace Sttp
{
    public class SttpMarkup
    {
        private byte[] m_data;
        public SttpMarkup(ByteReader rd)
        {
            m_data = rd.ReadBytes();
        }
        public SttpMarkup(byte[] data)
        {
            m_data = data;
        }
        public void Write(ByteWriter wr)
        {
            wr.Write(m_data);
        }

        public SttpMarkupReader MakeReader()
        {
            return new SttpMarkupReader(m_data);
        }

        public string ToXML()
        {
            var reader = MakeReader();

            var sb = new StringBuilder();
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            var xml = XmlWriter.Create(sb, settings);

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case SttpMarkupNodeType.Element:
                        xml.WriteStartElement(reader.ElementName);
                        xml.WriteAttributeString("Compatibility", reader.ElementCompatibility.ToString());
                        break;
                    case SttpMarkupNodeType.Value:
                        xml.WriteStartElement(reader.ValueName);
                        xml.WriteAttributeString("Compatibility", reader.ValueCompatibility.ToString());
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
            xml.Flush();
            return sb.ToString();
        }
    }


}

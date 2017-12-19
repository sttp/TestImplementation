using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Sttp.Codec.Metadata
{
    public class MetadataCommandBuilder
    {
        private SttpMarkupWriter m_stream;
        private CommandEncoder m_commandEncoder;
        private SessionDetails m_sessionDetails;

        public MetadataCommandBuilder(CommandEncoder commandEncoder, SessionDetails sessionDetails)
        {
            m_commandEncoder = commandEncoder;
            m_sessionDetails = sessionDetails;
            m_stream = new SttpMarkupWriter();
        }

        /// <summary>
        /// Begins a new metadata packet
        /// </summary>
        public void BeginCommand()
        {
            m_stream = new SttpMarkupWriter();
        }

        /// <summary>
        /// Ends a metadata packet and requests the buffer block that was allocated.
        /// </summary>
        /// <returns></returns>
        public void EndCommand()
        {
            if (m_stream.CurrentSize > 0)
            {
                m_commandEncoder.SendMarkupCommand("Metadata", m_stream);
                m_stream = new SttpMarkupWriter();
            }
        }

        public void DefineResponse(bool isUpdateQuery, long updatedFromRevision, Guid schemaVersion, long revision, string tableName, List<MetadataColumn> columns)
        {
            using (m_stream.StartElement("DefineResponse"))
            {
                m_stream.WriteValue("IsUpdateQuery", isUpdateQuery);
                m_stream.WriteValue("UpdatedFromRevision", updatedFromRevision);
                m_stream.WriteValue("SchemaVersion", schemaVersion);
                m_stream.WriteValue("Revision", revision);
                m_stream.WriteValue("TableName", tableName);
                using (m_stream.StartElement("Columns"))
                {
                    foreach (var c in columns)
                    {
                        using (m_stream.StartElement("Column"))
                        {
                            c.Save(m_stream);
                        }
                    }
                }
            }
        }

        public void DefineRow(SttpValue primaryKey, List<SttpValue> fields)
        {
            using (m_stream.StartElement("DefineRow"))
            {
                m_stream.WriteValue("PrimaryKey", primaryKey);
                using (m_stream.StartElement("Fields"))
                {
                    foreach (var c in fields)
                    {
                        m_stream.WriteValue("Field", c);
                    }
                }
            }
        }

        public void UndefineRow(SttpValue primaryKey)
        {
            using (m_stream.StartElement("UndefineRow"))
            {
                m_stream.WriteValue("PrimaryKey", primaryKey);
            }
        }

        public void Finished()
        {
            using (m_stream.StartElement("Finished"))
            {
            }
        }

    }
}

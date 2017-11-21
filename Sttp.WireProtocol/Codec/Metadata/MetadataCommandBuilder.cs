using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec.Metadata
{
    public class MetadataCommandBuilder 
    {
        private PayloadWriter m_stream;

        public MetadataCommandBuilder(CommandEncoder commandEncoder, SessionDetails sessionDetails)
        {
            m_stream = new PayloadWriter(sessionDetails, commandEncoder);
            m_stream.Clear();
        }

        /// <summary>
        /// Begins a new metadata packet
        /// </summary>
        public void BeginCommand()
        {
            m_stream.Clear();
        }

        /// <summary>
        /// Ends a metadata packet and requests the buffer block that was allocated.
        /// </summary>
        /// <returns></returns>
        public void EndCommand()
        {
            m_stream.Send(CommandCode.Metadata);
        }

        public void DefineResponse(bool isUpdateQuery, long updatedFromRevision, Guid schemaVersion, long revision, string tableName, List<Tuple<string, SttpValueTypeCode>> columns)
        {
            m_stream.Write(MetadataSubCommand.DefineResponse);
            m_stream.Write(isUpdateQuery);
            m_stream.Write(updatedFromRevision);
            m_stream.Write(schemaVersion);
            m_stream.Write(revision);
            m_stream.Write(tableName);
            m_stream.Write(columns);
        }

        public void DefineRow(SttpValue primaryKey, SttpValueSet fields)
        {
            m_stream.Write(MetadataSubCommand.DefineRow);
            m_stream.Write(primaryKey);
            m_stream.Write(fields);
        }

        public void UndefineRow(SttpValue primaryKey)
        {
            m_stream.Write(MetadataSubCommand.UndefineRow);
            m_stream.Write(primaryKey);
        }

        public void Finished()
        {
            m_stream.Write(MetadataSubCommand.Finished);
        }

    }
}

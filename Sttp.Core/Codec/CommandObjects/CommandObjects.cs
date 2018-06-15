using System;
using CTP;

namespace Sttp.Codec
{
    public class CommandObjects
    {
        private object m_decoder;

        public string CommandName { get; }
        private CtpDocument m_document;

        public CommandGetMetadataSchema GetMetadataSchema => m_decoder as CommandGetMetadataSchema;
        public CommandMetadataSchema MetadataSchema => m_decoder as CommandMetadataSchema;
        public CommandMetadataSchemaUpdate MetadataSchemaUpdate => m_decoder as CommandMetadataSchemaUpdate;
        public CommandMetadataSchemaVersion MetadataSchemaVersion => m_decoder as CommandMetadataSchemaVersion;

        public CommandGetMetadata GetMetadata => m_decoder as CommandGetMetadata;
        public CommandMetadataRequestFailed MetadataRequestFailed => m_decoder as CommandMetadataRequestFailed;
        public CommandBeginMetadataResponse BeginMetadataResponse => m_decoder as CommandBeginMetadataResponse;
        public CommandEndMetadataResponse EndMetadataResponse => m_decoder as CommandEndMetadataResponse;

        public CommandRaw Raw => m_decoder as CommandRaw;
        //public CommandUnknown Unknown => m_decoder as CommandUnknown;

        public string ToXMLString()
        {
            if (m_document == null)
                return "Null";

            return m_document.ToXML();
        }

        public CtpDocument Document => m_document;
    }
}
using System;
using CTP;

namespace CTP
{
    public class CommandObjects2
    {
        private object m_decoder;

        public CommandCode CommandCode { get; }
        public string CommandName { get; }
        private CtpDocument m_document;

        internal CommandObjects2(CtpDecoder decoder)
        {
            CommandCode = decoder.CommandCode;

            switch (decoder.CommandCode)
            {
                case CommandCode.Invalid:
                    throw new ArgumentOutOfRangeException("Command code of 0 is not permitted");
                case CommandCode.Document:
                    m_document = decoder.DocumentPayload;
                    CommandName = m_document.RootElement;
                    m_decoder = DocumentCommandBase.Create(CommandName, decoder.DocumentPayload);
                    break;
                case CommandCode.Binary:
                    m_document = null;
                    CommandName = "Raw";
                    m_decoder = new CommandRaw(decoder.BinaryChannelID, decoder.BinaryPayload);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public CommandRaw Raw => m_decoder as CommandRaw;
        public CommandUnknown Unknown => m_decoder as CommandUnknown;

        public string ToXMLString()
        {
            if (m_document == null)
                return "Null";

            return m_document.ToXML();
        }

        public CtpDocument Document => m_document;
    }
}
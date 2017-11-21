using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sttp.Codec.Metadata;

namespace Sttp.Codec
{
    public class CommandMetadata
    {
        private PayloadReader m_payload = new PayloadReader(new SessionDetails());
        private CmdFinished m_finished;
        private CmdDefineRow m_defineRow;
        private CmdDefineResponse m_defineResponse;
        private CmdUndefineRow m_undefineRow;

        public CommandMetadata()
        {
            m_finished = new CmdFinished();
            m_defineRow = new CmdDefineRow();
            m_defineResponse = new CmdDefineResponse();
            m_undefineRow = new CmdUndefineRow();
        }

        public CommandCode CommandCode => CommandCode.Metadata;

        public void Fill(PayloadReader reader)
        {
            m_payload = reader;
        }

        public MetadataSubCommandObjects NextCommand()
        {
            if (m_payload.Position == m_payload.Length)
                return null;

            switch (m_payload.Read<MetadataSubCommand>())
            {
                case MetadataSubCommand.Invalid:
                    throw new ArgumentOutOfRangeException("Invalid is not permitted");
                case MetadataSubCommand.DefineResponse:
                    m_defineResponse.Load(m_payload);
                    return new MetadataSubCommandObjects(MetadataSubCommand.DefineResponse,m_defineResponse);
                case MetadataSubCommand.DefineRow:
                    m_defineRow.Load(m_payload);
                    return new MetadataSubCommandObjects(MetadataSubCommand.DefineRow, m_defineRow);
                case MetadataSubCommand.UndefineRow:
                    m_undefineRow.Load(m_payload);
                    return new MetadataSubCommandObjects(MetadataSubCommand.UndefineRow, m_undefineRow);
                case MetadataSubCommand.Finished:
                    m_finished.Load(m_payload);
                    return new MetadataSubCommandObjects(MetadataSubCommand.Finished, m_finished);
                default:
                    throw new ArgumentOutOfRangeException("Subcommand not recognized ");
            }
        }
    }
}

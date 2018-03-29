using System;
using Sttp.Codec.Metadata;

namespace Sttp.Codec
{
    public class CommandMetadata : CommandBase
    {
        private SttpMarkupReader m_reader;
        private CmdFinished m_finished;
        private CmdDefineRow m_defineRow;
        private CmdDefineResponse m_defineResponse;
        private CmdUndefineRow m_undefineRow;

        public CommandMetadata(SttpMarkupReader reader)
            : base("Metadata")
        {
            m_reader = reader;
            m_finished = new CmdFinished();
            m_defineRow = new CmdDefineRow();
            m_defineResponse = new CmdDefineResponse();
            m_undefineRow = new CmdUndefineRow();
        }

        public MetadataSubCommandObjects NextCommand()
        {
            if (m_reader.Read())
            {
                switch (m_reader.ElementName)
                {
                    case "DefineResponse":
                        m_defineResponse.Load(m_reader);
                        return new MetadataSubCommandObjects(MetadataSubCommand.DefineResponse, m_defineResponse);
                    case "DefineRow":
                        m_defineRow.Load(m_reader);
                        return new MetadataSubCommandObjects(MetadataSubCommand.DefineRow, m_defineRow);
                    case "UndefineRow":
                        m_undefineRow.Load(m_reader);
                        return new MetadataSubCommandObjects(MetadataSubCommand.UndefineRow, m_undefineRow);
                    case "Finished":
                        m_finished.Load(m_reader);
                        return new MetadataSubCommandObjects(MetadataSubCommand.Finished, m_finished);
                    default:
                        throw new ArgumentOutOfRangeException("Subcommand not recognized ");
                }
            }
            return null;
        }

        public override void Save(SttpMarkupWriter writer)
        {
            throw new Exception("This command is for reading only and cannot encode");
        }
    }
}

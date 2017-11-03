namespace Sttp.WireProtocol.GetMetadataResponse
{
    public interface ICmd
    {
        SubCommand SubCommand { get; }
        void Load(PacketReader reader);
    }

    public class Cmd
    {
        private ICmd m_command;
        private SubCommand m_commandCode;

        internal void Load(ICmd command)
        {
            m_command = command;
            m_commandCode = command.SubCommand;
        }

        public SubCommand SubCommand => m_commandCode;

        public CmdDatabaseVersion DatabaseVersion => m_command as CmdDatabaseVersion;
        public CmdAddColumn AddColumn => m_command as CmdAddColumn;
        public CmdAddRow AddRow => m_command as CmdAddRow;
        public CmdAddTable AddTable => m_command as CmdAddTable;
        public CmdAddValue AddValue => m_command as CmdAddValue;
        public CmdClear Clear => m_command as CmdClear;
        public CmdDeleteRow DeleteRow => m_command as CmdDeleteRow;
    }
}
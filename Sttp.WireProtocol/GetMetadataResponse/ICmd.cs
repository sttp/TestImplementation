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
        public CmdDefineColumn DefineColumn => m_command as CmdDefineColumn;
        public CmdDefineRow DefineRow => m_command as CmdDefineRow;
        public CmdDefineTable AddTable => m_command as CmdDefineTable;
        public CmdDefineValue AddValue => m_command as CmdDefineValue;
        public CmdClear Clear => m_command as CmdClear;
        public CmdRemoveRow RemoveRow => m_command as CmdRemoveRow;
    }
}
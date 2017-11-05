namespace Sttp.WireProtocol.GetMetadata
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
        public CmdSelect Select => m_command as CmdSelect;
        public CmdWhereCompare WhereCompare => m_command as CmdWhereCompare;
        public CmdWhereInString WhereInString => m_command as CmdWhereInString;
        public CmdWhereInValue WhereInValue => m_command as CmdWhereInValue;
        public CmdWhereOperator WhereOperator => m_command as CmdWhereOperator;
    }
}
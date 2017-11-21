namespace Sttp.Codec.Metadata
{
    public interface ICmd
    {
        SubCommand SubCommand { get; }
        void Load(PayloadReader reader);
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

        public CmdFinished Finished => m_command as CmdFinished;
        public CmdDefineRow DefineRow => m_command as CmdDefineRow;
        public CmdDefineResponse DefineResponse => m_command as CmdDefineResponse;
        public CmdUndefineRow UndefineRow => m_command as CmdUndefineRow;
    }
}
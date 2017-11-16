namespace Sttp.WireProtocol.Subscribe
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
        public CmdConfigureOptions ConfigureOptions => m_command as CmdConfigureOptions;
        public CmdAllDataPoints AllDataPoints => m_command as CmdAllDataPoints;
        public CmdByQuery ByQuery => m_command as CmdByQuery;
        public CmdDataPointByID DataPointByID => m_command as CmdDataPointByID;
    }
}
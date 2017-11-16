namespace Sttp.WireProtocol.NegotiateSession
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

        public CmdInitiateReverseConnection InitiateReverseConnection => m_command as CmdInitiateReverseConnection;
        public CmdSupportedFunctionality SupportedFunctionality => m_command as CmdSupportedFunctionality;
        public CmdChangeInstance ChangeInstance => m_command as CmdChangeInstance;
        public CmdGetAllInstances GetAllInstances => m_command as CmdGetAllInstances;
        public CmdChangeUdpCiper ChangeUdpCiper => m_command as CmdChangeUdpCiper;
    }
}
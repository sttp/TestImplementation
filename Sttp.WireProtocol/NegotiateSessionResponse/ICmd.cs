namespace Sttp.WireProtocol.NegotiateSessionResponse
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

        public CmdRequestFailed RequestFailed => m_command as CmdRequestFailed;
        public CmdReverseConnectionSuccess ReverseConnectionSuccess => m_command as CmdReverseConnectionSuccess;
        public CmdDesiredOperation DesiredOperation => m_command as CmdDesiredOperation;
        public CmdChangeInstanceSuccess ChangeInstanceSuccess => m_command as CmdChangeInstanceSuccess;
        public CmdInstanceList InstanceList => m_command as CmdInstanceList;
        public CmdChangeUdpCiperResponse ChangeUdpCiperResponse => m_command as CmdChangeUdpCiperResponse;
    }
}
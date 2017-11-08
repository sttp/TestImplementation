namespace Sttp.WireProtocol.BulkTransport
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
        public CmdBeginSend BeginSend => m_command as CmdBeginSend;
        public CmdSendFragment SendFragment => m_command as CmdSendFragment;
        public CmdCancelSend CancelSend => m_command as CmdCancelSend;
    }
}
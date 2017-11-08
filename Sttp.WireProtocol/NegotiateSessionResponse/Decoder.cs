using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.NegotiateSessionResponse
{
    public class Decoder
    {
        private ICmd[] m_commands;
        private Cmd m_cmd;

        private PacketReader m_packet = new PacketReader(new SessionDetails());

        public Decoder()
        {
            m_cmd = new Cmd();
            m_commands = new ICmd[10];
            m_commands[(byte)SubCommand.RequestFailed] = new CmdRequestFailed();
            m_commands[(byte)SubCommand.ReverseConnectionSuccess] = new CmdReverseConnectionSuccess();
            m_commands[(byte)SubCommand.DesiredOperation] = new CmdDesiredOperation();
            m_commands[(byte)SubCommand.ChangeInstanceSuccess] = new CmdChangeInstanceSuccess();
            m_commands[(byte)SubCommand.InstanceList] = new CmdInstanceList();
            m_commands[(byte)SubCommand.ChangeUdpCiperResponse] = new CmdChangeUdpCiperResponse();
        }

        public CommandCode CommandCode => CommandCode.NegotiateSessionResponse;

        public void Fill(PacketReader buffer)
        {
            m_packet = buffer;
        }

        public Cmd NextCommand()
        {
            if (m_packet.Position == m_packet.Length)
                return null;

            SubCommand subCommand = m_packet.Read<SubCommand>();
            m_commands[(byte)subCommand].Load(m_packet);
            m_cmd.Load(m_commands[(byte)subCommand]);
            return m_cmd;
        }
    }
}

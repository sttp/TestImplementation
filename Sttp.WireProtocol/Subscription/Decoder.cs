using System;

namespace Sttp.WireProtocol.Subscription
{
    /// <summary>
    /// Responsible for encoding each command into bytes
    /// </summary>
    public class Decoder 
    {
        private ICmd[] m_commands;
        private Cmd m_cmd;
        private PacketReader m_packet = new PacketReader(new SessionDetails());

        public Decoder()
        {
            m_cmd = new Cmd();
            m_commands = new ICmd[10];
            m_commands[(byte)SubCommand.ConfigureOptions] = new CmdConfigureOptions();
            m_commands[(byte)SubCommand.AllDataPoints] = new CmdAllDataPoints();
            m_commands[(byte)SubCommand.ByQuery] = new CmdByQuery();
            m_commands[(byte)SubCommand.DataPointByID] = new CmdDataPointByID();
        }

        public CommandCode CommandCode => CommandCode.Subscription;

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

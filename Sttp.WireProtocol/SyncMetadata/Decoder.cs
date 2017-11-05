using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.SyncMetadata
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
            m_commands[(byte)SubCommand.Select] = new CmdSelect();
            m_commands[(byte)SubCommand.Join] = new CmdJoin();
            m_commands[(byte)SubCommand.WhereInString] = new CmdWhereInString();
            m_commands[(byte)SubCommand.WhereInValue] = new CmdWhereInValue();
            m_commands[(byte)SubCommand.WhereCompare] = new CmdWhereCompare();
            m_commands[(byte)SubCommand.WhereOperator] = new CmdWhereOperator();
            m_commands[(byte)SubCommand.DatabaseVersion] = new CmdDatabaseVersion();
        }

        public CommandCode CommandCode => CommandCode.GetMetadata;

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

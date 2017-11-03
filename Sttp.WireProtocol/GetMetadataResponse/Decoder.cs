using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.GetMetadataResponse
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
            m_commands[(byte)SubCommand.DatabaseVersion] = new CmdDatabaseVersion();
            m_commands[(byte)SubCommand.AddColumn] = new CmdAddColumn();
            m_commands[(byte)SubCommand.AddRow] = new CmdAddRow();
            m_commands[(byte)SubCommand.AddTable] = new CmdAddTable();
            m_commands[(byte)SubCommand.AddValue] = new CmdAddValue();
            m_commands[(byte)SubCommand.Clear] = new CmdClear();
            m_commands[(byte)SubCommand.DeleteRow] = new CmdDeleteRow();
        }

        public CommandCode CommandCode => CommandCode.GetMetadataResponse;

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.GetMetadataSchemaResponse
{
    public class Decoder
    {
        private ICmd[] m_commands;
        private Cmd m_cmd;

        private PacketReader m_packet = new PacketReader(new SessionDetails());

        public Decoder()
        {
            m_cmd = new Cmd();
            m_commands = new ICmd[5];
            m_commands[(byte)SubCommand.DatabaseVersion] = new CmdDatabaseVersion();
            m_commands[(byte)SubCommand.DefineColumn] = new CmdDefineColumn();
            m_commands[(byte)SubCommand.DefineTable] = new CmdDefineTable();
        }

        public CommandCode CommandCode => CommandCode.GetMetadataSchemaResponse;

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

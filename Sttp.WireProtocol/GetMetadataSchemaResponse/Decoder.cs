using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sttp.WireProtocol.Data;

namespace Sttp.WireProtocol.GetMetadataSchemaResponse
{
    public class Decoder
    {
        private ICmd[] m_commands;

        private PacketReader m_packet = new PacketReader(new SessionDetails());

        public Decoder()
        {
            m_commands = new ICmd[5];
            m_commands[(byte)SubCommand.DatabaseVersion] = new CmdDatabaseVersion();
            m_commands[(byte)SubCommand.AddColumn] = new CmdAddColumn();
            m_commands[(byte)SubCommand.AddTable] = new CmdAddTable();
        }

        public CommandCode CommandCode => CommandCode.GetMetadataSchemaResponse;

        public void Fill(PacketReader buffer)
        {
            m_packet = buffer;
        }

        public ICmd NextCommand()
        {
            if (m_packet.Position == m_packet.Length)
                return null;

            SubCommand subCommand = m_packet.Read<SubCommand>();
            m_commands[(byte)subCommand].Load(m_packet);
            return m_commands[(byte)subCommand];
        }
    }
}

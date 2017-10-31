﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.GetMetadataSchema
{
    public class Decoder
    {
        private ICmd[] m_commands;

        private PacketReader m_packet = new PacketReader(new SessionDetails());

        public Decoder()
        {
            m_commands = new ICmd[3];
            m_commands[(byte)SubCommand.GetDatabaseSchema] = new CmdGetDatabaseSchema();
            m_commands[(byte)SubCommand.GetDatabaseVersion] = new CmdGetDatabaseVersion();
        }

        public CommandCode CommandCode => CommandCode.GetMetadataSchema;

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
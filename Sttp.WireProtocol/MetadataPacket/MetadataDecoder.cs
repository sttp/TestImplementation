using System;
using System.Collections.Generic;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.WireProtocol.Data
{
    public class MetadataDecoder : IPacketDecoder
    {
        private IMetadataParams[] m_commands;


        private PacketReader m_packet = new PacketReader(new SessionDetails());
        private SessionDetails m_details;

        public MetadataDecoder(SessionDetails details)
        {
            m_details = details;

            m_commands = new IMetadataParams[20];
            m_commands[(byte)MetadataSubCommand.Clear] = new MetadataClearParams();
            m_commands[(byte)MetadataSubCommand.AddTable] = new MetadataAddTableParams();
            m_commands[(byte)MetadataSubCommand.AddColumn] = new MetadataAddColumnParams();
            m_commands[(byte)MetadataSubCommand.AddRow] = new MetadataAddRowParams();
            m_commands[(byte)MetadataSubCommand.AddValue] = new MetadataAddValueParams();
            m_commands[(byte)MetadataSubCommand.DeleteRow] = new MetadataDeleteRowParams();
            m_commands[(byte)MetadataSubCommand.DatabaseVersion] = new MetadataDatabaseVersionParams();
            m_commands[(byte)MetadataSubCommand.Select] = new MetadataSelectParams();
            m_commands[(byte)MetadataSubCommand.Join] = new MetadataJoinParams();
            m_commands[(byte)MetadataSubCommand.WhereInString] = new MetadataWhereInStringParams();
            m_commands[(byte)MetadataSubCommand.WhereInValue] = new MetadataWhereInValueParams();
            m_commands[(byte)MetadataSubCommand.WhereCompare] = new MetadataWhereCompareParams();
            m_commands[(byte)MetadataSubCommand.WhereOperator] = new MetadataWhereOperatorParams();
            m_commands[(byte)MetadataSubCommand.GetDatabaseSchema] = new MetadataGetDatabaseSchemaParams();
            m_commands[(byte)MetadataSubCommand.GetDatabaseVersion] = new MetadataGetDatabaseVersionParams();

        }

        public CommandCode CommandCode => CommandCode.Metadata;

        public void Fill(PacketReader buffer)
        {
            m_packet = buffer;
        }

        public IMetadataParams NextCommand()
        {
            if (m_packet.Position == m_packet.Length)
                return null;

            MetadataSubCommand subCommand = m_packet.Read<MetadataSubCommand>();
            m_commands[(byte)subCommand].Load(m_packet);
            return m_commands[(byte)subCommand];

        }

    }
}
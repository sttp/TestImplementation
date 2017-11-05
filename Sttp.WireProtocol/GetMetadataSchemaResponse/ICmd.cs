namespace Sttp.WireProtocol.GetMetadataSchemaResponse
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

        public CmdDatabaseVersion DatabaseVersion => m_command as CmdDatabaseVersion;
        public CmdDefineColumn DefineColumn => m_command as CmdDefineColumn;
        public CmdDefineTable DefineTable => m_command as CmdDefineTable;
        public CmdDefineTableRelationship DefineTableRelationship => m_command as CmdDefineTableRelationship;
    }
}
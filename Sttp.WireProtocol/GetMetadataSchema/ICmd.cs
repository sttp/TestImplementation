namespace Sttp.WireProtocol.GetMetadataSchema
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

        public CmdGetDatabaseSchema GetDatabaseSchema => m_command as CmdGetDatabaseSchema;
        public CmdGetDatabaseVersion GetDatabaseVersion => m_command as CmdGetDatabaseVersion;
    }
}
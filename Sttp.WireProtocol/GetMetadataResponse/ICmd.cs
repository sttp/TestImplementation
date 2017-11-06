﻿namespace Sttp.WireProtocol.GetMetadataResponse
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
        public CmdRequestFailed DefineColumn => m_command as CmdRequestFailed;
        public CmdDefineRow DefineRow => m_command as CmdDefineRow;
        public CmdDefineTable DefineTable => m_command as CmdDefineTable;
        public CmdVersionNotCompatible VersionNotCompatible => m_command as CmdVersionNotCompatible;
        public CmdUndefineRow UndefineRow => m_command as CmdUndefineRow;
    }
}
﻿namespace Sttp.WireProtocol.Subscription
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
        public CmdUnsubscribeFromAll UnsubscribeFromAll => m_command as CmdUnsubscribeFromAll;
        public CmdAllDataPoints AllDataPoints => m_command as CmdAllDataPoints;
        public CmdByQuery ByQuery => m_command as CmdByQuery;
        public CmdDataPointByID DataPointByID => m_command as CmdDataPointByID;
    }
}
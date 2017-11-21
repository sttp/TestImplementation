using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec.Metadata
{
    public class Decoder
    {
        private ICmd[] m_commands;
        private Cmd m_cmd;

        private PayloadReader m_payload = new PayloadReader(new SessionDetails());

        public Decoder()
        {
            m_cmd = new Cmd();
            m_commands = new ICmd[10];
            m_commands[(byte)SubCommand.Finished] = new CmdFinished();
            m_commands[(byte)SubCommand.DefineRow] = new CmdDefineRow();
            m_commands[(byte)SubCommand.DefineResponse] = new CmdDefineResponse();
            m_commands[(byte)SubCommand.UndefineRow] = new CmdUndefineRow();
            
        }

        public CommandCode CommandCode => CommandCode.Metadata;

        public void Fill(PayloadReader buffer)
        {
            m_payload = buffer;
        }

        public Cmd NextCommand()
        {
            if (m_payload.Position == m_payload.Length)
                return null;

            SubCommand subCommand = m_payload.Read<SubCommand>();
            m_commands[(byte)subCommand].Load(m_payload);
            m_cmd.Load(m_commands[(byte)subCommand]);
            return m_cmd;
        }
    }
}

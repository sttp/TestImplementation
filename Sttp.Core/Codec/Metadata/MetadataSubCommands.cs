//namespace Sttp.Codec.Metadata
//{
//    public class MetadataSubCommandObjects
//    {
//        private object m_command;
//        private MetadataSubCommand m_commandCode;

//        internal MetadataSubCommandObjects(MetadataSubCommand commandCode, object command)
//        {
//            m_commandCode = commandCode;
//            m_command = command;
//        }

//        public MetadataSubCommand SubCommand => m_commandCode;
//        public CmdFinished Finished => m_command as CmdFinished;
//        public CmdDefineRow DefineRow => m_command as CmdDefineRow;
//        public CmdDefineResponse DefineResponse => m_command as CmdDefineResponse;
//        public CmdUndefineRow UndefineRow => m_command as CmdUndefineRow;
//    }
//}
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using CTP.Net;
//using GSF.Threading;
//using Sttp.Codec;
//using Sttp.Core;

//namespace Sttp.Services
//{
//    public class SttpServer
//    {
//        public WireCodec Codec;
//        private Dictionary<string, ISttpCommandHandler> m_handler;
//        private SyncWorker m_processReads;

//        public SttpServer(SessionToken session)
//        {
//            m_handler = new Dictionary<string, ISttpCommandHandler>();
//            Codec = new WireCodec(session.FinalStream);
//            Codec.DataReceived += CodecDataReceived;
//            m_processReads = new SyncWorker(ProcessRequest);
//        }

//        public void Start()
//        {
//            m_processReads.Run();
//        }

//        private void CodecDataReceived()
//        {
//            m_processReads.Run();
//        }

//        public void RegisterCommandHandler(ISttpCommandHandler handler)
//        {
//            foreach (var name in handler.CommandsHandled())
//            {
//                m_handler[name] = handler;
//            }
//        }

//        private void ProcessRequest()
//        {
//            CommandObjects obj;
//            while ((obj = Codec.NextCommand()) != null)
//            {
//                if (m_handler.TryGetValue(obj.CommandName, out ISttpCommandHandler handler))
//                {
//                    handler.HandleCommand(obj, Codec);
//                }
//                else
//                {
//                    Codec.RequestFailed(obj.CommandName, "Command Unknown", "Specified command is either not recognized or the user does not have sufficient permissions to execute this command.");
//                }
//            }
//        }

//    }
//}

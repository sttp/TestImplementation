//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Sttp.Codec
//{
//    /// <summary>
//    /// If a command code is unknown by this API layer, it will be reported in a raw format.
//    /// </summary>
//    public class CommandUnknown
//    {
//        public readonly CommandCode CommandCode;
//        public readonly byte[] Data;

//        public CommandUnknown(PayloadReader reader)
//        {
//            CommandCode = reader.Command;
//            Data = reader.ReadBytes();
//        }
//    }
//}

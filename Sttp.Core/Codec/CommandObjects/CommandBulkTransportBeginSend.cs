//using System;

//namespace Sttp.Codec
//{
//    public class CommandBulkTransportBeginSend
//    {
//        public readonly CommandCode Code = CommandCode.LargeObject;

//        public readonly Guid ID;
//        public readonly BulkTransportMode Mode;
//        public readonly BulkTransportCompression Compression;
//        public readonly long OrigionalSize;
//        public readonly byte[] Data;

//        public CommandBulkTransportBeginSend(SttpMarkupElement element)
//        {
//            ID = (Guid)element.GetValue("ID");
//            Mode = (BulkTransportMode)reader.ReadByte();
//            Compression = (BulkTransportCompression)reader.ReadByte();
//            OrigionalSize = reader.ReadInt64();
//            Data = reader.ReadBytes();
//        }

//        public SttpMarkup Save()
//        {
//            var sml = new SttpMarkupWriter();
//            using (sml.StartElement("BulkTransportBeginSend"))
//            {
//                sml.WriteValue("ID", ID);
//                sml.WriteValue("BulkTransportMode", Mode.ToString());
//                sml.WriteValue("BulkTransportCompression", Compression.ToString());
//                sml.WriteValue("OriginalSize", OrigionalSize);
//                sml.WriteValue("Data", Data);
//            }
//            return sml.ToSttpMarkup();
//        }

//    }
//}
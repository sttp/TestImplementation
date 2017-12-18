//using System.Collections.Generic;

//namespace Sttp.Codec.Metadata
//{
//    public class CmdDefineRow
//    {
//        public MetadataSubCommand SubCommand => MetadataSubCommand.DefineRow;
//        public SttpValue PrimaryKey;
//        public List<SttpValue> Values = new List<SttpValue>();

//        public void Load(PayloadReader reader)
//        {
//            PrimaryKey = SttpValueEncodingNative.Load(reader);
//            int cnt = (int)reader.Read4BitSegments();
//            while (cnt > 0)
//            {
//                cnt--;
//                Values.Add(SttpValueEncodingNative.Load(reader));
//            }
//        }


//    }
//}
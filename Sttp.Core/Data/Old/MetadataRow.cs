//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Sttp.Data
//{
//    public class MetadataRow
//    {
//        public readonly SttpValue Key;
//        public readonly List<SttpValue> Fields;
//        /// <summary>
//        /// The index position in he table that has this foreign key. 
//        /// -1 means the row does not exist.
//        /// </summary>
//        public readonly int[] ForeignKeys;
//        public readonly long Revision;

//        public MetadataRow(SttpValue key, List<SttpValue> fields, int foreignKeyCount, long revision)
//        {
//            Revision = revision;
//            Key = key.Clone();
//            Fields = fields;
//            ForeignKeys = new int[foreignKeyCount];
//            for (int x = 0; x < foreignKeyCount; x++)
//            {
//                ForeignKeys[x] = -1;
//            }
//        }

//        public override string ToString()
//        {
//            return Key.ToString();
//        }
//    }
//}

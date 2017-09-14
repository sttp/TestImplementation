using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sttp;

namespace SampleCode.Encoding_Layer
{
    //------------------------
    //
    //  Provides a means to serialize a relational data set. 
    //
    //------------------------

    public class TreeMetadata
    {
        public int RuntimeID;
        public int ParentObject;
        public string AttributeName;
        public ValueTypeCode AttributeType;
        public byte[] AttributeValue;
    }
}

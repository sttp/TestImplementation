using System;

namespace Sttp.WireProtocol.SendDataPoints
{
    public class SttpScientificTime
    {
        //ToDo: As part of this protocol, we'll need to settle on one.
        //For extremely long timestamps with scientific precision, consider some of the following examples:
        //https://github.com/numpy/numpy/blob/master/numpy/core/src/multiarray/datetime.c
        //https://docs.scipy.org/doc/numpy-1.13.0/reference/arrays.datetime.html
        //We could even consider using the data type Decimal.

        public string Time = DateTime.Now.ToString("yyyy MM dd HH mm ss fffffff");
        
        public SttpScientificTime(SttpTimestamp timestamp)
        {

        }
        public SttpTimestamp ToSttpTimestamp()
        {
            return default(SttpTimestamp);
        }
    }
}
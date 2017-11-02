using System;

namespace Sttp.WireProtocol.SendDataPoints
{
    public class SttpScientificTime
    {
        //ToDo: As part of this protocol, we'll need to settle on one.
        private SttpTicks m_ticks; 
        private uint m_highPrecision;  //9 digits to be tacked on to the upper part of the date.
        private uint m_lowPrecision;   //9 digits to be tacked on to the lower part of the date.

        //For extremely long timestamps with scientific precision, consider some of the following examples:
        //https://github.com/numpy/numpy/blob/master/numpy/core/src/multiarray/datetime.c
        //https://docs.scipy.org/doc/numpy-1.13.0/reference/arrays.datetime.html
        //We could even consider using the data type Decimal.

        public SttpScientificTime(SttpTicks ticks)
        {
            ;
        }
        public SttpTicks ToSttpTimestamp()
        {
            return default(SttpTicks);
        }
    }
}